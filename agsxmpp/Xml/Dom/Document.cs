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

using System.IO;

namespace agsXMPP.Xml.Dom
{
    /// <summary>
    /// </summary>
    public class Document : Node
    {
        public Document()
        {
            NodeType = NodeType.Document;
        }

        public Element RootElement
        {
            get
            {
                foreach (Node n in ChildNodes)
                {
                    if (n.NodeType == NodeType.Element)
                        return n as Element;
                }
                return null;
            }
        }

        /// <summary>
        ///     Clears the Document
        /// </summary>
        public void Clear()
        {
            ChildNodes.Clear();
        }

        #region << Properties and Member Variables >>

        public string Encoding { get; set; } = null;

        public string Version { get; set; } = null;

        #endregion

        #region << Load functions >>		

        public void LoadXml(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                DomLoader.Load(xml, this);
            }
        }

        public bool LoadFile(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    using (var sr = new StreamReader(filename))
                    {
                        DomLoader.Load(sr, this);
                        sr.Close();
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public bool LoadStream(Stream stream)
        {
            try
            {
                using (var sr = new StreamReader(stream))
                {
                    DomLoader.Load(sr, this);
                    sr.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void Save(string filename)
        {
            using (var w = new StreamWriter(filename))
            {
                w.Write(ToString(System.Text.Encoding.UTF8));
                w.Flush();
                w.Close();
            }
        }

        #endregion
    }
}