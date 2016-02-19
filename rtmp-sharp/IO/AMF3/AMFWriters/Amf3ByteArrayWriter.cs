namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3ByteArrayWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.ByteArray);
            writer.WriteAmf3ByteArray(obj as ByteArray);
        }
    }
}