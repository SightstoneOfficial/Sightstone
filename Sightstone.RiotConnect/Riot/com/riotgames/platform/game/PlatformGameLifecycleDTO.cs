using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.PlatformGameLifecycleDTO")]
    public class PlatformGameLifecycleDTO : IRiotRtmpObject
    {
        [SerializedName("gameSpecificLoyaltyRewards")]
        public object GameSpecificLoyaltyRewards { get; set; }

        [SerializedName("reconnectDelay")]
        public int ReconnectDelay { get; set; }

        [SerializedName("lastModifiedDate")]
        public object LastModifiedDate { get; set; }

        [SerializedName("game")]
        public GameDTO Game { get; set; }

        [SerializedName("playerCredentials")]
        public PlayerCredentialsDto PlayerCredentials { get; set; }

        [SerializedName("gameName")]
        public string GameName { get; set; }

        [SerializedName("connectivityStateEnum")]
        public object ConnectivityStateEnum { get; set; }
    }
}