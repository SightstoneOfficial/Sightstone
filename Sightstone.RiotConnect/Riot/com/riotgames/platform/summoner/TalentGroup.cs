using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.TalentGroup")]
    public class TalentGroup : IRiotRtmpObject
    {
        [SerializedName("index")]
        public int Index { get; set; }

        [SerializedName("talentRows")]
        public List<TalentRow> TalentRows { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("tltGroupId")]
        public int TltGroupId { get; set; }
    }
}