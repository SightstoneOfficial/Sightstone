using System;

namespace Sightstone.Chat
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Contact Sender;

        public string MessageId;

        public string Subject;

        public string Message;

        public DateTime Timestamp;

        public MessageReceivedEventArgs(Contact sender, string messageId, string subject, string message, DateTime timestamp)
        {
            Sender = sender;
            MessageId = messageId;
            Subject = subject;
            Message = message;
            Timestamp = timestamp;
        }
    }
}