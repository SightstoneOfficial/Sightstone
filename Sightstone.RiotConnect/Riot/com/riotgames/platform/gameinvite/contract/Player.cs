using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.gameinvite.contract
{
    [Serializable]
    [SerializedName("com.riotgames.platform.gameinvite.contract.Player")]
    public class Player : IRiotRtmpObject
    {
        [SerializedName("summonerName")]
        public string SummonerName { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}