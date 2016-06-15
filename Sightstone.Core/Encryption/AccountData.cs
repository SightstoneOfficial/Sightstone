using Sightstone.Core.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.Core.Encryption
{
    public class AccountData
    {

    }
    public class RiotAccountData
    {
        /// <summary>
        /// Riot login info
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Riot login info
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Riot sum info
        /// </summary>
        public int UserIcon { get; set; }

        /// <summary>
        /// Riot sum info
        /// </summary>
        public string SumName { get; set; }

        /// <summary>
        /// Riot region
        /// </summary>
        public BaseRegion Region { get; set; }

        /// <summary>
        /// Sightstone login data
        /// </summary>
        public DefaultLoginStatus DefaultLoginStatus { get; set; }

    }
    public enum DefaultLoginStatus
    {
        Available,
        Away,
        Invisible,
        /// <summary>
        /// Will not sign in
        /// </summary>
        Offline
    }
}
