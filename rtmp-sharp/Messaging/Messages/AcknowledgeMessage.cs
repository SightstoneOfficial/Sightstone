using System;
using RtmpSharp.IO;

namespace RtmpSharp.Messaging.Messages
{
    [Serializable]
    [SerializedName("flex.messaging.messages.AcknowledgeMessage")]
    public class AcknowledgeMessage : AsyncMessage
    {
        public AcknowledgeMessage()
        {
            Timestamp = Environment.TickCount;
        }
    }
}