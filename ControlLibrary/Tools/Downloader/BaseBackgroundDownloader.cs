using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace ControlLibrary.Tools.Downloader
{
    internal class BaseBackgroundDownloader : IDownloader<StorageFile>
    {
        public async Task<StorageFile> Download(string url, string fileName, StorageFolder folder)
        {
            bool fileExist = false;
            StorageFile file = null;
            try
            {
                file = await folder.GetFileAsync(fileName);
                fileExist = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                fileExist = false;
            }

            if (fileExist == false)
            {
                try
                {
                    file = await folder.CreateFileAsync(fileName);
                    var downloader = new BackgroundDownloader();
                    var operation = downloader.CreateDownload(new Uri(url), file);
                    await operation.StartAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return file;
        }

        public async Task<StorageFile> Download(string url, string fileName, string folderName)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            return await Download(url, fileName, folder);
        }
    }
}
