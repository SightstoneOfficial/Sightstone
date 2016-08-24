/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2016 by AG-Software 											 *
 * All Rights Reserved.																 *
 * Contact information for AG-Software is available at http://www.ag-software.de	 *
 *																					 *
 * Licence:																			 *
 * The agsXMPP SDK is released under a dual licence									 *
 * agsXMPP can be used under either of two licences									 *
 * 																					 *
 * A commercial licence which is probably the most appropriate for commercial 		 *
 * corporate use and closed source projects. 										 *
 *																					 *
 * The GNU Public License (GPL) is probably most appropriate for inclusion in		 *
 * other open source projects.														 *
 *																					 *
 * See README.html for details.														 *
 *																					 *
 * For general enquiries visit our website at:										 *
 * http://www.ag-software.de														 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections;
using System.Globalization;
using agsXMPP.Exceptions;
using agsXMPP.Util;
using agsXMPP.Xml.Dom;

namespace agsXMPP.protocol.iq.rpc
{
    /// <summary>
    ///     The method Response element.
    /// </summary>
    public class MethodResponse : Element
    {
        /*
         
         <methodResponse>
           <fault>
              <value>
                 <struct>
                    <member>
                       <name>faultCode</name>
                       <value><int>4</int></value>
                       </member>
                    <member>
                       <name>faultString</name>
                       <value><string>Too many parameters.</string></value>
                       </member>
                    </struct>
                 </value>
              </fault>
           </methodResponse>
         
         */

        public MethodResponse()
        {
            TagName = "methodResponse";
            Namespace = Uri.IQ_RPC;
        }

        public void WriteResponse(ArrayList Params)
        {
            // remove this tag if exists, in case this function gets
            // calles multiple times by some guys
            RemoveTag("params");

            var elParams = RpcHelper.WriteParams(Params);

            if (elParams != null)
                AddChild(elParams);
        }

        /// <summary>
        ///     Parses the XML-RPC resonse and returns an ArrayList with all Parameters.
        ///     In there is an XML-RPC Error it returns an XmlRpcException as single parameter in the ArrayList.
        /// </summary>
        /// <returns>Arraylist with parameters, or Arraylist with an exception</returns>
        public ArrayList GetResponse()
        {
            return ParseResponse();
        }

        /// <summary>
        ///     parse the response
        /// </summary>
        /// <returns></returns>
        private ArrayList ParseResponse()
        {
            var al = new ArrayList();

            // If an error occurred, the server will return fault
            var fault = SelectSingleElement("fault");
            if (fault != null)
            {
                var ht = ParseStruct(fault.SelectSingleElement("struct", true));
                al.Add(new XmlRpcException((int) ht["faultCode"], (string) ht["faultString"]));
            }
            else
            {
                var elParams = SelectSingleElement("params");
                var nl = elParams.SelectElements("param");


                foreach (Element p in nl)
                {
                    var value = p.SelectSingleElement("value");
                    if (value != null)
                        al.Add(ParseValue(value));
                }
            }
            return al;
        }

        /// <summary>
        ///     Parse a single response value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object ParseValue(Element value)
        {
            object result = null;

            if (value != null)
            {
                if (value.HasChildElements)
                {
                    var next = value.FirstChild;
                    if (next.TagName == "string")
                        result = next.Value;
                    else if (next.TagName == "boolean")
                        result = next.Value == "1";
                    else if (next.TagName == "i4")
                        result = int.Parse(next.Value);
                    else if (next.TagName == "int") // occurs in fault
                        result = int.Parse(next.Value);
                    else if (next.TagName == "double")
                    {
                        var numberInfo = new NumberFormatInfo();
                        numberInfo.NumberDecimalSeparator = ".";
                        result = double.Parse(next.Value, numberInfo);
                    }
                    else if (next.TagName == "dateTime.iso8601")
                        result = Time.ISO_8601Date(next.Value);
                    else if (next.TagName == "base64")
                        result = Convert.FromBase64String(next.Value);
                    else if (next.TagName == "struct")
                        result = ParseStruct(next);
                    else if (next.TagName == "array")
                        result = ParseArray(next);
                }
                else
                {
                    result = value.Value;
                }
            }
            return result;
        }

        /// <summary>
        ///     parse a response array
        /// </summary>
        /// <param name="elArray"></param>
        /// <returns></returns>
        private ArrayList ParseArray(Element elArray)
        {
            //<array>
            //    <data>
            //        <value><string>one</string></value>
            //        <value><string>two</string></value>
            //        <value><string>three</string></value>
            //        <value><string>four</string></value>
            //        <value><string>five</string></value>
            //    </data>
            //</array>

            var data = elArray.SelectSingleElement("data");
            if (data != null)
            {
                var al = new ArrayList();
                var values = data.SelectElements("value");

                foreach (Element val in values)
                {
                    al.Add(ParseValue(val));
                }
                return al;
            }
            return null;
        }

        /// <summary>
        ///     parse a response struct
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private Hashtable ParseStruct(Element el)
        {
            //<struct>
            //   <member>
            //      <name>x</name>
            //      <value><i4>20</i4></value>
            //   </member>
            //   <member>
            //      <name>y</name>
            //      <value><string>cow</string></value>
            //   </member>
            //   <member>
            //      <name>z</name>
            //      <value><double>3,14</double></value>
            //   </member>
            //</struct>

            var ht = new Hashtable();

            var members = el.SelectElements("member");

            foreach (Element member in members)
            {
                var name = member.GetTag("name");

                // parse this member value
                var value = member.SelectSingleElement("value");
                if (value != null)
                    ht[name] = ParseValue(value);
            }
            return ht;
        }
    }
}