using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RtmpSharp.IO;
using RtmpSharp.IO.AMF3;

namespace Sightstone.RiotConnect.Riot.com.riotgames.platform.broadcast
{
    [Serializable]
    [SerializedName("com.riotgames.platform.broadcast.BroadcastNotification")]
    public class BroadcastNotification : IExternalizable, IRiotRtmpObject
    {
        public ArrayList broadcastMessages { get; set; }
        public string Json { get; set; }

        public void ReadExternal(IDataInput input)
        {
            Json = input.ReadUtf((int) input.ReadUInt32());
            
            Dictionary<string, object> deserializedJSON = JsonConvert.DeserializeObject<Dictionary<string, object>>(Json);

            var classType = typeof (BroadcastNotification);
            foreach (var keyPair in deserializedJSON)
            {
                var f = classType.GetProperty(keyPair.Key);
                f.SetValue(this, keyPair.Value);
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