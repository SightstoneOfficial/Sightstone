using System;
using System.Collections.Generic;
using RtmpSharp.IO;
using Sightstone.RiotConnect.Riot.com.riotgames.leagues.pojo;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.leagues.client.dto
{
    [Serializable]
    [SerializedName("com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO")]
    public class SummonerLeagueItemsDTO : IRiotRtmpObject
    {
        [SerializedName("summonerLeagues")]
        public List<LeagueItemDTO> SummonerLeagues { get; set; }
    }
}