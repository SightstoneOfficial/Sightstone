using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using Complete;
using Complete.Threading;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using RtmpSharp.Messaging.Events;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Net
{
    internal class RtmpProxySource
    {
        private readonly TaskCallbackManager<int, object> callbackManager;

        private readonly RemoteCertificateValidationCallback certificateValidator =
            (sender, certificate, chain, errors) => true;

        private readonly ObjectEncoding objectEncoding;
        private readonly SerializationContext serializationContext;
        private bool disconnectRequested;
        public bool ExclusiveAddressUse;
        private bool hasConnected;
        private int invokeId;
        public IPEndPoint LocalEndPoint;
        public bool NoDelay = true;
        private RtmpPacketReader reader;
        private Thread readerThread;
        public int ReceiveTimeout;
        private string reconnectData;
        private bool reconnecting;
        public int SendTimeout;
        private RtmpPacketWriter writer;
        private Thread writerThread;

        public RtmpProxySource(SerializationContext serializationContext, Stream stream) : this(serializationContext)
        {
            DoHandshake(stream);
            EstablishThreads(stream);
            objectEncoding = ObjectEncoding.Amf3;
        }

        public RtmpProxySource(SerializationContext serializationContext)
        {
            if (serializationContext == null) throw new ArgumentNullException("serializationContext");

            this.serializationContext = serializationContext;
            callbackManager = new TaskCallbackManager<int, object>();
        }

        public bool IsDisconnected { get; set; }
        public event EventHandler Disconnected;
        internal event EventHandler<RemotingMessageReceivedEventArgs> RemotingMessageReceived;
        internal event EventHandler<CommandMessageReceivedEventArgs> CommandMessageReceived;
        internal event EventHandler<ConnectMessageEventArgs> ConnectMessageReceived;
        public event EventHandler<Exception> CallbackException;

        private void DoHandshake(Stream stream)
        {
            // read c0+c1
            var c01 = RtmpHandshake.Read(stream, true);

            var random = new Random();
            var randomBytes = new byte[1528];
            random.NextBytes(randomBytes);

            // write s0+s1+s2
            var s01 = new RtmpHandshake
            {
                Version = 3,
                Time = (uint) Environment.TickCount,
                Time2 = 0,
                Random = randomBytes
            };
            var s02 = s01.Clone();
            s02.Time2 = (uint) Environment.TickCount;
            RtmpHandshake.WriteAsync(stream, s01, s02, true);

            // read c02
            var c02 = RtmpHandshake.Read(stream, false);
            hasConnected = true;
        }

        private Task<object> QueueCommandAsTask(Command command, int streamId, int messageStreamId)
        {
            if (IsDisconnected)
                return CreateExceptedTask(new ClientDisconnectedException("disconnected"));

            var task = callbackManager.Create(command.InvokeId);
            writer.Queue(command, streamId, messageStreamId);
            return task;
        }

        public void EstablishThreads(Stream stream)
        {
            writer = new RtmpPacketWriter(new AmfWriter(stream, serializationContext), ObjectEncoding.Amf3);
            reader = new RtmpPacketReader(new AmfReader(stream, serializationContext));
            reader.EventReceived += EventReceivedCallback;
            reader.Disconnected += OnPacketProcessorDisconnected;
            writer.Disconnected += OnPacketProcessorDisconnected;

            writerThread = new Thread(reader.ReadLoop) {IsBackground = true};
            readerThread = new Thread(writer.WriteLoop) {IsBackground = true};

            writerThread.Start();
            readerThread.Start();
        }

        private void OnPacketProcessorDisconnected(object sender, ExceptionalEventArgs e)
        {
            OnDisconnect(e);
        }

        private void OnDisconnect(ExceptionalEventArgs e)
        {
            if (IsDisconnected)
                return;
            IsDisconnected = true;

            if (writer != null) writer.Continue = false;
            if (reader != null) reader.Continue = false;

            try
            {
                writerThread.Abort();
            }
            catch
            {
            }
            try
            {
                readerThread.Abort();
            }
            catch
            {
            }

            WrapCallback(
                () => callbackManager.SetExceptionForAll(new ClientDisconnectedException(e.Description, e.Exception)));
            invokeId = 0;

            WrapCallback(() =>
            {
                if (Disconnected != null)
                    Disconnected(this, e);
            });
        }

        public void Close()
        {
            OnDisconnect(new ExceptionalEventArgs("closed"));
        }

        private async void EventReceivedCallback(object sender, EventReceivedEventArgs e)
        {
            try
            {
                switch (e.Event.MessageType)
                {
                    case MessageType.UserControlMessage:
                        var m = (UserControlMessage) e.Event;
                        if (m.EventType == UserControlMessageType.PingRequest)
                            WriteProtocolControlMessage(new UserControlMessage(UserControlMessageType.PingResponse,
                                m.Values));
                        break;

                    case MessageType.DataAmf3:
#if DEBUG
                        // Have no idea what the contents of these packets are.
                        // Study these packets if we receive them.
                        Debugger.Break();
#endif
                        break;
                    case MessageType.CommandAmf3:
                    case MessageType.DataAmf0:
                    case MessageType.CommandAmf0:
                        var command = (Command) e.Event;
                        var call = command.MethodCall;

                        var param = call.Parameters.Length == 1 ? call.Parameters[0] : call.Parameters;
                        if (call.Name == "_result" || call.Name == "_error" || call.Name == "receive")
                        {
                            //should not happen here
                            throw new InvalidDataException();
                        }
                        if (call.Name == "onstatus")
                        {
                            Debug.Print("Received status.");
                        }
                        else if (call.Name == "connect")
                        {
                            var message = (CommandMessage) call.Parameters[3];
                            object endpoint;
                            message.Headers.TryGetValue(AsyncMessageHeaders.Endpoint, out endpoint);
                            object id;
                            message.Headers.TryGetValue(AsyncMessageHeaders.ID, out id);
                            //ClientId = (string) id;

                            var args = new ConnectMessageEventArgs((string) call.Parameters[1],
                                (string) call.Parameters[2], message, (string) endpoint, (string) id, command.InvokeId,
                                (AsObject) command.ConnectionParameters);
                            if (ConnectMessageReceived != null)
                                ConnectMessageReceived(this, args);
                            if (message.Operation == CommandOperation.ClientPing)
                                await InvokeConnectResultAsync(command.InvokeId, (AsObject) args.Result.Body);
                            else
                                await InvokeReconnectResultInvokeAsync(command.InvokeId, (AsObject) args.Result.Body);
                        }
                        else if (param is RemotingMessage)
                        {
                            var message = param as RemotingMessage;

                            object endpoint;
                            message.Headers.TryGetValue(AsyncMessageHeaders.Endpoint, out endpoint);
                            object id;
                            message.Headers.TryGetValue(AsyncMessageHeaders.ID, out id);

                            var args = new RemotingMessageReceivedEventArgs(message, (string) endpoint, (string) id,
                                command.InvokeId);
                            if (RemotingMessageReceived != null)
                                RemotingMessageReceived(this, args);
                            if (args.Error == null)
                                InvokeResult(command.InvokeId, args.Result);
                            else
                                InvokeError(command.InvokeId, args.Error);
                        }
                        else if (param is CommandMessage)
                        {
                            var message = param as CommandMessage;

                            object endpoint;
                            message.Headers.TryGetValue(AsyncMessageHeaders.Endpoint, out endpoint);
                            object id;
                            message.Headers.TryGetValue(AsyncMessageHeaders.ID, out id);

                            var args = new CommandMessageReceivedEventArgs(message, endpoint as string, id as string,
                                command.InvokeId);
                            if (CommandMessageReceived != null)
                                CommandMessageReceived(this, args);
                            InvokeResult(command.InvokeId, args.Result);
                        }
                        else
                        {
#if DEBUG
                            Debug.Print("Unknown RTMP Command: " + call.Name);
                            Debugger.Break();
#endif
                        }
                        break;
                }
            }
            catch (ClientDisconnectedException)
            {
                //Close();
            }
        }

        internal void InvokeResult(int invokeId, AcknowledgeMessageExt message)
        {
            if (objectEncoding != ObjectEncoding.Amf3)
                throw new NotSupportedException("Flex RPC requires AMF3 encoding.");

            var invoke = new InvokeAmf3
            {
                InvokeId = invokeId,
                MethodCall = new Method("_result", new object[] {message}, true, CallStatus.Result)
            };
            QueueCommandAsTask(invoke, 3, 0);
        }

        internal void InvokeError(int invokeId, ErrorMessage message)
        {
            if (objectEncoding != ObjectEncoding.Amf3)
                throw new NotSupportedException("Flex RPC requires AMF3 encoding.");

            var invoke = new InvokeAmf3
            {
                InvokeId = invokeId,
                MethodCall = new Method("_error", new object[] {message}, false, CallStatus.Result)
            };
            QueueCommandAsTask(invoke, 3, 0);
        }

        public void InvokeError(int invokeId, string correlationId, object rootCause, string faultDetail,
            string faultString, string faultCode)
        {
            var call = new ErrorMessage
            {
                ClientId = Uuid.NewUuid(),
                MessageId = Uuid.NewUuid(),
                CorrelationId = correlationId,
                RootCause = rootCause
            };

            InvokeError(invokeId, call);
        }

        internal void InvokeReceive(string clientId, string subtopic, object body)
        {
            var invoke = new InvokeAmf3
            {
                InvokeId = 0,
                MethodCall = new Method("receive", new object[]
                {
                    new AsyncMessageExt
                    {
                        Headers = new AsObject {{FlexMessageHeaders.FlexSubtopic, subtopic}},
                        ClientId = clientId,
                        Body = body,
                        MessageId = Uuid.NewUuid()
                    }
                })
            };
            QueueCommandAsTask(invoke, 3, 0);
        }

        public async Task<AsObject> ConnectResultInvokeAsync(object[] parameters)
        {
            //Write ServerBW & ClientBW
            var ServerBW = new WindowAcknowledgementSize(245248000);
            WriteProtocolControlMessage(ServerBW);
            var ClientBW = new PeerBandwidth(250000, 2);
            WriteProtocolControlMessage(ClientBW);

            SetChunkSize(50000);
            var connectResult = new InvokeAmf0
            {
                MethodCall = new Method("_result", new object[1]
                {
                    new AsObject
                    {
                        {"objectEncoding", 3.0},
                        {"level", "status"},
                        {"details", null},
                        {"description", "Connection succeeded."},
                        {"DSMessagingVersion", 1.0},
                        {"code", "NetConnection.Connect.Success"},
                        {"id", Uuid.NewUuid()}
                    }
                }),
                InvokeId = GetNextInvokeId()
            };

            return (AsObject) await QueueCommandAsTask(connectResult, 3, 0);
        }

        public async Task<AsObject> InvokeConnectResultAsync(int invokeId, AsObject param)
        {
            //Write ServerBW & ClientBW
            var ServerBW = new WindowAcknowledgementSize(245248000);
            WriteProtocolControlMessage(ServerBW);
            var ClientBW = new PeerBandwidth(250000, 2);
            WriteProtocolControlMessage(ClientBW);

            SetChunkSize(50000);
            var connectResult = new InvokeAmf0
            {
                MethodCall = new Method("_result", new[] {param}),
                InvokeId = invokeId
            };

            return (AsObject) await QueueCommandAsTask(connectResult, 3, 0);
        }

        public async Task<AsObject> InvokeReconnectResultInvokeAsync(int invokeId, AsObject param)
        {
            //Write ServerBW & ClientBW
            //var ServerBW = new WindowAcknowledgementSize(245248000);
            //WriteProtocolControlMessage(ServerBW);
            //var ClientBW = new PeerBandwidth(250000, 2);
            //WriteProtocolControlMessage(ClientBW);

            SetChunkSize(50000);
            var connectResult = new InvokeAmf0
            {
                MethodCall = new Method("_result", new[] {param}),
                InvokeId = invokeId
            };

            return (AsObject) await QueueCommandAsTask(connectResult, 3, 0);
        }

        public void SetChunkSize(int size)
        {
            WriteProtocolControlMessage(new ChunkSize(size));
        }

        private void WriteProtocolControlMessage(RtmpEvent @event)
        {
            writer.Queue(@event, 2, 0);
        }

        private int GetNextInvokeId()
        {
            // interlocked.increment wraps overflows
            return Interlocked.Increment(ref invokeId);
        }
#pragma warning disable 0168 //Disable unused variable warning
        private void WrapCallback(Action action)
        {
            try
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (CallbackException != null)
                        CallbackException(this, ex);
                }
            }
            catch (Exception unhandled)
            {
#if DEBUG //&& BREAK_ON_EXCEPTED_CALLBACK
                Debug.Print("UNHANDLED EXCEPTION IN CALLBACK: {0}: {1} @ {2}", unhandled.GetType(), unhandled.Message,
                    unhandled.StackTrace);
                Debugger.Break();
#endif
            }
        }

        private static Task<object> CreateExceptedTask(Exception exception)
        {
            var source = new TaskCompletionSource<object>();
            source.SetException(exception);
            return source.Task;
        }
    }
}