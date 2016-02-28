using System;

namespace Sightstone.Chat
{
    public class ErrorReceivedEventArgs : EventArgs
    {
        public readonly string Jid;

        public readonly string Category;

        public readonly string Reason;

        public readonly string Detail;

        public ErrorReceivedEventArgs(string jid, string category, string reason, string detail)
        {
            Jid = jid;
            Category = category;
            Reason = reason;
            Detail = detail;
        }
    }
}