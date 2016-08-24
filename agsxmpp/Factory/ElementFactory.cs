/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2016 by AG-Software 											 *
 * All Rights Reserved.																 *
 * Contact information for AG-Software is available at http://www.ag-software.de	 *
 *																					 *
 * Licence:																			 *
 * The agsXMPP SDK is released under a dual licence									 *
 * agsXMPP can be used under either of two licences									 *
 * 																					 *
 * A commercial licence which is probably the most appropriate for commercial 		 *
 * corporate use and closed source projects. 										 *
 *																					 *
 * The GNU Public License (GPL) is probably most appropriate for inclusion in		 *
 * other open source projects.														 *
 *																					 *
 * See README.html for details.														 *
 *																					 *
 * For general enquiries visit our website at:										 *
 * http://www.ag-software.de														 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections;
using agsXMPP.protocol.Base;
using agsXMPP.protocol.component;
using agsXMPP.protocol.extensions.amp;
using agsXMPP.protocol.extensions.bookmarks;
using agsXMPP.protocol.extensions.bytestreams;
using agsXMPP.protocol.extensions.caps;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.extensions.commands;
using agsXMPP.protocol.extensions.compression;
using agsXMPP.protocol.extensions.featureneg;
using agsXMPP.protocol.extensions.filetransfer;
using agsXMPP.protocol.extensions.geoloc;
using agsXMPP.protocol.extensions.html;
using agsXMPP.protocol.extensions.ibb;
using agsXMPP.protocol.extensions.jivesoftware.phone;
using agsXMPP.protocol.extensions.msgreceipts;
using agsXMPP.protocol.extensions.nickname;
using agsXMPP.protocol.extensions.ping;
using agsXMPP.protocol.extensions.primary;
using agsXMPP.protocol.extensions.pubsub;
using agsXMPP.protocol.extensions.pubsub.owner;
using agsXMPP.protocol.extensions.shim;
using agsXMPP.protocol.extensions.si;
using agsXMPP.protocol.iq.agent;
using agsXMPP.protocol.iq.bind;
using agsXMPP.protocol.iq.browse;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.last;
using agsXMPP.protocol.iq.oob;
using agsXMPP.protocol.iq.privacy;
using agsXMPP.protocol.iq.@private;
using agsXMPP.protocol.iq.register;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.iq.rpc;
using agsXMPP.protocol.iq.search;
using agsXMPP.protocol.iq.session;
using agsXMPP.protocol.iq.time;
using agsXMPP.protocol.iq.vcard;
using agsXMPP.protocol.sasl;
using agsXMPP.protocol.stream;
using agsXMPP.protocol.stream.feature.compression;
using agsXMPP.protocol.tls;
using agsXMPP.protocol.x;
using agsXMPP.protocol.x.data;
using agsXMPP.protocol.x.muc;
using agsXMPP.protocol.x.muc.iq.admin;
using agsXMPP.protocol.x.muc.iq.owner;
using agsXMPP.protocol.x.muc.owner;
using agsXMPP.protocol.x.rosterx;
using agsXMPP.protocol.x.vcard_update;
using agsXMPP.Xml.Dom;
using Active = agsXMPP.protocol.iq.privacy.Active;
using Affiliation = agsXMPP.protocol.extensions.pubsub.Affiliation;
using Auth = agsXMPP.protocol.iq.auth.Auth;
using Avatar = agsXMPP.protocol.iq.avatar.Avatar;
using Conference = agsXMPP.protocol.x.Conference;
using Configure = agsXMPP.protocol.extensions.pubsub.owner.Configure;
using Data = agsXMPP.protocol.x.data.Data;
using Delete = agsXMPP.protocol.extensions.pubsub.owner.Delete;
using Error = agsXMPP.protocol.client.Error;
using Event = agsXMPP.protocol.x.Event;
using Failure = agsXMPP.protocol.tls.Failure;
using IQ = agsXMPP.protocol.client.IQ;
using Item = agsXMPP.protocol.iq.privacy.Item;
using Items = agsXMPP.protocol.extensions.pubsub.@event.Items;
using Message = agsXMPP.protocol.client.Message;
using Presence = agsXMPP.protocol.client.Presence;
using PubSub = agsXMPP.protocol.extensions.pubsub.owner.PubSub;
using Purge = agsXMPP.protocol.extensions.pubsub.owner.Purge;
using RosterItem = agsXMPP.protocol.iq.roster.RosterItem;
using Status = agsXMPP.protocol.x.muc.Status;
using Stream = agsXMPP.protocol.Stream;
using Type = System.Type;
using Version = agsXMPP.protocol.iq.version.Version;

namespace agsXMPP.Factory
{
    /// <summary>
    ///     Factory class that implements the factory pattern for builing our Elements.
    /// </summary>
    public class ElementFactory
    {
        /// <summary>
        ///     This Hashtable stores Mapping of protocol (tag/namespace) to the agsXMPP objects
        /// </summary>
        private static readonly Hashtable m_table = new Hashtable();

        static ElementFactory()
        {
            AddElementType("iq", Uri.CLIENT, typeof(IQ));
            AddElementType("message", Uri.CLIENT, typeof(Message));
            AddElementType("presence", Uri.CLIENT, typeof(Presence));
            AddElementType("error", Uri.CLIENT, typeof(Error));

            AddElementType("agent", Uri.IQ_AGENTS, typeof(Agent));

            AddElementType("item", Uri.IQ_ROSTER, typeof(RosterItem));
            AddElementType("group", Uri.IQ_ROSTER, typeof(Group));
            AddElementType("group", Uri.X_ROSTERX, typeof(Group));

            AddElementType("item", Uri.IQ_SEARCH, typeof(SearchItem));

            // Stream stuff
            AddElementType("stream", Uri.STREAM, typeof(Stream));
            AddElementType("error", Uri.STREAM, typeof(protocol.Error));

            AddElementType("query", Uri.IQ_AUTH, typeof(Auth));
            AddElementType("query", Uri.IQ_AGENTS, typeof(Agents));
            AddElementType("query", Uri.IQ_ROSTER, typeof(Roster));
            AddElementType("query", Uri.IQ_LAST, typeof(Last));
            AddElementType("query", Uri.IQ_VERSION, typeof(Version));
            AddElementType("query", Uri.IQ_TIME, typeof(Time));
            AddElementType("query", Uri.IQ_OOB, typeof(Oob));
            AddElementType("query", Uri.IQ_SEARCH, typeof(Search));
            AddElementType("query", Uri.IQ_BROWSE, typeof(Browse));
            AddElementType("query", Uri.IQ_AVATAR, typeof(Avatar));
            AddElementType("query", Uri.IQ_REGISTER, typeof(Register));
            AddElementType("query", Uri.IQ_PRIVATE, typeof(Private));

            // Privacy Lists
            AddElementType("query", Uri.IQ_PRIVACY, typeof(Privacy));
            AddElementType("item", Uri.IQ_PRIVACY, typeof(Item));
            AddElementType("list", Uri.IQ_PRIVACY, typeof(List));
            AddElementType("active", Uri.IQ_PRIVACY, typeof(Active));
            AddElementType("default", Uri.IQ_PRIVACY, typeof(Default));

            // Browse
            AddElementType("service", Uri.IQ_BROWSE, typeof(Service));
            AddElementType("item", Uri.IQ_BROWSE, typeof(BrowseItem));

            // Service Discovery			
            AddElementType("query", Uri.DISCO_ITEMS, typeof(DiscoItems));
            AddElementType("query", Uri.DISCO_INFO, typeof(DiscoInfo));
            AddElementType("feature", Uri.DISCO_INFO, typeof(DiscoFeature));
            AddElementType("identity", Uri.DISCO_INFO, typeof(DiscoIdentity));
            AddElementType("item", Uri.DISCO_ITEMS, typeof(DiscoItem));

            AddElementType("x", Uri.X_DELAY, typeof(Delay));
            AddElementType("x", Uri.X_AVATAR, typeof(protocol.x.Avatar));
            AddElementType("x", Uri.X_CONFERENCE, typeof(Conference));
            AddElementType("x", Uri.X_EVENT, typeof(Event));

            //AddElementType("x",					Uri.STORAGE_AVATAR,	typeof(agsXMPP.protocol.storage.Avatar));
            AddElementType("query", Uri.STORAGE_AVATAR, typeof(protocol.storage.Avatar));

            // XData Stuff
            AddElementType("x", Uri.X_DATA, typeof(Data));
            AddElementType("field", Uri.X_DATA, typeof(Field));
            AddElementType("option", Uri.X_DATA, typeof(Option));
            AddElementType("value", Uri.X_DATA, typeof(Value));
            AddElementType("reported", Uri.X_DATA, typeof(Reported));
            AddElementType("item", Uri.X_DATA, typeof(protocol.x.data.Item));

            AddElementType("features", Uri.STREAM, typeof(Features));

            AddElementType("register", Uri.FEATURE_IQ_REGISTER, typeof(protocol.stream.feature.Register));
            AddElementType("compression", Uri.FEATURE_COMPRESS, typeof(Compression));
            AddElementType("method", Uri.FEATURE_COMPRESS, typeof(Method));

            AddElementType("bind", Uri.BIND, typeof(Bind));
            AddElementType("session", Uri.SESSION, typeof(Session));

            // TLS stuff
            AddElementType("failure", Uri.TLS, typeof(Failure));
            AddElementType("proceed", Uri.TLS, typeof(Proceed));
            AddElementType("starttls", Uri.TLS, typeof(StartTls));

            // SASL stuff
            AddElementType("mechanisms", Uri.SASL, typeof(Mechanisms));
            AddElementType("mechanism", Uri.SASL, typeof(Mechanism));
            AddElementType("auth", Uri.SASL, typeof(protocol.sasl.Auth));
            AddElementType("response", Uri.SASL, typeof(Response));
            AddElementType("challenge", Uri.SASL, typeof(Challenge));

            // TODO, this is a dirty hacks for the buggy BOSH Proxy
            // BEGIN
            AddElementType("challenge", Uri.CLIENT, typeof(Challenge));
            AddElementType("success", Uri.CLIENT, typeof(Success));
            // END

            AddElementType("failure", Uri.SASL, typeof(protocol.sasl.Failure));
            AddElementType("abort", Uri.SASL, typeof(Abort));
            AddElementType("success", Uri.SASL, typeof(Success));

            // Vcard stuff
            AddElementType("vCard", Uri.VCARD, typeof(Vcard));
            AddElementType("TEL", Uri.VCARD, typeof(Telephone));
            AddElementType("ORG", Uri.VCARD, typeof(Organization));
            AddElementType("N", Uri.VCARD, typeof(Name));
            AddElementType("EMAIL", Uri.VCARD, typeof(Email));
            AddElementType("ADR", Uri.VCARD, typeof(Address));
#if !CF
            AddElementType("PHOTO", Uri.VCARD, typeof(Photo));
#endif
            // Server stuff
            //AddElementType("stream",            Uri.SERVER,                 typeof(agsXMPP.protocol.server.Stream));
            //AddElementType("message",           Uri.SERVER,                 typeof(agsXMPP.protocol.server.Message));

            // Component stuff
            AddElementType("handshake", Uri.ACCEPT, typeof(Handshake));
            AddElementType("log", Uri.ACCEPT, typeof(Log));
            AddElementType("route", Uri.ACCEPT, typeof(Route));
            AddElementType("iq", Uri.ACCEPT, typeof(protocol.component.IQ));
            AddElementType("message", Uri.ACCEPT, typeof(protocol.component.Message));
            AddElementType("presence", Uri.ACCEPT, typeof(protocol.component.Presence));
            AddElementType("error", Uri.ACCEPT, typeof(protocol.component.Error));

            //Extensions (JEPS)
            AddElementType("header", Uri.SHIM, typeof(Header));
            AddElementType("headers", Uri.SHIM, typeof(Headers));
            AddElementType("roster", Uri.ROSTER_DELIMITER, typeof(Delimiter));
            AddElementType("p", Uri.PRIMARY, typeof(Primary));
            AddElementType("nick", Uri.NICK, typeof(Nickname));

            AddElementType("item", Uri.X_ROSTERX, typeof(protocol.x.rosterx.RosterItem));
            AddElementType("x", Uri.X_ROSTERX, typeof(RosterX));

            // Filetransfer stuff
            AddElementType("file", Uri.SI_FILE_TRANSFER, typeof(File));
            AddElementType("range", Uri.SI_FILE_TRANSFER, typeof(Range));

            // FeatureNeg
            AddElementType("feature", Uri.FEATURE_NEG, typeof(FeatureNeg));

            // Bytestreams
            AddElementType("query", Uri.BYTESTREAMS, typeof(ByteStream));
            AddElementType("streamhost", Uri.BYTESTREAMS, typeof(StreamHost));
            AddElementType("streamhost-used", Uri.BYTESTREAMS, typeof(StreamHostUsed));
            AddElementType("activate", Uri.BYTESTREAMS, typeof(Activate));
            AddElementType("udpsuccess", Uri.BYTESTREAMS, typeof(UdpSuccess));


            AddElementType("si", Uri.SI, typeof(SI));

            AddElementType("html", Uri.XHTML_IM, typeof(Html));
            AddElementType("body", Uri.XHTML, typeof(Body));

            AddElementType("compressed", Uri.COMPRESS, typeof(Compressed));
            AddElementType("compress", Uri.COMPRESS, typeof(Compress));
            AddElementType("failure", Uri.COMPRESS, typeof(protocol.extensions.compression.Failure));

            // MUC (JEP-0045 Multi User Chat)
            AddElementType("x", Uri.MUC, typeof(Muc));
            AddElementType("x", Uri.MUC_USER, typeof(User));
            AddElementType("item", Uri.MUC_USER, typeof(protocol.x.muc.Item));
            AddElementType("status", Uri.MUC_USER, typeof(Status));
            AddElementType("invite", Uri.MUC_USER, typeof(Invite));
            AddElementType("decline", Uri.MUC_USER, typeof(Decline));
            AddElementType("actor", Uri.MUC_USER, typeof(Actor));
            AddElementType("history", Uri.MUC, typeof(History));
            AddElementType("query", Uri.MUC_ADMIN, typeof(Admin));
            AddElementType("item", Uri.MUC_ADMIN, typeof(protocol.x.muc.iq.admin.Item));
            AddElementType("query", Uri.MUC_OWNER, typeof(Owner));
            AddElementType("destroy", Uri.MUC_OWNER, typeof(Destroy));
            AddElementType("destroy", Uri.MUC_USER, typeof(protocol.x.muc.user.Destroy));


            //Jabber RPC JEP 0009            
            AddElementType("query", Uri.IQ_RPC, typeof(Rpc));
            AddElementType("methodCall", Uri.IQ_RPC, typeof(MethodCall));
            AddElementType("methodResponse", Uri.IQ_RPC, typeof(MethodResponse));

            // Chatstates Jep-0085
            AddElementType("active", Uri.CHATSTATES, typeof(protocol.extensions.chatstates.Active));
            AddElementType("inactive", Uri.CHATSTATES, typeof(Inactive));
            AddElementType("composing", Uri.CHATSTATES, typeof(Composing));
            AddElementType("paused", Uri.CHATSTATES, typeof(Paused));
            AddElementType("gone", Uri.CHATSTATES, typeof(Gone));

            // Jivesoftware Extenstions
            AddElementType("phone-event", Uri.JIVESOFTWARE_PHONE, typeof(PhoneEvent));
            AddElementType("phone-action", Uri.JIVESOFTWARE_PHONE, typeof(PhoneAction));
            AddElementType("phone-status", Uri.JIVESOFTWARE_PHONE, typeof(PhoneStatus));

            // Jingle stuff is in heavy development, we commit this once the most changes on the Jeps are done            
            //AddElementType("jingle",            Uri.JINGLE,                 typeof(agsXMPP.protocol.extensions.jingle.Jingle));
            //AddElementType("candidate",         Uri.JINGLE,                 typeof(agsXMPP.protocol.extensions.jingle.Candidate));

            AddElementType("c", Uri.CAPS, typeof(Capabilities));

            AddElementType("geoloc", Uri.GEOLOC, typeof(GeoLoc));

            // Xmpp Ping
            AddElementType("ping", Uri.PING, typeof(Ping));

            //Ad-Hock Commands
            AddElementType("command", Uri.COMMANDS, typeof(Command));
            AddElementType("actions", Uri.COMMANDS, typeof(Actions));
            AddElementType("note", Uri.COMMANDS, typeof(Note));

            // **********
            // * PubSub *
            // **********
            // Owner namespace
            AddElementType("affiliate", Uri.PUBSUB_OWNER, typeof(Affiliate));
            AddElementType("affiliates", Uri.PUBSUB_OWNER, typeof(Affiliates));
            AddElementType("configure", Uri.PUBSUB_OWNER, typeof(Configure));
            AddElementType("delete", Uri.PUBSUB_OWNER, typeof(Delete));
            AddElementType("pending", Uri.PUBSUB_OWNER, typeof(Pending));
            AddElementType("pubsub", Uri.PUBSUB_OWNER, typeof(PubSub));
            AddElementType("purge", Uri.PUBSUB_OWNER, typeof(Purge));
            AddElementType("subscriber", Uri.PUBSUB_OWNER, typeof(Subscriber));
            AddElementType("subscribers", Uri.PUBSUB_OWNER, typeof(Subscribers));

            // Event namespace
            AddElementType("delete", Uri.PUBSUB_EVENT, typeof(protocol.extensions.pubsub.@event.Delete));
            AddElementType("event", Uri.PUBSUB_EVENT, typeof(protocol.extensions.pubsub.@event.Event));
            AddElementType("item", Uri.PUBSUB_EVENT, typeof(protocol.extensions.pubsub.@event.Item));
            AddElementType("items", Uri.PUBSUB_EVENT, typeof(Items));
            AddElementType("purge", Uri.PUBSUB_EVENT, typeof(protocol.extensions.pubsub.@event.Purge));

            // Main Pubsub namespace
            AddElementType("affiliation", Uri.PUBSUB, typeof(Affiliation));
            AddElementType("affiliations", Uri.PUBSUB, typeof(Affiliations));
            AddElementType("configure", Uri.PUBSUB, typeof(protocol.extensions.pubsub.Configure));
            AddElementType("create", Uri.PUBSUB, typeof(Create));
            AddElementType("configure", Uri.PUBSUB, typeof(protocol.extensions.pubsub.Configure));
            AddElementType("item", Uri.PUBSUB, typeof(protocol.extensions.pubsub.Item));
            AddElementType("items", Uri.PUBSUB, typeof(protocol.extensions.pubsub.Items));
            AddElementType("options", Uri.PUBSUB, typeof(Options));
            AddElementType("publish", Uri.PUBSUB, typeof(Publish));
            AddElementType("pubsub", Uri.PUBSUB, typeof(protocol.extensions.pubsub.PubSub));
            AddElementType("retract", Uri.PUBSUB, typeof(Retract));
            AddElementType("subscribe", Uri.PUBSUB, typeof(Subscribe));
            AddElementType("subscribe-options", Uri.PUBSUB, typeof(SubscribeOptions));
            AddElementType("subscription", Uri.PUBSUB, typeof(Subscription));
            AddElementType("subscriptions", Uri.PUBSUB, typeof(Subscriptions));
            AddElementType("unsubscribe", Uri.PUBSUB, typeof(Unsubscribe));

            // HTTP Binding XEP-0124
            AddElementType("body", Uri.HTTP_BIND, typeof(protocol.extensions.bosh.Body));

            // Message receipts XEP-0184
            AddElementType("received", Uri.MSG_RECEIPT, typeof(Received));
            AddElementType("request", Uri.MSG_RECEIPT, typeof(Request));

            // Bookmark storage XEP-0048         
            AddElementType("storage", Uri.STORAGE_BOOKMARKS, typeof(Storage));
            AddElementType("url", Uri.STORAGE_BOOKMARKS, typeof(Url));
            AddElementType("conference", Uri.STORAGE_BOOKMARKS, typeof(protocol.extensions.bookmarks.Conference));

            // XEP-0047: In-Band Bytestreams (IBB)
            AddElementType("open", Uri.IBB, typeof(Open));
            AddElementType("data", Uri.IBB, typeof(protocol.extensions.ibb.Data));
            AddElementType("close", Uri.IBB, typeof(Close));

            // XEP-0153: vCard-Based Avatars
            AddElementType("x", Uri.VCARD_UPDATE, typeof(VcardUpdate));

            // AMP
            AddElementType("amp", Uri.AMP, typeof(Amp));
            AddElementType("rule", Uri.AMP, typeof(Rule));

            // Urn Time
            AddElementType("time", Uri.URN_TIME, typeof(protocol.time.Time));

            // XEP-0145 Annotations
            AddElementType("storage", Uri.STORAGE_ROSTERNOTES, typeof(RosterNotes));
            AddElementType("note", Uri.STORAGE_ROSTERNOTES, typeof(RosterNote));
        }

        /// <summary>
        ///     Adds new Element Types to the Hashtable
        ///     Use this function also to register your own created Elements.
        ///     If a element is already registered it gets overwritten. This behaviour is also useful if you you want to overwrite
        ///     classes and add your own derived classes to the factory.
        /// </summary>
        /// <param name="tag">FQN</param>
        /// <param name="ns"></param>
        /// <param name="t"></param>
        public static void AddElementType(string tag, string ns, Type t)
        {
            var et = new ElementType(tag, ns);
            var key = et.ToString();
            // added thread safety on a user request
            lock (m_table)
            {
                if (m_table.ContainsKey(key))
                    m_table[key] = t;
                else
                    m_table.Add(et.ToString(), t);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tag"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        public static Element GetElement(string prefix, string tag, string ns)
        {
            if (ns == null)
                ns = "";

            var et = new ElementType(tag, ns);
            var t = (Type) m_table[et.ToString()];

            Element ret;
            if (t != null)
                ret = (Element) Activator.CreateInstance(t);
            else
                ret = new Element(tag);

            ret.Prefix = prefix;

            if (ns != "")
                ret.Namespace = ns;

            return ret;
        }
    }
}