using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace TestDemoApp.Helper.System
{
    public enum SystemShareType
    { 
        Link = 0,
        Text = 1
    }

    public class SystemShareHelper
    {
        private volatile static SystemShareHelper _instance = null;
        private static readonly object lockObject = new object();

        public static SystemShareHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemShareHelper();
                            _instance.dataTransferManager = DataTransferManager.GetForCurrentView();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataTransferManager dataTransferManager = null;
        private StorageFile ImageFile = null;
        private string ShareTitle = "Share YouTube Video.";
        private string ShareDescription = string.Empty;
        private string ShareLinkOrText = string.Empty;
        private string ImageUri = string.Empty;
        private bool isImageFile = false;
        private SystemShareType ShareType = SystemShareType.Link;

        public void Init()
        {
            DelectLocalFolder();
            if (dataTransferManager == null)
            { 
                this.dataTransferManager = DataTransferManager.GetForCurrentView();
            }
            this.dataTransferManager.DataRequested -= dataTransferManager_DataRequested;
            this.dataTransferManager.DataRequested += dataTransferManager_DataRequested;
        }

        public void Reset()
        {
            DelectLocalFolder();
            this.dataTransferManager.DataRequested -= dataTransferManager_DataRequested;
        }

        public void ShowShare(string shareDescription, string shareLinkOrText, SystemShareType systemShareType = SystemShareType.Link, StorageFile imageFile = null, string shareTitle = "Share YouTube Video.")
        {
            isImageFile = true;
            this.ShareDescription = shareDescription;
            this.ShareLinkOrText = shareLinkOrText;
            this.ShareType = systemShareType;
            this.ShareTitle = shareTitle;
            this.ImageFile = imageFile;
            DataTransferManager.ShowShareUI();
        }

        public void ShowShare(string shareDescription, string shareLinkOrText, SystemShareType systemShareType = SystemShareType.Link, string imageUri = "", string shareTitle = "Share YouTube Video.")
        {
            isImageFile = false;
            this.ShareDescription = shareDescription;
            this.ShareLinkOrText = shareLinkOrText;
            this.ShareType = systemShareType;
            this.ShareTitle = shareTitle;
            this.ImageUri = imageUri;
            DataTransferManager.ShowShareUI();
        }

        private async void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if ((await GetShareContent(args.Request)))
            {
                if (String.IsNullOrEmpty(args.Request.Data.Properties.Title) || String.IsNullOrWhiteSpace(args.Request.Data.Properties.Title))
                {
                    args.Request.FailWithDisplayText("Enter a title for what you are sharing and try again.");
                }
            }
        }

        private async Task<bool> GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            //Uri dataPackageUri = ValidateAndGetUri(ShareLink);
            //if (dataPackageUri != null)
            //{
            //    DataPackage requestData = request.Data;
            //    requestData.Properties.Title = ShareTitle;
            //    requestData.Properties.Description = ShareDescription;
            //    requestData.SetUri(dataPackageUri);

            //    if (!isImageFile && !string.IsNullOrEmpty(this.ImageUri) && !string.IsNullOrWhiteSpace(this.ImageUri))
            //    {
            //        await GetlocalUri(this.ImageUri);
            //    }

            //    if (this.ImageFile != null)
            //    {
            //        List<IStorageItem> imageItems = new List<IStorageItem>();
            //        imageItems.Add(this.ImageFile);
            //        requestData.SetStorageItems(imageItems);

            //        RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(this.ImageFile);
            //        if (imageStreamRef != null && (await imageStreamRef.OpenReadAsync()).Size > 0)
            //        {
            //            requestData.Properties.Thumbnail = imageStreamRef;
            //            requestData.SetBitmap(imageStreamRef);
            //        }
            //    }
            //    succeeded = true;
            //}
            //else
            //{
            //    request.FailWithDisplayText("Select an YouTube Link you would like to share and try again.");
            //}

            if (!String.IsNullOrEmpty(ShareTitle) && !string.IsNullOrWhiteSpace(ShareTitle))
            {
                DataPackage requestData = request.Data;
                requestData.Properties.Title = ShareTitle;
                requestData.Properties.Description = ShareDescription;
                if (ShareType == SystemShareType.Link)
                {
                    Uri dataPackageUri = ValidateAndGetUri(ShareLinkOrText);
                    if (dataPackageUri != null)
                    {
                        requestData.SetUri(dataPackageUri);
                    }
                    else
                    {
                        request.FailWithDisplayText("Select an YouTube link you would like to share and try again.");
                        return succeeded;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ShareLinkOrText) && !string.IsNullOrWhiteSpace(ShareLinkOrText))
                    {
                        requestData.SetText(ShareLinkOrText);
                    }
                    else
                    {
                        request.FailWithDisplayText("Select an YouTube content you would like to share and try again.");
                        return succeeded;
                    }
                }

                if (!isImageFile && !string.IsNullOrEmpty(this.ImageUri) && !string.IsNullOrWhiteSpace(this.ImageUri))
                {
                    await GetLocalUri(this.ImageUri);
                }

                if (this.ImageFile != null)
                {
                    List<IStorageItem> imageItems = new List<IStorageItem>();
                    imageItems.Add(this.ImageFile);
                    requestData.SetStorageItems(imageItems);

                    RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(this.ImageFile);
                    if (imageStreamRef != null && (await imageStreamRef.OpenReadAsync()).Size > 0)
                    {
                        requestData.Properties.Thumbnail = imageStreamRef;
                        requestData.SetBitmap(imageStreamRef);
                    }
                }
                succeeded = true;
            }
            else
            {
                request.FailWithDisplayText("Enter a title for what you are sharing and try again.");
            }

            return succeeded;
        }

        private Uri ValidateAndGetUri(string uriString)
        {
            Uri uri = null;
            if (!string.IsNullOrEmpty(uriString) && !string.IsNullOrWhiteSpace(uriString))
            {
                try
                {
                    uri = new Uri(uriString);
                }
                catch (FormatException fe)
                {
                    Debug.WriteLine(fe.Message);
                }
            }
            return uri;
        }

        //文件夹名
        private const string myFolder = "matShareFolder";
        //删除本地文件夹的方法
        private async void DelectLocalFolder()
        {
            try
            {
                //删除系统本地文件夹的myFolder文件夹及其子文件夹
                StorageFolder tempLocalFolder = ApplicationData.Current.LocalFolder;
                StorageFolder shareLocalFolder = await tempLocalFolder.GetFolderAsync(myFolder);
                if (shareLocalFolder != null)
                {
                    await shareLocalFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 把WebUri下完，存放到本地。返回本地的Uri的Uri
        /// </summary>
        /// <param name="WebUri"></param>
        /// <returns></returns>
        private async Task<Uri> GetLocalUri(string WebUri)
        {
            var fileName = string.Empty;
            StorageFolder shareLocalFolder = null;
            StorageFolder tempLocalFolder = ApplicationData.Current.LocalFolder;
            bool isShareLocalFolder = false;
            try
            {
                shareLocalFolder = await tempLocalFolder.CreateFolderAsync(myFolder, CreationCollisionOption.FailIfExists);
            }
            catch
            {
                isShareLocalFolder = true;
            }
            if (isShareLocalFolder)
            {
                shareLocalFolder = await tempLocalFolder.GetFolderAsync(myFolder);
            }
            if (shareLocalFolder != null)
            {
                var rass = RandomAccessStreamReference.CreateFromUri(new Uri(WebUri, UriKind.RelativeOrAbsolute));
                IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
                using (Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead())
                {
                    MemoryStream ms = new MemoryStream();
                    await tempStream.CopyToAsync(ms);
                    byte[] bytes = ms.ToArray();

                    string specialCharacters = @"[\/:*?\""-<>|]";
                    WebUri = Regex.Replace(WebUri, specialCharacters, "");
                    fileName = WebUri + ".jpg";
                    ImageFile = await shareLocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(ImageFile, bytes);
                }

                string filePath = ImageFile.Path;
                return new Uri("ms-appdata:///local/" + myFolder + "/" + fileName);
            }
            else
            {
                return new Uri("ms-appdata:///local/");
            }
        }
    }
}
