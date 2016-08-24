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

using System;

namespace agsXMPP.Net.Dns
{
    /// <summary>
    ///     Summary description for SRVRecord.
    /// </summary>
    public class SRVRecord : RecordBase, IComparable
    {
        // the fields exposed outside the assembly

        /// <summary>
        ///     Constructs a NS record by reading bytes from a return message
        /// </summary>
        /// <param name="pointer">A logical pointer to the bytes holding the record</param>
        internal SRVRecord(Pointer pointer)
        {
            Priority = pointer.ReadShort();
            Weight = pointer.ReadShort();
            Port = pointer.ReadShort();
            Target = pointer.ReadDomain();
        }

        public int Priority { get; }

        public int Weight { get; }

        public int Port { get; }

        public string Target { get; }

        /// <summary>
        ///     Implements the IComparable interface so that we can sort the SRV records by their
        ///     lowest priority
        /// </summary>
        /// <param name="other">the other SRVRecord to compare against</param>
        /// <returns>1, 0, -1</returns>
        public int CompareTo(object obj)
        {
            var srvOther = (SRVRecord) obj;

            // we want to be able to sort them by priority from lowest to highest.
            if (Priority < srvOther.Priority) return -1;
            if (Priority > srvOther.Priority) return 1;

            // if the priority is the same, sort by highest weight to lowest (higher
            // weighting means that server should get more of the requests)
            if (Weight > srvOther.Weight) return -1;
            if (Weight < srvOther.Weight) return 1;

            return 0;
        }

        public override string ToString()
        {
            return string.Format("\n   priority   = {0}\n   weight     = {1}\n   port       = {2}\n   target     = {3}",
                Priority,
                Weight,
                Port,
                Target);
        }
    }
}