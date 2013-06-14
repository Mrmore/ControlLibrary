using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ControlLibrary.Tools.Downloader
{
    public class UriDownloader : IDownloader<Uri>
    {
        BaseDownloader baseDownloader = null;
        public UriDownloader()
        {
            baseDownloader = new BaseDownloader();
        }

        public async Task<Uri> Download(string url, string fileName, StorageFolder folder)
        {
            StorageFile file = await baseDownloader.Download(url, fileName, folder);
            if (file != null)
            {
                return new Uri("ms-appdata:///local/" + folder.Name + "/" + fileName);
            }
            else
            {
                return null;
            }
        }

        public async Task<Uri> Download(string url, string fileName, string folderName)
        {
            StorageFile file = await baseDownloader.Download(url, fileName, folderName);
            if (file != null)
            {
                return new Uri("ms-appdata:///local/" + folderName + "/" + fileName);
            }
            else
            {
                return null;
            }
        }
    }
}
