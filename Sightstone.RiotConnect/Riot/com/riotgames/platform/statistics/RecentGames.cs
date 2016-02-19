using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.RecentGames")]
    public class RecentGames : IRiotRtmpObject
    {
        [SerializedName("recentGamesJson")]
        public object RecentGamesJson { get; set; }

        [SerializedName("playerGameStatsMap")]
        public object PlayerGameStatsMap { get; set; }

        [SerializedName("gameStatistics")]
        public List<PlayerGameStats> GameStatistics { get; set; }

        [SerializedName("userId")]
        public double UserId { get; set; }
    }
}