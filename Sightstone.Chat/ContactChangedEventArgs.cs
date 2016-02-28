using System;

namespace Sightstone.Chat
{
    public class ContactChangedEventArgs : EventArgs
    {
        public Contact Contact;

        public ContactChangeType ChangeType;

        public ContactChangedEventArgs(Contact contact, ContactChangeType type)
        {
            Contact = contact;
            ChangeType = type;
        }
    }
}