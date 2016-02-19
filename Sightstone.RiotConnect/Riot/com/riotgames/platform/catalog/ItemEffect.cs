using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog
{
    [Serializable]
    [SerializedName("com.riotgames.platform.catalog.ItemEffect")]
    public class ItemEffect : IRiotRtmpObject
    {
        [SerializedName("effectId")]
        public int EffectId { get; set; }

        [SerializedName("itemEffectId")]
        public int ItemEffectId { get; set; }

        [SerializedName("effect")]
        public Effect Effect { get; set; }

        [SerializedName("value")]
        public string Value { get; set; }

        [SerializedName("itemId")]
        public int ItemId { get; set; }
    }
}