using System;
using RtmpSharp.IO;

namespace RtmpSharp.Messaging.Messages
{
    [Serializable]
    [SerializedName("flex.messaging.messages.ErrorMessage")]
    public class ErrorMessage : FlexMessage
    {
        [SerializedName("faultCode")]
        public string FaultCode { get; set; }

        [SerializedName("faultString")]
        public string FaultString { get; set; }

        [SerializedName("faultDetail")]
        public string FaultDetail { get; set; }

        [SerializedName("rootCause")]
        public object RootCause { get; set; }

        [SerializedName("extendedData")]
        public object ExtendedData { get; set; }

        [SerializedName("correlationId")]
        public object CorrelationId { get; set; }
    }
}