using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Sightstone.Chat
{
    public static class ChatStatic
    {
        public static ChatStatic.XmlStateTransformFunction XmlStateTransform;

        public static ChatStatic.JsonStateTransformFunction JsonStateTransform;

        public static ChatStatic.StringStateTransformFunction StringStateTransform;

        internal static object ParseJsonState(JObject obj)
        {
            if (ChatStatic.JsonStateTransform == null)
            {
                return null;
            }
            return ChatStatic.JsonStateTransform(obj);
        }

        internal static object ParseStringState(string str)
        {
            if (ChatStatic.StringStateTransform == null)
            {
                return str;
            }
            return ChatStatic.StringStateTransform(str);
        }

        internal static object ParseXmlState(XDocument document)
        {
            if (ChatStatic.XmlStateTransform == null)
            {
                return null;
            }
            return ChatStatic.XmlStateTransform(document);
        }

        public delegate object JsonStateTransformFunction(JObject obj);

        public delegate object StringStateTransformFunction(string str);

        public delegate object XmlStateTransformFunction(XDocument document);
    }
}