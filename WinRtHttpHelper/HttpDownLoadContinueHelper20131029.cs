using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using WinRtHttpHelper.Data;

namespace WinRtHttpHelper
{
    public class HttpDownLoadContinueHelper : IDisposable, IContinueDownLoad
    {
        /*#region Singleton
        private static HttpDownLoadContinueHelper _instance = null;
        private static readonly object lockObject = new object();

        public static HttpDownLoadContinueHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        _instance = new HttpDownLoadContinueHelper();
                        _instance.hc = new HttpClient();
                        _instance.cts = new CancellationTokenSource();
                        if(_instance.DownloadBytesCount == null)
                        {
                            _instance.DownloadBytesCount = new DownLoadBytes();
                        }
                        _instance.Bytes = new List<byte>();
                    }
                }

                return _instance;
            }
        }
        #endregion*/

        private HttpClient hc = null;
        private CancellationTokenSource cts = null;
        private HttpRequestMessage request = null;
        private HttpResponseMessage response = null;
        //切片大小
        private long percentage = 0;
        //切片块数 进度系数 已完成的比例
        private int percentageCoefficient = 0, progressCoefficient = 0, completePercentage = 0;
        //下载拼装的所有字节数
        private List<byte> Bytes = null;
        //下载是否为暂停，默认为false没有暂停
        private bool isPause = false;

        //不声明委托
        //public delegate void DownLoadChanging(object sender, DownLoadChangingEventArgs args);
        /// <summary>
        /// 下载的实时进度事件
        /// </summary>
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

        public HttpDownLoadContinueHelper()
        {
            hc = new HttpClient();
            cts = new CancellationTokenSource();
            if (DownloadBytesCount == null)
            {
                DownloadBytesCount = new DownLoadBytes();
            }
            Bytes = new List<byte>();
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

        //暂时弃用
        private async Task<long> ReadBytesFromServer(Stream stream, List<byte> byteAll, CancellationToken ct)
        {
            if (stream == null)
            {
                return 0;
            }

            int readCount = 0;
            byte[] hasRead = new byte[1024];
            long read = DownloadBytesCount.BytesReceived;
            try
            {
                do
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    readCount = await stream.ReadAsync(hasRead, 0, hasRead.Length, ct);
                    read += readCount;
                    double percent = (double)read * 100 / (double)DownloadBytesCount.BytesReceived;
                    percent = Math.Round(percent, MidpointRounding.ToEven);
                    TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, Convert.ToInt32(percent), null));
                    if (readCount == 1024)
                    {
                        byteAll.AddRange(hasRead);
                    }
                    else
                    {
                        byte[] canbeAddBytes = hasRead.Take(readCount).ToArray();
                        byteAll.AddRange(canbeAddBytes);
                    }
                } while (readCount != 0 && read <= DownloadBytesCount.TotalBytesToReceive);
            }
            catch (TaskCanceledException)
            {
                return read;
            }

            return read;
        }

        /// <summary>
        /// 分析下载的资源大小，按照规则分配下载的策略
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<long> GetTotalBytes(Uri uri)
        {
            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Range = new RangeHeaderValue(0, 0);
            response = await hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
            DownloadBytesCount.TotalBytesToReceive = response.Content.Headers.ContentRange.Length.Value;
            if (response.Content.Headers.ContentRange.Length.Value > 10000)
            {
                percentageCoefficient = 100;
                progressCoefficient = 1;
            }
            if (response.Content.Headers.ContentRange.Length.Value > 1000 && response.Content.Headers.ContentRange.Length.Value < 10000)
            {
                percentageCoefficient = progressCoefficient = 10;
            }
            if (response.Content.Headers.ContentRange.Length.Value < 1000)
            {
                percentageCoefficient = 1;
                progressCoefficient = 100;
            }
            percentage = DownloadBytesCount.TotalBytesToReceive / percentageCoefficient;
            completePercentage = Convert.ToInt32(DownloadBytesCount.BytesReceived / percentage);
            return response.Content.Headers.ContentRange.Length.Value;
        }

        //0-100 101-200 101得到已下载大小 from 需要 -1，(from - 1)
        //0-100 101-200 100得到已下载大小 from 不需要 -1，(from)
        private async Task<long> GetTotalBytes(Uri uri, long fromBytes = 0)
        {
            if (DownloadBytesCount == null)
            {
                DownloadBytesCount = new DownLoadBytes();
            }
            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Range = new RangeHeaderValue(0, 0);
            response = await hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
            DownloadBytesCount.TotalBytesToReceive = response.Content.Headers.ContentRange.Length.Value;
            if (response.Content.Headers.ContentRange.Length.Value > 10000)
            {
                percentageCoefficient = 100;
                progressCoefficient = 1;
            }
            if (response.Content.Headers.ContentRange.Length.Value > 1000 && response.Content.Headers.ContentRange.Length.Value < 10000)
            {
                percentageCoefficient = progressCoefficient = 10;
            }
            if (response.Content.Headers.ContentRange.Length.Value < 1000)
            {
                percentageCoefficient = 1;
                progressCoefficient = 100;
            }
            percentage = DownloadBytesCount.TotalBytesToReceive / percentageCoefficient;
            //completePercentage = Convert.ToInt32((fromBytes - 1) / percentage);
            completePercentage = Convert.ToInt32((fromBytes) / percentage);
            DownloadBytesCount.BytesReceived = fromBytes;
            return response.Content.Headers.ContentRange.Length.Value;
        }

        //暂时不实现
        private async Task<long> GetTotalBytes(Uri uri, long fromBytes = 0, long toBytes = 0)
        {
            return 0;
        }

        //分段设置的方法
        private async Task<IRandomAccessStream> DownLoadIRandomAccessStream(Uri uri, long rangeFromValue, long rangeToValue, CancellationToken ct)
        {
            request = new HttpRequestMessage(HttpMethod.Get, uri);
            //var request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("keepAlive", "false");
            //request.Headers.Range = new RangeHeaderValue(0, 500);
            if (rangeToValue > 0 && rangeToValue > rangeFromValue)
            {
                request.Headers.Add("Range", "bytes=" + rangeFromValue + "-" + rangeToValue);
            }
            else
            {
                request.Headers.Add("Range", "bytes=" + rangeFromValue + "-");
            }
            response = await hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            //response = await hc.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                if (responseStream != null)
                {
                    var randomAccessStream = new InMemoryRandomAccessStream();
                    var outputStream = randomAccessStream.GetOutputStreamAt(0);
                    await RandomAccessStream.CopyAsync(responseStream.AsInputStream(), outputStream);
                    DownloadBytesCount.BytesReceived = Convert.ToInt64(randomAccessStream.Size);
                    return randomAccessStream as IRandomAccessStream;
                }
                return null;
            }
        }

        /*
        //下载全部
        private async Task<IRandomAccessStream> DownLoadIRandomAccessStream(Uri uri, long rangeFromValue, long rangeToValue, CancellationToken ct)
        {
            request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("keepAlive", "false");
            //request.Headers.Range = new RangeHeaderValue(0, 500);
            if (rangeToValue > 0 && rangeToValue > rangeFromValue)
            {
                request.Headers.Add("Range", "bytes=" + rangeFromValue + "-" + rangeToValue);
            }
            else
            {
                request.Headers.Add("Range", "bytes=" + rangeFromValue + "-");
            }
            response = await hc.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead, ct);
            DownloadBytesCount.TotalBytesToReceive = response.Content.Headers.ContentRange.Length.Value;
            using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                var randomAccessStream = new InMemoryRandomAccessStream();
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(responseStream.AsInputStream(), outputStream);
                DownloadBytesCount.BytesReceived = Convert.ToInt64(randomAccessStream.Size);
                return randomAccessStream as IRandomAccessStream;
            }
        }
        */

        public void Dispose()
        {
            CancelDownload();
            hc.Dispose();
        }

        /// <summary>
        /// 暂停下载
        /// </summary>
        /// <returns></returns>
        public async Task<IRandomAccessStream> SuspendDownload()
        {
            if (!isPause)
            {
                isPause = true;
                CancelDownload();
                return (await this.ByteToInMemoryRandomAccessStream(Bytes.ToArray()));
            }
            return null;
        }

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="formBytes"></param>
        /// <param name="toBytes"></param>
        /// <returns></returns>
        public async Task<IRandomAccessStream> ContinueDownload(Uri uri, long fromBytes = 0, long toBytes = 0, byte[] CompletedBytes = null)
        {
            hc = new HttpClient();
            cts = new CancellationTokenSource();
            byte[] bytes = null;
            if (Bytes == null)
            {
                Bytes = new List<byte>();
            }
            Bytes.AddRange(CompletedBytes);
            IRandomAccessStream idaStream = null;

            try
            {
                idaStream = await DownLoadIRandomAccessStream(uri, fromBytes, toBytes, cts.Token);
                bytes = Combine(idaStream);
                TriggerDownLoadComplete(new DownLoadCompleteEventArgs((await this.ByteToInMemoryRandomAccessStream((byte[])bytes.Clone()))));
                return (await this.ByteToInMemoryRandomAccessStream(bytes));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fromBytes"></param>
        /// <returns></returns>
        public async Task<IRandomAccessStream> ContinueDownload(Uri uri, long fromBytes = 0, byte[] CompletedBytes = null)
        {
            hc = new HttpClient();
            cts = new CancellationTokenSource();
            if (Bytes == null)
            {
                Bytes = new List<byte>();
            }
            //防止字节数组传递到事件，修改数组内容，所以拼大数组时，做了一个拷贝浅表副本
            Bytes.AddRange((byte[])CompletedBytes.Clone());
            byte[] bytes = null;
            IRandomAccessStream idaStream = null;
            await GetTotalBytes(uri, fromBytes);
            TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, completePercentage, CompletedBytes));
            try
            {
                //100, 10, 1
                for (int i = completePercentage; i < percentageCoefficient; )
                {
                    if (cts.IsCancellationRequested)
                    {
                        return (await this.ByteToInMemoryRandomAccessStream(bytes));
                    }

                    if (i == 0)
                    {
                        idaStream = (await DownLoadIRandomAccessStream(uri, i * percentage, (i + 1) * percentage, cts.Token));
                    }
                    else
                    {
                        idaStream = (await DownLoadIRandomAccessStream(uri, i * percentage + 1, (i + 1) * percentage, cts.Token));
                    }
                    if (idaStream != null)
                    {
                        bytes = Combine(idaStream);
                        DownloadBytesCount.BytesReceived = (i + 1) * percentage;
                        i++;
                        TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, i * progressCoefficient, bytes));
                        if (i * progressCoefficient == 100)
                        {
                            TriggerDownLoadComplete(new DownLoadCompleteEventArgs((await this.ByteToInMemoryRandomAccessStream(bytes))));
                        }
                    }
                    else
                    {
                        return (await this.ByteToInMemoryRandomAccessStream(Bytes.ToArray()));
                    }
                }
                return (await this.ByteToInMemoryRandomAccessStream(bytes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return this.ByteToInMemoryRandomAccessStream(bytes).Result;
            }
        }

        /// <summary>
        /// 继续下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<IRandomAccessStream> ContinueDownload(Uri uri)
        {
            if (isPause)
            {
                isPause = false;
                return (await Download(uri, DownLoadState.Continue));
            }
            return null;
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        public void CancelDownload()
        {
            cts.Cancel();
            cts.Dispose();
            request.Dispose();
            response.Dispose();
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<IRandomAccessStream> StartDownload(Uri uri)
        {
            isPause = false;
            return (await Download(uri));
        }

        //下载
        private async Task<IRandomAccessStream> Download(Uri uri, DownLoadState downLoadState = DownLoadState.Start)
        {
            hc = new HttpClient();
            cts = new CancellationTokenSource();
            byte[] bytes = null;
            IRandomAccessStream idaStream = null;
            if (downLoadState == DownLoadState.Start)
            {
                DownloadBytesCount = new DownLoadBytes();
                Bytes = new List<byte>();
                TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, 0, bytes));
                if (percentage == 0)
                {
                    await GetTotalBytes(uri);
                }
            }
            if (downLoadState == DownLoadState.Continue)
            {
                await GetTotalBytes(uri);
            }

            try
            {
                //100, 10, 1
                for (int i = completePercentage; i < percentageCoefficient; )
                {
                    if (cts.IsCancellationRequested)
                    {
                        return (await this.ByteToInMemoryRandomAccessStream(bytes));
                    }

                    if (i == 0)
                    {
                        idaStream = (await DownLoadIRandomAccessStream(uri, i * percentage, (i + 1) * percentage, cts.Token));
                    }
                    else
                    {
                        idaStream = (await DownLoadIRandomAccessStream(uri, i * percentage + 1, (i + 1) * percentage, cts.Token));
                    }
                    if (idaStream != null)
                    {
                        bytes = Combine(idaStream);
                        DownloadBytesCount.BytesReceived = (i + 1) * percentage;
                        i++;
                        TriggerDownLoadChanging(new DownLoadChangingEventArgs(DownloadBytesCount, i * progressCoefficient, bytes));
                        if (i * progressCoefficient == 100)
                        {
                            TriggerDownLoadComplete(new DownLoadCompleteEventArgs((await this.ByteToInMemoryRandomAccessStream(bytes))));
                        }
                    }
                    else
                    {
                        return (await this.ByteToInMemoryRandomAccessStream(Bytes.ToArray()));
                    }
                }
                return (await this.ByteToInMemoryRandomAccessStream(bytes));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return this.ByteToInMemoryRandomAccessStream(bytes).Result;
            }
        }

        //合成数据
        private byte[] Combine(IRandomAccessStream iRandomAccessStream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(iRandomAccessStream.GetInputStreamAt(0));
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                Bytes.AddRange(ms.ToArray());
                return Bytes.ToArray();
            }
        }

        //字节数组转换InMemoryRandomAccessStream数据流
        private async Task<InMemoryRandomAccessStream> ByteToInMemoryRandomAccessStream(byte[] bytes)
        {
            InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
            datawriter.WriteBytes(bytes);
            await datawriter.StoreAsync();
            return memoryStream;
        }
    }
}