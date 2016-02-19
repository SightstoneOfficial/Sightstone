using System;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Messaging
{
    public class CommandMessageReceivedEventArgs : EventArgs
    {
        public readonly string DSId;
        public readonly string Endpoint;
        public readonly int InvokeId;
        public readonly CommandMessage Message;
        public readonly CommandOperation Operation;
        public AcknowledgeMessageExt Result;

        internal CommandMessageReceivedEventArgs(CommandMessage message, string endpoint, string dsId, int invokeId)
        {
            DSId = dsId;
            Operation = message.Operation;
            Endpoint = endpoint;
            Message = message;
            InvokeId = invokeId;
        }
    }
}