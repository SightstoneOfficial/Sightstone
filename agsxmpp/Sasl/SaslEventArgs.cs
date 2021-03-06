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

using agsXMPP.protocol.sasl;

namespace agsXMPP.Sasl
{
    public delegate void SaslEventHandler(object sender, SaslEventArgs args);

    public class SaslEventArgs
    {
        // by default the library chooses the auth method

        /// <summary>
        ///     Set Auto to true if the library should choose the mechanism
        ///     Set it to false for choosing the authentication method yourself
        /// </summary>
        public bool Auto { get; set; } = true;

        /// <summary>
        ///     SASL Mechanism for authentication as string
        /// </summary>
        public string Mechanism { get; set; }

        public Mechanisms Mechanisms { get; set; }

        /// <summary>
        ///     Extra Data for special Sasl mechanisms
        /// </summary>
        public ExtendedData ExtentedData { get; set; }

        #region << Constructors >>

        public SaslEventArgs()
        {
        }

        public SaslEventArgs(Mechanisms mechanisms)
        {
            Mechanisms = mechanisms;
        }

        #endregion
    }
}