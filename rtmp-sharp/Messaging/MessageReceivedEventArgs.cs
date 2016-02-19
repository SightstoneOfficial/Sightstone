using System;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Messaging
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public readonly string ClientId;
        public readonly AsyncMessageExt Message;
        public readonly string Subtopic;

        internal MessageReceivedEventArgs(string clientId, string subtopic, AsyncMessageExt message)
        {
            ClientId = clientId;
            Subtopic = subtopic;
            Message = message;
        }
    }
}