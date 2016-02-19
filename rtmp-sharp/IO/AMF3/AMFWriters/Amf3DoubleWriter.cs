using System;

namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3DoubleWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.Double);
            writer.WriteAmf3Double(Convert.ToDouble(obj));
        }
    }
}