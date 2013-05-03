using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

#if WINRT
using Windows.Data.Xml.Dom;
#endif

namespace ControlLibrary.Tools.Xml
{
    //WinRt 需要大改
    //原作者地址https://xmlhelper.codeplex.com/
    /// <summary>
    /// A static class containing methods to complete a number of common tasks while working with XmlDocuments.
    /// </summary>
    public static class XmlHelper
    {
        #if !WINRT
        /// <summary>
        /// Creates an empty XmlDocument object with an Xml Declaration assigned as version 1.0 encoding UTF-8
        /// </summary>
        /// <returns>Empty XmlDocument object</returns>
        public static XmlDocument NewDoc()
        {
            XmlDocument OutputFile = new XmlDocument();
            XmlDeclaration dec = OutputFile.CreateXmlDeclaration("1.0", "UTF-8", null);
            OutputFile.AppendChild(dec);// Create the root element
            return OutputFile;
        }

        /// <summary>
        /// Create a root node on a given document
        /// </summary>
        /// <param name="AddTo">Xml document to add the new root node to</param>
        /// <param name="NodeName">Name of the new root node</param>
        /// <returns>A reference to the newly created root node</returns>
        public static XmlElement AddRootNode(XmlDocument AddTo, string NodeName)
        {
            XmlElement newRoot = AddTo.CreateElement(NodeName);
            AddTo.AppendChild(newRoot);

            return newRoot;
        }

        /// <summary>
        /// Adds the Named attribute with a given value to an existing Node
        /// </summary>
        /// <param name="AddTo">Node to add the new attribute</param>
        /// <param name="attribName">New attribute name</param>
        /// <param name="attribValue">Value to set</param>
        /// <returns>a link to the new attribute created</returns>
        public static XmlAttribute AddAttrib(XmlElement AddTo, string attribName, string attribValue)
        {
            XmlAttribute node = AddTo.OwnerDocument.CreateAttribute(attribName);
            node.Value = attribValue;
            AddTo.Attributes.Append(node);
            return node;
        }

        /// <summary>
        /// Create a new node on a given node
        /// </summary>
        /// <param name="AddTo">Node to add the new node to</param>
        /// <param name="NodeName">Name of the new node</param>
        /// <returns>A reference to the newly created node</returns>
        public static XmlElement AddNode(XmlElement AddTo, string NodeName)
        {
            XmlElement newChild = AddTo.OwnerDocument.CreateElement(NodeName);
            AddTo.AppendChild(newChild);
            return newChild;
        }

        /// <summary>
        /// Create a new comment node in a given node
        /// </summary>
        /// <param name="AddTo">Node to add the comment to</param>
        /// <param name="Comment">String to go in the Comment section</param>
        public static void AddComment(XmlElement AddTo, string Comment)
        {
            XmlComment newComment = AddTo.OwnerDocument.CreateComment(Comment);
            AddTo.AppendChild(newComment);
        }

        /// <summary>
        /// Create a new text node on a given node with a given value
        /// </summary>
        /// <param name="AddTo">Node to add the new node to</param>
        /// <param name="NodeName">Name of the new node</param>
        /// <param name="NodeValue">Text value to set the new node to</param>
        /// <returns>A reference to the newly created node</returns>
        public static XmlElement AddTextNode(XmlElement AddTo, string NodeName, string NodeValue)
        {
            XmlElement newChild = AddTo.OwnerDocument.CreateElement(NodeName);
            XmlText text = AddTo.OwnerDocument.CreateTextNode(NodeValue);
            newChild.AppendChild(text);
            AddTo.AppendChild(newChild);
            return newChild;
        }

        /// <summary>
        /// Attractively format the XML with consistent indentation.
        /// </summary>
        /// <param name="strXML">A well formed XML string</param>
        /// <returns>An XML string with carriage returns and indentations</returns>
        public static string PrettyPrint(string strXML)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlDocument node = new XmlDocument();
                node.LoadXml(strXML);
                XmlNodeReader reader = new XmlNodeReader(node);
                XmlTextWriter writer2 = new XmlTextWriter(writer);
                writer2.Formatting = Formatting.Indented;
                writer2.Indentation = 1;
                writer2.IndentChar = '\t';
                writer2.WriteNode(reader, true);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Attractively format the XML with consistent indentation.
        /// </summary>
        /// <param name="doc">The Xml Document you want to convert</param>
        /// <returns>An XML string with carriage returns and indentations</returns>
        public static string PrettyPrint(XmlDocument doc)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlNodeReader reader = new XmlNodeReader(doc);
                XmlTextWriter writer2 = new XmlTextWriter(writer);
                writer2.Formatting = Formatting.Indented;
                writer2.Indentation = 1;
                writer2.IndentChar = '\t';
                writer2.WriteNode(reader, true);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Read a value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="AttribName">Name of the Attribute</param>
        /// <returns>value of the attribute or NULL if not found</returns>
        public static string ReadAttrib(XmlElement ReadFrom, string AttribName)
        {
            if (ReadFrom.Attributes[AttribName] != null)
            {
                return ReadFrom.Attributes[AttribName].Value;
            }
            return null;
        }

        /// <summary>
        /// Read a bool value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="AttribName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static bool ReadAttrib(XmlElement ReadFrom, string AttribName, bool Def)
        {
            bool output = Def;
            if (ReadFrom.Attributes[AttribName] != null)
            {
                bool.TryParse(ReadFrom.Attributes[AttribName].Value, out output);
            }
            return output;
        }

        /// <summary>
        /// Read a int value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="AttribName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static int ReadAttrib(XmlElement ReadFrom, string AttribName, int Def)
        {
            int output = Def;
            if (ReadFrom.Attributes[AttribName] != null)
            {
                int.TryParse(ReadFrom.Attributes[AttribName].Value, out output);
            }
            return output;
        }

        /// <summary>
        /// Read a float value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="AttribName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static float ReadAttrib(XmlElement ReadFrom, string AttribName, float Def)
        {
            float output = Def;
            if (ReadFrom.Attributes[AttribName] != null)
            {
                float.TryParse(ReadFrom.Attributes[AttribName].Value, out output);
            }
            return output;
        }

        /// <summary>
        /// Read a string value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="AttribName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static string ReadAttrib(XmlElement ReadFrom, string AttribName, string Def)
        {
            string output = Def;
            if (ReadFrom.Attributes[AttribName] != null)
            {
                output = ReadFrom.Attributes[AttribName].Value;
            }
            return output;
        }

        /// <summary>
        /// Read a value from a given text node on an existing node
        /// </summary>
        /// <param name="ReadFrom">Node with the node to read</param>
        /// <param name="NodeName">Name of the node to read the value</param>
        /// <returns>inner text of the node.</returns>        
        public static string ReadTextNode(XmlElement ReadFrom, string NodeName)
        {
            if (ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)) != null)
            {
                return ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)).InnerText;
            }
            return null;
        }

        /// <summary>
        /// Read a bool value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="NodeName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static bool ReadTextNode(XmlElement ReadFrom, string NodeName, bool Def)
        {
            bool output = Def;
            if (ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)) != null)
            {
                bool.TryParse(XmlHelper.ReadTextNode(ReadFrom, NodeName), out output);
            }
            return output;
        }

        /// <summary>
        /// Read a int value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="NodeName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static int ReadTextNode(XmlElement ReadFrom, string NodeName, int Def)
        {
            int output = Def;
            if (ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)) != null)
            {
                int.TryParse(XmlHelper.ReadTextNode(ReadFrom, NodeName), out output);
            }
            return output;
        }

        /// <summary>
        /// Read a float value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="NodeName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static float ReadTextNode(XmlElement ReadFrom, string NodeName, float Def)
        {
            float output = Def;
            if (ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)) != null)
            {
                float.TryParse(XmlHelper.ReadTextNode(ReadFrom, NodeName), out output);
            }
            return output;
        }

        /// <summary>
        /// Read a string value from a given attribute on an existing node
        /// </summary>
        /// <param name="ReadFrom">node that has the attribute</param>
        /// <param name="NodeName">Name of the Attribute</param>
        /// <param name="Def">The default value to return if the attribute is not found</param>
        /// <returns>value of the attribute or value of Def if not found</returns>
        public static string ReadTextNode(XmlElement ReadFrom, string NodeName, string Def)
        {
            string output = Def;
            if (ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)) != null)
            {
                output = ReadFrom.SelectSingleNode(string.Format("./{0}", NodeName)).InnerText;
            }
            return output;
        }
#endif
    }
}
