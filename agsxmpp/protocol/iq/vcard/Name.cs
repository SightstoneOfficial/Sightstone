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

namespace agsXMPP.protocol.iq.vcard
{
    /// <summary>
    /// </summary>
    public class Name : Element
    {
        public string Family
        {
            get { return GetTag("FAMILY"); }
            set { SetTag("FAMILY", value); }
        }

        public string Given
        {
            get { return GetTag("GIVEN"); }
            set { SetTag("GIVEN", value); }
        }

        public string Middle
        {
            get { return GetTag("MIDDLE"); }
            set { SetTag("MIDDLE", value); }
        }

        // <N>
        //	<FAMILY>Saint-Andre<FAMILY>
        //	<GIVEN>Peter</GIVEN>
        //	<MIDDLE/>
        // </N>

        #region << Constructors >>

        public Name()
        {
            TagName = "N";
            Namespace = Uri.VCARD;
        }

        public Name(string family, string given, string middle) : this()
        {
            Family = family;
            Given = given;
            Middle = middle;
        }

        #endregion
    }
}