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
        public delegate void RequestProgress(List<Tuple<string, ulong, ulong>> sender);
        public event RequestProgress process;

        public delegate void RequestComplete(string sender);
        public event RequestComplete DownloadComplete;

        public delegate void RequestCancel(string sender);
        public event RequestCancel DownloadCancel;

        public delegate void RequestFail(string sender);
        public event RequestFail DownloadFail;
        #endregion

        #region 属性
        public static BackgroundDownloaderHelper _instance;
        public static BackgroundDownloaderHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BackgroundDownloaderHelper();
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
        public void AddDownLoader(Uri SoureUri, StorageFile file)
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
                }
            }
        }

        public async void StopAllDownLoad()
        {
            var res = await GetCurrentDownLoader();
            if (res.Count > 0)
            {
                foreach (var item in res)
                {

                    if (item.Progress.Status == BackgroundTransferStatus.Running)
                    {
                        item.Pause();
                    }
                    else if (item.Progress.Status == BackgroundTransferStatus.PausedNoNetwork)
                    {
                        item.Pause();
                    }
                }
            }
        }

        public async void ResumeDownLoad(Uri SoureUri)
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
                }
            }
        }

        public async void ResumeAllDownLoad(Uri SoureUri)
        {
            var res = await GetCurrentDownLoader();

            if (res.Count > 0)
            {
                foreach (var item in res)
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
                }
            }
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
            List<Tuple<string, ulong, ulong>> result = new List<Tuple<string, ulong, ulong>>();
            var res = await GetCurrentDownLoader();
            foreach (var item in res)
            {
                result.Add(Tuple.Create(item.RequestedUri.ToString(), item.Progress.BytesReceived, item.Progress.TotalBytesToReceive));
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
                    // Attach progress and completion handlers without waiting for completion               
                    if (downloadOperation.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                    {
                        downloadOperation.Pause();
                    }
                    else if (downloadOperation.Progress.Status == BackgroundTransferStatus.PausedNoNetwork)
                    {
                        await Task.Factory.StartNew(() =>
                        {
                            downloadOperation.Pause();
                            downloadOperation.Resume();
                        });

                    }
                    downloadOperation.AttachAsync().AsTask(progressCallback);

                    tasks.Add(downloadOperation.AttachAsync().AsTask());
                }

                while (tasks.Count > 0)
                {
                    // wait for ANY download task to finish
                    Task<DownloadOperation> task = await Task.WhenAny<DownloadOperation>(tasks);
                    tasks.Remove(task);

                    // process the completed task...
                    if (task.IsCanceled)
                    {
                        if (DownloadCancel != null)
                        {
                            DownloadCancel(task.Result.RequestedUri.ToString());
                        }
                    }
                    else if (task.IsFaulted)
                    {
                        if (DownloadFail != null)
                        {
                            DownloadFail(task.Result.RequestedUri.ToString());
                        }
                    }
                    else if (task.IsCompleted)
                    {
                        if (DownloadComplete != null)
                        {
                            DownloadComplete(task.Result.RequestedUri.ToString());
                        }
                    }
                    else
                    {
                        // should never get here....
                    }
                }
            }

        }
        #endregion
    }
}
