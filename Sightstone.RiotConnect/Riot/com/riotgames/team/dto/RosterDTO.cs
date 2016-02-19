using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team.dto
{
    [Serializable]
    [SerializedName("com.riotgames.team.dto.RosterDTO")]
    public class RosterDTO : IRiotRtmpObject
    {
        [SerializedName("ownerId")]
        public double OwnerId { get; set; }

        [SerializedName("memberList")]
        public List<TeamMemberInfoDTO> MemberList { get; set; }
    }
}