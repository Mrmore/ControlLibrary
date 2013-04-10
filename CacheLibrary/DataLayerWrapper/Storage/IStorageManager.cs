using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataLayerWrapper.Storage
{
    public interface IStorageAgent
    {
        Task<T> RetrieveAsync<T>(StorageFile file);
        Task<T> RetrieveAsync<T>(StorageFolder folder, string name);
        Task StoreAsync<T>(StorageFile file, T value);
        Task<IStorageFile> StoreAsync<T>(StorageFolder folder, string name, T value);
        Task DeleteAsync(StorageFile file);
        Task DeleteAsync(StorageFolder folder, string name);
    }
}
