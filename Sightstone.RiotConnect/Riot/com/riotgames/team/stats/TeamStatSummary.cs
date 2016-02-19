using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team.stats
{
    [Serializable]
    [SerializedName("com.riotgames.team.stats.TeamStatSummary")]
    public class TeamStatSummary : IRiotRtmpObject
    {
        [SerializedName("teamStatDetails")]
        public List<TeamStatDetail> TeamStatDetails { get; set; }

        [SerializedName("teamIdString")]
        public string TeamIdString { get; set; }

        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
    }
}