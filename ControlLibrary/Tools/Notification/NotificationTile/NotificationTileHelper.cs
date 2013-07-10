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
                if (notificationTile.TextHeading.Length > 30)
                    tileContent.TextHeading.Text = notificationTile.TextHeading.Substring(0, 30);
                else
                    tileContent.TextHeading.Text = notificationTile.TextHeading;

                if (notificationTile.TextBodyWrap.Length > 80)
                    tileContent.TextBodyWrap.Text = notificationTile.TextBodyWrap.Substring(0, 80);
                else
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
            if (notificationTileList != null && notificationTileList.Count > 0)
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
                        if (notificationTileList[i].TextHeading.Length > 30)
                            tileContent.TextHeading.Text = notificationTileList[i].TextHeading.Substring(0, 30);
                        else
                            tileContent.TextHeading.Text = notificationTileList[i].TextHeading;

                        if (notificationTileList[i].TextBodyWrap.Length > 80)
                            tileContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap.Substring(0, 80);
                        else
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

                string TextHeading = string.Empty;
                if (notificationTile.TextHeading.Length > 30)
                    TextHeading = notificationTile.TextHeading.Substring(0, 30);
                else
                    TextHeading = notificationTile.TextHeading;

                string TextBodyWrap = string.Empty;
                if (notificationTile.TextBodyWrap.Length > 80)
                    TextBodyWrap = notificationTile.TextBodyWrap.Substring(0, 80);
                else
                    TextBodyWrap = notificationTile.TextBodyWrap;
                TileNotification tileNotification = new TileNotification(statusTile.AddText(new[]
                {
                    TextHeading,
                    TextBodyWrap
                }));
                if (!string.IsNullOrEmpty(notificationTile.NotificationTileTag) || !string.IsNullOrWhiteSpace(notificationTile.NotificationTileTag))
                {
                    tileNotification.Tag = notificationTile.NotificationTileTag;
                }
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
        }

        public static void UpdateTileWithTexts(List<NotificationTile> notificationTileList)
        {
            if (notificationTileList != null && notificationTileList.Count > 0)
            {
                TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        string TextHeading = string.Empty;
                        if (notificationTileList[i].TextHeading.Length > 30)
                            TextHeading = notificationTileList[i].TextHeading.Substring(0, 30);
                        else
                            TextHeading = notificationTileList[i].TextHeading;

                        string TextBodyWrap = string.Empty;
                        if (notificationTileList[i].TextBodyWrap.Length > 80)
                            TextBodyWrap = notificationTileList[i].TextBodyWrap.Substring(0, 80);
                        else
                            TextBodyWrap = notificationTileList[i].TextBodyWrap;

                        var statusTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText02);
                        TileNotification tileNotification = new TileNotification(statusTile.AddText(new[]
                        {
                            TextHeading,
                            TextBodyWrap
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
                string TextBodyWrap = string.Empty;
                if (notificationTile.TextBodyWrap.Length > 80)
                    TextBodyWrap = notificationTile.TextBodyWrap.Substring(0, 80);
                else
                    TextBodyWrap = notificationTile.TextBodyWrap;
                var liveTilesIn = new List<XmlDocument>
                {
                    photoTileSmall.AddImages(new[]{ notificationTile.ImageUri }, new[]{ notificationTile.ImageAltName }).AddText(new[]{ TextBodyWrap }),
                    photoTileWide.AddImages(new[]{ notificationTile.ImageUri }, new[]{ notificationTile.ImageAltName }).AddText(new[]{ TextBodyWrap })
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
            if (notificationTileList != null && notificationTileList.Count > 0)
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
                        string TextBodyWrap = string.Empty;
                        if (notificationTileList[i].TextBodyWrap.Length > 80)
                            TextBodyWrap = notificationTileList[i].TextBodyWrap.Substring(0, 80);
                        else
                            TextBodyWrap = notificationTileList[i].TextBodyWrap;
                        var liveTilesIn = new List<XmlDocument>
                        {
                            photoTileSmall.AddImages(new[]{ notificationTileList[i].ImageUri }, new[]{ notificationTileList[i].ImageAltName }).AddText(new[]{ TextBodyWrap }),
                            photoTileWide.AddImages(new[]{ notificationTileList[i].ImageUri }, new[]{ notificationTileList[i].ImageAltName }).AddText(new[]{ TextBodyWrap })
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
                if (notificationTile.TextBodyWrap.Length > 80)
                    tileContent.TextCaptionWrap.Text = notificationTile.TextBodyWrap.Substring(0, 80);
                else
                    tileContent.TextCaptionWrap.Text = notificationTile.TextBodyWrap;

                ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                squareImageAndTextContent.Image.Src = notificationTile.ImageUri;
                squareImageAndTextContent.Image.Alt = notificationTile.ImageAltName;
                if (notificationTile.TextHeading.Length > 30)
                    squareImageAndTextContent.TextHeading.Text = notificationTile.TextHeading.Substring(0, 30);
                else
                    squareImageAndTextContent.TextHeading.Text = notificationTile.TextHeading;

                if (notificationTile.TextBodyWrap.Length > 80)
                    squareImageAndTextContent.TextBodyWrap.Text = notificationTile.TextBodyWrap.Substring(0, 80);
                else
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
            if (notificationTileList != null && notificationTileList.Count > 0)
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
                        if (notificationTileList[i].TextBodyWrap.Length > 80)
                            tileContent.TextCaptionWrap.Text = notificationTileList[i].TextBodyWrap.Substring(0, 80);
                        else
                            tileContent.TextCaptionWrap.Text = notificationTileList[i].TextBodyWrap;

                        ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                        squareImageAndTextContent.Image.Src = notificationTileList[i].ImageUri;
                        squareImageAndTextContent.Image.Alt = notificationTileList[i].ImageAltName;
                        if (notificationTileList[i].TextHeading.Length > 30)
                            squareImageAndTextContent.TextHeading.Text = notificationTileList[i].TextHeading.Substring(0, 30);
                        else
                            squareImageAndTextContent.TextHeading.Text = notificationTileList[i].TextHeading;

                        if (notificationTileList[i].TextBodyWrap.Length > 80)
                            squareImageAndTextContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap.Substring(0, 80);
                        else
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
