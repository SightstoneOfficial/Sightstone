using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.AggregatedStats")]
    public class AggregatedStats : IRiotRtmpObject
    {
        [SerializedName("lifetimeStatistics")]
        public List<AggregatedStat> LifetimeStatistics { get; set; }

        [SerializedName("modifyDate")]
        public object ModifyDate { get; set; }

        [SerializedName("key")]
        public AggregatedStatsKey Key { get; set; }

        [SerializedName("aggregatedStatsJson")]
        public string AggregatedStatsJson { get; set; }
    }
}