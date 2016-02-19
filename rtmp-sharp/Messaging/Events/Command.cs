using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal enum CallStatus
    {
        Request,
        Result
    }

    internal class Method
    {
        internal Method(string methodName, object[] parameters, bool isSuccess = true,
            CallStatus status = CallStatus.Request)
        {
            Name = methodName;
            Parameters = parameters;
            IsSuccess = isSuccess;
            CallStatus = status;
        }

        public CallStatus CallStatus { get; internal set; }
        public string Name { get; internal set; }
        public bool IsSuccess { get; internal set; }
        public object[] Parameters { get; internal set; }
    }

    internal class Command : RtmpEvent
    {
        public Command(MessageType messageType) : base(messageType)
        {
        }

        public Method MethodCall { get; internal set; }
        public byte[] Buffer { get; internal set; }
        public int InvokeId { get; internal set; }
        public object ConnectionParameters { get; internal set; }
    }

    internal abstract class Invoke : Command
    {
        protected Invoke(MessageType messageType) : base(messageType)
        {
        }
    }

    internal abstract class Notify : Command
    {
        protected Notify(MessageType messageType) : base(messageType)
        {
        }
    }

    internal class InvokeAmf3 : Invoke
    {
        public InvokeAmf3() : base(MessageType.CommandAmf3)
        {
        }
    }

    internal class NotifyAmf3 : Notify
    {
        public NotifyAmf3() : base(MessageType.DataAmf3)
        {
        }
    }

    internal class InvokeAmf0 : Invoke
    {
        public InvokeAmf0() : base(MessageType.CommandAmf0)
        {
        }
    }

    internal class NotifyAmf0 : Notify
    {
        public NotifyAmf0() : base(MessageType.DataAmf0)
        {
        }
    }
}