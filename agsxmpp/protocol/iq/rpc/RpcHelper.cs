using System;
using System.Collections;
using System.Globalization;
using agsXMPP.Util;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.iq.rpc
{
    internal class RpcHelper
    {
        public static Element WriteParams(ArrayList Params)
        {
            if (Params != null && Params.Count > 0)
            {
                var elParams = new Element("params");

                for (var i = 0; i < Params.Count; i++)
                {
                    var param = new Element("param");
                    WriteValue(Params[i], param);
                    elParams.AddChild(param);
                }
                return elParams;
            }
            return null;
        }

        /// <summary>
        ///     Writes a single value to a call
        /// </summary>
        /// <param name="param"></param>
        /// <param name="parent"></param>
        public static void WriteValue(object param, Element parent)
        {
            var value = new Element("value");

            if (param is string)
            {
                value.AddChild(new Element("string", param as string));
            }
            else if (param is int)
            {
                value.AddChild(new Element("i4", ((int) param).ToString()));
            }
            else if (param is double)
            {
                var numberInfo = new NumberFormatInfo();
                numberInfo.NumberDecimalSeparator = ".";
                //numberInfo.NumberGroupSeparator = ",";
                value.AddChild(new Element("double", ((double) param).ToString(numberInfo)));
            }
            else if (param is bool)
            {
                value.AddChild(new Element("boolean", (bool) param ? "1" : "0"));
            }
            // XML-RPC dates are formatted in iso8601 standard, same as xmpp,
            else if (param is DateTime)
            {
                value.AddChild(new Element("dateTime.iso8601", Time.ISO_8601Date((DateTime) param)));
            }
            // byte arrays must be encoded in Base64 encoding
            else if (param is byte[])
            {
                var b = (byte[]) param;
                value.AddChild(new Element("base64", Convert.ToBase64String(b, 0, b.Length)));
            }
            // Arraylist maps to an XML-RPC array
            else if (param is ArrayList)
            {
                //<array>  
                //    <data>
                //        <value>  <string>one</string>  </value>
                //        <value>  <string>two</string>  </value>
                //        <value>  <string>three</string></value>  
                //    </data> 
                //</array>
                var array = new Element("array");
                var data = new Element("data");

                var list = param as ArrayList;

                for (var i = 0; i < list.Count; i++)
                {
                    WriteValue(list[i], data);
                }

                array.AddChild(data);
                value.AddChild(array);
            }
            // java.util.Hashtable maps to an XML-RPC struct
            else if (param is Hashtable)
            {
                var elStruct = new Element("struct");

                var ht = (Hashtable) param;
                var myEnumerator = ht.Keys.GetEnumerator();
                while (myEnumerator.MoveNext())
                {
                    var member = new Element("member");
                    var key = myEnumerator.Current;

                    if (key != null)
                    {
                        member.AddChild(new Element("name", key.ToString()));
                        WriteValue(ht[key], member);
                    }

                    elStruct.AddChild(member);
                }

                value.AddChild(elStruct);
            }
            /*
            else
            {
                // Unknown Type
            }
            */
            parent.AddChild(value);
        }
    }
}