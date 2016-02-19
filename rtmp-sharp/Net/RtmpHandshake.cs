using System.IO;
using System.Threading.Tasks;
using RtmpSharp.IO;

namespace RtmpSharp.Net
{
    internal struct RtmpHandshake
    {
        private const int HandshakeRandomSize = 1528;
        private const int HandshakeSize = 4 + 4 + HandshakeRandomSize;
        public byte[] Random;
        // C1/S1/C2/S2
        public uint Time;
        // in C1/S1, MUST be zero. in C2/S2, time at which C1/S1 was read.
        public uint Time2;
        // C0/S0 only
        public byte Version;

        public RtmpHandshake Clone()
        {
            return new RtmpHandshake
            {
                Version = Version,
                Time = Time,
                Time2 = Time2,
                Random = Random
            };
        }

        public static async Task<RtmpHandshake> ReadAsync(Stream stream, bool readVersion)
        {
            var size = HandshakeSize + (readVersion ? 1 : 0);
            var buffer = await StreamHelper.ReadBytesAsync(stream, size);

            using (var reader = new AmfReader(new MemoryStream(buffer), null))
            {
                return new RtmpHandshake
                {
                    Version = readVersion ? reader.ReadByte() : default(byte),
                    Time = reader.ReadUInt32(),
                    Time2 = reader.ReadUInt32(),
                    Random = reader.ReadBytes(HandshakeRandomSize)
                };
            }
        }

        public static RtmpHandshake Read(Stream stream, bool readVersion)
        {
            var size = HandshakeSize + (readVersion ? 1 : 0);
            var buffer = StreamHelper.ReadBytes(stream, size);

            using (var reader = new AmfReader(new MemoryStream(buffer), null))
            {
                return new RtmpHandshake
                {
                    Version = readVersion ? reader.ReadByte() : default(byte),
                    Time = reader.ReadUInt32(),
                    Time2 = reader.ReadUInt32(),
                    Random = reader.ReadBytes(HandshakeRandomSize)
                };
            }
        }

        public static Task WriteAsync(Stream stream, RtmpHandshake h, bool writeVersion)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new AmfWriter(memoryStream, null))
            {
                if (writeVersion)
                    writer.WriteByte(h.Version);

                writer.WriteUInt32(h.Time);
                writer.WriteUInt32(h.Time2);
                writer.WriteBytes(h.Random);

                var buffer = memoryStream.ToArray();
                return stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public static Task WriteAsync(Stream stream, RtmpHandshake h, RtmpHandshake h2, bool writeVersion)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new AmfWriter(memoryStream, null))
            {
                if (writeVersion)
                    writer.WriteByte(h.Version);

                writer.WriteUInt32(h.Time);
                writer.WriteUInt32(h.Time2);
                writer.WriteBytes(h.Random);

                writer.WriteUInt32(h2.Time);
                writer.WriteUInt32(h2.Time2);
                writer.WriteBytes(h2.Random);

                var buffer = memoryStream.ToArray();
                return stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
    }
}