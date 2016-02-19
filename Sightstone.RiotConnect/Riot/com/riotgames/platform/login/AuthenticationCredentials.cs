using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.login
{
    [Serializable]
    [SerializedName("com.riotgames.platform.login.AuthenticationCredentials")]
    public class AuthenticationCredentials : IRiotRtmpObject
    {
        [SerializedName("oldPassword")]
        public object OldPassword { get; set; }

        [SerializedName("username")]
        public string Username { get; set; }

        [SerializedName("secUrityAnswer")]
        public object SecUrityAnswer { get; set; }

        [SerializedName("password")]
        public string Password { get; set; }

        [SerializedName("partnerCredentials")]
        public object PartnerCredentials { get; set; }

        [SerializedName("domain")]
        public string Domain { get; set; }

        [SerializedName("ipAddress")]
        public string IpAddress { get; set; }

        [SerializedName("clientVersion")]
        public string ClientVersion { get; set; }

        [SerializedName("locale")]
        public string Locale { get; set; }

        [SerializedName("authToken")]
        public string AuthToken { get; set; }

        [SerializedName("operatingSystem")]
        public string OperatingSystem { get; set; }
    }
}