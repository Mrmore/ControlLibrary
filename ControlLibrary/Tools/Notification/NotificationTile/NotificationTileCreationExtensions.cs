using System.Collections.Generic;
using Windows.Data.Xml.Dom;

namespace ControlLibrary.Tools
{
    /// <summary>
    /// Tile extensions to help create and update live tiles
    /// </summary>
    /// <remarks>
    /// After reviewing many of the "MSDN" live tile samples I found that they were too XML orientated and
    /// relied heavily on using the XML dom classes with direct collection manipulations making the resulting 
    /// code rather large and cumbersome.
    /// 
    /// While the MS TileNotificationFactory helps it still seemed messy I just needed a set of classes and tile manipulations
    /// that were simple to use and didn't rely on knowledge of the XML dom or huge string manipulations.
    /// 
    /// So I created these set of extensions to make tile creation straight forward.
    /// </remarks>
    public static class NotificationTileCreationExtensions
    {
        /// <summary>
        /// Add Image URL's and ALT text to the tile template
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="imageUrl"></param>
        /// <param name="imageAlt"></param>
        /// <returns></returns>
        public static XmlDocument AddImages(this XmlDocument tile, IList<string> imageUrl, IList<string> imageAlt = null)
        {
            return NotificationTileXmlHelper.SetTileImages(tile, imageUrl, imageAlt);
        }

        /// <summary>
        /// Add Text to the tile template
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tileText"></param>
        /// <returns></returns>
        public static XmlDocument AddText(this XmlDocument tile, IList<string> tileText)
        {
            return NotificationTileXmlHelper.SetTileText(tile, tileText);
        }

        /// <summary>
        /// Merge two or more tile templates into one.
        /// Useful for setting SMALL and WIDE tiles
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        public static XmlDocument MergeTiles(this IEnumerable<XmlDocument> tiles)
        {
            return NotificationTileXmlHelper.CreateMergedTile(tiles);
        }
    }
}