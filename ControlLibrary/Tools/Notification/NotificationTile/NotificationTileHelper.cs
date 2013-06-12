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
        public static void UpdateTileWithWidePeekImage(NotificationTile notificationTile)
        {
            if (notificationTile != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(false);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
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

        public static void UpdateTileWithWidePeekImages(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null)
            {
                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                        TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                        ITileWidePeekImage05 tileContent = TileContentFactory.CreateTileWidePeekImage05();
                        tileContent.ImageMain.Src = tileContent.ImageSecondary.Src = notificationTileList[i].ImageUri;
                        tileContent.ImageMain.Alt = tileContent.ImageSecondary.Alt = notificationTileList[i].ImageAltName;
                        tileContent.TextHeading.Text = notificationTileList[i].TextHeading;
                        tileContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap;

                        ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                        squareImageAndTextContent.Image.Src = notificationTileList[i].ImageUri;
                        squareImageAndTextContent.Image.Alt = notificationTileList[i].ImageAltName;
                        squareImageAndTextContent.TextHeading.Text = notificationTileList[i].TextHeading;
                        squareImageAndTextContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap;
                        tileContent.SquareContent = squareImageAndTextContent;

                        TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
                    }
                }
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
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(false);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                var statusTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText02);
                TileNotification tileNotification  = new TileNotification(statusTile.AddText(new[]
                {
                    notificationTile.TextHeading,
                    notificationTile.TextBodyWrap
                }));
                if(!string.IsNullOrEmpty(notificationTile.NotificationTileTag) || !string.IsNullOrWhiteSpace(notificationTile.NotificationTileTag))
                {
                    tileNotification.Tag = notificationTile.NotificationTileTag;
                }
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
        }

        public static void UpdateTileWithTexts(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        var statusTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText02);
                        TileNotification tileNotification = new TileNotification(statusTile.AddText(new[]
                        {
                            notificationTileList[i].TextHeading,
                            notificationTileList[i].TextBodyWrap
                        }));
                        if (!string.IsNullOrEmpty(notificationTileList[i].NotificationTileTag) || !string.IsNullOrWhiteSpace(notificationTileList[i].NotificationTileTag))
                        {
                            tileNotification.Tag = notificationTileList[i].NotificationTileTag;
                        }
                        TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                    }
                }
            }
        }

        /// <summary>
        /// Sample Merged tiles with images and text
        /// </summary>
        /// <param name="updater"></param>
        public static void UpdateTileWithWideTextImage(NotificationTile notificationTile)
        {
            if (notificationTile != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(false);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                var photoTileSmall = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText02);
                var photoTileWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);

                //Add several tile templates to an IEnumerable<> list for merging
                var liveTilesIn = new List<XmlDocument>
                {
                    photoTileSmall.AddImages(new[]{ notificationTile.ImageUri }, new[]{ notificationTile.ImageAltName }).AddText(new[]{ notificationTile.TextBodyWrap }),
                    photoTileWide.AddImages(new[]{ notificationTile.ImageUri }, new[]{ notificationTile.ImageAltName }).AddText(new[]{ notificationTile.TextBodyWrap })
                };

                //Use the Tag property to uniquely identify this tile
                TileNotification tileNotification = new TileNotification(liveTilesIn.MergeTiles());
                if (!string.IsNullOrEmpty(notificationTile.NotificationTileTag) || !string.IsNullOrWhiteSpace(notificationTile.NotificationTileTag))
                {
                    tileNotification.Tag = notificationTile.NotificationTileTag;
                }
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
        }

        public static void UpdateTileWithWideTextImageImages(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        var photoTileSmall = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText02);
                        var photoTileWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);

                        //Add several tile templates to an IEnumerable<> list for merging
                        var liveTilesIn = new List<XmlDocument>
                        {
                            photoTileSmall.AddImages(new[]{ notificationTileList[i].ImageUri }, new[]{ notificationTileList[i].ImageAltName }).AddText(new[]{ notificationTileList[i].TextBodyWrap }),
                            photoTileWide.AddImages(new[]{ notificationTileList[i].ImageUri }, new[]{ notificationTileList[i].ImageAltName }).AddText(new[]{ notificationTileList[i].TextBodyWrap })
                        };

                        //Use the Tag property to uniquely identify this tile
                        TileNotification tileNotification = new TileNotification(liveTilesIn.MergeTiles());
                        if (!string.IsNullOrEmpty(notificationTileList[i].NotificationTileTag) || !string.IsNullOrWhiteSpace(notificationTileList[i].NotificationTileTag))
                        {
                            tileNotification.Tag = notificationTileList[i].NotificationTileTag;
                        }
                        TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationTile"></param>
        public static void UpdateTileWithImage(NotificationTile notificationTile)
        {
            if (notificationTile != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(false);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
                tileContent.Image.Src = notificationTile.ImageUri;
                tileContent.Image.Alt = notificationTile.ImageAltName;
                tileContent.TextCaptionWrap.Text = notificationTile.TextBodyWrap;

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
        /// 
        /// </summary>
        /// <param name="notificationTileList"></param>
        public static void UpdateTileWithImages(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
                        tileContent.Image.Src = notificationTileList[i].ImageUri;
                        tileContent.Image.Alt = notificationTileList[i].ImageAltName;
                        tileContent.TextCaptionWrap.Text = notificationTileList[i].TextBodyWrap;

                        ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                        squareImageAndTextContent.Image.Src = notificationTileList[i].ImageUri;
                        squareImageAndTextContent.Image.Alt = notificationTileList[i].ImageAltName;
                        squareImageAndTextContent.TextHeading.Text = notificationTileList[i].TextHeading;
                        squareImageAndTextContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap;
                        tileContent.SquareContent = squareImageAndTextContent;

                        TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
                    }
                }
            }
        }

        public static void CloseTileUpdate()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(false);
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }
        
    }
}
