using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.matchmaking
{
    [Serializable]
    [SerializedName("com.riotgames.platform.matchmaking.QueueInfo")]
    public class QueueInfo : IRiotRtmpObject
    {
        [SerializedName("waitTime")]
        public double WaitTime { get; set; }

        [SerializedName("queueId")]
        public double QueueId { get; set; }

        [SerializedName("queueLength")]
        public int QueueLength { get; set; }
    }
}