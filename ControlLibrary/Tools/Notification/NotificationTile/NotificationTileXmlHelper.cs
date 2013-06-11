using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Data.Xml.Dom;

namespace ControlLibrary.Tools
{
    public class NotificationTileXmlHelper
    {
        /// <summary>
        /// Merge several LiveTiles into a single tile.
        /// Used for multiple bindings in viewing different tile sizes
        /// See <see cref="http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx"/>
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns>Merged Tile Visual Binding</returns>
        public static XmlDocument CreateMergedTile(IEnumerable<XmlDocument> tiles)
        {
            var culture = CultureInfo.CurrentCulture;
            //XML Document Elements
            var docRoot = new XmlDocument();
            var tileRoot = docRoot.CreateElement("tile");
            var visualRoot = docRoot.CreateElement("visual");

            //Set Language Attributes
            var langAttrib = docRoot.CreateAttribute("lang");
            langAttrib.Value = culture.Name;
            visualRoot.Attributes.SetNamedItem(langAttrib);

            //Append base documents
            docRoot.AppendChild(tileRoot);
            tileRoot.AppendChild(visualRoot);

            //Insert Bindings from each tile
            foreach (var tile in tiles)
            {
                var bindings = tile.GetElementsByTagName("binding");
                foreach (var binding in bindings)
                {
                    //Sadly a new XML documents needs to be created from the binding node
                    //or the ImportNode method throws a "Value not in expected range" exception
                    var tempDoc = new XmlDocument();
                    tempDoc.LoadXml(binding.GetXml());

                    var node = docRoot.ImportNode(tempDoc.DocumentElement, true);
                    visualRoot.AppendChild(node);
                }
            }

            return docRoot;
        }

        /// <summary>
        /// Sets the image SRC and ALT attributes in the tile template XSD schema
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/apps/br212859.aspx"/>
        /// </summary>
        /// <remarks>
        /// Generally there will be a 1:1 mapping between the image node attributes and the position in the incoming list.
        /// </remarks>
        /// <param name="tileTemplate"></param>
        /// <param name="imageUrl"></param>
        /// <param name="imageAlt"></param>
        /// <returns></returns>
        public static XmlDocument SetTileImages(XmlDocument tileTemplate, IList<string> imageUrl, IList<string> imageAlt = null)
        {
            var imageNodes = tileTemplate.GetElementsByTagName("image");
            if (imageNodes.Count > 0)
            {
                //Needs to be a for loop, since we modify the element during iteration
                for (var index = 0; index < imageNodes.Count && index < imageUrl.Count; index++)
                {
                    try
                    {
                        var node = imageNodes[index];

                        IXmlNode src = null;
                        IXmlNode alt = null;
                        var altAttrib = tileTemplate.CreateAttribute("alt");
                        var srcAttrib = tileTemplate.CreateAttribute("src");

                        if (null != node.Attributes)
                        {
                            src = node.Attributes.GetNamedItem("src");
                            node.Attributes.SetNamedItem(srcAttrib);

                            alt = node.Attributes.GetNamedItem("alt");
                            node.Attributes.SetNamedItem(altAttrib);
                        }

                        if (null != src)
                        {
                            src.InnerText = imageUrl[index];
                            node.Attributes.SetNamedItem(src);
                        }
                        else srcAttrib.Value = imageUrl[index];

                        if (null == imageAlt) continue;
                        if (imageAlt.Count <= 0) continue;
                        if (null != alt)
                        {
                            alt.InnerText = imageAlt[index];
                            node.Attributes.SetNamedItem(alt);
                        }
                        else altAttrib.Value = imageAlt[index];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break; //Shouldn't happen
                    }
                }
            }

            return tileTemplate;
        }

        /// <summary>
        /// Sets the text nodes in the tile template XSD schema
        /// <see cref="http://msdn.microsoft.com/en-us/library/windows/apps/br212859.aspx"/>
        /// </summary>
        /// <remarks>
        /// Generally there will be a 1:1 mapping between the text node and the position in the incoming list.
        /// </remarks>
        /// <param name="tileTemplate"></param>
        /// <param name="tileText"></param>
        /// <returns></returns>
        public static XmlDocument SetTileText(XmlDocument tileTemplate, IList<string> tileText)
        {
            var textNodes = tileTemplate.GetElementsByTagName("text");
            if (textNodes.Count > 0)
            {
                //Needs to be a for loop, since we modify the element during iteration
                for (var index = 0; index < textNodes.Count && index < tileText.Count; index++)
                {
                    try
                    {
                        var node = textNodes[index];
                        node.InnerText = tileText[index];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        break; //Shouldn't happen
                    }
                }
            }

            return tileTemplate;
        }
    }
}