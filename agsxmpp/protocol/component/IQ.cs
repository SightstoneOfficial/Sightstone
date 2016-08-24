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

#region Using directives

using agsXMPP.protocol.client;

#endregion

namespace agsXMPP.protocol.component
{
    /// <summary>
    ///     Summary description for Iq.
    /// </summary>
    public class IQ : client.IQ
    {
        /// <summary>
        ///     Error Child Element
        /// </summary>
        public new Error Error
        {
            get { return SelectSingleElement(typeof(Error)) as Error; }
            set
            {
                if (HasTag(typeof(Error)))
                    RemoveTag(typeof(Error));

                if (value != null)
                    AddChild(value);
            }
        }

        #region << Constructors >>

        public IQ()
        {
            Namespace = Uri.ACCEPT;
        }

        public IQ(IqType type) : base(type)
        {
            Namespace = Uri.ACCEPT;
        }

        public IQ(Jid from, Jid to) : base(from, to)
        {
            Namespace = Uri.ACCEPT;
        }

        public IQ(IqType type, Jid from, Jid to) : base(type, from, to)
        {
            Namespace = Uri.ACCEPT;
        }

        #endregion
    }
}