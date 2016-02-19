using System;
using System.Collections.Generic;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.team;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics.team
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO")]
    public class TeamAggregatedStatsDTO : IRiotRtmpObject
    {
        [SerializedName("queueType")]
        public string QueueType { get; set; }

        [SerializedName("serializedToJson")]
        public string SerializedToJson { get; set; }

        [SerializedName("playerAggregatedStatsList")]
        public List<TeamPlayerAggregatedStatsDTO> PlayerAggregatedStatsList { get; set; }

        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
    }
}