using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.SummonerDefaultSpells")]
    public class SummonerDefaultSpells : IRiotRtmpObject
    {
        [SerializedName("summonerDefaultSpellsJson")]
        public object SummonerDefaultSpellsJson { get; set; }

        [SerializedName("summonerDefaultSpellMap")]
        public object SummonerDefaultSpellMap { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}