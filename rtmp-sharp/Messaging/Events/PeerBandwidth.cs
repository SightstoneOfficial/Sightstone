using RtmpSharp.Net;

namespace RtmpSharp.Messaging.Events
{
    internal class PeerBandwidth : RtmpEvent
    {
        public enum BandwidthLimitType : byte
        {
            Hard = 0,
            Soft = 1,
            Dynamic = 2
        }

        private PeerBandwidth() : base(MessageType.SetPeerBandwidth)
        {
        }

        public PeerBandwidth(int acknowledgementWindowSize, BandwidthLimitType limitType) : this()
        {
            AcknowledgementWindowSize = acknowledgementWindowSize;
            LimitType = limitType;
        }

        public PeerBandwidth(int acknowledgementWindowSize, byte limitType) : this()
        {
            AcknowledgementWindowSize = acknowledgementWindowSize;
            LimitType = (BandwidthLimitType) limitType;
        }

        public int AcknowledgementWindowSize { get; private set; }
        public BandwidthLimitType LimitType { get; private set; }
    }
}