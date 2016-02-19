using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.Sender = sender;
            this.MessageId = messageId;
            this.Subject = subject;
            this.Message = message;
            this.Timestamp = timestamp;
        }
    }
}