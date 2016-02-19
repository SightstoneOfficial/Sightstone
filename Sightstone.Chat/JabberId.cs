using agsXMPP;
using System.Collections;

namespace Sightstone.Chat
{
    public class JabberId
    {
        private readonly Jid _jid;

        public string Bare => _jid.Bare;

        public string Resource
        {
            get
            {
                return _jid.Resource;
            }
            set
            {
                _jid.Resource = value;
            }
        }

        public string Server
        {
            get
            {
                return _jid.Server;
            }
            set
            {
                _jid.Server = value;
            }
        }

        public string User
        {
            get
            {
                return _jid.User;
            }
            set
            {
                _jid.User = value;
            }
        }

        public JabberId(string jid)
        {
            _jid = new Jid(jid);
        }

        public JabberId(string user, string server, string resource)
        {
            _jid = new Jid(user, server, resource);
        }

        public int CompareTo(object obj)
        {
            return _jid.CompareTo(obj);
        }

        public bool Equals(object other, IComparer comparer)
        {
            return _jid.Equals(other, comparer);
        }

        public bool Equals(Jid other)
        {
            return _jid.Equals(other);
        }

        public bool Parse(string fullJid)
        {
            return _jid.Parse(fullJid);
        }
    }
}