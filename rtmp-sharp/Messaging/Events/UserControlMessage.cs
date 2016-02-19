using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    // user control message
    internal class UserControlMessage : RtmpEvent
    {
        public UserControlMessage(UserControlMessageType eventType, int[] values) : base(MessageType.UserControlMessage)
        {
            EventType = eventType;
            Values = values;
        }

        public UserControlMessageType EventType { get; private set; }
        public int[] Values { get; private set; }
    }

    internal enum UserControlMessageType : ushort
    {
        StreamBegin = 0,
        StreamEof = 1,
        StreamDry = 2,
        SetBufferLength = 3,
        StreamIsRecorded = 4,
        PingRequest = 6,
        PingResponse = 7
    }
}