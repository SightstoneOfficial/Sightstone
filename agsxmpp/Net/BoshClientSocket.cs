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
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.bosh;
using agsXMPP.Util;
using agsXMPP.Xml.Dom;

namespace agsXMPP.Net
{
    public class WebRequestState
    {
        public int WebRequestId;

        public WebRequestState(WebRequest request)
        {
            WebRequest = request;
        }

        /// <summary>
        ///     when was this request started (timestamp)?
        /// </summary>
        public DateTime Started { get; set; }

        public bool IsSessionRequest { get; set; }

        public string Output { get; set; }

        public WebRequest WebRequest { get; set; }

        public Stream RequestStream { get; set; }

        public Timer TimeOutTimer { get; set; }

        public bool Aborted { get; set; }
    }

    public class BoshClientSocket : BaseSocket
    {
        private const string CONTENT_TYPE = "text/xml; charset=utf-8";
        private const string METHOD = "POST";
        private const string BOSH_VERSION = "1.6";
        private const int WEBREQUEST_TIMEOUT = 5000;
        private const int OFFSET_WAIT = 5000;

        private string[] Keys; // Array of keys
        private int activeRequests; // currently active (waiting) WebRequests
        private int CurrentKeyIdx; // index of the currect key
        private readonly Queue m_SendQueue = new Queue(); // Queue for stanzas to send
        private bool streamStarted; // is the stream started? received stream header?
        private int polling;
        private bool terminate;
        private bool terminated;
        private DateTime lastSend = DateTime.MinValue; // DateTime of the last activity/response

        private long rid;
        private bool restart; // stream state, are we currently restarting the stream?
        private string sid;
        private bool requestIsTerminating;

        private int webRequestId = 1;

        public BoshClientSocket(XmppConnection con)
        {
            m_XmppCon = con;
        }

        private void Init()
        {
            Keys = null;
            streamStarted = false;
            terminate = false;
            terminated = false;
        }

        #region << Properties >>

#if !CF && !CF_2
#else
    // set this lower on embedded devices because the key generation is slow there		
        private int             m_MinCountKeys  = 10;
        private int             m_MaxCountKeys  = 99;
#endif

        public Jid To { get; set; }

        /// <summary>
        ///     The longest time (in seconds) that the connection manager is allowed to wait before responding to any request
        ///     during the session.
        ///     This enables the client to prevent its TCP connection from expiring due to inactivity, as well as to limit the
        ///     delay before
        ///     it discovers any network failure.
        /// </summary>
        public int Wait { get; set; } = 300;

        public int Requests { get; set; } = 2;

        public int MaxCountKeys { get; set; } = 9999;

        public int MinCountKeys { get; set; } = 1000;

        /// <summary>
        ///     This attribute specifies the maximum number of requests the connection manager is allowed to keep waiting
        ///     at any one time during the session. If the client is not able to use HTTP Pipelining then this SHOULD be set to
        ///     "1".
        /// </summary>
        public int Hold { get; set; } = 1;

        /// <summary>
        ///     Keep Alive for HTTP Webrequests, its disables by default because not many BOSH implementations support Keep Alives
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>
        ///     If the connection manager supports session pausing (see Inactivity) then it SHOULD advertise that to the client
        ///     by including a 'maxpause' attribute in the session creation response element.
        ///     The value of the attribute indicates the maximum length of a temporary session pause (in seconds) that a client MAY
        ///     request.
        ///     0 is the default value and indicated that the connection manager supports no session pausing.
        /// </summary>
        public int MaxPause { get; set; }

        public bool SupportsSessionPausing
        {
            get { return !(MaxPause == 0); }
        }

        public WebProxy Proxy { get; set; } = null;

        #endregion

        private string DummyStreamHeader
        {
            get
            {
                // <stream:stream xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams' id='1075705237'>
                // create dummy stream header
                var sb = new StringBuilder();

                sb.Append("<stream:stream");

                sb.Append(" xmlns='");
                sb.Append(Uri.CLIENT);

                sb.Append("' xmlns:stream='");
                sb.Append(Uri.STREAM);

                sb.Append("' id='");
                sb.Append(sid);

                sb.Append("' version='");
                sb.Append("1.0");

                sb.Append("'>");

                return sb.ToString();
            }
        }

        /// <summary>
        ///     Generates a bunch of keys
        /// </summary>
        private void GenerateKeys()
        {
            /*
            13.3 Generating the Key Sequence

            Prior to requesting a new session, the client MUST select an unpredictable counter ("n") and an unpredictable value ("seed").
            The client then processes the "seed" through a cryptographic hash and converts the resulting 160 bits to a hexadecimal string K(1).
            It does this "n" times to arrive at the initial key K(n). The hashing algorithm MUST be SHA-1 as defined in RFC 3174.

            Example 25. Creating the key sequence

                    K(1) = hex(SHA-1(seed))
                    K(2) = hex(SHA-1(K(1)))
                    ...
                    K(n) = hex(SHA-1(K(n-1)))

            */
            var countKeys = GetRandomNumber(MinCountKeys, MaxCountKeys);

            Keys = new string[countKeys];
            var prev = GenerateSeed();

            for (var i = 0; i < countKeys; i++)
            {
                Keys[i] = Hash.Sha1Hash(prev);
                prev = Keys[i];
            }
            CurrentKeyIdx = countKeys - 1;
        }

        private string GenerateSeed()
        {
            var m_lenght = 10;

#if CF
            util.RandomNumberGenerator rng = util.RandomNumberGenerator.Create();
#else
            var rng = RandomNumberGenerator.Create();
#endif
            var buf = new byte[m_lenght];
            rng.GetBytes(buf);

            return Hash.HexToString(buf);
        }

        private int GenerateRid()
        {
            var min = 1;
            var max = int.MaxValue;

            var rnd = new Random();

            return rnd.Next(min, max);
        }

        private int GetRandomNumber(int min, int max)
        {
            var rnd = new Random();
            return rnd.Next(min, max);
        }

        public override void Reset()
        {
            base.Reset();

            streamStarted = false;
            restart = true;
        }

        public void RequestBoshSession()
        {
            /*
            Example 1. Requesting a BOSH session

            POST /webclient HTTP/1.1
            Host: httpcm.jabber.org
            Accept-Encoding: gzip, deflate
            Content-Type: text/xml; charset=utf-8
            Content-Length: 104

            <body content='text/xml; charset=utf-8'
                  hold='1'
                  rid='1573741820'
                  to='jabber.org'
                  route='xmpp:jabber.org:9999'
                  secure='true'
                  ver='1.6'
                  wait='60'
                  ack='1'
                  xml:lang='en'
                  xmlns='http://jabber.org/protocol/httpbind'/>
             */

            lastSend = DateTime.Now;

            // Generate the keys
            GenerateKeys();
            rid = GenerateRid();
            var body = new Body();
            /*
             * <body hold='1' xmlns='http://jabber.org/protocol/httpbind' 
             *  to='vm-2k' 
             *  wait='300' 
             *  rid='782052' 
             *  newkey='8e7d6cec12004e2bfcf7fc000310fda87bc8337c' 
             *  ver='1.6' 
             *  xmpp:xmlns='urn:xmpp:xbosh' 
             *  xmpp:version='1.0'/>
             */
            //window='5' content='text/xml; charset=utf-8'
            //body.SetAttribute("window", "5");
            //body.SetAttribute("content", "text/xml; charset=utf-8");

            body.Version = BOSH_VERSION;
            body.XmppVersion = "1.0";
            body.Hold = Hold;
            body.Wait = Wait;
            body.Rid = rid;
            body.Polling = 0;
            body.Requests = Requests;
            body.To = new Jid(m_XmppCon.Server);

            body.NewKey = Keys[CurrentKeyIdx];

            body.SetAttribute("xmpp:xmlns", "urn:xmpp:xbosh");

            activeRequests++;

            var req = (HttpWebRequest) CreateWebrequest(Address);

            var state = new WebRequestState(req);
            state.Started = DateTime.Now;
            state.Output = body.ToString();
            state.IsSessionRequest = true;

            req.Method = METHOD;
            req.ContentType = CONTENT_TYPE;
            req.Timeout = Wait*1000;
            req.KeepAlive = KeepAlive;
            req.ContentLength = Encoding.UTF8.GetBytes(state.Output).Length; // state.Output.Length;

            try
            {
                var result = req.BeginGetRequestStream(OnGetSessionRequestStream, state);
            }
            catch (Exception)
            {
            }
        }

        private void OnGetSessionRequestStream(IAsyncResult ar)
        {
            try
            {
                var state = ar.AsyncState as WebRequestState;
                var req = state.WebRequest as HttpWebRequest;

                var outputStream = req.EndGetRequestStream(ar);

                var bytes = Encoding.UTF8.GetBytes(state.Output);

                state.RequestStream = outputStream;
                var result = outputStream.BeginWrite(bytes, 0, bytes.Length, OnEndWrite, state);
            }
            catch (WebException ex)
            {
                FireOnError(ex);
                Disconnect();
            }
        }

        private void OnGetSessionRequestResponse(IAsyncResult result)
        {
            // grab the custom state object
            var state = (WebRequestState) result.AsyncState;
            var request = (HttpWebRequest) state.WebRequest;

            //state.TimeOutTimer.Dispose();

            // get the Response
            var resp = (HttpWebResponse) request.EndGetResponse(result);

            // The server must always return a 200 response code,
            // sending any session errors as specially-formatted identifiers.
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            var rs = resp.GetResponseStream();

            int readlen;
            var readbuf = new byte[1024];
            var ms = new MemoryStream();
            while ((readlen = rs.Read(readbuf, 0, readbuf.Length)) > 0)
            {
                ms.Write(readbuf, 0, readlen);
            }

            var recv = ms.ToArray();

            if (recv.Length > 0)
            {
                string body = null;
                string stanzas = null;

                var res = Encoding.UTF8.GetString(recv, 0, recv.Length);

                ParseResponse(res, ref body, ref stanzas);

                var doc = new Document();
                doc.LoadXml(body);
                var boshBody = doc.RootElement as Body;

                sid = boshBody.Sid;
                polling = boshBody.Polling;
                MaxPause = boshBody.MaxPause;

                var bin = Encoding.UTF8.GetBytes(DummyStreamHeader + stanzas);

                FireOnReceive(bin, bin.Length);

                // cleanup webrequest resources
                ms.Close();
                rs.Close();
                resp.Close();

                activeRequests--;

                if (activeRequests == 0)
                    StartWebRequest();
            }
        }

        /// <summary>
        ///     This is ugly code, but currently all BOSH server implementaions are not namespace correct,
        ///     which means we can't use the XML parser here and have to spit it with string functions.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="body"></param>
        /// <param name="stanzas"></param>
        private void ParseResponse(string res, ref string body, ref string stanzas)
        {
            res = res.Trim();
            if (res.EndsWith("/>"))
            {
                // <body ..../>
                // empty response
                body = res;
                stanzas = null;
            }
            else
            {
                /* 
                 * <body .....>
                 *  <message/>
                 *  <presence/>
                 * </body>  
                 */

                // find position of the first closing angle bracket
                var startPos = res.IndexOf(">");
                // find position of the last opening angle bracket
                var endPos = res.LastIndexOf("<");

                body = res.Substring(0, startPos) + "/>";
                stanzas = res.Substring(startPos + 1, endPos - startPos - 1);
            }
        }

        #region << Public Methods and Functions >>

        public override void Connect()
        {
            base.Connect();

            Init();
            FireOnConnect();

            RequestBoshSession();
        }

        public override void Disconnect()
        {
            base.Disconnect();

            FireOnDisconnect();
            //m_Connected = false;
        }

        public override void Send(byte[] bData)
        {
            base.Send(bData);

            Send(Encoding.UTF8.GetString(bData, 0, bData.Length));
        }


        public override void Send(string data)
        {
            base.Send(data);

            // This are hacks because we send no stream headers and footer in Bosh
            if (data.StartsWith("<stream:stream"))
            {
                if (!streamStarted && !restart)
                    streamStarted = true;
                else
                {
                    var bin = Encoding.UTF8.GetBytes(DummyStreamHeader);
                    FireOnReceive(bin, bin.Length);
                }
                return;
            }

            if (data.EndsWith("</stream:stream>"))
            {
                var pres = new Presence();
                pres.Type = PresenceType.unavailable;
                data = pres.ToString(); //= "<presence type='unavailable' xmlns='jabber:client'/>";
                terminate = true;
            }
            //    return;

            lock (m_SendQueue)
            {
                m_SendQueue.Enqueue(data);
            }

            CheckDoRequest();
        }

        #endregion

        private void CheckDoRequest()
        {
            /*
             * requestIsTerminating is true when a webrequest is ending
             * when the requests ends a new request gets started immedialtely,
             * so we don't have to create another request in the case
             */
            if (!requestIsTerminating)
                Request();
        }

        private void Request()
        {
            if (activeRequests < 2)
                StartWebRequest();
        }

        private string BuildPostData()
        {
            CurrentKeyIdx--;
            rid++;

            var sb = new StringBuilder();

            var body = new Body();

            body.Rid = rid;
            body.Key = Keys[CurrentKeyIdx];

            if (CurrentKeyIdx == 0)
            {
                // this is our last key
                // Generate a new key sequence
                GenerateKeys();
                body.NewKey = Keys[CurrentKeyIdx];
            }

            body.Sid = sid;
            //body.Polling    = 0;
            body.To = new Jid(m_XmppCon.Server);

            if (restart)
            {
                body.XmppRestart = true;
                restart = false;
            }

            lock (m_SendQueue)
            {
                if (terminate && m_SendQueue.Count == 1)
                    body.Type = BoshType.terminate;

                if (m_SendQueue.Count > 0)
                {
                    sb.Append(body.StartTag());

                    while (m_SendQueue.Count > 0)
                    {
                        var data = m_SendQueue.Dequeue() as string;
                        sb.Append(data);
                    }

                    sb.Append(body.EndTag());

                    return sb.ToString();
                }
                return body.ToString();
            }
        }

        private void StartWebRequest()
        {
            StartWebRequest(false, null);
        }

        private void StartWebRequest(bool retry, string content)
        {
            lock (this)
            {
                webRequestId++;
            }

            activeRequests++;

            lastSend = DateTime.Now;

            var req = (HttpWebRequest) CreateWebrequest(Address);
            ;

            var state = new WebRequestState(req);
            state.Started = DateTime.Now;
            state.WebRequestId = webRequestId;

            if (!retry)
                state.Output = BuildPostData();
            else
                state.Output = content;

            req.Method = METHOD;
            req.ContentType = CONTENT_TYPE;
            req.Timeout = Wait*1000;
            req.KeepAlive = KeepAlive;
            req.ContentLength = Encoding.UTF8.GetBytes(state.Output).Length;

            // Create the delegate that invokes methods for the timer.            
            TimerCallback timerDelegate = TimeOutGetRequestStream;
            var timeoutTimer = new Timer(timerDelegate, state, WEBREQUEST_TIMEOUT, WEBREQUEST_TIMEOUT);
            state.TimeOutTimer = timeoutTimer;

            try
            {
                req.BeginGetRequestStream(OnGetRequestStream, state);
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        public void TimeOutGetRequestStream(object stateObj)
        {
            //Console.WriteLine("Web Request timed out");

            var state = stateObj as WebRequestState;
            state.TimeOutTimer.Dispose();
            state.Aborted = true;
            state.WebRequest.Abort();
        }

        private void OnGetRequestStream(IAsyncResult ar)
        {
            try
            {
                var state = ar.AsyncState as WebRequestState;

                if (state.Aborted)
                {
                    activeRequests--;
                    StartWebRequest(true, state.Output);
                }
                else
                {
                    state.TimeOutTimer.Dispose();
                    var req = state.WebRequest as HttpWebRequest;

                    var requestStream = req.EndGetRequestStream(ar);
                    state.RequestStream = requestStream;
                    var bytes = Encoding.UTF8.GetBytes(state.Output);
                    requestStream.BeginWrite(bytes, 0, bytes.Length, OnEndWrite, state);
                }
            }
            catch (Exception)
            {
                activeRequests--;

                var state = ar.AsyncState as WebRequestState;
                StartWebRequest(true, state.Output);
            }
        }

        private void OnEndWrite(IAsyncResult ar)
        {
            var state = ar.AsyncState as WebRequestState;

            var req = state.WebRequest as HttpWebRequest;
            var requestStream = state.RequestStream;

            requestStream.EndWrite(ar);
            requestStream.Close();

            //IAsyncResult result;

            try
            {
                if (state.IsSessionRequest)
                    req.BeginGetResponse(OnGetSessionRequestResponse, state);
                else
                    req.BeginGetResponse(OnGetResponse, state);
            }
            catch (Exception)
            {
                //Console.WriteLine(ex.Message);
            }
        }

        private void OnGetResponse(IAsyncResult ar)
        {
            try
            {
                requestIsTerminating = true;
                // grab the custom state object
                var state = (WebRequestState) ar.AsyncState;
                var request = (HttpWebRequest) state.WebRequest;
                HttpWebResponse resp = null;

                if (request.HaveResponse)
                {
                    // TODO, its crashing mostly here
                    // get the Response
                    try
                    {
                        resp = (HttpWebResponse) request.EndGetResponse(ar);
                    }
                    catch (WebException ex)
                    {
                        activeRequests--;
                        requestIsTerminating = false;
                        if (ex.Response == null)
                        {
                            StartWebRequest();
                        }
                        else
                        {
                            var res = ex.Response as HttpWebResponse;
                            if (res.StatusCode == HttpStatusCode.NotFound)
                            {
                                TerminateBoshSession();
                            }
                        }
                        return;
                    }

                    // The server must always return a 200 response code,
                    // sending any session errors as specially-formatted identifiers.
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        activeRequests--;
                        requestIsTerminating = false;
                        if (resp.StatusCode == HttpStatusCode.NotFound)
                        {
                            //Console.WriteLine("Not Found");
                            TerminateBoshSession();
                        }
                        return;
                    }
                }

                var rs = resp.GetResponseStream();

                int readlen;
                var readbuf = new byte[1024];
                var ms = new MemoryStream();
                while ((readlen = rs.Read(readbuf, 0, readbuf.Length)) > 0)
                {
                    ms.Write(readbuf, 0, readlen);
                }

                var recv = ms.ToArray();

                if (recv.Length > 0)
                {
                    string sbody = null;
                    string stanzas = null;

                    ParseResponse(Encoding.UTF8.GetString(recv, 0, recv.Length), ref sbody, ref stanzas);

                    if (stanzas != null)
                    {
                        var bStanzas = Encoding.UTF8.GetBytes(stanzas);
                        FireOnReceive(bStanzas, bStanzas.Length);
                    }
                    else
                    {
                        if (sbody != null)
                        {
                            var doc = new Document();
                            doc.LoadXml(sbody);
                            if (doc.RootElement != null)
                            {
                                var body = doc.RootElement as Body;
                                if (body.Type == BoshType.terminate)
                                    TerminateBoshSession();
                            }
                        }

                        if (terminate && !terminated)
                        {
                            // empty teminate response
                            TerminateBoshSession();
                        }
                    }
                }

                // cleanup webrequest resources
                ms.Close();
                rs.Close();
                resp.Close();

                activeRequests--;
                requestIsTerminating = false;

                //if (activeRequests == 0 && !terminated)
                if ((activeRequests == 0 && !terminated)
                    || (activeRequests == 1 && m_SendQueue.Count > 0))
                {
                    StartWebRequest();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private WebRequest CreateWebrequest(string address)
        {
            var webReq = WebRequest.Create(address);
#if !CF_2
            if (Proxy != null)
                webReq.Proxy = Proxy;
            else
            {
                if (webReq.Proxy != null)
                    webReq.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }

#endif
            return webReq;
        }

        private void TerminateBoshSession()
        {
            // empty teminate response
            var bStanzas = Encoding.UTF8.GetBytes("</stream:stream>");
            FireOnReceive(bStanzas, bStanzas.Length);
            terminated = true;
        }
    }
}