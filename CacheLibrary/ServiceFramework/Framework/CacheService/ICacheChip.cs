using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RenrenCoreWrapper.Framework.CacheService
{
    public interface ICacheChip<T>
    {
        StorageFolder Folder { get; set; }
        DateTime? ExpirationTime { get; set; }
        string HashKey { get; set; }
        IDictionary<string, ICacheChip<T>> CachIndexs { get; set; }

        Task<T> PickCacheData();
        Task<IStorageFile> PickCacheFile();
        Task SaveCacheData(params object[] contents);
        Task Reset();

        // handle progress & cancel issue
        void Cancel();
        void Pause();
        void Resume();
        void AttachProgress(Progress<int> progress);
        void DetachProgress();
    }
}
