using System;

namespace RtmpSharp.IO.AMF0.AMFWriters
{
    internal class Amf0GuidWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf0TypeMarkers.String);
            writer.WriteUtfPrefixed(((Guid) obj).ToString());
        }
    }
}