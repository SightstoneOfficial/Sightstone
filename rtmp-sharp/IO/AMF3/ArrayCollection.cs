using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace RtmpSharp.IO.AMF3
{
    [Serializable]
    [TypeConverter(typeof (ArrayCollectionConverter))]
    [SerializedName("flex.messaging.io.ArrayCollection")]
    public class ArrayCollection : List<object>, IExternalizable
    {
        public void ReadExternal(IDataInput input)
        {
            var obj = input.ReadObject() as object[];
            if (obj != null)
                AddRange(obj);
        }

        public void WriteExternal(IDataOutput output)
        {
            output.WriteObject(ToArray());
        }
    }

    public class ArrayCollectionConverter : TypeConverter
    {
        private static readonly Type[] ConvertibleTypes =
        {
            typeof (ArrayCollection),
            typeof (IList)
        };

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            return MiniTypeConverter.ConvertTo(value, destinationType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType.IsArray || ConvertibleTypes.Any(x => x == destinationType);
        }
    }
}