using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.trade.api.contract
{
    [Serializable]
    [SerializedName("com.riotgames.platform.trade.api.contract.PotentialTradersDTO")]
    public class PotentialTradersDTO : IRiotRtmpObject
    {
        [SerializedName("potentialTraders")]
        public List<string> PotentialTraders { get; set; }
    }
}