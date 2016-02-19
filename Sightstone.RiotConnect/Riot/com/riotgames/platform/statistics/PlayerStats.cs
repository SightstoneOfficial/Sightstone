using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.PlayerStats")]
    public class PlayerStats : IRiotRtmpObject
    {
        [SerializedName("timeTrackedStats")]
        public List<TimeTrackedStat> TimeTrackedStats { get; set; }

        [SerializedName("promoGamesPlayed")]
        public int PromoGamesPlayed { get; set; }

        [SerializedName("promoGamesPlayedLastUpdated")]
        public object PromoGamesPlayedLastUpdated { get; set; }
    }
}