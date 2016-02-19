using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.matchmaking
{
    [Serializable]
    [SerializedName("com.riotgames.platform.matchmaking.MatchMakerParams")]
    public class MatchMakerParams : IRiotRtmpObject
    {
        [SerializedName("lastMaestroMessage")]
        public object LastMaestroMessage { get; set; }

        [SerializedName("teamId")]
        public object TeamId { get; set; }

        [SerializedName("languages")]
        public object Languages { get; set; }

        [SerializedName("botDifficulty")]
        public string BotDifficulty { get; set; }

        [SerializedName("team")]
        public List<int> Team { get; set; }

        [SerializedName("queueIds")]
        public int[] QueueIds { get; set; }

        [SerializedName("invitationId")]
        public object InvitationId { get; set; }
    }
}