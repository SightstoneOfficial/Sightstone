using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.reroll.pojo
{
    [Serializable]
    [SerializedName("com.riotgames.platform.reroll.pojo.SimpleDialogMessage")]
    public class SimpleDialogMessage : IRiotRtmpObject
    {
        [SerializedName("titleCode")]
        public string TitleCode { get; set; }

        [SerializedName("accountId")]
        public double AccountId { get; set; }

        [SerializedName("params")]
        public object Params { get; set; }

        [SerializedName("type")]
        public string Type { get; set; }
    }
}