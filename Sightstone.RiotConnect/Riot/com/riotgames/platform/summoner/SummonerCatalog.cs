using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.SummonerCatalog")]
    public class SummonerCatalog : IRiotRtmpObject
    {
        [SerializedName("items")]
        public object Items { get; set; }

        [SerializedName("talentTree")]
        public List<TalentGroup> TalentTree { get; set; }

        [SerializedName("spellBookConfig")]
        public List<RuneSlot> SpellBookConfig { get; set; }
    }
}