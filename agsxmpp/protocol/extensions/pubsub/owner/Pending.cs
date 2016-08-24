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

using agsXMPP.protocol.x.data;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.extensions.pubsub.owner
{
    public class Pending : Element
    {
        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        /// <summary>
        ///     The x-Data Element
        /// </summary>
        public Data Data
        {
            get { return SelectSingleElement(typeof(Data)) as Data; }
            set
            {
                if (HasTag(typeof(Data)))
                    RemoveTag(typeof(Data));

                if (value != null)
                    AddChild(value);
            }
        }

        #region << Constructors >>

        public Pending()
        {
            TagName = "pending";
            Namespace = Uri.PUBSUB_OWNER;
        }

        public Pending(string node) : this()
        {
            Node = node;
        }

        #endregion
    }
}