using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.RawStatDTO")]
    public class RawStatDTO : IRiotRtmpObject
    {
        [SerializedName("value")]
        public double Value { get; set; }

        [SerializedName("statTypeName")]
        public string StatTypeName { get; set; }
    }
}