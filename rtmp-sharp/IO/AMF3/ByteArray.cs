using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Complete.IO.Zlib;

namespace RtmpSharp.IO.AMF3
{
    [TypeConverter(typeof (ByteArrayConverter))]
    [Serializable]
    [SerializedName("flex.messaging.io.ByteArray")]
    public class ByteArray
    {
        public enum CompressionAlgorithm
        {
            Deflate,
            Zlib
        }

        private readonly SerializationContext serializationContext;
        private DataInput dataInput;
        private DataOutput dataOutput;
        private ObjectEncoding objectEncoding = ObjectEncoding.Amf3;

        public ByteArray()
        {
            MemoryStream = new MemoryStream();
            ReloadStreams();
        }

        public ByteArray(SerializationContext serializationContext) : this()
        {
            this.serializationContext = serializationContext;
        }

        public ByteArray(MemoryStream ms, SerializationContext serializationContext)
        {
            this.serializationContext = serializationContext;

            MemoryStream = ms;
            ReloadStreams();
        }

        public ByteArray(byte[] buffer, SerializationContext serializationContext)
        {
            this.serializationContext = serializationContext;

            MemoryStream = new MemoryStream(buffer);
            ReloadStreams();
        }

        public uint Length
        {
            get { return (uint) MemoryStream.Length; }
        }

        public uint Position
        {
            get { return (uint) MemoryStream.Position; }
            set { MemoryStream.Position = value; }
        }

        public uint BytesAvailable
        {
            get { return Length - Position; }
        }

        internal MemoryStream MemoryStream { get; private set; }

        public ObjectEncoding ObjectEncoding
        {
            get { return objectEncoding; }
            set
            {
                objectEncoding = value;
                dataInput.ObjectEncoding = value;
                dataOutput.ObjectEncoding = value;
            }
        }

        private void ReloadStreams()
        {
            dataOutput = new DataOutput(new AmfWriter(MemoryStream, serializationContext, objectEncoding));
            dataInput = new DataInput(new AmfReader(MemoryStream, serializationContext));
        }

        // The data array backing this ByteArray
        public byte[] GetBuffer()
        {
            return MemoryStream.GetBuffer();
        }

        // Returns a byte[] of the current ByteArray from start to end, without regard for the current stream position
        public byte[] ToArray()
        {
            return MemoryStream.ToArray();
        }

        public void Compress()
        {
            Compress(CompressionAlgorithm.Zlib);
        }

        public void Deflate()
        {
            Compress(CompressionAlgorithm.Deflate);
        }

        public void Compress(CompressionAlgorithm algorithm)
        {
            var buffer = MemoryStream.ToArray();
            MemoryStream.Close();
            var ms = new MemoryStream();

            var stream = algorithm == CompressionAlgorithm.Zlib
                ? new ZlibStream(ms, CompressionMode.Compress, true)
                : new DeflateStream(ms, CompressionMode.Compress, true);

            using (stream)
                stream.Write(buffer, 0, buffer.Length);

            MemoryStream = ms;
            dataOutput = new DataOutput(new AmfWriter(MemoryStream, serializationContext));
            dataInput = new DataInput(new AmfReader(MemoryStream, serializationContext));
        }

        public void Inflate()
        {
            Uncompress(CompressionAlgorithm.Deflate);
        }

        public void Uncompress()
        {
            Uncompress(CompressionAlgorithm.Zlib);
        }

        public void Uncompress(CompressionAlgorithm algorithm)
        {
            Position = 0;
            var ms = new MemoryStream();
            var buffer = new byte[1024];

            // The zlib format is specified by RFC 1950. Zlib also uses deflate, plus 2 or 6 header bytes, and a 4 byte checksum at the end. 
            // The first 2 bytes indicate the compression method and flags. If the dictionary flag is set, then 4 additional bytes will follow.
            // Preset dictionaries aren't very common and we don't support them
            var deflateStream = algorithm == CompressionAlgorithm.Zlib
                ? new ZlibStream(MemoryStream, CompressionMode.Decompress, false)
                : new DeflateStream(MemoryStream, CompressionMode.Decompress, false);

            while (true)
            {
                var readCount = deflateStream.Read(buffer, 0, buffer.Length);
                if (readCount == 0)
                    break;
                ms.Write(buffer, 0, readCount);
            }

            MemoryStream.Dispose();
            MemoryStream = ms;
            MemoryStream.Position = 0;
            dataOutput = new DataOutput(new AmfWriter(MemoryStream, serializationContext));
            dataInput = new DataInput(new AmfReader(MemoryStream, serializationContext));
        }

        #region IDataInput Members

        public bool ReadBoolean()
        {
            return dataInput.ReadBoolean();
        }

        public byte ReadByte()
        {
            return dataInput.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return dataInput.ReadBytes(count);
        }

        public double ReadDouble()
        {
            return dataInput.ReadDouble();
        }

        public float ReadFloat()
        {
            return dataInput.ReadFloat();
        }

        public short ReadInt16()
        {
            return dataInput.ReadInt16();
        }

        public int ReadInt32()
        {
            return dataInput.ReadInt32();
        }

        public object ReadObject()
        {
            return dataInput.ReadObject();
        }

        public ushort ReadUInt16()
        {
            return dataInput.ReadUInt16();
        }

        public int ReadUInt24()
        {
            return dataInput.ReadUInt24();
        }

        public uint ReadUInt32()
        {
            return dataInput.ReadUInt32();
        }

        public string ReadUtf()
        {
            return dataInput.ReadUtf();
        }

        public string ReadUtf(int length)
        {
            return dataInput.ReadUtf(length);
        }

        #endregion

        #region IDataOutput members

        public void WriteBoolean(bool value)
        {
            dataOutput.WriteBoolean(value);
        }

        public void WriteByte(byte value)
        {
            dataOutput.WriteByte(value);
        }

        public void WriteBytes(byte[] buffer)
        {
            dataOutput.WriteBytes(buffer);
        }

        public void WriteDouble(double value)
        {
            dataOutput.WriteDouble(value);
        }

        public void WriteFloat(float value)
        {
            dataOutput.WriteFloat(value);
        }

        public void WriteInt16(short value)
        {
            dataOutput.WriteInt16(value);
        }

        public void WriteInt32(int value)
        {
            dataOutput.WriteInt32(value);
        }

        public void WriteObject(object value)
        {
            dataOutput.WriteObject(value);
        }

        public void WriteUInt16(ushort value)
        {
            dataOutput.WriteUInt16(value);
        }

        public void WriteUInt24(int value)
        {
            dataOutput.WriteUInt24(value);
        }

        public void WriteUInt32(uint value)
        {
            dataOutput.WriteUInt32(value);
        }

        public void WriteUtf(string value)
        {
            dataOutput.WriteUtf(value);
        }

        public void WriteUtfBytes(string value)
        {
            dataOutput.WriteUtfBytes(value);
        }

        #endregion
    }

    public class ByteArrayConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (byte[]))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (destinationType == typeof (byte[]))
            {
                return ((ByteArray) value).MemoryStream.ToArray();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}