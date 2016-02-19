using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal class ChunkSize : RtmpEvent
    {
        public ChunkSize(int size) : base(MessageType.SetChunkSize)
        {
            if (size > 0xFFFFFF)
                size = 0xFFFFFF;
            Size = size;
        }

        public int Size { get; private set; }
    }
}