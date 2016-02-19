using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sightstone.Chat
{
    public class ErrorReceivedEventArgs : EventArgs
    {
        private string Jid;

        private string Category;

        private string Reason;

        private string Detail;

        public ErrorReceivedEventArgs(string jid, string category, string reason, string detail)
        {
            this.Jid = jid;
            this.Category = category;
            this.Reason = reason;
            this.Detail = detail;
        }
    }
}