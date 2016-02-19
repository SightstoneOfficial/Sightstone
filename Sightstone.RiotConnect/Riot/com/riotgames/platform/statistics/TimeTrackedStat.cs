using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.TimeTrackedStat")]
    public class TimeTrackedStat : IRiotRtmpObject
    {
        [SerializedName("timestamp")]
        public DateTime Timestamp { get; set; }

        [SerializedName("type")]
        public string Type { get; set; }
    }
}