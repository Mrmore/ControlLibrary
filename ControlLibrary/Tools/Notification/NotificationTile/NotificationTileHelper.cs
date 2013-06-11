using NotificationsExtensions.TileContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ControlLibrary.Tools
{
    public class NotificationTileHelper
    {
        /// <summary>
        /// Sample Merged tiles with images and text
        /// </summary>
        /// <param name="updater"></param>
        public static void UpdateTileWithImage(NotificationTile notificationTile)
        {
            if (notificationTile != null)
            {
                ITileWidePeekImage05 tileContent = TileContentFactory.CreateTileWidePeekImage05();
                tileContent.ImageMain.Src = tileContent.ImageSecondary.Src = notificationTile.ImageUri;
                tileContent.ImageMain.Alt = tileContent.ImageSecondary.Alt = notificationTile.ImageAltName;
                tileContent.TextHeading.Text = notificationTile.TextHeading;
                tileContent.TextBodyWrap.Text = notificationTile.TextBodyWrap;

                ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                squareImageAndTextContent.Image.Src = notificationTile.ImageUri;
                squareImageAndTextContent.Image.Alt = notificationTile.ImageAltName;
                squareImageAndTextContent.TextHeading.Text = notificationTile.TextHeading;
                squareImageAndTextContent.TextBodyWrap.Text = notificationTile.TextBodyWrap;
                tileContent.SquareContent = squareImageAndTextContent;

                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
            }
        }

        /// <summary>
        /// Sample Merged Tiles text only
        /// </summary>
        /// <param name="updater"></param>
        public static void UpdateTileWithText(NotificationTile notificationTile)
        {
            if (notificationTile != null)
            {
                var statusTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText02);
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(statusTile.AddText(new[]
                {
                    notificationTile.TextHeading,
                    notificationTile.TextBodyWrap
                })) { Tag = notificationTile.NotificationTileTag });
            }
        }

        /// <summary>
        /// Sample Merged tiles with images and text
        /// </summary>
        /// <param name="updater"></param>
        public static void UpdateTileWithImages(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null)
            {
                var photoTileSmall = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText02);
                var photoTileWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWidePeekImageCollection05);
                var photoUrl = (from tileList in notificationTileList
                                select tileList.ImageUri).ToList();
                var photoAlt = (from tileList in notificationTileList
                                select tileList.ImageAltName).ToList();
                var photoPeekText = (from tileList in notificationTileList
                                     select tileList.TextBodyWrap).ToList();

                //Add several tile templates to an IEnumerable<> list for merging
                var liveTilesIn = new List<XmlDocument>
                {
                    photoTileSmall.AddImages(photoUrl, photoAlt).AddText(photoPeekText),
                    photoTileWide.AddImages(photoUrl, photoAlt).AddText(photoPeekText)
                };

                //Use the Tag property to uniquely identify this tile
                TileUpdateManager.CreateTileUpdaterForApplication().Update(new TileNotification(liveTilesIn.MergeTiles()) { Tag = notificationTileList.ElementAt(0).NotificationTileTag });
            }
        }
    }
}
