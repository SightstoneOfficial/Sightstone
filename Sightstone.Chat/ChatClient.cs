using System;
using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.x.muc;
using agsXMPP.Xml.Dom;
using RosterItem = agsXMPP.protocol.iq.roster.RosterItem;

namespace Sightstone.Chat
{
    public class ChatClient
    {
        private const string CDataStart = "<![CDATA[";
        private const string CDataEnd = "]]>";
        private static readonly int CDataStartLength;
        private static readonly int CDataTotalLength;
        public readonly Chat SightstoneChat;
        private readonly XmppClientConnection _connection;
        public readonly Contacts SightstoneContacts;
        public readonly Muc SightstoneMuc;
        public readonly Presence SightstonePresence;
        private readonly Dictionary<string, Contact> _roster;
        public string Host;
        public string Password;
        public int Port;
        public string Server;
        public string Username;

        static ChatClient()
        {
            CDataStartLength = CDataStart.Length;
            var cDataEndLength = CDataEnd.Length;
            CDataTotalLength = CDataStartLength + cDataEndLength;
        }

        public ChatClient()
        {
            _roster = new Dictionary<string, Contact>();
            var xmppClientConnection = new XmppClientConnection(0)
            {
                AutoAgents = false,
                AutoPresence = true,
                AutoResolveConnectServer = false,
                AutoRoster = true,
                KeepAlive = true,
                Priority = 50,
                Resource = "xiff",
                UseSSL = true
            };
            _connection = xmppClientConnection;
            _connection.OnRosterStart += ConnectiOnRosterStart;
            _connection.OnRosterEnd += ConnectiOnRosterEnd;
            _connection.OnRosterItem += ConnectiOnRosterItem;
            _connection.OnPresence += ConnectiOnPresence;
            _connection.OnMessage += ConnectiOnMessage;
            _connection.OnLogin += ConnectiOnLogin;
            _connection.OnClose += ConnectiOnClose;
            _connection.OnAuthError += ConnectiOnAuthError;
            _connection.OnError += ConnectiOnError;
            _connection.OnXmppConnectionStateChanged += ConnectiOnXmppConnectionStateChanged;
            SightstoneChat = new Chat(_connection);
            SightstoneMuc = new Muc(_connection);
            SightstoneContacts = new Contacts(_connection);
            SightstonePresence = new Presence(_connection);
            ConferenceServers = new List<string>();
        }

        public List<string> ConferenceServers { get; }

        public List<Contact> Roster => _roster.Values.ToList();

        public void Close()
        {
            _connection.Close();
        }

        public void Connect()
        {
            if (_connection == null || GetChatClientState(_connection.XmppConnectionState) == ChatClientState.Disconnected)
            {
                return;
            }
            _connection.ConnectServer = Host;
            _connection.Port = Port;
            _connection.Username = Username;
            _connection.Server = Server;
            _connection.Password = Password;
            _connection.Open();
        }

        private void ConnectiOnAuthError(object sender, Element element)
        {
            OnDisconnected();
        }

        private void ConnectiOnClose(object sender)
        {
            OnDisconnected();
        }

        private void ConnectiOnError(object sender, Exception exception)
        {
            UnhandledException?.Invoke(sender, exception);
        }

        private void ConnectiOnLogin(object sender)
        {
        }

        private void ConnectiOnMessage(object _, Message msg)
        {
            var body = msg.Body;
            var from = msg.From;
            var id = msg.Id;
            var subject = msg.Subject;
            var type = msg.Type;
            var dateTime = msg.XDelay?.Stamp.ToUniversalTime() ?? DateTime.UtcNow;
            if (body.StartsWith(CDataStart) && body.EndsWith(CDataEnd))
            {
                body = body.Substring(CDataStartLength, body.Length - CDataTotalLength);
            }
            switch ((int) type)
            {
                case -1:
                {
                    OnMailReceived(from, id, subject, body, dateTime);
                    return;
                }
                case 0:
                {
                    OnError(msg.From, msg.Error);
                    return;
                }
                case 1:
                case 2:
                {
                    OnMessagedReceived(from, id, subject, body, dateTime);
                    return;
                }
                case 3:
                {
                    return;
                }
                default:
                {
                    return;
                }
            }
        }

        private void ConnectiOnPresence(object sender, agsXMPP.protocol.client.Presence agsPresence)
        {
            Contact contact;
            var from = agsPresence.From;
            var contactId = GetContactId(from);
            var flag = _roster.TryGetValue(contactId, out contact);
            var type = agsPresence.Type;
            if ((int) type != -1)
            {
                switch ((int) type)
                {
                    case 4:
                    {
                        break;
                    }
                    case 5:
                    {
                        return;
                    }
                    case 6:
                    {
                        OnError(agsPresence.From, agsPresence.Error);
                        return;
                    }
                    default:
                    {
                        return;
                    }
                }
            }
            if (!flag && (agsPresence.MucUser == null || !IsConferenceServer(agsPresence.From.Server))) return;
            contact = contact ?? ConstructUnknownContact(@from);
            var internalPresences = contact.InternalPresences;
            if ((int) agsPresence.Type != -1)
            {
                Sightstone.Chat.Presence presence;
                internalPresences.TryRemove(agsPresence.From, out presence);
            }
            else
            {
                var orAdd = internalPresences.GetOrAdd(agsPresence.From, new Sightstone.Chat.Presence());
                orAdd.Resource = agsPresence.From.Resource;
                orAdd.PresenceType = GetPresenceType(agsPresence.Show);
                orAdd.RawStatus = agsPresence.Status;
                orAdd.ParseState();
            }
            if (!flag)
            {
                OnPresenceChanged(contact);
                return;
            }
            OnContactChanged(contact, ContactChangeType.Update);
        }

        private void ConnectiOnRosterEnd(object sender)
        {
            OnRosterReceived(_roster.Values.ToArray());
        }

        private void ConnectiOnRosterItem(object sender, RosterItem item)
        {
            var contact = new Contact
            {
                Id = GetContactId(item.Jid),
                Jid = item.Jid,
                Name = item.Name,
                Groups = (
                    from g in item.GetGroups().OfType<Group>()
                    select g.Name).ToArray(),
                ConferenceUser = false
            };
            var contact1 = contact;
            _roster[contact1.Id] = contact1;
            OnContactChanged(contact1, ContactChangeType.Add);
        }

        private void ConnectiOnRosterStart(object sender)
        {
            _roster.Clear();
        }

        private void ConnectiOnXmppConnectionStateChanged(object sender, XmppConnectionState state)
        {
            if (GetChatClientState(state) == ChatClientState.Disconnected)
            {
                OnDisconnected();
            }
        }

        private Contact ConstructUnknownContact(Jid jid)
        {
            string bare;
            string contactId;
            var flag = IsConferenceServer(jid);
            var contact = new Contact();
            var contact1 = contact;
            if (flag)
            {
                bare = jid.Bare;
            }
            else
            {
                bare = jid;
            }
            contact1.Jid = bare;
            var contact2 = contact;
            if (flag)
            {
                contactId = jid;
            }
            else
            {
                contactId = GetContactId(jid);
            }
            contact2.Id = contactId;
            contact.Name = (flag ? jid.Resource : jid.User);
            contact.ConferenceUser = flag;
            return contact;
        }

        private static ChatClientState GetChatClientState(XmppConnectionState state)
        {
            switch (state)
            {
                case XmppConnectionState.Disconnected:
                {
                    return ChatClientState.Disconnected;
                }
                case XmppConnectionState.Connecting:
                case XmppConnectionState.Authenticating:
                case XmppConnectionState.Binding:
                case XmppConnectionState.StartSession:
                case XmppConnectionState.StartCompression:
                case XmppConnectionState.Compressed:
                case XmppConnectionState.SessionStarted:
                case XmppConnectionState.Securing:
                case XmppConnectionState.Registering:
                case XmppConnectionState.Registered:
                {
                    return ChatClientState.Connecting;
                }
                case XmppConnectionState.Connected:
                case XmppConnectionState.Authenticated:
                case XmppConnectionState.Binded:
                {
                    return ChatClientState.Connected;
                }
            }
            return ChatClientState.Disconnected;
        }

        private static string GetContactId(Jid jid)
        {
            return jid.Bare;
        }

        private static PresenceType GetPresenceType(ShowType type)
        {
            switch (type)
            {
                case ShowType.NONE:
                case ShowType.chat:
                {
                    return PresenceType.Online;
                }
                case ShowType.away:
                case ShowType.xa:
                {
                    return PresenceType.Away;
                }
                case ShowType.dnd:
                {
                    return PresenceType.Busy;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        private static ShowType GetShowType(PresenceType type)
        {
            switch (type)
            {
                case PresenceType.Offline:
                {
                    return ShowType.NONE;
                }
                case PresenceType.Online:
                {
                    return ShowType.chat;
                }
                case PresenceType.Busy:
                {
                    return ShowType.away;
                }
                case PresenceType.Away:
                {
                    return ShowType.away;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(type));
        }

        private bool IsConferenceServer(Jid jid)
        {
            return ConferenceServers.Contains(jid.Server);
        }

        private void OnContactChanged(Contact contact, ContactChangeType changeType)
        {
            ContactChanged?.Invoke(this, new ContactChangedEventArgs(contact, changeType));
        }

        private void OnDisconnected()
        {
            Disconnected?.Invoke(this, new EventArgs());
        }

        private void OnError(Jid about, Error error)
        {
            var attribute = error.GetAttribute("type");
            var tagName = error.FirstChild.TagName;
            var errorText = error.ErrorText;
            ErrorReceived?.Invoke(this, new ErrorReceivedEventArgs(about, attribute, tagName, errorText));
        }

        private void OnMailReceived(Jid sender, string messageId, string subject, string message, DateTime timestamp)
        {
            Contact contact;
            if (_roster == null || !_roster.TryGetValue(GetContactId(sender), out contact))
            {
                contact = ConstructUnknownContact(sender);
            }
            MailReceived?.Invoke(this, new MessageReceivedEventArgs(contact, messageId, subject, message, timestamp));
        }

        private void OnMessagedReceived(Jid sender, string messageId, string subject, string message, DateTime timestamp)
        {
            Contact contact;
            if (_roster == null || !_roster.TryGetValue(GetContactId(sender), out contact))
            {
                contact = ConstructUnknownContact(sender);
            }
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs(contact, messageId, subject, message, timestamp));
        }

        private void OnPresenceChanged(Contact contact)
        {
            PresenceChanged?.Invoke(this, new ContactChangedEventArgs(contact, ContactChangeType.Update));
        }

        private void OnRosterReceived(Contact[] contacts)
        {
            RosterReceived?.Invoke(this, new RosterReceivedEventArgs(contacts));
        }

        public event EventHandler<ContactChangedEventArgs> ContactChanged;
        public event EventHandler Disconnected;
        public event EventHandler<ErrorReceivedEventArgs> ErrorReceived;
        public event EventHandler<MessageReceivedEventArgs> MailReceived;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<ContactChangedEventArgs> PresenceChanged;
        public event EventHandler<RosterReceivedEventArgs> RosterReceived;
        public event EventHandler<Exception> UnhandledException;
        
        public class Chat
        {
            private readonly XmppClientConnection _connection;

            public Chat(XmppClientConnection connection)
            {
                _connection = connection;
            }

            public void SendChat(string jid, string message)
            {
                _connection.Send(new Message(jid, MessageType.chat, message));
            }

            public void GroupChat(string jid, string message)
            {
                _connection.Send(new Message(jid, MessageType.groupchat, message));
            }

            public void Message(string jid, string subject, string message)
            {
                _connection.Send(new Message(jid, MessageType.normal, message, subject));
            }
        }

        // ReSharper disable once InconsistentNaming
        public class Contacts
        {
            private readonly PresenceManager _presence;
            private readonly RosterManager _roster;

            public Contacts(XmppClientConnection connection)
            {
                _presence = new PresenceManager(connection);
                _roster = new RosterManager(connection);
            }

            public void AcceptRequest(string jid, string nickname, string group)
            {
                _presence.ApproveSubscriptionRequest(jid);
                Add(jid, nickname, group);
            }

            public void Add(string jid, string nickname, string group)
            {
                _presence.Subscribe(jid);
                _roster.AddRosterItem(jid, nickname, group);
            }

            public void DeclineRequest(string jid)
            {
                _presence.RefuseSubscriptionRequest(jid);
            }

            public void Delete(string jid)
            {
                _presence.Unsubscribe(jid);
                _roster.RemoveRosterItem(jid);
            }

            public void Update(string jid, string nickname, string group)
            {
                _roster.UpdateRosterItem(jid, nickname, group);
            }

            public void Update(string jid, string nickname, string[] groups)
            {
                _roster.UpdateRosterItem(jid, nickname, groups);
            }
        }

        // ReSharper disable once InconsistentNaming
        public class Muc
        {
            private readonly XmppClientConnection _connection;
            private readonly MucManager _muc;

            public Muc(XmppClientConnection connection)
            {
                _connection = connection;
                _muc = new MucManager(connection);
            }

            public void DeclineInvite(string invitorJid, string roomJid)
            {
                _muc.Decline(invitorJid, roomJid);
            }

            public void DeclineInvite(string invitorJid, string roomJid, string reason)
            {
                _muc.Decline(invitorJid, roomJid, reason);
            }

            public void Invite(string roomJid, string jid)
            {
                _muc.Invite(roomJid, jid);
            }

            public void Invite(string roomJid, string jid, string reason)
            {
                _muc.Invite(roomJid, jid, reason);
            }

            public void Join(string roomJid)
            {
                _muc.JoinRoom(roomJid, _connection.Username);
            }

            public void Join(string roomJid, string password)
            {
                _muc.JoinRoom(roomJid, _connection.Username, password);
            }

            public void Leave(string roomJid)
            {
                _muc.LeaveRoom(roomJid, _connection.Username);
            }
        }

        // ReSharper disable once InconsistentNaming
        public class Presence
        {
            private readonly XmppClientConnection _connection;

            public Presence(XmppClientConnection connection)
            {
                _connection = connection;
            }

            public string Message
            {
                get { return _connection.Status; }
                set { _connection.Status = value; }
            }

            public PresenceType Status
            {
                get { return GetPresenceType(_connection.Show); }
                set { _connection.Show = GetShowType(value); }
            }

            public void Post()
            {
                _connection.SendMyPresence();
            }
        }
    }
}