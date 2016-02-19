namespace RtmpSharp.Net
{
    internal class RtmpHeader
    {
        // size of the chunk, including the header and payload
        public int PacketLength { get; set; }
        public int StreamId { get; set; }
        public MessageType MessageType { get; set; }
        public int MessageStreamId { get; set; }
        public int Timestamp { get; set; }
        public bool IsTimerRelative { get; set; }

        public static int GetHeaderLength(ChunkMessageHeaderType chunkMessageHeaderType)
        {
            switch (chunkMessageHeaderType)
            {
                case ChunkMessageHeaderType.New:
                    return 11;
                case ChunkMessageHeaderType.SameSource:
                    return 7;
                case ChunkMessageHeaderType.TimestampAdjustment:
                    return 3;
                case ChunkMessageHeaderType.Continuation:
                    return 0;
                default:
                    return -1;
            }
        }

        public RtmpHeader Clone()
        {
            return (RtmpHeader) MemberwiseClone();
        }
    }
}