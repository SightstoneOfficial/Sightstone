using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.masterybook
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.masterybook.MasteryBookDTO")]
    public class MasteryBookDTO : IRiotRtmpObject
    {
        [SerializedName("bookPagesJson")]
        public object BookPagesJson { get; set; }

        [SerializedName("bookPages")]
        public List<MasteryBookPageDTO> BookPages { get; set; }

        [SerializedName("dateString")]
        public string DateString { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}