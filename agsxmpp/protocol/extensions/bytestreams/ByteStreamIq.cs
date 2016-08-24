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

namespace agsXMPP.protocol.extensions.bytestreams
{
    /// <summary>
    ///     a Bytestream IQ
    /// </summary>
    public class ByteStreamIq : IQ
    {
        public ByteStreamIq()
        {
            base.Query = Query;
            GenerateId();
        }

        public ByteStreamIq(IqType type) : this()
        {
            Type = type;
        }

        public ByteStreamIq(IqType type, Jid to)
            : this(type)
        {
            To = to;
        }

        public ByteStreamIq(IqType type, Jid to, Jid from)
            : this(type, to)
        {
            From = from;
        }

        public ByteStreamIq(IqType type, Jid to, Jid from, string Id)
            : this(type, to, from)
        {
            this.Id = Id;
        }

        public new ByteStream Query { get; } = new ByteStream();
    }
}