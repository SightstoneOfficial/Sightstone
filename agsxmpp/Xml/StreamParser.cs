/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright (c) 2003-2012 by AG-Software 											 *
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
using agsXMPP.Factory;
using agsXMPP.Xml.Dom;
using agsXMPP.Xml.Xpnet;
using Encoding = System.Text.Encoding;
using UTF8Encoding = agsXMPP.Xml.Xpnet.UTF8Encoding;

namespace agsXMPP.Xml
{
    public delegate void StreamError(object sender, Exception ex);

    public delegate void StreamHandler(object sender, Node e);

    /// <summary>
    ///     Stream Parser is a lightweight Streaming XML Parser.
    /// </summary>
    public class StreamParser
    {
        private static readonly Encoding Utf = Encoding.UTF8;
        private readonly Xpnet.Encoding _mEnc = new UTF8Encoding();
        private readonly NamespaceStack _mNamespaceStack = new NamespaceStack();
        private Element _current;
        private BufferAggregate _mBuf = new BufferAggregate();
        private bool _mCdata;

        private int _mDepth;
        private Node _mRoot;

        /// <summary>
        ///     Reset the XML Stream
        /// </summary>
        public long Depth => _mDepth;

        // Stream Event Handlers
        public event StreamHandler OnStreamStart;
        public event StreamHandler OnStreamEnd;
        public event StreamHandler OnStreamElement;

        /// <summary>
        ///     Event for XML-Stream errors
        /// </summary>
        public event StreamError OnStreamError;

        /// <summary>
        ///     Event for general errors
        /// </summary>
        public event ErrorHandler OnError;


        /// <summary>
        ///     Reset the XML Stream
        /// </summary>
        public void Reset()
        {
            _mDepth = 0;
            _mRoot = null;
            _current = null;
            _mCdata = false;

            _mBuf = null;
            _mBuf = new BufferAggregate();

            //m_buf.Clear(0);
            _mNamespaceStack.Clear();
        }

        //private object _thisLock = new object();

        /// <summary>
        ///     Put bytes into the parser.
        /// </summary>
        /// <param name="buf">The bytes to put into the parse stream</param>
        /// <param name="offset">Offset into buf to start at</param>
        /// <param name="length">Number of bytes to write</param>
        public void Push(byte[] buf, int offset, int length)
        {
            // or assert, really, but this is a little nicer.
            if (length == 0)
                return;

            // No locking is required.  Read() won't get called again
            // until this method returns.

            // TODO: only do this copy if we have a partial token at the
            // end of parsing.
            var copy = new byte[length];
            Buffer.BlockCopy(buf, offset, copy, 0, length);
            _mBuf.Write(copy);

            var b = _mBuf.GetBuffer();
            var off = 0;
            var ct = new ContentToken();
            try
            {
                while (off < b.Length)
                {
                    var tok = _mCdata
                        ? _mEnc.tokenizeCdataSection(b, off, b.Length, ct)
                        : _mEnc.tokenizeContent(b, off, b.Length, ct);

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (tok)
                    {
                        case TOK.EMPTY_ELEMENT_NO_ATTS:
                        case TOK.EMPTY_ELEMENT_WITH_ATTS:
                            StartTag(b, off, ct, tok);
                            EndTag();
                            break;
                        case TOK.START_TAG_NO_ATTS:
                        case TOK.START_TAG_WITH_ATTS:
                            StartTag(b, off, ct, tok);
                            break;
                        case TOK.END_TAG:
                            EndTag();
                            break;
                        case TOK.DATA_CHARS:
                        case TOK.DATA_NEWLINE:
                            AddText(Utf.GetString(b, off, ct.TokenEnd - off));
                            break;
                        case TOK.CHAR_REF:
                        case TOK.MAGIC_ENTITY_REF:
                            AddText(new string(new[] {ct.RefChar1}));
                            break;
                        case TOK.CHAR_PAIR_REF:
                            AddText(new string(new[]
                            {
                                ct.RefChar1,
                                ct.RefChar2
                            }));
                            break;
                        case TOK.COMMENT:
                            if (_current != null)
                            {
                                // <!-- 4
                                //  --> 3
                                var start = off + 4*_mEnc.MinBytesPerChar;
                                var end = ct.TokenEnd - off -
                                          7*_mEnc.MinBytesPerChar;
                                var text = Utf.GetString(b, start, end);
                                _current.AddChild(new Comment(text));
                            }
                            break;
                        case TOK.CDATA_SECT_OPEN:
                            _mCdata = true;
                            break;
                        case TOK.CDATA_SECT_CLOSE:
                            _mCdata = false;
                            break;
                        case TOK.XML_DECL:
                            // thou shalt use UTF8, and XML version 1.
                            // i shall ignore evidence to the contrary...

                            // TODO: Throw an exception if these assumptions are
                            // wrong
                            break;
                        case TOK.ENTITY_REF:
                        case TOK.PI:
#if CF
					    throw new util.NotImplementedException("Token type not implemented: " + tok);
#else
                            throw new NotImplementedException("Token type not implemented: " + tok);
#endif
                    }
                    off = ct.TokenEnd;
                }
            }
            catch (PartialTokenException)
            {
                // ignored;
            }
            catch (ExtensibleTokenException)
            {
                // ignored;
            }
            catch (Exception ex)
            {
                OnStreamError?.Invoke(this, ex);
            }
            finally
            {
                _mBuf.Clear(off);
            }
        }

        private void StartTag(byte[] buf, int offset,
            ContentToken ct, TOK tok)
        {
            _mDepth++;
            int colon;
            string name;
            string prefix;
            var ht = new Hashtable();

            _mNamespaceStack.Push();

            // if i have attributes
            if ((tok == TOK.START_TAG_WITH_ATTS) ||
                (tok == TOK.EMPTY_ELEMENT_WITH_ATTS))
            {
                for (var i = 0; i < ct.getAttributeSpecifiedCount(); i++)
                {
                    var start = ct.getAttributeNameStart(i);
                    var end = ct.getAttributeNameEnd(i);
                    name = Utf.GetString(buf, start, end - start);

                    start = ct.getAttributeValueStart(i);
                    end = ct.getAttributeValueEnd(i);
                    //val = utf.GetString(buf, start, end - start);

                    var val = NormalizeAttributeValue(buf, start, end - start);
                    // <foo b='&amp;'/>
                    // <foo b='&amp;amp;'
                    // TODO: if val includes &amp;, it gets double-escaped
                    if (name.StartsWith("xmlns:"))
                    {
                        colon = name.IndexOf(':');
                        prefix = name.Substring(colon + 1);
                        _mNamespaceStack.AddNamespace(prefix, val);
                    }
                    else if (name == "xmlns")
                    {
                        _mNamespaceStack.AddNamespace(string.Empty, val);
                    }
                    else
                    {
                        ht.Add(name, val);
                    }
                }
            }

            name = Utf.GetString(buf,
                offset + _mEnc.MinBytesPerChar,
                ct.NameEnd - offset - _mEnc.MinBytesPerChar);

            colon = name.IndexOf(':');
            string ns;
            prefix = null;
            if (colon > 0)
            {
                prefix = name.Substring(0, colon);
                name = name.Substring(colon + 1);
                ns = _mNamespaceStack.LookupNamespace(prefix);
            }
            else
            {
                ns = _mNamespaceStack.DefaultNamespace;
            }

            var newel = ElementFactory.GetElement(prefix, name, ns);

            foreach (string attrname in ht.Keys)
            {
                newel.SetAttribute(attrname, (string) ht[attrname]);
            }

            if (_mRoot == null)
            {
                _mRoot = newel;
                //FireOnDocumentStart(m_root);
                OnStreamStart?.Invoke(this, _mRoot);
            }
            else
            {
                _current?.AddChild(newel);
                _current = newel;
            }
        }

        private void EndTag()
        {
            _mDepth--;
            _mNamespaceStack.Pop();

            if (_current == null)
            {
// end of doc
                OnStreamEnd?.Invoke(this, _mRoot);
//				FireOnDocumentEnd();
                return;
            }

            /*
			string name = null;

			if ((tok == TOK.EMPTY_ELEMENT_WITH_ATTS) ||
				(tok == TOK.EMPTY_ELEMENT_NO_ATTS))
				name = utf.GetString(buf,
					offset + m_enc.MinBytesPerChar,
					ct.NameEnd - offset -
					m_enc.MinBytesPerChar);
			else
				name = utf.GetString(buf,
					offset + m_enc.MinBytesPerChar*2,
					ct.NameEnd - offset -
					m_enc.MinBytesPerChar*2);
                //*/

//			if (current.Name != name)
//				throw new Exception("Invalid end tag: " + name +
//					" != " + current.Name);

            var parent = (Element) _current.Parent;
            if (parent == null)
            {
                DoRaiseOnStreamElement(_current);
                //if (OnStreamElement!=null)
                //    OnStreamElement(this, current);
                //FireOnElement(current);
            }
            _current = parent;
        }

        /// <summary>
        ///     If users didn't use the library correctly and had no local error handles
        ///     it always crashed here and disconnected the socket.
        ///     Catch this errors here now and forward them.
        /// </summary>
        /// <param name="el"></param>
        internal void DoRaiseOnStreamElement(Element el)
        {
            try
            {
                OnStreamElement?.Invoke(this, el);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, ex);
            }
        }

        private string NormalizeAttributeValue(byte[] buf, int offset, int length)
        {
            if (length == 0)
                return null;

            string val = null;
            var buffer = new BufferAggregate();
            var copy = new byte[length];
            Buffer.BlockCopy(buf, offset, copy, 0, length);
            buffer.Write(copy);
            var b = buffer.GetBuffer();
            var off = 0;
            var ct = new ContentToken();
            try
            {
                while (off < b.Length)
                {
                    //tok = m_enc.tokenizeContent(b, off, b.Length, ct);
                    var tok = _mEnc.tokenizeAttributeValue(b, off, b.Length, ct);

                    // ReSharper disable once SwitchStatementMissingSomeCases
                    switch (tok)
                    {
                        case TOK.ATTRIBUTE_VALUE_S:
                        case TOK.DATA_CHARS:
                        case TOK.DATA_NEWLINE:
                            val += Utf.GetString(b, off, ct.TokenEnd - off);
                            break;
                        case TOK.CHAR_REF:
                        case TOK.MAGIC_ENTITY_REF:
                            val += new string(new[] {ct.RefChar1});
                            break;
                        case TOK.CHAR_PAIR_REF:
                            val += new string(new[] {ct.RefChar1, ct.RefChar2});
                            break;
                        case TOK.ENTITY_REF:
#if CF
						    throw new util.NotImplementedException("Token type not implemented: " + tok);
#else
                            throw new NotImplementedException("Token type not implemented: " + tok);
#endif
                    }
                    off = ct.TokenEnd;
                }
            }
            catch (PartialTokenException)
            {
                // ignored;
            }
            catch (ExtensibleTokenException)
            {
                // ignored;
            }
            catch (Exception ex)
            {
                OnStreamError?.Invoke(this, ex);
            }
            finally
            {
                buffer.Clear(off);
            }
            return val;
        }

        private void AddText(string text)
        {
            if (text == "")
                return;

            //Console.WriteLine("AddText:" + text);
            //Console.WriteLine(lastTOK);

            if (_current != null)
            {
                if (_mCdata)
                {
                    var last = _current.LastNode;
                    if (last != null && last.NodeType == NodeType.Cdata)
                        last.Value = last.Value + text;
                    else
                        _current.AddChild(new CData(text));
                }
                else
                {
                    var last = _current.LastNode;
                    if (last != null && last.NodeType == NodeType.Text)
                        last.Value = last.Value + text;
                    else
                        _current.AddChild(new Text(text));
                }
            }
            else
            {
                // text in root element
                var last = ((Element) _mRoot).LastNode;
                if (_mCdata)
                {
                    if (last != null && last.NodeType == NodeType.Cdata)
                        last.Value = last.Value + text;
                    else
                        _mRoot.AddChild(new CData(text));
                }
                else
                {
                    if (last != null && last.NodeType == NodeType.Text)
                        last.Value = last.Value + text;
                    else
                        _mRoot.AddChild(new Text(text));
                }
            }
        }
    }
}