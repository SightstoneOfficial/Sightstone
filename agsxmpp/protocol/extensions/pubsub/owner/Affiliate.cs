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

namespace agsXMPP.protocol.extensions.pubsub.owner
{
    /*
        <iq type='result'
            from='pubsub.shakespeare.lit'
            to='hamlet@denmark.lit/elsinore'
            id='ent1'>
          <pubsub xmlns='http://jabber.org/protocol/pubsub#owner'>
            <affiliates node='blogs/princely_musings'>
              <affiliate jid='hamlet@denmark.lit' affiliation='owner'/>
              <affiliate jid='polonius@denmark.lit' affiliation='outcast'/>
            </affiliates>
          </pubsub>
        </iq>
        
        <xs:element name='affiliate'>
            <xs:complexType>
              <xs:simpleContent>
                <xs:extension base='empty'>
                  <xs:attribute name='affiliation' use='required'>
                    <xs:simpleType>
                      <xs:restriction base='xs:NCName'>
                        <xs:enumeration value='none'/>
                        <xs:enumeration value='outcast'/>
                        <xs:enumeration value='owner'/>
                        <xs:enumeration value='publisher'/>
                      </xs:restriction>
                    </xs:simpleType>
                  </xs:attribute>
                  <xs:attribute name='jid' type='xs:string' use='required'/>
                </xs:extension>
              </xs:simpleContent>
            </xs:complexType>
        </xs:element>
     
    */

    public class Affiliate : Element
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

        public AffiliationType Affiliation
        {
            get { return (AffiliationType) GetAttributeEnum("affiliation", typeof(AffiliationType)); }
            set { SetAttribute("affiliation", value.ToString()); }
        }

        #region << Constructors >>

        public Affiliate()
        {
            TagName = "affiliate";
            Namespace = Uri.PUBSUB_OWNER;
        }

        public Affiliate(Jid jid, AffiliationType affiliation) : this()
        {
            Jid = jid;
            Affiliation = affiliation;
        }

        #endregion
    }
}