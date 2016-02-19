using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.BannedChampion")]
    public class BannedChampion : IRiotRtmpObject
    {
        [SerializedName("pickTurn")]
        public int PickTurn { get; set; }

        [SerializedName("championId")]
        public int ChampionId { get; set; }

        [SerializedName("teamId")]
        public int TeamId { get; set; }
    }
}