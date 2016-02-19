using System;

namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3DateTimeWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.Date);
            writer.WriteAmf3DateTime((DateTime) obj);
        }
    }
}