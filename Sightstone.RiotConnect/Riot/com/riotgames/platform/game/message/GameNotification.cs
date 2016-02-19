using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game.message
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.message.GameNotification")]
    public class GameNotification : IRiotRtmpObject
    {
        [SerializedName("messageCode")]
        public string MessageCode { get; set; }

        [SerializedName("type")]
        public string Type { get; set; }

        [SerializedName("messageArgument")]
        public object MessageArgument { get; set; }
    }
}