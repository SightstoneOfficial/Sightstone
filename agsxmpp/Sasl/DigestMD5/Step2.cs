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
using System.Security.Cryptography;
using System.Text;
using agsXMPP.Util;
#if CF
using agsXMPP.util;
#endif

namespace agsXMPP.Sasl.DigestMD5
{
    /// <summary>
    ///     Summary description for Step2.
    /// </summary>
    public class Step2 : Step1
    {
        public Step2()
        {
        }

        /// <summary>
        ///     builds a step2 message reply to the given step1 message
        /// </summary>
        /// <param name="step1"></param>
        public Step2(Step1 step1, string username, string password, string server)
        {
            Nonce = step1.Nonce;

            // fixed for SASL n amessage servers (jabberd 1.x)
            if (SupportsAuth(step1.Qop))
                Qop = "auth";

            Realm = step1.Realm;
            Charset = step1.Charset;
            Algorithm = step1.Algorithm;

            Username = username;
            Password = password;
            Server = server;

            GenerateCnonce();
            GenerateNc();
            GenerateDigestUri();
            GenerateResponse();
        }

        /// <summary>
        ///     parses a message and returns the step2 object
        /// </summary>
        /// <param name="message"></param>
        public Step2(string message)
        {
            // TODO, important for server stuff
        }

        /// <summary>
        ///     Does the server support Auth?
        /// </summary>
        /// <param name="qop"></param>
        /// <returns></returns>
        private bool SupportsAuth(string qop)
        {
            var auth = qop.Split(',');
            // This overload was not available in the CF, so updated this to the following
            //bool ret = Array.IndexOf(auth, "auth") < 0 ? false : true;
            var ret = Array.IndexOf(auth, "auth", auth.GetLowerBound(0), auth.Length) < 0 ? false : true;
            return ret;
        }


        public override string ToString()
        {
            return GenerateMessage();
        }


        private void GenerateCnonce()
        {
            // Lenght of the Session ID on bytes,
            // 32 bytes equaly 64 chars
            // 16^64 possibilites for the session IDs (4.294.967.296)
            // This should be unique enough
            var m_lenght = 32;

            var RNG = RandomNumberGenerator.Create();

            var buf = new byte[m_lenght];
            RNG.GetBytes(buf);

            Cnonce = Hash.HexToString(buf).ToLower();

//			m_Cnonce = "e163ceed6cfbf8c1559a9ff373b292c2f926b65719a67a67c69f7f034c50aba3";
        }

        private void GenerateNc()
        {
            var nc = 1;
            Nc = nc.ToString().PadLeft(8, '0');
        }

        private void GenerateDigestUri()
        {
            DigestUri = "xmpp/" + Server;
        }


        //	HEX( KD ( HEX(H(A1)),
        //	{
        //		nonce-value, ":" nc-value, ":",
        //		cnonce-value, ":", qop-value, ":", HEX(H(A2)) }))
        //
        //	If authzid is specified, then A1 is
        //
        //	A1 = { H( { username-value, ":", realm-value, ":", passwd } ),
        //	":", nonce-value, ":", cnonce-value, ":", authzid-value }
        //
        //	If authzid is not specified, then A1 is
        //
        //	A1 = { H( { username-value, ":", realm-value, ":", passwd } ),
        //	":", nonce-value, ":", cnonce-value }
        //
        //	where
        //
        //	passwd   = *OCTET
        public void GenerateResponse()
        {
            byte[] H1;
            byte[] H2;
            byte[] H3;
            //byte[] temp;
            string A1;
            string A2;
            string A3;
            string p1;
            string p2;

            var sb = new StringBuilder();
            sb.Append(Username);
            sb.Append(":");
            sb.Append(Realm);
            sb.Append(":");
            sb.Append(Password);

#if !CF
            H1 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
#else
    //H1 = Encoding.Default.GetBytes(util.Hash.MD5Hash(sb.ToString()));
			H1 = util.Hash.MD5Hash(Encoding.UTF8.GetBytes(sb.ToString()));
#endif

            sb.Remove(0, sb.Length);
            sb.Append(":");
            sb.Append(Nonce);
            sb.Append(":");
            sb.Append(Cnonce);

            if (Authzid != null)
            {
                sb.Append(":");
                sb.Append(Authzid);
            }
            A1 = sb.ToString();


//			sb.Remove(0, sb.Length);			
//			sb.Append(Encoding.Default.GetChars(H1));
//			//sb.Append(Encoding.ASCII.GetChars(H1));
//			
//			sb.Append(A1);			
            var bA1 = Encoding.ASCII.GetBytes(A1);
            var bH1A1 = new byte[H1.Length + bA1.Length];

            //Array.Copy(H1, bH1A1, H1.Length);
            Array.Copy(H1, 0, bH1A1, 0, H1.Length);
            Array.Copy(bA1, 0, bH1A1, H1.Length, bA1.Length);
#if !CF
            H1 = new MD5CryptoServiceProvider().ComputeHash(bH1A1);
            //Console.WriteLine(util.Hash.HexToString(H1));
#else
    //H1 = Encoding.Default.GetBytes(util.Hash.MD5Hash(sb.ToString()));
    //H1 =util.Hash.MD5Hash(Encoding.Default.GetBytes(sb.ToString()));
			H1 =util.Hash.MD5Hash(bH1A1);
#endif
            sb.Remove(0, sb.Length);
            sb.Append("AUTHENTICATE:");
            sb.Append(DigestUri);
            if (Qop.CompareTo("auth") != 0)
            {
                sb.Append(":00000000000000000000000000000000");
            }
            A2 = sb.ToString();
            H2 = Encoding.ASCII.GetBytes(A2);

#if !CF
            H2 = new MD5CryptoServiceProvider().ComputeHash(H2);
#else
    //H2 = Encoding.Default.GetBytes(util.Hash.MD5Hash(H2));
			H2 =util.Hash.MD5Hash(H2);
#endif
            // create p1 and p2 as the hex representation of H1 and H2
            p1 = Hash.HexToString(H1).ToLower();
            p2 = Hash.HexToString(H2).ToLower();

            sb.Remove(0, sb.Length);
            sb.Append(p1);
            sb.Append(":");
            sb.Append(Nonce);
            sb.Append(":");
            sb.Append(Nc);
            sb.Append(":");
            sb.Append(Cnonce);
            sb.Append(":");
            sb.Append(Qop);
            sb.Append(":");
            sb.Append(p2);

            A3 = sb.ToString();
#if !CF
            H3 = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(A3));
#else
    //H3 = Encoding.Default.GetBytes(util.Hash.MD5Hash(A3));
			H3 =util.Hash.MD5Hash(Encoding.ASCII.GetBytes(A3));
#endif
            Response = Hash.HexToString(H3).ToLower();
        }

        private string GenerateMessage()
        {
            var sb = new StringBuilder();
            sb.Append("username=");
            sb.Append(AddQuotes(Username));
            sb.Append(",");
            sb.Append("realm=");
            sb.Append(AddQuotes(Realm));
            sb.Append(",");
            sb.Append("nonce=");
            sb.Append(AddQuotes(Nonce));
            sb.Append(",");
            sb.Append("cnonce=");
            sb.Append(AddQuotes(Cnonce));
            sb.Append(",");
            sb.Append("nc=");
            sb.Append(Nc);
            sb.Append(",");
            sb.Append("qop=");
            sb.Append(Qop);
            sb.Append(",");
            sb.Append("digest-uri=");
            sb.Append(AddQuotes(DigestUri));
            sb.Append(",");
            sb.Append("charset=");
            sb.Append(Charset);
            sb.Append(",");
            sb.Append("response=");
            sb.Append(Response);

            return sb.ToString();
        }

        /// <summary>
        ///     return the given string with quotes
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string AddQuotes(string s)
        {
            // fixed, s can be null (eg. for realm in ejabberd)
            if (s != null && s.Length > 0)
                s = s.Replace(@"\", @"\\");

            var quote = "\"";
            return quote + s + quote;
        }

        #region << Properties and member variables >>

        public string Cnonce { get; set; }

        public string Nc { get; set; }

        public string DigestUri { get; set; }

        public string Response { get; set; }

        public string Authzid { get; set; }

        #endregion
    }
}