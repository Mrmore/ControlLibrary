using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataLayerWrapper.Downloader;
using DataLayerWrapper.Storage;
using RenrenCoreWrapper.Framework.CacheService;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace RenrenCoreWrapper.CacheService
{
    [DataContract]
    public class ImageCacheChip : ICacheChip<BitmapImage>
    {
        public event ChipCompleteEventHandler CacheChipDownloadCompleted;

        private IStorageAgent _impl = null;
        private IProgress<int> _progress = null;
        public ImageCacheChip(IDictionary<string, ICacheChip<BitmapImage>> indexs, string hashKey, StorageFolder folder, DateTime? expTime = null, IProgress<int> progress = null)
        {
            this.CachIndexs = indexs;
            this.HashKey = hashKey;
            this.Folder = folder;
            this.ExpirationTime = expTime;
            _impl = new StorageAgentImpl();
            _imageUrl = new Uri(string.Format("ms-appdata:///local/{0}/{1}", Folder.Name, HashKey));
            _progress = progress;
        }

        public StorageFolder Folder
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? ExpirationTime
        {
            get;
            set;
        }

        [DataMember]
        public string HashKey
        {
            get;
            set;
        }

        public IDictionary<string, ICacheChip<BitmapImage>> CachIndexs { get; set; }

        private Uri _imageUrl = null;
        public async Task<BitmapImage> PickCacheData()
        {
            BitmapImage result = null;
            {
                result = new BitmapImage(_imageUrl);
            }
            return result;
        }

        private volatile bool _imageReady = false;
        public void AddImageUpdate(DependencyObject observer)
        {
            if (!_updateObservers.Contains(observer))
                _updateObservers.Add(observer);
        }

        StorageFile _file = null;
        DownloadOperation _download = null;
        private CancellationTokenSource _cancelHandler = new CancellationTokenSource();
        private IList<DependencyObject> _updateObservers = new List<DependencyObject>();
        public bool ImageReady { get { return _imageReady; } set { _imageReady = value; } }
        public async Task SaveCacheData(params object[] contents)
        {
            if (contents.Length < 1) throw new ArgumentException();

            string url = (string)contents[0];

            try
            {
                try
                {
                    bool fileExist = false;
                    try
                    {
                        _file = await Folder.GetFileAsync(HashKey);
                        fileExist = true;
                        _imageReady = true;
                        return;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        fileExist = false;
                    }
                    if (fileExist == false)
                    {
                        _file = await Folder.CreateFileAsync(HashKey, CreationCollisionOption.ReplaceExisting);
                    }

                    _imageReady = false;
                    if (CacheChipDownloadCompleted != null)
                    {
                        CacheChipDownloadCompleted(this, CacheStatus.PENDING, this._imageUrl);
                    }

                    var downloader = new BackgroundDownloader();
                    var uri = new Uri(url);
                    _download = downloader.CreateDownload(uri, _file);
                    // TODO: Add the progress info notification
                    Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                    await _download.StartAsync().AsTask(_cancelHandler.Token, progressCallback);
                    _imageReady = true;


                    foreach (var item in this._updateObservers)
                    {
                        item.Dispatcher.RunIdleAsync((e) =>
                        {
                            //有几率在这里BUG,打开文件失败，数据流为NULL
                            //using (var stream = file.OpenReadAsync().AsTask().Result)
                            {
                                if (item is Image)
                                {
                                    (item as Image).Source = null;
                                    (item as Image).Source = new BitmapImage(this._imageUrl);
                                }
                                else if (item is ImageBrush)
                                {
                                    (item as ImageBrush).ImageSource = null;
                                    (item as ImageBrush).ImageSource = new BitmapImage(this._imageUrl);
                                }
                            }
                        });
                    }

                    _updateObservers.Clear();
                    if (CacheChipDownloadCompleted != null)
                    {
                        CacheChipDownloadCompleted(this, CacheStatus.READY, this._imageUrl);
                    }
                }
                catch (Exception ex)
                {
                    if (CacheChipDownloadCompleted != null)
                    {
                        CacheChipDownloadCompleted(this, CacheStatus.FAILED, this._imageUrl);
                    }
                    Reset();
                    Debug.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        public async Task Reset()
        {
            try
            {
                var folder = this.Folder;
                var fileName = HashKey;
                this._imageReady = false;
                await _impl.DeleteAsync(folder, fileName);
            }
            catch (Exception)
            { }
        }


        public async Task<IStorageFile> PickCacheFile()
        {
            if (_file != null)
            {
                return _file;
            }
            else
            {
                return await this.Folder.GetFileAsync(this.HashKey);
            }
        }

        // Note that this event is invoked on a background thread, so we cannot access the UI directly.
        private void DownloadProgress(DownloadOperation download)
        {
            Debug.WriteLine(String.Format("Progress: {0}, Status: {1}", download.Guid, download.Progress.Status));

            double percent = 100;
            if (download.Progress.TotalBytesToReceive > 0)
            {
                percent = download.Progress.BytesReceived * 100 / download.Progress.TotalBytesToReceive;
            }

            Debug.WriteLine(String.Format(" - Transfered bytes: {0} of {1}, {2}%",
                download.Progress.BytesReceived, download.Progress.TotalBytesToReceive, percent));
            if (_progress != null)
            {
                _progress.Report((int)percent);
            }


            if (download.Progress.HasRestarted)
            {
                Debug.WriteLine(" - Download restarted");
            }

            if (download.Progress.HasResponseChanged)
            {
                // We've received new response headers from the server.
                Debug.WriteLine(" - Response updated; Header count: " + download.GetResponseInformation().Headers.Count);

                // If you want to stream the response data this is a good time to start.
                // download.GetResultStreamAt(0);
            }
        }


        public void Cancel()
        {
            if (this._download != null)
            {
                this._cancelHandler.Cancel();
                this._cancelHandler.Dispose();

                this._cancelHandler = new CancellationTokenSource();
                this.Reset();
                Debug.WriteLine("Canceled Download: " + _download.Guid);
            }
        }

        public void Pause()
        {
            if (this._download != null)
            {
                this._download.Pause();
                Debug.WriteLine("Paused Download: " + _download.Guid);
            }
        }

        public void Resume()
        {
            if (this._download != null)
            {
                this._download.Resume();
                Debug.WriteLine("Paused Download: " + _download.Guid);
            }
        }

        public void AttachProgress(Progress<int> progress)
        {
            if (progress == null) throw new ArgumentNullException();

            this._progress = progress;
        }


        public void DetachProgress()
        {
            this._progress = null;
        }
    }
}
