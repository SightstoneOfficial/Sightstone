using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.spellbook
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.spellbook.SpellBookPageDTO")]
    public class SpellBookPageDTO : IRiotRtmpObject
    {
        [SerializedName("slotEntries")]
        public List<SlotEntry> SlotEntries { get; set; }

        [SerializedName("summonerId")]
        public int SummonerId { get; set; }

        [SerializedName("createDate")]
        public DateTime CreateDate { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("pageId")]
        public int PageId { get; set; }

        [SerializedName("current")]
        public bool Current { get; set; }
    }
}