using System;
using RtmpSharp.Messaging;

namespace RtmpSharp.Net
{
    internal class EventReceivedEventArgs : EventArgs
    {
        public EventReceivedEventArgs(RtmpEvent @event)
        {
            Event = @event;
        }

        public RtmpEvent Event { get; set; }
    }
}