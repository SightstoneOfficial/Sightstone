using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics.team
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO")]
    public class TeamPlayerAggregatedStatsDTO : IRiotRtmpObject
    {
        [SerializedName("playerId")]
        public double PlayerId { get; set; }

        [SerializedName("aggregatedStats")]
        public AggregatedStats AggregatedStats { get; set; }
    }
}