namespace RtmpSharp.IO.AMF0.AMFWriters
{
    internal class Amf0AsObjectWriter : IAmfItemWriter
    {
        public void WriteData(AmfWriter writer, object obj)
        {
            writer.WriteAmf0AsObject(obj as AsObject);
        }
    }
}