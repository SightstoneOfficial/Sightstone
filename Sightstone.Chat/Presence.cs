using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;


namespace Sightstone.Chat
{
    public class Presence
    {
        public PresenceType PresenceType;

        public string Resource;

        public object State;

        [JsonIgnore]
        public string RawStatus;

        public Presence()
        {
        }

        public void ParseState()
        {
            try
            {
                this.ParseStateInternal(this.RawStatus);
            }
            catch
            {
            }
        }

        private void ParseStateInternal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                this.State = null;
                return;
            }
            if (str[0] == '{')
            {
                this.State = ChatStatic.ParseJsonState(JObject.Parse(str));
                return;
            }
            if (str[0] != '<' || str[str.Length - 1] != '>')
            {
                this.State = ChatStatic.ParseStringState(str);
                return;
            }
            this.State = ChatStatic.ParseXmlState(XDocument.Parse(str));
        }
    }
}