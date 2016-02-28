using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public void ParseState()
        {
            try
            {
                ParseStateInternal(RawStatus);
            }
            catch
            {
                // ignored
            }
        }

        private void ParseStateInternal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                State = null;
                return;
            }
            if (str[0] == '{')
            {
                State = ChatStatic.ParseJsonState(JObject.Parse(str));
                return;
            }
            if (str[0] != '<' || str[str.Length - 1] != '>')
            {
                State = ChatStatic.ParseStringState(str);
                return;
            }
            State = ChatStatic.ParseXmlState(XDocument.Parse(str));
        }
    }
}