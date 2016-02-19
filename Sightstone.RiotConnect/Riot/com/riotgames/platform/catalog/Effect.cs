using System;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.runes;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog
{
    [Serializable]
    [SerializedName("com.riotgames.platform.catalog.Effect")]
    public class Effect : IRiotRtmpObject
    {
        [SerializedName("effectId")]
        public int EffectId { get; set; }

        [SerializedName("gameCode")]
        public string GameCode { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }

        [SerializedName("categoryId")]
        public object CategoryId { get; set; }

        [SerializedName("runeType")]
        public RuneType RuneType { get; set; }
    }
}