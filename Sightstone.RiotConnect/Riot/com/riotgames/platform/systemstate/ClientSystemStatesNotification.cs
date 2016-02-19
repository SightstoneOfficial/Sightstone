using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RtmpSharp.IO;
using RtmpSharp.IO.AMF3;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.systemstate
{
    [Serializable]
    [SerializedName("com.riotgames.platform.systemstate.ClientSystemStatesNotification")]
    public class ClientSystemStatesNotification : IExternalizable, IRiotRtmpObject
    {
        public bool championTradeThroughLCDS { get; set; }
        public bool practiceGameEnabled { get; set; }
        public bool advancedTutorialEnabled { get; set; }
        public int[] practiceGameTypeConfigIdList { get; set; }
        public int minNumPlayersForPracticeGame { get; set; }
        public int[] PracticeGameTypeConfigIdList { get; set; }
        public int[] freeToPlayChampionIdList { get; set; }
        public int[] inactiveChampionIdList { get; set; }
        public int[] inactiveSpellIdList { get; set; }
        public int[] inactiveTutorialSpellIdList { get; set; }
        public int[] inactiveClassicSpellIdList { get; set; }
        public int[] inactiveOdinSpellIdList { get; set; }
        public int[] inactiveAramSpellIdList { get; set; }
        public int[] enabledQueueIdsList { get; set; }
        public int[] unobtainableChampionSkinIDList { get; set; }
        public int[] freeToPlayChampionForNewPlayersIdList { get; set; }
        public Dictionary<string, object> gameModeToInactiveSpellIds { get; set; }
        public bool archivedStatsEnabled { get; set; }
        public Dictionary<string, object> queueThrottleDTO { get; set; }
        public Dictionary<string, object>[] gameMapEnabledDTOList { get; set; }
        public bool storeCustomerEnabled { get; set; }
        public bool socialIntegrationEnabled { get; set; }
        public bool runeUniquePerSpellBook { get; set; }
        public bool tribunalEnabled { get; set; }
        public bool observerModeEnabled { get; set; }
        public int currentSeason { get; set; }
        public int freeToPlayChampionsForNewPlayersMaxLevel { get; set; }
        public int spectatorSlotLimit { get; set; }
        public int clientHeartBeatRateSeconds { get; set; }
        public string[] observableGameModes { get; set; }
        public string observableCustomGameModes { get; set; }
        public bool teamServiceEnabled { get; set; }
        public bool leagueServiceEnabled { get; set; }
        public bool modularGameModeEnabled { get; set; }
        public decimal riotDataServiceDataSendProbability { get; set; }
        public bool displayPromoGamesPlayedEnabled { get; set; }
        public bool masteryPageOnServer { get; set; }
        public int maxMasteryPagesOnServer { get; set; }
        public bool tournamentSendStatsEnabled { get; set; }
        public string replayServiceAddress { get; set; }
        public bool kudosEnabled { get; set; }
        public bool buddyNotesEnabled { get; set; }
        public bool localeSpecificChatRoomsEnabled { get; set; }
        public Dictionary<string, object> replaySystemStates { get; set; }
        public bool sendFeedbackEventsEnabled { get; set; }
        public string[] knownGeographicGameServerRegions { get; set; }
        public bool leaguesDecayMessagingEnabled { get; set; }
        public bool tournamentShortCodesEnabled { get; set; }
        public string Json { get; set; }

        public void ReadExternal(IDataInput input)
        {
            Json = input.ReadUtf((int) input.ReadUInt32());
            
            Dictionary<string, object> deserializedJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(Json);

            var classType = typeof (ClientSystemStatesNotification);
            foreach (var keyPair in deserializedJSON)
            {
                var f = classType.GetProperty(keyPair.Key);
                if (f == null)
                {
                    //Client.Log("New Client System State: " + keyPair.Key);
                    continue;
                }
                if (keyPair.Value.GetType() == typeof (ArrayList))
                {
                    var tempArrayList = keyPair.Value as ArrayList;
                    if (tempArrayList != null && tempArrayList.Count > 0)
                        f.SetValue(this, tempArrayList.ToArray(f.PropertyType.GetElementType()));
                    else
                        f.SetValue(this, null);
                }
                else
                {
                    f.SetValue(this, keyPair.Value);
                }
            }
        }

        public void WriteExternal(IDataOutput output)
        {
            var bytes = Encoding.UTF8.GetBytes(Json);

            output.WriteInt32(bytes.Length);
            output.WriteBytes(bytes);
        }
    }
}