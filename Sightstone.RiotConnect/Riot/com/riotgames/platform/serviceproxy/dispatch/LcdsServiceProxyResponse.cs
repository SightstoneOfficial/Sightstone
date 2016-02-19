using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.serviceproxy.dispatch
{
    [Serializable]
    [SerializedName("com.riotgames.platform.serviceproxy.dispatch.LcdsServiceProxyResponse")]
    public class LcdsServiceProxyResponse : IRiotRtmpObject
    {
        [SerializedName("status")]
        public string Status { get; set; }

        [SerializedName("payload")]
        public string Payload { get; set; }

        [SerializedName("messageId")]
        public string messageId { get; set; }

        [SerializedName("methodName")]
        public string MethodName { get; set; }

        [SerializedName("serviceName")]
        public string ServiceName { get; set; }
    }
}