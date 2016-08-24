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
using agsXMPP.protocol.sasl;
using agsXMPP.Sasl.Anonymous;
using agsXMPP.Sasl.DigestMD5;
using agsXMPP.Sasl.Facebook;
using agsXMPP.Sasl.Plain;
using agsXMPP.Sasl.XGoogleToken;
using Mechanism = agsXMPP.Sasl.Mechanism;
#if !(CF || CF_2)
using agsXMPP.Sasl.Scram;
using agsXMPP.Sasl.Gssapi;

#endif

namespace agsXMPP.Factory
{
    /// <summary>
    ///     SASL factory
    /// </summary>
    public class SaslFactory
    {
        /// <summary>
        ///     This Hashtable stores Mapping of mechanism <--> SASL class in agsXMPP
        /// </summary>
        private static readonly Hashtable m_table = new Hashtable();

        static SaslFactory()
        {
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.PLAIN), typeof(PlainMechanism));
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.DIGEST_MD5), typeof(DigestMD5Mechanism));
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.ANONYMOUS), typeof(AnonymousMechanism));
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.X_GOOGLE_TOKEN),
                typeof(XGoogleTokenMechanism));
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.X_FACEBOOK_PLATFORM),
                typeof(FacebookMechanism));
#if !(CF || CF_2)
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.SCRAM_SHA_1), typeof(ScramSha1Mechanism));
            AddMechanism(protocol.sasl.Mechanism.GetMechanismName(MechanismType.GSSAPI), typeof(GssapiMechanism));
#endif
        }


        public static Mechanism GetMechanism(string mechanism)
        {
            var t = (Type) m_table[mechanism];
            if (t != null)
                return (Mechanism) Activator.CreateInstance(t);
            return null;
        }

        /// <summary>
        ///     Adds new Element Types to the Hashtable
        ///     Use this function to register new SASL mechanisms
        /// </summary>
        /// <param name="mechanism"></param>
        /// <param name="t"></param>
        public static void AddMechanism(string mechanism, Type t)
        {
            m_table.Add(mechanism, t);
        }
    }
}