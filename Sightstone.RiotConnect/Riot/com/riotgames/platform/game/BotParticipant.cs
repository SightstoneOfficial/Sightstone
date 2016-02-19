using System;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.champion;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.BotParticipant")]
    public class BotParticipant : Participant, IRiotRtmpObject
    {
        [SerializedName("champion")]
        public ChampionDTO Champion { get; set; }

        [SerializedName("botSkillLevel")]
        public int BotSkillLevel { get; set; }

        [SerializedName("teamId")]
        public string teamId { get; set; }

        [SerializedName("botSkillLevelName")]
        public string botSkillLevelName { get; set; }

        [SerializedName("pickMode")]
        public int pickMode { get; set; }

        [SerializedName("isMe")]
        public bool IsMe { get; set; }

        [SerializedName("team")]
        public int Team { get; set; }

        [SerializedName("pickTurn")]
        public int PickTurn { get; set; }

        [SerializedName("badges")]
        public int Badges { get; set; }

        [SerializedName("teamName")]
        public object TeamName { get; set; }

        [SerializedName("isGameOwner")]
        public bool IsGameOwner { get; set; }

        [SerializedName("summonerInternalName")]
        public string SummonerInternalName { get; set; }

        [SerializedName("summonerName")]
        public string SummonerName { get; set; }

        [SerializedName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }
    }
}