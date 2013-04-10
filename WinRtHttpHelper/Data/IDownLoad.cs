using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace WinRtHttpHelper.Data
{
    public interface IDownLoad
    {
        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<IRandomAccessStream> StartDownload(Uri uri);

        /// <summary>
        /// 取消下载
        /// </summary>
        void CancelDownload();
    }
}
