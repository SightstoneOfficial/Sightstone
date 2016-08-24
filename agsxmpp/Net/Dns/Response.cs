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

//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

using System;

namespace agsXMPP.Net.Dns
{
    /// <summary>
    ///     A Response is a logical representation of the byte data returned from a DNS query
    /// </summary>
    public class Response
    {
        // these are fields we're interested in from the message

        /// <summary>
        ///     Construct a Response object from the supplied byte array
        /// </summary>
        /// <param name="message">a byte array returned from a DNS server query</param>
        internal Response(byte[] message)
        {
            // the bit flags are in bytes 2 and 3
            var flags1 = message[2];
            var flags2 = message[3];

            // get return code from lowest 4 bits of byte 3
            var returnCode = flags2 & 15;

            // if its in the reserved section, set to other
            if (returnCode > 6) returnCode = 6;
            ReturnCode = (ReturnCode) returnCode;

            // other bit flags
            AuthoritativeAnswer = (flags1 & 4) != 0;
            RecursionAvailable = (flags2 & 128) != 0;
            MessageTruncated = (flags1 & 2) != 0;

            // create the arrays of response objects
            Questions = new Question[GetShort(message, 4)];
            Answers = new Answer[GetShort(message, 6)];
            NameServers = new NameServer[GetShort(message, 8)];
            AdditionalRecords = new AdditionalRecord[GetShort(message, 10)];

            // need a pointer to do this, position just after the header
            var pointer = new Pointer(message, 12);

            // and now populate them, they always follow this order
            for (var index = 0; index < Questions.Length; index++)
            {
                try
                {
                    // try to build a quesion from the response
                    Questions[index] = new Question(pointer);
                }
                catch (Exception ex)
                {
                    // something grim has happened, we can't continue
                    throw new InvalidResponseException(ex);
                }
            }
            for (var index = 0; index < Answers.Length; index++)
            {
                Answers[index] = new Answer(pointer);
            }
            for (var index = 0; index < NameServers.Length; index++)
            {
                NameServers[index] = new NameServer(pointer);
            }
            for (var index = 0; index < AdditionalRecords.Length; index++)
            {
                AdditionalRecords[index] = new AdditionalRecord(pointer);
            }
        }

        // these fields are readonly outside the assembly - use r/o properties
        public ReturnCode ReturnCode { get; }
        public bool AuthoritativeAnswer { get; }
        public bool RecursionAvailable { get; }
        public bool MessageTruncated { get; }
        public Question[] Questions { get; }
        public Answer[] Answers { get; }
        public NameServer[] NameServers { get; }
        public AdditionalRecord[] AdditionalRecords { get; }

        /// <summary>
        ///     Convert 2 bytes to a short. It would have been nice to use BitConverter for this,
        ///     it however reads the bytes in the wrong order (at least on Windows)
        /// </summary>
        /// <param name="message">byte array to look in</param>
        /// <param name="position">position to look at</param>
        /// <returns>short representation of the two bytes</returns>
        private static short GetShort(byte[] message, int position)
        {
            return (short) (message[position] << 8 | message[position + 1]);
        }
    }
}