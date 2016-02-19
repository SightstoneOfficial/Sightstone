using System;
using System.Collections.Generic;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.StartChampSelectDTO")]
    public class StartChampSelectDTO : IRiotRtmpObject
    {
        [SerializedName("invalidPlayers")]
        public List<object> InvalidPlayers { get; set; }
    }
}