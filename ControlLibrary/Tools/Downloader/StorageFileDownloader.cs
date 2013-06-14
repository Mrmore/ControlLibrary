using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ControlLibrary.Tools.Downloader
{
    public class StorageFileDownloader : IDownloader<StorageFile>
    {
        BaseDownloader baseDownloader = null;
        public StorageFileDownloader()
        {
            baseDownloader = new BaseDownloader();
        }

        public async Task<StorageFile> Download(string url, string fileName, StorageFolder folder)
        {
            return (await baseDownloader.Download(url, fileName, folder));
        }

        public async Task<StorageFile> Download(string url, string fileName, string folderName)
        {
            return (await baseDownloader.Download(url, fileName, folderName));
        }
    }
}
