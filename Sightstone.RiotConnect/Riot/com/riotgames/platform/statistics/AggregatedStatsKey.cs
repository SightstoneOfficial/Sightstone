using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.AggregatedStatsKey")]
    public class AggregatedStatsKey : IRiotRtmpObject
    {
        [SerializedName("gameMode")]
        public string GameMode { get; set; }

        [SerializedName("userId")]
        public double UserId { get; set; }

        [SerializedName("gameModeString")]
        public string GameModeString { get; set; }
    }
}