using ControlLibrary.Tools.Downloader;
using NotificationsExtensions.TileContent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using ControlLibrary.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Storage;

namespace ControlLibrary.Tools
{
    public enum NotificationSecondaryTileType
    {
        DefaultTile = 0,
        CustomTile = 1
    }

    public class NotificationSecondaryTileHelper
    {
        public static string SecondaryTileFolder = "Pin2Start";
        public static bool CheckSecondaryTileExist(string tileId)
        {
            return SecondaryTile.Exists(tileId);
        }

        public async static Task<string> GetUserNameFromTile(string tileId)
        {
            IReadOnlyList<SecondaryTile> tiles = await SecondaryTile.FindAllAsync();
            string userName = string.Empty;

            if (tiles.Count > 0)
            {
                foreach (var item in tiles)
                {
                    if (item.TileId == tileId)
                    {
                        userName = item.ShortName;
                        break;
                    }
                }
            }
            return userName;
        }

        public async static Task RemoveAllSecondarys()
        {
            try
            {
                IReadOnlyList<SecondaryTile> tiles = await SecondaryTile.FindAllAsync();

                if (tiles.Count > 0)
                {
                    foreach (var item in tiles)
                    {
                        await item.RequestDeleteAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void UpdateSecondaryTileWithImage(string tileId, NotificationTile notificationTile, NotificationSecondaryTileType tileType)
        {
            if (!string.IsNullOrEmpty(tileId) && !string.IsNullOrWhiteSpace(tileId) && notificationTile != null)
            {
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).EnableNotificationQueue(false);
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Clear();

                ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                squareImageAndTextContent.Image.Src = notificationTile.ImageUri;
                squareImageAndTextContent.Image.Alt = notificationTile.ImageAltName;
                squareImageAndTextContent.TextHeading.Text = notificationTile.TextHeading;
                squareImageAndTextContent.TextBodyWrap.Text = notificationTile.TextBodyWrap;
                if (tileType == NotificationSecondaryTileType.CustomTile)
                {
                    ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
                    tileContent.Image.Src = notificationTile.ImageUri;
                    tileContent.Image.Alt = notificationTile.ImageAltName;
                    tileContent.TextCaptionWrap.Text = notificationTile.TextBodyWrap;
                    tileContent.SquareContent = squareImageAndTextContent;
                    TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Update(tileContent.CreateNotification());
                }
                else
                {
                    ITileWidePeekImage05 tileContent = TileContentFactory.CreateTileWidePeekImage05();
                    tileContent.ImageMain.Src = tileContent.ImageSecondary.Src = notificationTile.ImageUri;
                    tileContent.ImageMain.Alt = tileContent.ImageSecondary.Alt = notificationTile.ImageAltName;
                    tileContent.TextHeading.Text = notificationTile.TextHeading;
                    tileContent.TextBodyWrap.Text = notificationTile.TextBodyWrap;
                    tileContent.SquareContent = squareImageAndTextContent;
                    TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Update(tileContent.CreateNotification());
                }
            }
        }

        private async static Task<bool> CreateSecondaryTile(string tileId, NotificationTile notificationTile, FrameworkElement element)
        {
            //Uri logo = new Uri("ms-appx:///Assets/blue_squ.png");
            Uri logo = null;
            Uri wideLogo = null;
            bool fetchHead = false;
            bool result = false;
            string shortName = string.Empty;
            string displayName = string.Empty;
            string arguments = string.Empty;
            try
            {
                if (notificationTile != null)
                {
                    //var uri = await new UriDownloader().Download(notificationTile.ImageUri, notificationTile.ImageUri.ComputeMD5(), SecondaryTileFolder);
                    var uri = await new UriDownloader().Download(notificationTile.ImageUri, tileId.ComputeMD5(), SecondaryTileFolder);
                    if (uri != null)
                    {
                        logo = uri;
                        wideLogo = uri;
                        shortName = notificationTile.TextHeading;
                        displayName = notificationTile.TextHeading;
                        fetchHead = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                fetchHead = false;
            }

            if (!fetchHead)
            {
                logo = new Uri("ms-appx:///Assets/Logo.png");
                wideLogo = new Uri("ms-appx:///Assets/WideLogo.png");
                shortName = "YouTube Secondary Tile";
                displayName = "YouTube Secondary Tile";
            }

            arguments = tileId + DateTime.Now.ToLocalTime().ToString();

            // Create a Secondary tile
            SecondaryTile secondaryTile =
                new SecondaryTile(tileId,
                        shortName,
                        displayName,
                        arguments,
                        TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
                        logo,
                        wideLogo);

            secondaryTile.ForegroundText = ForegroundText.Light;

            if (element != null)
            {
                result =
                    await secondaryTile.RequestCreateForSelectionAsync(
                        GetElementRect((FrameworkElement)element), Windows.UI.Popups.Placement.Below);
            }
            else
            {
                result =
                    await secondaryTile.RequestCreateAsync();
            }
            return result;
        }

        private static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        public async static Task<bool> PinSecondaryTileWithImage(string tileId, NotificationTile notificationTile, NotificationSecondaryTileType tileType = NotificationSecondaryTileType.DefaultTile, FrameworkElement element = null)
        {
            bool isPin = false;
            if (!string.IsNullOrEmpty(tileId) && !string.IsNullOrWhiteSpace(tileId))
            {
                if (CheckSecondaryTileExist(tileId)) isPin = true;
                else
                {
                    var isCreateSecondaryTile = await CreateSecondaryTile(tileId, notificationTile, element);
                    if (isCreateSecondaryTile)
                    {
                        UpdateSecondaryTileWithImage(tileId, notificationTile, tileType);
                        isPin = true;
                    }
                    else
                    {
                        SecondaryTile secondaryTile = new SecondaryTile(tileId);
                        await secondaryTile.RequestDeleteAsync();
                    }
                }
            }
            return isPin;
        }

        public async static Task<bool> UnPinSecondaryTileWithImage(string tileId)
        {
            bool isUnPin = false;
            if (!string.IsNullOrEmpty(tileId) && !string.IsNullOrWhiteSpace(tileId))
            {
                if (CheckSecondaryTileExist(tileId))
                {
                    SecondaryTile secondaryTile = new SecondaryTile(tileId);
                    isUnPin = await secondaryTile.RequestDeleteAsync();
                    if(isUnPin)
                    {
                        await DelectPinStorageFile(tileId);
                    }
                }
            }
            return isUnPin;
        }

        public static void CloseSecondaryTileUpdate(string tileId)
        {
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).EnableNotificationQueue(false);
            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Clear();
        }

        public static async void DelectPinFolder()
        {
            try
            {
                //删除系统本地文件夹的myFolder文件夹及其子文件夹
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder pinLocalFolder = await localFolder.GetFolderAsync(SecondaryTileFolder);
                if (pinLocalFolder != null)
                {
                    await pinLocalFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async Task DelectPinStorageFile(string tileId)
        {
            try
            {
                //删除系统本地文件夹的myFolder文件夹及其子文件夹
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFolder pinLocalFolder = await localFolder.GetFolderAsync(SecondaryTileFolder);
                if (pinLocalFolder != null)
                {
                    StorageFile file = await pinLocalFolder.GetFileAsync(tileId.ComputeMD5());
                    if (file != null)
                    {
                        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async static Task<bool> PinSecondaryTileWithImages(string tileId, List<NotificationTile> notificationTileList, NotificationSecondaryTileType tileType = NotificationSecondaryTileType.DefaultTile, FrameworkElement element = null)
        {
            bool isPin = false;
            if (!string.IsNullOrEmpty(tileId) && !string.IsNullOrWhiteSpace(tileId))
            {
                if (CheckSecondaryTileExist(tileId)) isPin = true;
                else
                {
                    var isCreateSecondaryTile = await CreateSecondaryTiles(tileId, notificationTileList, element);
                    if (isCreateSecondaryTile)
                    {
                        UpdateSecondaryTileWithImages(tileId, notificationTileList, tileType);
                        isPin = true;
                    }
                    else
                    {
                        SecondaryTile secondaryTile = new SecondaryTile(tileId);
                        await secondaryTile.RequestDeleteAsync();
                    }
                }
            }
            return isPin;
        }

        private async static Task<bool> CreateSecondaryTiles(string tileId, List<NotificationTile> notificationTileList, FrameworkElement element = null)
        {
            //Uri logo = new Uri("ms-appx:///Assets/blue_squ.png");
            Uri logo = null;
            Uri wideLogo = null;
            bool fetchHead = false;
            bool result = false;
            string shortName = string.Empty;
            string displayName = string.Empty;
            string arguments = string.Empty;
            try
            {
                if (notificationTileList != null)
                {
                    //var uri = await new UriDownloader().Download(notificationTileList[0].ImageUri, notificationTile[0].ImageUri.ComputeMD5(), SecondaryTileFolder);
                    var uri = await new UriDownloader().Download(notificationTileList[0].ImageUri, tileId.ComputeMD5(), SecondaryTileFolder);
                    if (uri != null)
                    {
                        logo = uri;
                        wideLogo = uri;
                        shortName = notificationTileList[0].TextHeading;
                        displayName = notificationTileList[0].TextHeading;
                        fetchHead = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                fetchHead = false;
            }

            if (!fetchHead)
            {
                logo = new Uri("ms-appx:///Assets/Logo.png");
                wideLogo = new Uri("ms-appx:///Assets/WideLogo.png");
                shortName = "YouTube Secondary Tile";
                displayName = "YouTube Secondary Tile";
            }

            arguments = tileId + DateTime.Now.ToLocalTime().ToString();

            // Create a Secondary tile
            SecondaryTile secondaryTile =
                new SecondaryTile(tileId,
                        shortName,
                        displayName,
                        arguments,
                        TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
                        logo,
                        wideLogo);

            secondaryTile.ForegroundText = ForegroundText.Light;

            if (element != null)
            {
                result =
                    await secondaryTile.RequestCreateForSelectionAsync(
                        GetElementRect((FrameworkElement)element), Windows.UI.Popups.Placement.Below);
            }
            else
            {
                result =
                    await secondaryTile.RequestCreateAsync();
            }
            return result;
        }

        private static void UpdateSecondaryTileWithImages(string tileId, List<NotificationTile> notificationTileList, NotificationSecondaryTileType tileType)
        {
            if (!string.IsNullOrEmpty(tileId) && !string.IsNullOrWhiteSpace(tileId) && notificationTileList != null)
            {
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).EnableNotificationQueue(true);
                TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Clear();

                for (int i = 0; i < notificationTileList.Count; i++)
                {
                    if (i < 5)
                    {
                        ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
                        squareImageAndTextContent.Image.Src = notificationTileList[i].ImageUri;
                        squareImageAndTextContent.Image.Alt = notificationTileList[i].ImageAltName;
                        squareImageAndTextContent.TextHeading.Text = notificationTileList[i].TextHeading;
                        squareImageAndTextContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap;
                        if (tileType == NotificationSecondaryTileType.CustomTile)
                        {
                            ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
                            tileContent.Image.Src = notificationTileList[i].ImageUri;
                            tileContent.Image.Alt = notificationTileList[i].ImageAltName;
                            tileContent.TextCaptionWrap.Text = notificationTileList[i].TextBodyWrap;
                            tileContent.SquareContent = squareImageAndTextContent;
                            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Update(tileContent.CreateNotification());
                        }
                        else
                        {
                            ITileWidePeekImage05 tileContent = TileContentFactory.CreateTileWidePeekImage05();
                            tileContent.ImageMain.Src = tileContent.ImageSecondary.Src = notificationTileList[i].ImageUri;
                            tileContent.ImageMain.Alt = tileContent.ImageSecondary.Alt = notificationTileList[i].ImageAltName;
                            tileContent.TextHeading.Text = notificationTileList[i].TextHeading;
                            tileContent.TextBodyWrap.Text = notificationTileList[i].TextBodyWrap;
                            tileContent.SquareContent = squareImageAndTextContent;
                            TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId).Update(tileContent.CreateNotification());
                        }
                    }
                }
            }
        }
    }
}
