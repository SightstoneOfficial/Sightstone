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
using System.Text;
using System.Threading;
using agsXMPP.Idn;
using agsXMPP.Net;
using agsXMPP.protocol;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

namespace agsXMPP
{
    public delegate void XmlHandler(object sender, string xml);

    public delegate void ErrorHandler(object sender, Exception ex);

    public delegate void XmppConnectionStateHandler(object sender, XmppConnectionState state);

    /// <summary>
    ///     abstract base class XmppConnection.
    /// </summary>
    public abstract class XmppConnection
    {
        private Timer m_KeepaliveTimer;

        internal void DoChangeXmppConnectionState(XmppConnectionState state)
        {
            XmppConnectionState = state;

            if (OnXmppConnectionStateChanged != null)
                OnXmppConnectionStateChanged(this, state);
        }

        private void InitSocket()
        {
            ClientSocket = null;

            // Socket Stuff
            if (m_SocketConnectionType == SocketConnectionType.HttpPolling)
                ClientSocket = new PollClientSocket();
            else if (m_SocketConnectionType == SocketConnectionType.Bosh)
                ClientSocket = new BoshClientSocket(this);
            else
                ClientSocket = new ClientSocket();

            ClientSocket.OnConnect += SocketOnConnect;
            ClientSocket.OnDisconnect += SocketOnDisconnect;
            ClientSocket.OnReceive += SocketOnReceive;
            ClientSocket.OnError += SocketOnError;
        }

        /// <summary>
        ///     Starts connecting of the socket
        /// </summary>
        public virtual void SocketConnect()
        {
            DoChangeXmppConnectionState(XmppConnectionState.Connecting);
            ClientSocket.Connect();
        }

        public void SocketConnect(string server, int port)
        {
            ClientSocket.Address = server;
            ClientSocket.Port = port;
            SocketConnect();
        }

        public void SocketDisconnect()
        {
            ClientSocket.Disconnect();
        }

        /// <summary>
        ///     Send a xml string over the XmppConnection
        /// </summary>
        /// <param name="xml"></param>
        public void Send(string xml)
        {
            FireOnWriteXml(this, xml);
            ClientSocket.Send(xml);

            if (OnWriteSocketData != null)
                OnWriteSocketData(this, Encoding.UTF8.GetBytes(xml), xml.Length);

            // reset keep alive timer if active to make sure the interval is always idle time from the last 
            // outgoing packet
            if (KeepAlive && m_KeepaliveTimer != null)
                m_KeepaliveTimer.Change(KeepAliveInterval*1000, KeepAliveInterval*1000);
        }

        /// <summary>
        ///     Send a xml element over the XmppConnection
        /// </summary>
        /// <param name="e"></param>
        public virtual void Send(Element e)
        {
            Send(e.ToString());
        }

        public void Open(string xml)
        {
            Send(xml);
        }

        /// <summary>
        ///     Send the end of stream
        /// </summary>
        public virtual void Close()
        {
            Send("</stream:stream>");
        }

        protected void FireOnReadXml(object sender, string xml)
        {
            if (OnReadXml != null)
                OnReadXml(sender, xml);
        }

        protected void FireOnWriteXml(object sender, string xml)
        {
            if (OnWriteXml != null)
                OnWriteXml(sender, xml);
        }

        protected void FireOnError(object sender, Exception ex)
        {
            if (OnError != null)
                OnError(sender, ex);
        }

        #region << Events >>

        /// <summary>
        ///     This event just informs about the current state of the XmppConnection
        /// </summary>
        public event XmppConnectionStateHandler OnXmppConnectionStateChanged;

        /// <summary>
        ///     a XML packet or text is received.
        ///     This are no winsock events. The Events get generated from the XML parser
        /// </summary>
        public event XmlHandler OnReadXml;

        /// <summary>
        ///     XML or Text is written to the Socket this includes also the keep alive packages (a single space)
        /// </summary>
        public event XmlHandler OnWriteXml;

        public event ErrorHandler OnError;

        /// <summary>
        ///     Data received from the Socket
        /// </summary>
        public event BaseSocket.OnSocketDataHandler OnReadSocketData;

        /// <summary>
        ///     Data was sent to the socket for sending
        /// </summary>
        public event BaseSocket.OnSocketDataHandler OnWriteSocketData;

        #endregion

        #region << Constructors >>

        public XmppConnection()
        {
            InitSocket();
            // Streamparser stuff
            StreamParser = new StreamParser();

            StreamParser.OnStreamStart += StreamParserOnStreamStart;
            StreamParser.OnStreamEnd += StreamParserOnStreamEnd;
            StreamParser.OnStreamElement += StreamParserOnStreamElement;
            StreamParser.OnStreamError += StreamParserOnStreamError;
            StreamParser.OnError += StreamParserOnError;
        }

        public XmppConnection(SocketConnectionType type) : this()
        {
            m_SocketConnectionType = SocketConnectionType.Direct;
        }

        #endregion

        #region << Properties and Member Variables >>

        private string m_Server;
        private SocketConnectionType m_SocketConnectionType = SocketConnectionType.Direct;

        /// <summary>
        ///     The Port of the remote server for the connection
        /// </summary>
        public int Port { get; set; } = 5222;

        /// <summary>
        ///     domain or ip-address of the remote server for the connection
        /// </summary>
        public string Server
        {
            get { return m_Server; }
            set
            {
#if !STRINGPREP
                if (value != null)
				    m_Server = value.ToLower();
                else
                    m_Server = null;
#else
                if (value != null)
                    m_Server = Stringprep.NamePrep(value);
                else
                    m_Server = null;
#endif
            }
        }

        /// <summary>
        /// </summary>
        public string ConnectServer { get; set; } = null;

        /// <summary>
        ///     the id of the current xmpp xml-stream
        /// </summary>
        public string StreamId { get; set; } = "";

        /// <summary>
        ///     Set to null for old Jabber Protocol without SASL andstream features
        /// </summary>
        public string StreamVersion { get; set; } = "1.0";

        public XmppConnectionState XmppConnectionState { get; private set; } = XmppConnectionState.Disconnected;

        /// <summary>
        ///     Read Online Property ClientSocket
        ///     returns the SOcket object used for this connection
        /// </summary>
        public BaseSocket ClientSocket { get; private set; }

        /// <summary>
        ///     the underlaying XMPP StreamParser. Normally you don't need it, but we make it accessible for
        ///     low level access to the stream
        /// </summary>
        public StreamParser StreamParser { get; }

        public SocketConnectionType SocketConnectionType
        {
            get { return m_SocketConnectionType; }
            set
            {
                m_SocketConnectionType = value;
                InitSocket();
            }
        }

        public bool AutoResolveConnectServer { get; set; } = true;

        /// <summary>
        ///     <para>
        ///         the keep alive interval in seconds.
        ///         Default value is 120
        ///     </para>
        ///     <para>
        ///         Keep alive packets prevent disconenct on NAT and broadband connections which often
        ///         disconnect if they are idle.
        ///     </para>
        /// </summary>
        public int KeepAliveInterval { get; set; } = 120;

        /// <summary>
        ///     Send Keep Alives (for NAT)
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        #endregion

        #region << Socket handers >>

        public virtual void SocketOnConnect(object sender)
        {
            DoChangeXmppConnectionState(XmppConnectionState.Connected);
        }

        public virtual void SocketOnDisconnect(object sender)
        {
        }

        public virtual void SocketOnReceive(object sender, byte[] data, int count)
        {
            if (OnReadSocketData != null)
                OnReadSocketData(sender, data, count);

            // put the received bytes to the parser
            lock (this)
            {
                StreamParser.Push(data, 0, count);
            }
        }

        public virtual void SocketOnError(object sender, Exception ex)
        {
        }

        #endregion

        #region << StreamParser Events >>

        public virtual void StreamParserOnStreamStart(object sender, Node e)
        {
            var xml = e.ToString().Trim();
            xml = xml.Substring(0, xml.Length - 2) + ">";

            FireOnReadXml(this, xml);

            var st = (Stream) e;
            if (st != null)
            {
                StreamId = st.StreamId;
                StreamVersion = st.Version;
            }
        }

        public virtual void StreamParserOnStreamEnd(object sender, Node e)
        {
            var tag = e as Element;

            string qName;
            if (tag.Prefix == null)
                qName = tag.TagName;
            else
                qName = tag.Prefix + ":" + tag.TagName;

            var xml = "</" + qName + ">";

            FireOnReadXml(this, xml);
        }

        public virtual void StreamParserOnStreamElement(object sender, Node e)
        {
            FireOnReadXml(this, e.ToString());
        }

        public virtual void StreamParserOnStreamError(object sender, Exception ex)
        {
        }

        public virtual void StreamParserOnError(object sender, Exception ex)
        {
            FireOnError(sender, ex);
        }

        #endregion

        #region << Keepalive Timer functions >>

        protected void CreateKeepAliveTimer()
        {
            // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = KeepAliveTick;
            var interval = KeepAliveInterval*1000;
            // Create a timer that waits x seconds, then invokes every x seconds.
            m_KeepaliveTimer = new Timer(timerDelegate, null, interval, interval);
        }

        protected void DestroyKeepAliveTimer()
        {
            if (m_KeepaliveTimer == null)
                return;

            m_KeepaliveTimer.Dispose();
            m_KeepaliveTimer = null;
        }

        private void KeepAliveTick(object state)
        {
            // Send a Space for Keep Alive
            Send(" ");
        }

        #endregion
    }
}