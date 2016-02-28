using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Sightstone.Chat
{
    public static class ChatStatic
    {
        public static XmlStateTransformFunction XmlStateTransform;

        public static JsonStateTransformFunction JsonStateTransform;

        public static StringStateTransformFunction StringStateTransform;

        internal static object ParseJsonState(JObject obj)
        {
            return JsonStateTransform?.Invoke(obj);
        }

        internal static object ParseStringState(string str)
        {
            return StringStateTransform == null ? str : StringStateTransform(str);
        }

        internal static object ParseXmlState(XDocument document)
        {
            return XmlStateTransform?.Invoke(document);
        }

        public delegate object JsonStateTransformFunction(JObject obj);

        public delegate object StringStateTransformFunction(string str);

        public delegate object XmlStateTransformFunction(XDocument document);
    }
}