using System;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Messaging
{
    public class RemotingMessageReceivedEventArgs : EventArgs
    {
        public readonly string Destination;
        public readonly string Endpoint;
        public readonly int InvokeId;
        public readonly RemotingMessage Message;
        public readonly string MessageId;
        public readonly string Operation;
        public ErrorMessage Error;
        public AcknowledgeMessageExt Result;

        internal RemotingMessageReceivedEventArgs(RemotingMessage message, string endpoint, string clientId,
            int invokeId)
        {
            Message = message;
            Operation = message.Operation;
            Destination = message.Destination;
            Endpoint = endpoint;
            MessageId = clientId;
            InvokeId = invokeId;
        }
    }
}