using System.Xml.Linq;

namespace RtmpSharp.IO.AMF3.AMFWriters
{
    internal class Amf3XDocumentWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteMarker(Amf3TypeMarkers.Xml);
            writer.WriteAmf3XDocument(obj as XDocument);
        }
    }
}