using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.icon
{
    [Serializable]
    [SerializedName("com.riotgames.platform.catalog.icon.Icon")]
    public class Icon : IRiotRtmpObject
    {
        [SerializedName("purchaseDate")]
        public DateTime PurchaseDate { get; set; }

        [SerializedName("iconId")]
        public double IconId { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }
    }
}