/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2012 by AG-Software 											 *
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

namespace agsXMPP.protocol.x.muc.iq.admin
{
    public class Item : muc.Item
    {
        /// <summary>
        /// </summary>
        public Item()
        {
            Namespace = Uri.MUC_ADMIN;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation"></param>
        public Item(Affiliation affiliation) : this()
        {
            Affiliation = affiliation;
        }

        public Item(Affiliation affiliation, Jid jid) : this(affiliation)
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="role"></param>
        public Item(Role role) : this()
        {
            Role = role;
        }

        public Item(Role role, Jid jid) : this(role)
        {
            Jid = jid;
        }

        public Item(Jid jid) : this()
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        public Item(Affiliation affiliation, Role role) : this(affiliation)
        {
            Role = role;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        /// <param name="jid"></param>
        public Item(Affiliation affiliation, Role role, Jid jid) : this(affiliation, role)
        {
            Jid = jid;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        /// <param name="reason"></param>
        public Item(Affiliation affiliation, Role role, string reason) : this(affiliation, role)
        {
            Reason = reason;
        }

        /// <summary>
        /// </summary>
        /// <param name="affiliation"></param>
        /// <param name="role"></param>
        /// <param name="jid"></param>
        /// <param name="reason"></param>
        public Item(Affiliation affiliation, Role role, Jid jid, string reason) : this(affiliation, role, jid)
        {
            Reason = reason;
        }
    }
}