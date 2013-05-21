using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Media.PlayTo;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.FileProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage9 : Page
    {
        private BitmapImage bmp = null;
        private DispatcherTimer dispatcherTimer = null;
        private InMemoryRandomAccessStream memoryStream = null;
        private List<BitFrame> bitFrame = null;

        public BlankPage9()
        {
            this.InitializeComponent();
            bmp = new BitmapImage();
            dispatcherTimer = new DispatcherTimer();
            memoryStream = new InMemoryRandomAccessStream();
            dispatcherTimer.Tick -= dispatcherTimer_Tick;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            clipImageBrush.ImageDownLoadProgress -= clipImageBrush_ImageDownLoadProgress;
            clipImageBrush.ImageDownLoadProgress += clipImageBrush_ImageDownLoadProgress;
        }

        void clipImageBrush_ImageDownLoadProgress(int downloadValue)
        {
            progressBar.Value = downloadValue;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            clipImageBrush.Source = tb.Text;
        }

        private void bt_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            clipImageBrush.IsCacheImage = true;
        }

        private async void bt_Copy1_Click_1(object sender, RoutedEventArgs e)
        {
            bool temp = await clipImageBrush.SaveImage();
        }

        private void bt_Copy2_Click_1(object sender, RoutedEventArgs e)
        {
            clipImageBrush.IsCacheImage = false;
        }

        private async void bt_Copy3_Click_1(object sender, RoutedEventArgs e)
        {
            bool temp = await clipImageBrush.SaveFileImage();
        }

        private void bt_Copy4_Click_1(object sender, RoutedEventArgs e)
        {
            //bt_Copy4
            //Windows.UI.Core.CoreCursor coreCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 0);
            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 1);
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Custom, 101);
        }

        private async void bt_Copy5_Click_1(object sender, RoutedEventArgs e)
        {
            BitmapImage bi = new BitmapImage();
            //bi.UriSource = new Uri(tbgif.Text, UriKind.RelativeOrAbsolute);
            //imageGif.Source = bi;
            //bi.ImageOpened += (ee, ss) =>
            //    {
            //        var s1 = ee as BitmapImage;
            //        var h = s1.PixelHeight;
            //    };
            //imageGif.ImageOpened += (ss, ee) =>
            //    {
            //        var s1 = ss as Image;
            //        PlayToSource PlayToSource = s1.PlayToSource;
            //        PlayToConnection Connection = PlayToSource.Connection;
            //        var yy = Connection.State.ToString();
            //    };
            //tbgif.Text = "http://img.t.sinajs.cn/t35/style/images/common/face/ext/normal/c9/geili_org.gif";
            //tbgif.Text = "http://ww2.sinaimg.cn/bmiddle/666d8d19jw1dvg3m7e0q0j.jpg";   
            RandomAccessStreamReference rass = RandomAccessStreamReference.CreateFromUri(new Uri(tbgif.Text, UriKind.RelativeOrAbsolute));
            IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
            Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);
            //bi.SetSource(randomAccessStream);
            //imageGif.Source = bi;
            //imageBrush.ImageSource = bi;
            Gif(randomAccessStream);
        }

        private async void Gif(IRandomAccessStream fileStream)
        {
            /*
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
            BitmapFrame frame = await decoder.GetFrameAsync(0);
            int frameCount = Convert.ToInt32(decoder.FrameCount);
            var frameProperties = await frame.BitmapProperties.GetPropertiesAsync(new List<string>());

            var imageDescriptionProperties = await (frameProperties["/imgdesc"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Top", "/Left", "/Width", "/Height" });
            int top = Int32.Parse(imageDescriptionProperties["/Top"].Value.ToString());
            int left = Int32.Parse(imageDescriptionProperties["/Left"].Value.ToString());
            int width = Int32.Parse(imageDescriptionProperties["/Width"].Value.ToString());
            int height = Int32.Parse(imageDescriptionProperties["/Height"].Value.ToString());

            var gifControlExtensionProperties = await (frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay", "/UserInputFlag" });
            TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
            bool userInputFlag = bool.Parse(gifControlExtensionProperties["/UserInputFlag"].Value.ToString());
            */
            
            ////WriteableBitmap wb = new WriteableBitmap(width, height);

            //PixelDataProvider pdp = await frame.GetPixelDataAsync();
            //byte[] bytes = pdp.DetachPixelData();
            //IBuffer iBuffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
            //InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
            //DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
            //datawriter.WriteBuffer(iBuffer, 0, iBuffer.Length);
            //await datawriter.StoreAsync();


            //BitmapTransform transform = new BitmapTransform();
            //transform.Bounds = new BitmapBounds() { Width = Convert.ToUInt32(width), Height = Convert.ToUInt32(height) };
            //PixelDataProvider pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Rgba8, decoder.BitmapAlphaMode, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
            //byte[] bytes = pixelData.DetachPixelData();
            //IBuffer iBuffer = WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
            //InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
            //DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
            //datawriter.WriteBuffer(iBuffer, 0, iBuffer.Length);
            //await datawriter.StoreAsync();


            //Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(fileStream.GetInputStreamAt(0));
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //}

            //新的篇章

            CreateGif(fileStream);

            /*
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(memoryStream.GetInputStreamAt(0));
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
            }

            StorageFile storageFile = null;
            storageFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("aaa", CreationCollisionOption.ReplaceExisting);
            //await FileIO.WriteBytesAsync(storageFile, bytes);
            await FileIO.WriteBytesAsync(storageFile, buffer);
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("图片类型", new List<string>() { ".jpg" });

            savePicker.SuggestedFileName = storageFile.Name;
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await storageFile.CopyAndReplaceAsync(file);//, file.Name, NameCollisionOption.GenerateUniqueName);
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    
                }
            }*/

            //BitmapImage bi = new BitmapImage();
            //bi.SetSource(inStream);
            //imageGif.Source = bi;
            //imageBrush.ImageSource = bi;
        }

        private async void CreateGif(IRandomAccessStream fileStream)
        {
            var decoderId = BitmapDecoder.GifDecoderId;
            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, fileStream);
            var frame = await bitDecoder.GetFrameAsync(Convert.ToUInt32(2));

            //
            var frameProperties = await frame.BitmapProperties.GetPropertiesAsync(new List<string>());

            var imageDescriptionProperties = await(frameProperties["/imgdesc"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Top", "/Left", "/Width", "/Height" });
            int top = Int32.Parse(imageDescriptionProperties["/Top"].Value.ToString());
            int left = Int32.Parse(imageDescriptionProperties["/Left"].Value.ToString());
            int width = Int32.Parse(imageDescriptionProperties["/Width"].Value.ToString());
            int height = Int32.Parse(imageDescriptionProperties["/Height"].Value.ToString());

            var gifControlExtensionProperties = await(frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay", "/UserInputFlag" });
            TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
            bool userInputFlag = bool.Parse(gifControlExtensionProperties["/UserInputFlag"].Value.ToString());
            //dispatcherTimer.Interval = delay;
            //

            bt_Copy5.Content = "Gif帧数" + bitDecoder.FrameCount;
            var pixels = await frame.GetPixelDataAsync();
            //InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
            var encoderId = BitmapEncoder.JpegEncoderId;
            var encoder = await BitmapEncoder.CreateAsync(encoderId, memoryStream);
            var bytes = pixels.DetachPixelData();
            encoder.SetPixelData(
                frame.BitmapPixelFormat,
                frame.BitmapAlphaMode,
                frame.PixelWidth,
                frame.PixelHeight,
                frame.DpiX,
                frame.DpiY,
                bytes);
            await encoder.FlushAsync();
            memoryStream.Seek(0);

            bmp.SetSource(memoryStream);
            imageGif.Source = bmp;
            imageBrush.ImageSource = bmp;
        }

        //显示Gif图片开始
        private async void bt_Copy6_Click_1(object sender, RoutedEventArgs e)
        {
            //tbgif.Text = "http://img.t.sinajs.cn/t35/style/images/common/face/ext/normal/c9/geili_org.gif";
            RandomAccessStreamReference rass = RandomAccessStreamReference.CreateFromUri(new Uri(tbgif.Text, UriKind.RelativeOrAbsolute));
            IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
            Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);
            await CreateGifBitFrame(randomAccessStream);
            PlayGif();
            //dispatcherTimer.Stop();
            //dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            
        }

        private async void PlayGif()
        {
            if (bitFrame != null && bitFrame.Count > 0)
            {
                for (int i = 0; i <= bitFrame.Count; )
                {
                    if (i == bitFrame.Count)
                    {
                        i = 0;
                    }
                    var frame = bitFrame.ElementAt(i);
                    if (frame != null)
                    {
                        await Task.Delay(frame.Delay);
                        imageGif.Width = grid.Width = frame.Width;
                        imageGif.Height = grid.Height = frame.Height;
                        bmp.SetSource(frame.MemoryStream);
                        imageGif.Source = bmp;
                        imageBrush.ImageSource = bmp;
                        frame.MemoryStream.Seek(0);
                        await frame.MemoryStream.FlushAsync();
                    }
                    i++;
                }
                //while (true)
                //{
                //    for (int i = 0; i < bitFrame.Count; i++)
                //    {
                //        var frame = bitFrame.ElementAt(i);
                //        if (frame != null)
                //        {
                //            await Task.Delay(frame.Delay);
                //            bmp.SetSource(frame.MemoryStream);
                //            imageGif.Source = bmp;
                //            imageBrush.ImageSource = bmp;
                //        }
                //    }
                //}
                    //bmp.SetSource(bitFrame.ElementAt(1).MemoryStream);
                    //imageGif.Source = bmp;
                    //imageBrush.ImageSource = bmp;
            }
        }

        private async Task CreateGifBitFrame(IRandomAccessStream fileStream)
        {
            var decoderId = BitmapDecoder.GifDecoderId;
            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, fileStream);
            bitFrame = new List<BitFrame>();
            for (int i = 0; i < bitDecoder.FrameCount; i++)
            {
                var frame = await bitDecoder.GetFrameAsync(Convert.ToUInt32(i));

                var frameProperties = await frame.BitmapProperties.GetPropertiesAsync(new List<string>());
                var imageDescriptionProperties = await(frameProperties["/imgdesc"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Width", "/Height" });
                int width = Int32.Parse(imageDescriptionProperties["/Width"].Value.ToString());
                int height = Int32.Parse(imageDescriptionProperties["/Height"].Value.ToString());
                var gifControlExtensionProperties = await (frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay" });
                TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
                var pixels = await frame.GetPixelDataAsync();
                var encoderId = BitmapEncoder.JpegEncoderId;
                InMemoryRandomAccessStream tmemoryStream = new InMemoryRandomAccessStream();
                var encoder = await BitmapEncoder.CreateAsync(encoderId, tmemoryStream);
                var bytes = pixels.DetachPixelData();
                encoder.SetPixelData(
                    frame.BitmapPixelFormat,
                    frame.BitmapAlphaMode,
                    frame.PixelWidth,
                    frame.PixelHeight,
                    frame.DpiX,
                    frame.DpiY,
                    bytes);
                await encoder.FlushAsync();
                tmemoryStream.Seek(0);
                bitFrame.Add(new BitFrame() { Delay = delay, MemoryStream = tmemoryStream, Height = height, Width = width });
            }
        }

        private void bt_Copy7_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CeShiGifPage));
        }
    }

    public class BitFrame
    {
        //每帧的动画时间
        public TimeSpan Delay
        { get; set; }

        //每帧的数据流
        public InMemoryRandomAccessStream MemoryStream
        { get; set; }

        //每帧的高度
        public double Height
        { get; set; }

        //每帧的宽度
        public double Width
        { get; set; }
    }
}
