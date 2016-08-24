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

using agsXMPP.protocol.client;

namespace agsXMPP.protocol.component
{
    /// <summary>
    ///     Summary description for Error.
    /// </summary>
    public class Error : client.Error
    {
        public Error()
        {
            Namespace = Uri.ACCEPT;
        }

        public Error(int code)
            : base(code)
        {
            Namespace = Uri.ACCEPT;
        }

        public Error(ErrorCode code)
            : base(code)
        {
            Namespace = Uri.ACCEPT;
        }

        public Error(ErrorType type)
            : base(type)
        {
            Namespace = Uri.ACCEPT;
        }

        /// <summary>
        ///     Creates an error Element according the the condition
        ///     The type attrib as added automatically as decribed in the XMPP specs
        ///     This is the prefered way to create error Elements
        /// </summary>
        /// <param name="condition"></param>
        public Error(ErrorCondition condition)
            : base(condition)
        {
            Namespace = Uri.ACCEPT;
        }
    }
}