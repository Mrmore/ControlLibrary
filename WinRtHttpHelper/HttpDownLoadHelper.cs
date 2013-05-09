using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using WinRt.Net;
using WinRtHttpHelper.Data;
using System.IO;
using System.Net;

namespace WinRtHttpHelper
{
    public class HttpDownLoadHelper : IDisposable, IDownLoad
    {
        /*#region Singleton
        private static HttpDownLoadHelper _instance = null;
        private static readonly object lockObject = new object();

        public static HttpDownLoadHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        _instance = new HttpDownLoadHelper();
                        _instance.cts = new CancellationTokenSource();
                        if (_instance.DownloadBytesCount == null)
                        {
                            _instance.DownloadBytesCount = new DownLoadBytes();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion*/

        private HttpHelper httpHelper = null;
        private CancellationTokenSource cts = null;

        public event EventHandler<DownLoadChangingEventArgs> DownLoadChanging;

        public event EventHandler<DownLoadCompleteEventArgs> DownLoadComplete;

        /// <summary>
        /// 已下载的字节数和总的字节数
        /// </summary>
        public DownLoadBytes DownloadBytesCount
        {
            get;
            private set;
        }

        public HttpDownLoadHelper()
        {
            cts = new CancellationTokenSource();
            if (DownloadBytesCount == null)
            {
                DownloadBytesCount = new DownLoadBytes();
            }
        }

        private void TriggerDownLoadChanging(DownLoadChangingEventArgs e)
        {
            EventHandler<DownLoadChangingEventArgs> handler = DownLoadChanging;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void TriggerDownLoadComplete(DownLoadCompleteEventArgs e)
        {
            EventHandler<DownLoadCompleteEventArgs> handler = DownLoadComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private Task<Stream> GetAsyncStreamDownLoad(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            httpHelper = new HttpHelper(uri);
            var request = httpHelper.HttpWebRequest;
            return httpHelper
                .OpenReadTaskAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled) t.Wait();
                    DownloadBytesCount.TotalBytesToReceive = httpHelper.HttpWebResponse.ContentLength;
                    return t.Result;
                });

            /*
            var httpWebRequest = (HttpWebRequest)WebRequest.CreateHttp(uri);
            InMemoryRandomAccessStream ras = null;
            httpWebRequest.Method = "GET";
            //httpWebRequest.Headers["Range"] = "bytes=" + rangeValue + "-";//设置Range值
            await Task.Run(() =>
                {
                    httpWebRequest.BeginGetResponse(async (IAsyncResult result) =>
                    {
                        HttpWebRequest webRequest = result.AsyncState as HttpWebRequest;
                        HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(result);
                        Stream streamResult = webResponse.GetResponseStream();
                        ras = new InMemoryRandomAccessStream();
                        MemoryStream ms = new MemoryStream();
                        await streamResult.CopyToAsync(ms);
                        byte[] bytes = ms.ToArray();
                        DataWriter datawriter = new DataWriter(ras.GetOutputStreamAt(0));
                        datawriter.WriteBytes(bytes);
                        await datawriter.StoreAsync();

                    }, httpWebRequest);
                });
            return ras as IRandomAccessStream;
            */
        }

        private Task<IRandomAccessStream> GetAsyncIRandomAccessStreamDownLoad(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            httpHelper = new HttpHelper(uri);
            var request = httpHelper.HttpWebRequest;
            return httpHelper
                .OpenReadTaskAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    if (t.IsFaulted || t.IsCanceled) t.Wait();
                    DownloadBytesCount.TotalBytesToReceive = httpHelper.HttpWebResponse.ContentLength;
                    return Task.Run(async () =>
                    {
                        using (var stream = t.Result)
                        {
                            var randomAccessStream = new InMemoryRandomAccessStream();
                            var outputStream = randomAccessStream.GetOutputStreamAt(0);
                            await RandomAccessStream.CopyAsync(stream.AsInputStream(), outputStream);
                            DownloadBytesCount.BytesReceived = Convert.ToInt64(randomAccessStream.Size);
                            return randomAccessStream as IRandomAccessStream;
                        }
                    }).Result;
                });
        }

        public async Task<IRandomAccessStream> StartDownload(Uri uri)
        {
            byte[] bytes = null;
            TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, 0, bytes));
            cts = new CancellationTokenSource();
            DownloadBytesCount = new DownLoadBytes();

            var stream = await GetAsyncStreamDownLoad(uri, cts.Token);

            if (!cts.IsCancellationRequested && stream != null)
            {
                using (stream)
                {
                    var randomAccessStream = new InMemoryRandomAccessStream();
                    var outputStream = randomAccessStream.GetOutputStreamAt(0);
                    await RandomAccessStream.CopyAsync(stream.AsInputStream(), outputStream);
                    DownloadBytesCount.BytesReceived = Convert.ToInt64(randomAccessStream.Size);
                    var randomAccessStreamTemp = randomAccessStream.CloneStream();
                    Stream streamTemp = WindowsRuntimeStreamExtensions.AsStreamForRead(randomAccessStreamTemp.GetInputStreamAt(0));
                    var bytesTemp = ConvertStreamTobyte(streamTemp);
                    TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, 100, bytesTemp));
                    //TriggerDownLoadComplete(new DownLoadCompleteEventArgs(randomAccessStream));
                    TriggerDownLoadComplete(new DownLoadCompleteEventArgs(randomAccessStream.CloneStream()));
                    return randomAccessStream as IRandomAccessStream;
                }
            }
            return null;
        }

        public void CancelDownload()
        {
            httpHelper.CancelAsync();
            cts.Cancel();
        }

        public void Dispose()
        {
            CancelDownload();
        }

        public byte[] ConvertStreamTobyte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
