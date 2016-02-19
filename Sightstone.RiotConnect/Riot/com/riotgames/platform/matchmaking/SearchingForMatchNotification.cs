using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.matchmaking
{
    [Serializable]
    [SerializedName("com.riotgames.platform.matchmaking.SearchingForMatchNotification")]
    public class SearchingForMatchNotification : IRiotRtmpObject
    {
        [SerializedName("playerJoinFailures")]
        public List<QueueDodger> PlayerJoinFailures { get; set; }

        [SerializedName("ghostGameSummoners")]
        public object GhostGameSummoners { get; set; }

        [SerializedName("joinedQueues")]
        public List<QueueInfo> JoinedQueues { get; set; }
    }
}