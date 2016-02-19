using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.kudos.dto
{
    [Serializable]
    [SerializedName("com.riotgames.kudos.dto.PendingKudosDTO")]
    public class PendingKudosDTO : IRiotRtmpObject
    {
        [SerializedName("pendingCounts")]
        public int[] PendingCounts { get; set; }
    }
}