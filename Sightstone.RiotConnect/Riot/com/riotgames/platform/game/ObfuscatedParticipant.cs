using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.ObfuscatedParticipant")]
    public class ObfuscatedParticipant : Participant, IRiotRtmpObject
    {
        [SerializedName("badges")]
        public int Badges { get; set; }

        [SerializedName("index")]
        public int Index { get; set; }

        [SerializedName("clientInSynch")]
        public bool ClientInSynch { get; set; }

        [SerializedName("gameUniqueId")]
        public int GameUniqueId { get; set; }

        [SerializedName("pickMode")]
        public int PickMode { get; set; }
    }
}