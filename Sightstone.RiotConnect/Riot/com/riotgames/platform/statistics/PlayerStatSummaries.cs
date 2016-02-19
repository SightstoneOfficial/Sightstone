using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.PlayerStatSummaries")]
    public class PlayerStatSummaries : IRiotRtmpObject
    {
        [SerializedName("playerStatSummarySet")]
        public List<PlayerStatSummary> PlayerStatSummarySet { get; set; }

        [SerializedName("userId")]
        public double UserId { get; set; }
    }
}