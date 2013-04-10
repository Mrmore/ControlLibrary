using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace DataLayerWrapper.Downloader
{
    public class StorageFileDownloader : IDownloader<StorageFile>
    {
        /// <summary>
        /// args[0] : url, arg[1] : StorageFolder, args[2] : fileName, arg[3] : forceDownload
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<StorageFile> Download(params object[] args)
        {
            if (args.Length < 4) throw new ArgumentException();

            string url = (string)args[0];
            StorageFolder folder = (StorageFolder)args[1];
            string fileName = (string)args[2];
            bool forceDownload = (bool)args[3];

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

            if (fileExist == false || forceDownload)
            {
                try
                {
                    file = await folder.CreateFileAsync(fileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                var downloader = new BackgroundDownloader();
                var operation = downloader.CreateDownload(new Uri(url), file);
                await operation.StartAsync();
            }

            return file;
        }
    }
}
