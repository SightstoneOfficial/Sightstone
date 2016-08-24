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

namespace agsXMPP.protocol.extensions.si
{
    /*
    <iq id="jcl_18" to="xxx" type="result" from="yyy">
        <si xmlns="http://jabber.org/protocol/si">
            <feature xmlns="http://jabber.org/protocol/feature-neg">
                <x xmlns="jabber:x:data" type="submit">
                    <field var="stream-method">
                        <value>http://jabber.org/protocol/bytestreams</value>
                    </field>
                </x>
            </feature>
        </si>
    </iq>
 
    */

    /// <summary>
    /// </summary>
    public class SIIq : IQ
    {
        public SIIq()
        {
            GenerateId();
            AddChild(SI);
        }

        public SIIq(IqType type)
            : this()
        {
            Type = type;
        }

        public SIIq(IqType type, Jid to)
            : this(type)
        {
            To = to;
        }

        public SIIq(IqType type, Jid to, Jid from)
            : this(type, to)
        {
            From = from;
        }

        public SI SI { get; } = new SI();
    }
}