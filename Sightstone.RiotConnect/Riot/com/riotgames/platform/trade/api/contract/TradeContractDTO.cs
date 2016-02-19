using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.trade.api.contract
{
    [Serializable]
    [SerializedName("com.riotgames.platform.trade.api.contract.TradeContractDTO")]
    public class TradeContractDTO : IRiotRtmpObject
    {
        [SerializedName("requesterInternalSummonerName")]
        public string RequesterInternalSummonerName { get; set; }

        [SerializedName("requesterChampionId")]
        public double RequesterChampionId { get; set; }

        [SerializedName("state")]
        public string State { get; set; }

        [SerializedName("responderChampionId")]
        public double ResponderChampionId { get; set; }

        [SerializedName("responderInternalSummonerName")]
        public string ResponderInternalSummonerName { get; set; }
    }
}