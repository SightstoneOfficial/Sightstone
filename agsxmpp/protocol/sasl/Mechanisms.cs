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

using agsXMPP.Xml.Dom;

//	<mechanisms xmlns='urn:ietf:params:xml:ns:xmpp-sasl'>
//		<mechanism>DIGEST-MD5</mechanism>
//		<mechanism>PLAIN</mechanism>
//	</mechanisms>

namespace agsXMPP.protocol.sasl
{
    /// <summary>
    ///     Summary description for Mechanisms.
    /// </summary>
    public class Mechanisms : Element
    {
        public Mechanisms()
        {
            TagName = "mechanisms";
            Namespace = Uri.SASL;
        }

        public Mechanism[] GetMechanisms()
        {
            var elements = SelectElements("mechanism");

            var items = new Mechanism[elements.Count];
            var i = 0;
            foreach (Element e in elements)
            {
                items[i] = (Mechanism) e;
                i++;
            }
            return items;
        }

        public bool SupportsMechanism(MechanismType type)
        {
            foreach (var m in GetMechanisms())
            {
                if (m.MechanismType == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     Gets the given mechanism.
        /// </summary>
        /// <param name="type">The mechanism type.</param>
        /// <returns></returns>
        public Mechanism GetMechanism(MechanismType type)
        {
            foreach (var m in GetMechanisms())
            {
                if (m.MechanismType == type)
                    return m;
            }
            return null;
        }
    }
}