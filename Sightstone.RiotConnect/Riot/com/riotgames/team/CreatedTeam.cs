using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team
{
    [Serializable]
    [SerializedName("com.riotgames.team.CreatedTeam")]
    public class CreatedTeam : IRiotRtmpObject
    {
        [SerializedName("timeStamp")]
        public double TimeStamp { get; set; }

        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }
    }
}