using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace WinRtHttpHelper.Data
{
    public interface IContinueDownLoad : IDownLoad
    {
        /// <summary>
        /// 暂停下载
        /// </summary>
        Task<IRandomAccessStream> SuspendDownload();

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fromBytes"></param>
        /// <param name="toBytes"></param>
        /// <param name="CompletedBytes"></param>
        Task<IRandomAccessStream> ContinueDownload(Uri uri, long fromBytes = 0, long toBytes = 0, byte[] CompletedBytes = null);

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fromBytes"></param>
        /// <param name="CompletedBytes"></param>
        Task<IRandomAccessStream> ContinueDownload(Uri uri, long fromBytes = 0, byte[] CompletedBytes = null);

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        Task<IRandomAccessStream> ContinueDownload(Uri uri);
    }
}
