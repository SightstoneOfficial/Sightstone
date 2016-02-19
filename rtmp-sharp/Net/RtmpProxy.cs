using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Net
{
    public class RtmpProxy
    {
        private readonly X509Certificate2 _cert;
        private readonly TcpListener _listener;
        private readonly Uri _remoteUri;
        private readonly SerializationContext _serializationContext;

        private readonly RemoteCertificateValidationCallback certificateValidator =
            (sender, certificate, chain, errors) => true;

        private RtmpProxyRemote _remote;
        private RtmpProxySource _source;
        private IPEndPoint _sourceEndpoint;

        public RtmpProxy(IPEndPoint source, Uri remote, SerializationContext context, X509Certificate2 cert = null)
        {
            //SubscribedChannels = new List<string>();
            _cert = cert;
            _serializationContext = context;
            _remoteUri = remote;
            _sourceEndpoint = source;

            _listener = new TcpListener(source);
        }

        public event EventHandler<RemotingMessageReceivedEventArgs> RemotingMessageReceived;
        public event EventHandler<RemotingMessageReceivedEventArgs> ErrorMessageReceived;
        public event EventHandler<CommandMessageReceivedEventArgs> CommandMessageReceived;
        public event EventHandler<RemotingMessageReceivedEventArgs> AcknowledgeMessageReceived;
        public event EventHandler<MessageReceivedEventArgs> AsyncMessageReceived;
        public event EventHandler<EventArgs> Connected;
        public event EventHandler<EventArgs> Disconnected;

        public void Listen()
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(OnClientAccepted, _listener);
        }

        public void Close()
        {
            _listener.Stop();
        }

        private void OnClientAccepted(IAsyncResult ar)
        {
            var listener = ar.AsyncState as TcpListener;
            try
            {
                var client = listener.EndAcceptTcpClient(ar);
                if (!client.Connected)
                    return;
                var stream = GetRtmpStream(client);

                _source = new RtmpProxySource(_serializationContext, stream);
                _source.RemotingMessageReceived += OnRemotingMessageReceived;
                _source.CommandMessageReceived += OnCommandMessageReceived;
                _source.ConnectMessageReceived += OnConnectMessageReceived;
                _source.Disconnected += OnClientDisconnected;
            }
            catch (ObjectDisposedException)
            {
                //disconnect
            }
        }

        private void OnServerDisconnected(object sender, EventArgs e)
        {
            _remote.Close();
            _listener.Stop();
            if (Disconnected != null)
                Disconnected(this, new EventArgs());
            Listen();
        }

        private void OnClientDisconnected(object sender, EventArgs e)
        {
            _source.Close();
            _listener.Stop();
            if (Disconnected != null)
                Disconnected(this, new EventArgs());
            Listen();
        }

        private void OnConnectMessageReceived(object sender, ConnectMessageEventArgs e)
        {
            if (e.Message.Operation == CommandOperation.ClientPing)
            {
                _remote = new RtmpProxyRemote(_remoteUri, _serializationContext, ObjectEncoding.Amf3);
                _remote.MessageReceived += OnAsyncMessageReceived;
                _remote.Disconnected += OnServerDisconnected;
                e.Result =
                    _remote.ConnectAckAsync(e.InvokeId, e.ConnectionParameters, false, e.ClientId, e.AuthToken,
                        e.Message).Result;
                if (Connected != null)
                    Connected(this, new EventArgs());
            }
            else
            {
                _remote = new RtmpProxyRemote(_remoteUri, _serializationContext, ObjectEncoding.Amf3);
                _remote.MessageReceived += OnAsyncMessageReceived;
                _remote.Disconnected += OnServerDisconnected;
                e.Result =
                    _remote.ReconnectAckAsync(e.InvokeId, e.ConnectionParameters, false, e.ClientId, e.AuthToken,
                        e.Message).Result;
                if (Connected != null)
                    Connected(this, new EventArgs());
            }
        }

        private void OnAsyncMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (AsyncMessageReceived != null)
                AsyncMessageReceived(this, e);
            _source.InvokeReceive(e.ClientId, e.Subtopic, e.Message.Body);
        }

        private void OnCommandMessageReceived(object sender, CommandMessageReceivedEventArgs e)
        {
            e.Result = _remote.InvokeAckAsync(e.InvokeId, null, e.Message).Result;
        }

        private void OnRemotingMessageReceived(object sender, RemotingMessageReceivedEventArgs e)
        {
            try
            {
                if (RemotingMessageReceived != null)
                    RemotingMessageReceived(this, e);
                e.Result = _remote.InvokeAckAsync(e.InvokeId, e.Message).Result;
                //TODO it's probably better to copy the eventargs
                if (AcknowledgeMessageReceived != null)
                    AcknowledgeMessageReceived(this, e);
            }
            catch (AggregateException ex)
            {
                var exception = ex.InnerException as InvocationException;
                if (exception != null)
                {
                    e.Error = (ErrorMessage) exception.SourceException;
                    if (ErrorMessageReceived != null)
                        ErrorMessageReceived(this, e);
                }
                else
                    throw;
            }
        }

        public async Task<object> InvokeAsync(string destination, string operation, params object[] arguments)
        {
            return await _remote.InvokeAsync<object>("my-rtmps", destination, operation, arguments);
        }

        private Stream GetRtmpStream(TcpClient client)
        {
            var stream = client.GetStream();
            if (_cert != null)
            {
                var ssl = new SslStream(stream, false, certificateValidator);
                ssl.AuthenticateAsServer(_cert);
                return ssl;
            }
            return stream;
        }
    }
}