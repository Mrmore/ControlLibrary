using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerWrapper.Downloader
{
    /// <summary>
    /// args[0] : url, arg[1] : StorageFolder, args[2] : fileName, arg[3] : forceDownload
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IDownloader<T>
    {
        Task<T> Download(params object[] args);
    }
}
