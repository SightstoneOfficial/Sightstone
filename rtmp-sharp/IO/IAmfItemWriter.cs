namespace RtmpSharp.IO
{
    internal interface IAmfItemWriter
    {
        void WriteData(AmfWriter writer, object obj);
    }
}