using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.SummaryAggStats")]
    public class SummaryAggStats : IRiotRtmpObject
    {
        [SerializedName("statsJson")]
        public object StatsJson { get; set; }

        [SerializedName("stats")]
        public List<SummaryAggStat> Stats { get; set; }
    }
}