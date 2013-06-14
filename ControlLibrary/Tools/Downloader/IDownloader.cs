using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ControlLibrary.Tools.Downloader
{
    /// <summary>
    /// Downloader uitlity interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDownloader<T>
    {
        Task<T> Download(string url, string fileName, StorageFolder folder);
        Task<T> Download(string url, string fileName, string folderName);
    }
}
