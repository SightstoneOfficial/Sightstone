using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.matchmaking
{
    [Serializable]
    [SerializedName("com.riotgames.platform.matchmaking.MatchingThrottleConfig")]
    public class MatchingThrottleConfig : IRiotRtmpObject
    {
        [SerializedName("limit")]
        public double Limit { get; set; }

        [SerializedName("matchingThrottleProperties")]
        public List<object> MatchingThrottleProperties { get; set; }

        [SerializedName("cacheName")]
        public string CacheName { get; set; }
    }
}