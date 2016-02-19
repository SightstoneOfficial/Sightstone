using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.masterybook
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO")]
    public class MasteryBookPageDTO : IRiotRtmpObject
    {
        [SerializedName("talentEntries")]
        public List<TalentEntry> TalentEntries { get; set; }

        [SerializedName("pageId")]
        public double PageId { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("current")]
        public bool Current { get; set; }

        [SerializedName("createDate")]
        public object CreateDate { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}