using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal class Acknowledgement : RtmpEvent
    {
        public Acknowledgement(int bytesRead) : base(MessageType.Acknowledgement)
        {
            BytesRead = bytesRead;
        }

        public int BytesRead { get; private set; }
    }
}