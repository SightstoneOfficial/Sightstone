using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.leagues.pojo
{
    [Serializable]
    [SerializedName("com.riotgames.leagues.pojo.LeagueListDTO")]
    public class LeagueListDTO : IRiotRtmpObject
    {
        [SerializedName("queue")]
        public string Queue { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("tier")]
        public string Tier { get; set; }

        [SerializedName("requestorsRank")]
        public string RequestorsRank { get; set; }

        [SerializedName("entries")]
        public List<LeagueItemDTO> Entries { get; set; }

        [SerializedName("requestorsName")]
        public string RequestorsName { get; set; }
    }
}