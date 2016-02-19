using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team
{
    [Serializable]
    [SerializedName("com.riotgames.team.TeamId")]
    public class TeamId : IRiotRtmpObject
    {
        [SerializedName("fullId")]
        public string FullId { get; set; }
    }
}