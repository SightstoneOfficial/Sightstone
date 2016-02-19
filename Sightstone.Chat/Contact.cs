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

        public string BareJid
        {
            get
            {
                return (new JabberId(this.Jid)).Bare;
            }
            set
            {
            }
        }

        public Presence[] Presences
        {
            get
            {
                return this.InternalPresences.Values.ToArray<Presence>();
            }
        }

        public Contact()
        {
            this.Groups = new string[0];
            this.InternalPresences = new ConcurrentDictionary<string, Presence>(StringComparer.OrdinalIgnoreCase);
        }
    }
}