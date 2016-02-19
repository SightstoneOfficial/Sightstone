namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3CharWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.String);
            writer.WriteAmf3Utf(obj.ToString());
        }
    }
}