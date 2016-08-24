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

using agsXMPP.protocol.client;

// Request Agents:
// <iq id='someid' to='myjabber.net' type='get'>
//		<query xmlns='jabber:iq:agents'/>
// </iq>

namespace agsXMPP.protocol.iq.agent
{
    /// <summary>
    ///     Summary description for AgentsIq.
    /// </summary>
    public class AgentsIq : IQ
    {
        public AgentsIq()
        {
            base.Query = Query;
            GenerateId();
        }

        public AgentsIq(IqType type) : this()
        {
            Type = type;
        }

        public AgentsIq(IqType type, Jid to) : this(type)
        {
            To = to;
        }

        public AgentsIq(IqType type, Jid to, Jid from) : this(type, to)
        {
            From = from;
        }

        public new Agents Query { get; } = new Agents();
    }
}