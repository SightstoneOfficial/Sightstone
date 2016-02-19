using System;
using RtmpSharp.IO;

namespace RtmpSharp.Messaging.Messages
{
    [Serializable]
    [SerializedName("flex.messaging.messages.RemotingMessage")]
    public class RemotingMessage : FlexMessage
    {
        [SerializedName("source")]
        public string Source { get; set; }

        [SerializedName("operation")]
        public string Operation { get; set; }
    }
}