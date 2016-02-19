using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team.dto
{
    [Serializable]
    [SerializedName("com.riotgames.team.dto.TeamMemberInfoDTO")]
    public class TeamMemberInfoDTO : IRiotRtmpObject
    {
        [SerializedName("joinDate")]
        public DateTime JoinDate { get; set; }

        [SerializedName("playerName")]
        public string PlayerName { get; set; }

        [SerializedName("inviteDate")]
        public DateTime InviteDate { get; set; }

        [SerializedName("status")]
        public string Status { get; set; }

        [SerializedName("playerId")]
        public double PlayerId { get; set; }
    }
}