using System;

namespace RtmpSharp.IO.AMF0.AMFWriters
{
    internal class Amf0EnumWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf0TypeMarkers.Number);
            writer.WriteDouble(Convert.ToDouble(obj));
        }
    }
}