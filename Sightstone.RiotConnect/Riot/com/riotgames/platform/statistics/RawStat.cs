using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.RawStat")]
    public class RawStat : IRiotRtmpObject
    {
        [SerializedName("statType")]
        public string StatType { get; set; }

        [SerializedName("value")]
        public double Value { get; set; }
    }
}