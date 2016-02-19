using System;
using System.Collections.Generic;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.icon;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.icon
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.icon.SummonerIconInventoryDTO")]
    public class SummonerIconInventoryDTO : IRiotRtmpObject
    {
        [SerializedName("summonerId")]
        public double SummonerId { get; set; }

        [SerializedName("summonerIcons")]
        public List<Icon> SummonerIcons { get; set; }
    }
}