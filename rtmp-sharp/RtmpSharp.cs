using System.ComponentModel;
using StringConverter = RtmpSharp.IO.TypeConverters.StringConverter;

namespace RtmpSharp
{
    public static class TypeSerializer
    {
        public static void RegisterTypeConverters()
        {
            TypeDescriptor.AddAttributes(typeof (string), new TypeConverterAttribute(typeof (StringConverter)));
        }
    }
}