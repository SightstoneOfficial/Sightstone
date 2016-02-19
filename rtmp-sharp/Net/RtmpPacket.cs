using System;
using RtmpSharp.Messaging;

namespace RtmpSharp.Net
{
    internal class RtmpPacket
    {
        public RtmpPacket(RtmpHeader header)
        {
            Header = header;
            Length = header.PacketLength;
            Buffer = new byte[Length];
        }

        public RtmpPacket(RtmpEvent body)
        {
            Body = body;
        }

        public RtmpPacket(RtmpHeader header, RtmpEvent body) : this(header)
        {
            Body = body;
            Length = header.PacketLength;
        }

        public RtmpHeader Header { get; set; }
        public RtmpEvent Body { get; set; }
        public byte[] Buffer { get; }
        public int Length { get; }
        public int CurrentLength { get; private set; }

        public bool IsComplete
        {
            get { return Length == CurrentLength; }
        }

        internal void AddBytes(byte[] bytes)
        {
            Array.Copy(bytes, 0, Buffer, CurrentLength, bytes.Length);
            CurrentLength += bytes.Length;
        }
    }
}