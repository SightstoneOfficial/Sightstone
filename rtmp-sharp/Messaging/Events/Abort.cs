using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal class Abort : RtmpEvent
    {
        public Abort(int streamId) : base(MessageType.AbortMessage)
        {
            StreamId = streamId;
        }

        public int StreamId { get; private set; }
    }
}