using System;

namespace RtmpSharp.IO.AMF0.AMFWriters
{
    internal class Amf0DateTimeWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf0TypeMarkers.Date);
            writer.WriteAmf0DateTime((DateTime) obj);
        }
    }
}