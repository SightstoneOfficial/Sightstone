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

namespace agsXMPP.protocol.iq.time
{
    /// <summary>
    ///     Summary description for TimeIq.
    /// </summary>
    public class TimeIq : IQ
    {
        public TimeIq()
        {
            base.Query = Query;
            GenerateId();
        }

        public TimeIq(IqType type) : this()
        {
            Type = type;
        }

        public TimeIq(IqType type, Jid to) : this(type)
        {
            To = to;
        }

        public TimeIq(IqType type, Jid to, Jid from) : this(type, to)
        {
            From = from;
        }

        public new Time Query { get; } = new Time();
    }
}