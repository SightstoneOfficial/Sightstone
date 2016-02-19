using RtmpSharp.IO;
using RtmpSharp.Messaging.Messages;

namespace RtmpSharp.Messaging
{
    public class ConnectMessageEventArgs : CommandMessageReceivedEventArgs
    {
        public readonly string AuthToken;
        public readonly string ClientId;
        public readonly AsObject ConnectionParameters;

        internal ConnectMessageEventArgs(string clientId, string authToken, CommandMessage message, string endpoint,
            string dsId, int invokeId, AsObject cParameters) : base(message, endpoint, dsId, invokeId)
        {
            ClientId = clientId;
            AuthToken = authToken;
            ConnectionParameters = cParameters;
        }
    }
}