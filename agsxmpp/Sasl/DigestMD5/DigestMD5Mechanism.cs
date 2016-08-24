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

using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;

namespace agsXMPP.Sasl.DigestMD5
{
    /// <summary>
    ///     Handels the SASL Digest MD5 authentication
    /// </summary>
    public class DigestMD5Mechanism : Mechanism
    {
        public override void Init(XmppClientConnection con)
        {
            XmppClientConnection = con;
            XmppClientConnection.Send(new Auth(MechanismType.DIGEST_MD5));
        }

        public override void Parse(Node e)
        {
            if (e is Challenge)
            {
                var c = e as Challenge;

                var step1 = new Step1(c.TextBase64);
                if (step1.Rspauth == null)
                {
                    //response xmlns="urn:ietf:params:xml:ns:xmpp-sasl">dXNlcm5hbWU9ImduYXVjayIscmVhbG09IiIsbm9uY2U9IjM4MDQzMjI1MSIsY25vbmNlPSIxNDE4N2MxMDUyODk3N2RiMjZjOWJhNDE2ZDgwNDI4MSIsbmM9MDAwMDAwMDEscW9wPWF1dGgsZGlnZXN0LXVyaT0ieG1wcC9qYWJiZXIucnUiLGNoYXJzZXQ9dXRmLTgscmVzcG9uc2U9NDcwMTI5NDU4Y2EwOGVjYjhhYTIxY2UzMDhhM2U5Nzc
                    var s2 = new Step2(step1, Username, Password, Server);
                    var r = new Response(s2.ToString());
                    XmppClientConnection.Send(r);
                }
                else
                {
                    // SEND: <response xmlns="urn:ietf:params:xml:ns:xmpp-sasl"/>
                    XmppClientConnection.Send(new Response());
                }
            }
        }
    }
}