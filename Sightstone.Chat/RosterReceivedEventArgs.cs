using System;

namespace Sightstone.Chat
{
    public class RosterReceivedEventArgs : EventArgs
    {
        public Contact[] Contacts;

        public RosterReceivedEventArgs(Contact[] contacts)
        {
            Contacts = contacts;
        }
    }
}