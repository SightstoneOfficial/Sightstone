using System;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.platform.account;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.login
{
    [Serializable]
    [SerializedName("com.riotgames.platform.login.Session")]
    public class Session : IRiotRtmpObject
    {
        [SerializedName("token")]
        public string Token { get; set; }

        [SerializedName("password")]
        public string Password { get; set; }

        [SerializedName("accountSummary")]
        public AccountSummary AccountSummary { get; set; }
    }
}