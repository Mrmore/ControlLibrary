using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

//Ep:
/*
var forecast = new Forecast { Date = DateTime.Now, Type = FeedType.Daily };
var isoStorage = new IsoStorage<Forecast>(StorageType.Local);
isoStorage.SaveAsync("daily", forecast);

var isoStorage = new IsoStorage<List<Forecast>>(StorageType.Local);

var isoStorage = new IsoStorage<Forecast>(StorageType.Local);
Forecast forecast = await isoStorage.LoadAsync("daily");
*/

namespace WinRtCacheHelper
{
    public class IsoStorage<T>
    {
        private ApplicationData appData = Windows.Storage.ApplicationData.Current;
        private XmlSerializer xmlSerializer;
        private StorageFolder storageFolder;
        private StorageType storageType;
        public StorageType StorageType
        {
            get { return storageType; }
            set
            {
                storageType = value;
                // set the storage folder
                switch (storageType)
                {
                    case StorageType.Local:
                        storageFolder = appData.LocalFolder;
                        break;
                    case StorageType.Temporary:
                        storageFolder = appData.TemporaryFolder;
                        break;
                    case StorageType.Roaming:
                        storageFolder = appData.RoamingFolder;
                        break;
                    default:
                        throw new Exception(String.Format("Unknown StorageType: {0}", storageType));
                }
            }
        }

        public IsoStorage() : this(StorageType.Local) { }
        public IsoStorage(StorageType type)
        {
            xmlSerializer = new XmlSerializer(typeof(T));
            StorageType = type;
        }

        /// <summary>
        /// Saves a serialized object to storage asynchronously
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="obj"></param>
        public async void SaveAsync(string fileName, T data)
        {
            try
            {
                if (data == null)
                    return;
                fileName = AppendExt(fileName);
                var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                var writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                var outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result;
                xmlSerializer.Serialize(outStream, data);
                writeStream.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete a file asynchronously
        /// </summary>
        /// <param name="fileName"></param>
        public async void DeleteAsync(string fileName)
        {
            try
            {
                fileName = AppendExt(fileName);
                var file = await GetFileIfExistsAsync(fileName);
                if (file != null)
                    await file.DeleteAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// At the moment the only way to check if a file exists to catch an exception... :/
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<StorageFile> GetFileIfExistsAsync(string fileName)
        {
            try
            { return await storageFolder.GetFileAsync(fileName); }
            catch
            { return null; }
        }

        /// <summary>
        /// Load a given filename asynchronously
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<T> LoadAsync(string fileName)
        {
            try
            {
                fileName = AppendExt(fileName);
                StorageFile file = null;
                file = await storageFolder.GetFileAsync(fileName);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                Stream inStream = Task.Run(() => readStream.AsStreamForRead()).Result;
                return (T)xmlSerializer.Deserialize(inStream);
            }
            catch (FileNotFoundException)
            {
                //file not existing is perfectly valid so simply return the default 
                return default(T);
                //throw;
            }
            catch (Exception)
            {
                //Unable to load contents of file
                throw;
            }
        }

        /// <summary>
        /// Appends the file extension to the given filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string AppendExt(string fileName)
        {
            if (fileName.Contains(".xml"))
                return fileName;
            else
                return string.Format("{0}.xml", fileName);
        }
    }
}
