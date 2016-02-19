using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.catalog.runes
{
    [Serializable]
    [SerializedName("com.riotgames.platform.catalog.runes.RuneType")]
    public class RuneType : IRiotRtmpObject
    {
        [SerializedName("runeTypeId")]
        public int RuneTypeId { get; set; }

        [SerializedName("name")]
        public string Name { get; set; }
    }
}