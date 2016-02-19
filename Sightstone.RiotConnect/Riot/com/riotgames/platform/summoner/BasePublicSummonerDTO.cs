using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.BasePublicSummonerDTO")]
    public class BasePublicSummonerDTO : IRiotRtmpObject
    {
        [SerializedName("seasonTwoTier")]
        public string SeasonTwoTier { get; set; }

        [SerializedName("publicName")]
        public string InternalName { get; set; }

        [SerializedName("seasonOneTier")]
        public string SeasonOneTier { get; set; }

        [SerializedName("acctId")]
        public double AcctId { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("sumId")]
        public double SumId { get; set; }

        [SerializedName("profileIconId")]
        public int ProfileIconId { get; set; }
    }
}