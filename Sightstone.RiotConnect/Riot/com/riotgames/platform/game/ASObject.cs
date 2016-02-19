using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.ASObject")]
    public class ASObject : IRiotRtmpObject
    {
        [SerializedName("LEAVER_BUSTER_ACCESS_TOKEN")]
        public string Token { get; set; }

        [SerializedName("TypeName")]
        public object Tname { get; set; }
    }
}