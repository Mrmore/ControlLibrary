using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataLayerWrapper.Http
{
    public interface IRenRenHttpAgent
    {
        void AddParameters(string Name, string Value);
        void RemoveParameters(string Name);
        void ClearParameters();
        Task<string> DownloadString(Uri uri, StorageFile file = null);
    }
}
