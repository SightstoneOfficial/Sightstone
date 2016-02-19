using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.spellbook
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.spellbook.SpellBookDTO")]
    public class SpellBookDTO
    {
        [SerializedName("bookPagesJson")]
        public object BookPagesJson { get; set; }

        [SerializedName("bookPages")]
        public List<SpellBookPageDTO> BookPages { get; set; }

        [SerializedName("dateString")]
        public string DateString { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }

        [SerializedName("defaultPage")]
        public SpellBookPageDTO DefaultPage { get; set; }
    }
}