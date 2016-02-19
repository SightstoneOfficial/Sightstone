using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.harassment
{
    [Serializable]
    [SerializedName("com.riotgames.platform.harassment.LcdsResponseString")]
    public class LcdsResponseString : IRiotRtmpObject
    {
        [SerializedName("value")]
        public string Value { get; set; }
    }
}