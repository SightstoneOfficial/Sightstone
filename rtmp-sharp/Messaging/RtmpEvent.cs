using RtmpSharp.Net;

namespace RtmpSharp.Messaging
{
    internal abstract class RtmpEvent
    {
        protected RtmpEvent(MessageType messageType)
        {
            MessageType = messageType;
        }

        public RtmpHeader Header { get; set; }
        public int Timestamp { get; set; }
        public MessageType MessageType { get; set; }
    }
}