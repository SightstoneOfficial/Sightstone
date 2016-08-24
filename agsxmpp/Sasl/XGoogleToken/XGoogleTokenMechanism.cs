/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2012 by AG-Software 											 *
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
using System.IO;
using System.Net;
using System.Text;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;

namespace agsXMPP.Sasl.XGoogleToken
{
    /// <summary>
    ///     X-GOOGLE-TOKEN Authentication
    /// </summary>
    public class XGoogleTokenMechanism : Mechanism
    {
        private const string METHOD = "POST";
        private const string CONTENT_TYPE = "application/x-www-form-urlencoded";
        private const string URL_ISSUE_AUTH_TOKEN = "https://www.google.com/accounts/IssueAuthToken";
        private const string URL_CLIENT_AUTH = "https://www.google.com/accounts/ClientAuth";

        /*
        
        see Google API documentation at:
        http://code.google.com/apis/accounts/AuthForInstalledApps.html
        http://code.google.com/apis/accounts/AuthForWebApps.html
        
        */

        private string _Auth;
        private string _Base64Token;
        private string _Lsid;
        private string _Sid;

        public override void Init(XmppClientConnection con)
        {
            XmppClientConnection = con;

            DoClientAuth();
        }

        public override void Parse(Node e)
        {
            // not needed here in X-GOOGLE-TOKEN mechanism
        }

        private void DoSaslAuth()
        {
            // <auth xmlns=”urn:ietf:params:xml:ns:xmpp-sasl” mechanism=”X-GOOGLE-TOKEN”>Base 64 Token goes here</auth>            
            var auth = new Auth(MechanismType.X_GOOGLE_TOKEN, _Base64Token);
            XmppClientConnection.Send(auth);
        }

        private void DoClientAuth()
        {
            var request = (HttpWebRequest) WebRequest.Create(URL_CLIENT_AUTH);

            request.Method = METHOD;
            request.ContentType = CONTENT_TYPE;

#if CF || CF_2
    //required for bug workaround
            request.AllowWriteStreamBuffering = true; 
#endif

            request.BeginGetRequestStream(OnGetClientAuthRequestStream, request);
        }

        private void OnGetClientAuthRequestStream(IAsyncResult result)
        {
            var request = (WebRequest) result.AsyncState;
            var outputStream = request.EndGetRequestStream(result);

            string data = null;
            data += "Email=" + XmppClientConnection.MyJID.Bare;
            data += "&Passwd=" + Password;
            data += "&PersistentCookie=false";
            //data += "&source=googletalk";
            data += "&source=" + XmppClientConnection.Resource;
            data += "&service=mail";


            var bytes = Encoding.UTF8.GetBytes(data);
            outputStream.Write(bytes, 0, bytes.Length);
            outputStream.Close();

            request.BeginGetResponse(OnGetClientAuthResponse, request);
        }

        private void OnGetClientAuthResponse(IAsyncResult result)
        {
            try
            {
                var request = (WebRequest) result.AsyncState;
                var response = (HttpWebResponse) request.EndGetResponse(result);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var dataStream = response.GetResponseStream();

                    ParseClientAuthResponse(dataStream);

                    dataStream.Close();
                    response.Close();

                    _Base64Token = GetToken(_Auth);

                    DoSaslAuth();
                }
                else
                    XmppClientConnection.Close();
            }
            catch (WebException we)
            {
                if (we.Response is HttpWebResponse // this is also false when Response is null
                    && ((HttpWebResponse) we.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    XmppClientConnection.FireOnAuthError(null);
                }
                XmppClientConnection.Close();
            }
        }

        private void ParseClientAuthResponse(Stream responseStream)
        {
            var reader = new StreamReader(responseStream);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("SID="))
                    _Sid = line.Substring(4);
                else if (line.StartsWith("LSID="))
                    _Lsid = line.Substring(5);
                else if (line.StartsWith("Auth="))
                    _Auth = line.Substring(5);
            }

            reader.Close();
        }

        private string GetToken(string line)
        {
            var temp = "\0" + XmppClientConnection.MyJID.Bare + "\0" + line;
            var b = Encoding.UTF8.GetBytes(temp);
            return Convert.ToBase64String(b, 0, b.Length);
        }
    }
}