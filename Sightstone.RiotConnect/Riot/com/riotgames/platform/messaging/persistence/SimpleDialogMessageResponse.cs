using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.messaging.persistence
{
    [Serializable]
    [SerializedName("com.riotgames.platform.messaging.persistence.SimpleDialogMessageResponse")]
    public class SimpleDialogMessageResponse : IRiotRtmpObject
    {
        [SerializedName("command")]
        public string Command { get; set; }

        [SerializedName("accountId")]
        public double AccountId { get; set; }

        [SerializedName("msgId")]
        public double MessageId { get; set; }
    }
}