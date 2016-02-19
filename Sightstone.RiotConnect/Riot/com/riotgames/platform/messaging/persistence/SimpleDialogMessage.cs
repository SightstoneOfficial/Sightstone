using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.messaging.persistence
{
    [Serializable]
    [SerializedName("com.riotgames.platform.messaging.persistence.SimpleDialogMessage")]
    public class SimpleDialogMessage : IRiotRtmpObject
    {
        [SerializedName("titleCode")]
        public string TitleCode { get; set; }

        [SerializedName("accountId")]
        public double AccountId { get; set; }

        [SerializedName("params")]
        public object Params { get; set; }

        [SerializedName("msgId")]
        public double MessageId { get; set; }

        [SerializedName("type")]
        public string Type { get; set; }

        [SerializedName("bodyCode")]
        public string BodyCode { get; set; }
    }
}