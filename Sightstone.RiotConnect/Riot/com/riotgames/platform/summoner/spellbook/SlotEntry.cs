using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.summoner.spellbook
{
    [Serializable]
    [SerializedName("com.riotgames.platform.summoner.spellbook.SlotEntry")]
    public class SlotEntry : IRiotRtmpObject
    {
        [SerializedName("runeId")]
        public int RuneId { get; set; }

        [SerializedName("runeSlotId")]
        public int RuneSlotId { get; set; }
    }
}