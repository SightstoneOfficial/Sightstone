using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.TalentRow")]
    public class TalentRow : IRiotRtmpObject
    {
        [SerializedName("index")]
        public int Index { get; set; }

        [SerializedName("talents")]
        public List<Talent> Talents { get; set; }

        [SerializedName("tltGroupId")]
        public int TltGroupId { get; set; }

        [SerializedName("pointsToActivate")]
        public int PointsToActivate { get; set; }

        [SerializedName("tltRowId")]
        public int TltRowId { get; set; }
    }
}