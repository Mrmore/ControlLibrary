using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataLayerWrapper.Helper;
using Windows.Storage;
using Windows.Storage.Search;

namespace DataLayerWrapper.Storage
{
    public class StorageAgentImpl : IStorageAgent
    {
        public StorageAgentImpl()
        {
        }

        public async Task<T> RetrieveAsync<T>(StorageFile file)
        {
            // Validate parameters
            if (file == null)
                throw new ArgumentNullException("file");

            // Open the file from the file stream

            using (Stream fileStream = await file.OpenStreamForReadAsync())
            {
                // Copy the file to a MemoryStream (as we can do this async)
                //fileStream.Position = 0;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(memoryStream);
                }
            }
        }

        public async Task<T> RetrieveAsync<T>(StorageFolder folder, string name)
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException("folder");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            // Open the file, if it doesn't exist then return default, otherwise pass on
            // NB : Currently the way to check if a file exists in WinRT is by catching the exception
            // TODO : Check with Windows 8 RTM whether GetFileAsync(...) method returns null if a file doesn't exist or another method allows checking of this

            try
            {
                StorageFile file = await folder.GetFileAsync(name);
                return await RetrieveAsync<T>(file);
            }
            catch (FileNotFoundException e)
            {
                return default(T);
            }
        }

        public async Task StoreAsync<T>(StorageFile file, T value)
        {
            // Validate parameters

            if (file == null)
                throw new ArgumentNullException("file");

            // Write the object to a MemoryStream using the DataContractSerializer
            // NB: Do this so that,
            //        (i)  We store the state of the object at this point in case it changes before we open the file
            //        (ii) DataContractSerializer doesn't provide async methods for writing to storage
            // TODO : Alternatively we could perform this directly on the file stream and call 'await fileStream.FlushAsync()' (will this ever block?)

            using (MemoryStream dataStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(dataStream, value);

                // Save the data to the file stream

                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    dataStream.Seek(0, SeekOrigin.Begin);
                    await dataStream.CopyToAsync(fileStream);
                }
            }
        }

        public async Task<IStorageFile> StoreAsync<T>(StorageFolder folder, string name, T value)
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException("folder");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            // Create the new file, overwriting the existing data, then pass on

            StorageFile file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await StoreAsync<T>(file, value);
            return file;
        }


        public async Task DeleteAsync(StorageFile file)
        {
            await file.DeleteAsync();
        }

        public async Task DeleteAsync(StorageFolder folder, string name)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            StorageFile file = await folder.GetFileAsync(name);
            await DeleteAsync(file);
        }
    }
}
