using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal abstract class ByteData : RtmpEvent
    {
        protected ByteData(byte[] data, MessageType messageType) : base(messageType)
        {
            Data = data;
        }

        public byte[] Data { get; private set; }
    }

    internal class AudioData : ByteData
    {
        public AudioData(byte[] data) : base(data, MessageType.Audio)
        {
        }
    }

    internal class VideoData : ByteData
    {
        public VideoData(byte[] data) : base(data, MessageType.Video)
        {
        }
    }
}