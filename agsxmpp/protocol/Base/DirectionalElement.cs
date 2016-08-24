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

namespace agsXMPP.protocol.Base
{
    /// <summary>
    ///     Base XMPP Element
    ///     This must ne used to build all other new packets
    /// </summary>
    public abstract class DirectionalElement : Element
    {
        public DirectionalElement()
        {
        }

        public DirectionalElement(string tag)
            : base(tag)
        {
        }

        public DirectionalElement(string tag, string ns)
            : base(tag)
        {
            Namespace = ns;
        }

        public DirectionalElement(string tag, string text, string ns)
            : base(tag, text)
        {
            Namespace = ns;
        }

        public Jid From
        {
            get
            {
                if (HasAttribute("from"))
                    return new Jid(GetAttribute("from"));
                return null;
            }
            set
            {
                if (value != null)
                    SetAttribute("from", value.ToString());
                else
                    RemoveAttribute("from");
            }
        }

        public Jid To
        {
            get
            {
                if (HasAttribute("to"))
                    return new Jid(GetAttribute("to"));
                return null;
            }
            set
            {
                if (value != null)
                    SetAttribute("to", value.ToString());
                else
                    RemoveAttribute("to");
            }
        }

        /// <summary>
        ///     Switches the from and to attributes when existing
        /// </summary>
        public void SwitchDirection()
        {
            var from = From;
            var to = To;

            // Remove from and to now
            RemoveAttribute("from");
            RemoveAttribute("to");

            Jid helper = null;

            helper = from;
            from = to;
            to = helper;

            From = from;
            To = to;
        }
    }
}