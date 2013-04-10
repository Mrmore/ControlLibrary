using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataLayerWrapper.Storage;
using RenrenCoreWrapper.Framework.CacheService;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace RenrenCoreWrapper.CacheService
{
    [DataContract]
    class ServiceCacheChip<T> : ICacheChip<T>
    {
        private IStorageAgent _impl = null;
        public ServiceCacheChip(IDictionary<string, ICacheChip<T>> indexs, string hashKey, StorageFolder folder, DateTime? expTime = null)
        {
            this.CachIndexs = indexs;
            this.HashKey = hashKey;
            this.Folder = folder;
            this.ExpirationTime = expTime;
            _impl = new StorageAgentImpl();
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

        public IDictionary<string, ICacheChip<T>> CachIndexs { get; set; }
        T _backup;
        public async Task<T> PickCacheData()
        {
            if (_backup != null) return _backup;

            T result = default(T);

            if (this.CachIndexs.ContainsKey(HashKey))
            {
                var folder = this.CachIndexs[HashKey].Folder;
                DateTime time = (DateTime)Convert.ChangeType(this.ExpirationTime, typeof(DateTime));
                var fileName = HashKey + "." + time.Ticks;
                result = await _impl.RetrieveAsync<T>(folder, fileName);
                _backup = result;
            }

            return result;
        }

        IStorageFile _file = null;
        public async Task SaveCacheData(params object[] contents)
        {
            if (contents.Length < 1) throw new ArgumentException();

            T content = (T)contents[0];
            _backup = content;

            if (this.CachIndexs.ContainsKey(HashKey))
            {
                this.CachIndexs[HashKey] = this;
            }
            else
            {
                this.CachIndexs.Add(HashKey, this);
            }

            var folder = this.Folder;
            DateTime time = (DateTime)Convert.ChangeType(this.ExpirationTime, typeof(DateTime));
            var fileName = HashKey + "." + time.Ticks;
            _file = await _impl.StoreAsync<T>(folder, fileName, content);
        }


        public async Task Reset()
        {
            var folder = this.Folder;
            DateTime time = (DateTime)Convert.ChangeType(this.ExpirationTime, typeof(DateTime));
            var fileName = HashKey + "." + time.Ticks;
            await _impl.DeleteAsync(folder, fileName);
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


        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void AttachProgress(Progress<int> progress)
        {
            throw new NotImplementedException();
        }

        public void DetachProgress()
        {
            throw new NotImplementedException();
        }
    }
}
