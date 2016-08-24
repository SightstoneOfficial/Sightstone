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

//	Example 1. Requesting Search Fields
//
//	<iq type='get'
//		from='romeo@montague.net/home'
//		to='characters.shakespeare.lit'
//		id='search1'
//		xml:lang='en'>
//		<query xmlns='jabber:iq:search'/>
//	</iq>

namespace agsXMPP.protocol.iq.search
{
    /// <summary>
    ///     Summary description for SearchIq.
    /// </summary>
    public class SearchIq : IQ
    {
        public SearchIq()
        {
            base.Query = Query;
            GenerateId();
        }

        public SearchIq(IqType type) : this()
        {
            Type = type;
        }

        public SearchIq(IqType type, Jid to) : this(type)
        {
            To = to;
        }

        public SearchIq(IqType type, Jid to, Jid from) : this(type, to)
        {
            From = from;
        }

        public new Search Query { get; } = new Search();
    }
}