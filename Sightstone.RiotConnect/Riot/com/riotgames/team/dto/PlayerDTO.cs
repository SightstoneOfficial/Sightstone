using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team.dto
{
    [Serializable]
    [SerializedName("com.riotgames.team.dto.PlayerDTO")]
    public class PlayerDTO : IRiotRtmpObject
    {
        [SerializedName("playerId")]
        public double PlayerId { get; set; }

        [SerializedName("teamsSummary")]
        public List<object> TeamsSummary { get; set; }

        [SerializedName("createdTeams")]
        public List<object> CreatedTeams { get; set; }

        [SerializedName("playerTeams")]
        public List<object> PlayerTeams { get; set; }
    }
}