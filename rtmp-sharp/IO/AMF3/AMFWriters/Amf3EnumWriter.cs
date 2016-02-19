using System;

namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3EnumWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.Integer);
            writer.WriteAmf3Int(Convert.ToInt32(obj));
        }
    }
}