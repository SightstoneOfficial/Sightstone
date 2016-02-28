using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Sightstone.Chat
{
    public class Contact
    {
        public string Id;

        public string Jid;

        public string Name;

        public string[] Groups;

        public bool ConferenceUser;

        internal ConcurrentDictionary<string, Presence> InternalPresences;

        public string BareJid => (new JabberId(Jid)).Bare;

        public Presence[] Presences => InternalPresences.Values.ToArray();

        public Contact()
        {
            Groups = new string[0];
            InternalPresences = new ConcurrentDictionary<string, Presence>(StringComparer.OrdinalIgnoreCase);
        }
    }
}