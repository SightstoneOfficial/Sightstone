using System;
using System.Collections.Generic;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.team.stats;

namespace Sightstone.RiotConnect.Riot.com.riotgames.team.dto
{
    [Serializable]
    [SerializedName("com.riotgames.team.dto.TeamDTO")]
    public class TeamDTO : IRiotRtmpObject
    {
        [SerializedName("teamStatSummary")]
        public TeamStatSummary TeamStatSummary { get; set; }

        [SerializedName("status")]
        public string Status { get; set; }

        [SerializedName("tag")]
        public string Tag { get; set; }

        [SerializedName("roster")]
        public RosterDTO Roster { get; set; }

        [SerializedName("lastGameDate")]
        public object LastGameDate { get; set; }

        [SerializedName("modifyDate")]
        public DateTime ModifyDate { get; set; }

        [SerializedName("messageOfDay")]
        public object MessageOfDay { get; set; }

        [SerializedName("teamId")]
        public TeamId TeamId { get; set; }

        [SerializedName("lastJoinDate")]
        public DateTime LastJoinDate { get; set; }

        [SerializedName("secondLastJoinDate")]
        public DateTime SecondLastJoinDate { get; set; }

        [SerializedName("secondsUntilEligibleForDeletion")]
        public double SecondsUntilEligibleForDeletion { get; set; }

        [SerializedName("matchHistory")]
        public List<object> MatchHistory { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("thirdLastJoinDate")]
        public DateTime ThirdLastJoinDate { get; set; }

        [SerializedName("createDate")]
        public DateTime CreateDate { get; set; }
    }
}