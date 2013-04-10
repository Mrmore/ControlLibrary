using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace DataLayerWrapper.Downloader
{
    public class ImageDownloaderImpl : IDownloader<BitmapImage>
    {
        /// <summary>
        /// args[0] : url, arg[1] : StorageFolder, args[2] : fileName, arg[3] : forceDownload
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<BitmapImage> Download(params object[] args)
        {
            BitmapImage bitmapImage = null;
            try
            {
                IDownloader<StorageFile> impl = new StorageFileDownloader();

                StorageFile file = await impl.Download(args);

                using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("##Download image failed: " + args.ToString() + "_");
                Debug.WriteLine(ex.Message);
            }

            return bitmapImage;
        }
    }
}
