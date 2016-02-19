using System;
using RtmpSharp.IO;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.game
{
    [Serializable]
    [SerializedName("com.riotgames.platform.game.PlayerCredentialsDto")]
    public class PlayerCredentialsDto : IRiotRtmpObject
    {
        [SerializedName("encryptionKey")]
        public string EncryptionKey { get; set; }

        [SerializedName("gameId")]
        public double GameId { get; set; }

        [SerializedName("lastSelectedSkinIndex")]
        public int LastSelectedSkinIndex { get; set; }

        [SerializedName("serverIp")]
        public string ServerIp { get; set; }

        [SerializedName("observer")]
        public bool Observer { get; set; }

        [SerializedName("summonerId")]
        public double SummonerId { get; set; }

        [SerializedName("observerServerIp")]
        public string ObserverServerIp { get; set; }

        [SerializedName("handshakeToken")]
        public string HandshakeToken { get; set; }

        [SerializedName("playerId")]
        public double PlayerId { get; set; }

        [SerializedName("serverPort")]
        public int ServerPort { get; set; }

        [SerializedName("observerServerPort")]
        public int ObserverServerPort { get; set; }

        [SerializedName("summonerName")]
        public string SummonerName { get; set; }

        [SerializedName("observerEncryptionKey")]
        public string ObserverEncryptionKey { get; set; }

        [SerializedName("championId")]
        public int ChampionId { get; set; }
    }
}