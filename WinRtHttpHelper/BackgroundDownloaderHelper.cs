using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace WinRtHttpHelper
{
    public class BackgroundDownloaderHelper
    {
        #region 构造函数
        private BackgroundDownloaderHelper()
        {
            backgroundDownloader = new BackgroundDownloader();
            progressCallback = new Progress<DownloadOperation>(DownloadProgress);
            cts = new CancellationTokenSource();
            DownLoadControl();
        }
        #endregion

        #region Member
        private BackgroundDownloader backgroundDownloader;
        private Progress<DownloadOperation> progressCallback;
        private CancellationTokenSource cts;
        public delegate void RequestProgress(List<Tuple<string, ulong, ulong, BackgroundTransferStatus>> sender);
        public event RequestProgress process;
        #endregion

        #region 属性
        private volatile static BackgroundDownloaderHelper _instance = null;
        private static readonly object lockObject = new object();

        public static BackgroundDownloaderHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new BackgroundDownloaderHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        private int count = 0;
        public int Count
        {
            set
            {
                count = value;
            }
            get
            {
                return count;
            }
        }


        #endregion

        #region Public Method
        public void AddDownLoader(Uri SoureUri, IStorageFile file)
        {
            backgroundDownloader.CreateDownload(SoureUri, file).StartAsync().AsTask(cts.Token, progressCallback);
        }

        public async void RemoveDownLoadr(Uri SoureUri)
        {
            var res = await GetCurrentDownLoader();
            var findreult = from item in res
                            where item.RequestedUri == SoureUri
                            select item;
            IReadOnlyList<DownloadOperation> result = findreult.ToList();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    await item.ResultFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    break;
                }
            }
        }

        public async void StopDownLoad(Uri SoureUri)
        {
            var res = await GetCurrentDownLoader();
            var findreult = from item in res
                            where item.RequestedUri == SoureUri
                            select item;
            IReadOnlyList<DownloadOperation> result = findreult.ToList();
            if (result.Count > 0)
            {
                foreach (var item in result)
                {

                    if (item.Progress.Status == BackgroundTransferStatus.Running)
                    {
                        item.Pause();
                    }
                    else if (item.Progress.Status == BackgroundTransferStatus.PausedNoNetwork)
                    {
                        item.Pause();
                    }
                    else if (item.Progress.Status == BackgroundTransferStatus.Error)
                    {
                        AddDownLoader(item.RequestedUri, item.ResultFile);
                        item.Pause();

                    }
                }
            }
        }

        public async void ResumeDownLoad(Uri SoureUri)
        {
            try
            {
                var res = await GetCurrentDownLoader();
                var findreult = from item in res
                                where item.RequestedUri == SoureUri
                                select item;
                IReadOnlyList<DownloadOperation> result = findreult.ToList();
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {

                        if (item.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                        {
                            item.Resume();
                        }
                        else if (item.Progress.Status == BackgroundTransferStatus.PausedNoNetwork)
                        {
                            item.Pause();
                            item.Resume();
                        }
                        else if (item.Progress.Status == BackgroundTransferStatus.Error ||
                            item.Progress.Status == BackgroundTransferStatus.Completed)
                        {
                            AddDownLoader(item.RequestedUri, item.ResultFile);
                        }

                    }
                }
            }
            catch
            { }
        }

        public async Task<IReadOnlyList<DownloadOperation>> GetCurrentDownLoader()
        {
            var result = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (result != null)
            {
                Count = result.Count;
            }
            return result;
        }
        #endregion

        #region Private Method
        private async void DownloadProgress(DownloadOperation download)
        {
            List<Tuple<string, ulong, ulong, BackgroundTransferStatus>> result = new List<Tuple<string, ulong, ulong, BackgroundTransferStatus>>();
            var res = await GetCurrentDownLoader();
            foreach (var item in res)
            {
                result.Add(Tuple.Create(item.RequestedUri.ToString(), item.Progress.BytesReceived,
                    item.Progress.TotalBytesToReceive, item.Progress.Status));

            }
            if (process != null)
            {
                process(result);
            }
        }

        private async void DownLoadControl()
        {
            IReadOnlyList<DownloadOperation> currentDownloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (currentDownloads.Count > 0)
            {
                List<Task<DownloadOperation>> tasks = new List<Task<DownloadOperation>>();
                foreach (DownloadOperation downloadOperation in currentDownloads)
                {
                    // 每次进入时恢复所有下载               
                    //if (downloadOperation.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                    //{
                    //    downloadOperation.Resume();
                    //}
                    //else if (downloadOperation.Progress.Status == BackgroundTransferStatus.PausedNoNetwork)
                    //{
                    //    await Task.Factory.StartNew(() =>
                    //        {
                    //            downloadOperation.Pause();
                    //            downloadOperation.Resume();                              
                    //        });

                    //}
                    await downloadOperation.AttachAsync().AsTask(progressCallback);
                    tasks.Add(downloadOperation.AttachAsync().AsTask());
                }
            }

        }
        #endregion
    }
}
