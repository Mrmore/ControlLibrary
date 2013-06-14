using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ControlLibrary.Tools.Downloader
{
    internal class BaseDownloader : IDownloader<StorageFile>
    {
        public async Task<StorageFile> Download(string url, string fileName, Windows.Storage.StorageFolder folder)
        {
            bool fileExist = false;
            StorageFile file = null;
            try
            {
                file = await folder.GetFileAsync(fileName);
                fileExist = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                fileExist = false;
            }

            if (fileExist == false)
            {
                try
                {
                    var streamRef = RandomAccessStreamReference.CreateFromUri(new Uri(url));
                    using (var stream = await streamRef.OpenReadAsync())
                    {
                        using (Stream tempStream = stream.GetInputStreamAt(0).AsStreamForRead())
                        {
                            MemoryStream ms = new MemoryStream();
                            await tempStream.CopyToAsync(ms);
                            byte[] bytes = ms.ToArray();

                            file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteBytesAsync(file, bytes);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            return file;
        }

        public async Task<StorageFile> Download(string url, string fileName, string folderName)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var folder = await localFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            return await Download(url, fileName, folder);
        }
    }
}
