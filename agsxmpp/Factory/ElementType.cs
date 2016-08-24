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

namespace agsXMPP.Factory
{
    /// <summary>
    /// </summary>
    public class ElementType
    {
        private readonly string m_Namespace;
        private readonly string m_TagName;

        /// <summary>
        /// </summary>
        /// <param name="TagName"></param>
        /// <param name="Namespace"></param>
        public ElementType(string TagName, string Namespace)
        {
            m_TagName = TagName;
            m_Namespace = Namespace;
        }

        public override string ToString()
        {
            if ((m_Namespace != null) && (m_Namespace != string.Empty))
            {
                return m_Namespace + ":" + m_TagName;
            }
            return m_TagName;
        }
    }
}