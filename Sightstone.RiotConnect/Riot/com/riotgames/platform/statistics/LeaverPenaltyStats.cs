using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.statistics
{
    [Serializable]
    [SerializedName("com.riotgames.platform.statistics.LeaverPenaltyStats")]
    public class LeaverPenaltyStats : IRiotRtmpObject
    {
        [SerializedName("lastLevelIncrease")]
        public object LastLevelIncrease { get; set; }

        [SerializedName("level")]
        public int Level { get; set; }

        [SerializedName("lastDecay")]
        public DateTime LastDecay { get; set; }

        [SerializedName("userInformed")]
        public bool UserInformed { get; set; }

        [SerializedName("points")]
        public int Points { get; set; }
    }
}