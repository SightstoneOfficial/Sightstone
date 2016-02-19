using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.Chat
{
    public class RosterReceivedEventArgs : EventArgs
    {
        public Contact[] Contacts;

        public RosterReceivedEventArgs(Contact[] contacts)
        {
            this.Contacts = contacts;
        }
    }
}