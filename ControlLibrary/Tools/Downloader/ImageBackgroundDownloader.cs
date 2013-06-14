using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools.Downloader
{
    public class ImageBackgroundDownloader : IDownloader<BitmapImage>
    {
        IDownloader<StorageFile> isf = null;
        public ImageBackgroundDownloader()
        {
            isf = new BaseBackgroundDownloader();
        }

        public async Task<BitmapImage> Download(string url, string fileName, StorageFolder folder)
        {
            StorageFile file = await isf.Download(url, fileName, folder);
            return await StorageFileToBitmapImage(file, url);
        }

        public async Task<BitmapImage> Download(string url, string fileName, string folderName)
        {
            StorageFile file = await isf.Download(url, fileName, folderName);
            return await StorageFileToBitmapImage(file, url);
        }

        private async Task<BitmapImage> StorageFileToBitmapImage(StorageFile sf, string url)
        {
            BitmapImage bitmapImage = null;
            try
            {
                //RandomAccessStreamReference rStream = RandomAccessStreamReference.CreateFromFile(sf);
                //IRandomAccessStreamWithContentType stream = await rStream.OpenReadAsync();
                using (IRandomAccessStreamWithContentType stream = await sf.OpenReadAsync())
                {
                    bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("##Download image failed: " + url.ToString() + "_");
                Debug.WriteLine(ex.Message);
            }

            return bitmapImage;
        }
    }
}
