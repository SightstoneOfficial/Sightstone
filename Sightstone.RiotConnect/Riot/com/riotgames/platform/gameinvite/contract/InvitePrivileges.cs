using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.gameinvite.contract
{
    [Serializable]
    [SerializedName("com.riotgames.platform.gameinvite.contract.InvitePrivileges")]
    public class InvitePrivileges : IRiotRtmpObject
    {
        [SerializedName("canInvite")]
        public bool canInvite { get; set; }
    }
}