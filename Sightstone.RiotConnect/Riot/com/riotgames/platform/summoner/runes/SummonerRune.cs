using System;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.runes;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.runes
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.runes.SummonerRune")]
    public class SummonerRune : IRiotRtmpObject
    {
        [SerializedName("purchased")]
        public DateTime Purchased { get; set; }

        [SerializedName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [SerializedName("runeId")]
        public int RuneId { get; set; }

        [SerializedName("quantity")]
        public int Quantity { get; set; }

        [SerializedName("rune")]
        public Rune Rune { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}