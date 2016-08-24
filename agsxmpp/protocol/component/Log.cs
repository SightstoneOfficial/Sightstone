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

using agsXMPP.protocol.Base;

namespace agsXMPP.protocol.component
{
    public enum LogType
    {
        NONE = -1,
        warn,
        info,
        verbose,
        debug,
        notice
    }

    /// <summary>
    ///     Zusammenfassung für Log.
    /// </summary>
    public class Log : Stanza
    {
        public Log()
        {
            TagName = "log";
            Namespace = Uri.ACCEPT;
        }

        /// <summary>
        ///     creates a new Log Packet with the given message
        /// </summary>
        /// <param name="message"></param>
        public Log(string message) : this()
        {
            Value = message;
        }


        /// <summary>
        ///     Gets or Sets the logtype
        /// </summary>
        public LogType Type
        {
            get { return (LogType) GetAttributeEnum("type", typeof(LogType)); }
            set
            {
                if (value == LogType.NONE)
                    RemoveAttribute("type");
                else
                    SetAttribute("type", value.ToString());
            }
        }

        /// <summary>
        ///     The namespace for logging
        /// </summary>
        public string LogNamespace
        {
            get { return GetAttribute("ns"); }
            set { SetAttribute("ns", value); }
        }
    }
}