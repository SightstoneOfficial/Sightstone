using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.ChampionBanInfoDTO")]
    public class ChampionBanInfoDTO : IRiotRtmpObject
    {
        [SerializedName("enemyOwned")]
        public bool EnemyOwned { get; set; }

        [SerializedName("championId")]
        public int ChampionId { get; set; }

        [SerializedName("owned")]
        public bool Owned { get; set; }
    }
}