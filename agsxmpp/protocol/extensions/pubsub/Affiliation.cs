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

namespace agsXMPP.protocol.extensions.pubsub
{
    /*
        <affiliation node='node1' jid='francisco@denmark.lit' affiliation='owner'/>
    */

    public class Affiliation : Element
    {
        public Jid Jid
        {
            get
            {
                if (HasAttribute("jid"))
                    return new Jid(GetAttribute("jid"));
                return null;
            }
            set
            {
                if (value != null)
                    SetAttribute("jid", value.ToString());
            }
        }

        public string Node
        {
            get { return GetAttribute("node"); }
            set { SetAttribute("node", value); }
        }

        public AffiliationType AffiliationType
        {
            get { return (AffiliationType) GetAttributeEnum("affiliation", typeof(AffiliationType)); }
            set { SetAttribute("affiliation", value.ToString()); }
        }

        #region << Constructors >>

        public Affiliation()
        {
            TagName = "affiliation";
            Namespace = Uri.PUBSUB;
        }

        public Affiliation(Jid jid, AffiliationType affiliation)
        {
            Jid = jid;
            AffiliationType = affiliation;
        }

        public Affiliation(string node, Jid jid, AffiliationType affiliation) : this(jid, affiliation)
        {
            Node = node;
        }

        #endregion
    }
}