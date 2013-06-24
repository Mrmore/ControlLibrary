using ControlLibrary.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace ControlLibrary
{
    public sealed class ImageGif : Control, IDisposable
    {
        private BitmapImage bmp = null;
        private List<BitFrame> bitFrame = null;
        private Image imageGif = null;
        private double width = double.NaN;
        private double height = double.NaN;
        private Uri baseUri = new Uri("ms-appx://");

        /// <summary>
        /// 避免被释放多次的标记
        /// </summary>
        private bool disposed = false;

        public ImageGif()
        {
            this.DefaultStyleKey = typeof(ImageGif);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            this.width = finalSize.Width;
            this.height = finalSize.Height;
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            imageGif = this.GetTemplateChild("imageGif") as Image;
            bitFrame = new List<BitFrame>();
            bmp = new BitmapImage();
            this.SizeChanged -= ImageGif_SizeChanged;
            this.SizeChanged += ImageGif_SizeChanged;
            if (this.imageGif != null)
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    this.Start(this.Source);
                }
                else
                {
                    this.imageGif.Source = null;
                }
            }
        }

        void ImageGif_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.width = e.NewSize.Width;
            this.height = e.NewSize.Height;
        }

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageGif), new PropertyMetadata(Stretch.Uniform));

        public bool Stop
        {
            get { return (bool)GetValue(StopProperty); }
            set { SetValue(StopProperty, value); }
        }

        public static readonly DependencyProperty StopProperty = DependencyProperty.Register("Stop", typeof(bool), typeof(ImageGif), new PropertyMetadata(false, new PropertyChangedCallback(onStopPropertyChanged)));

        private static void onStopPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageGif = d as ImageGif;
            if (imageGif != null && imageGif.imageGif != null)
            {

            }
        }

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ImageGif), new PropertyMetadata(string.Empty, new PropertyChangedCallback(onSourcePropertyChanged)));

        private static void onSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var imageGif = sender as ImageGif;
            if (imageGif != null && imageGif.imageGif != null)
            {
                if (!string.IsNullOrEmpty(imageGif.Source))
                {
                    imageGif.Start(imageGif.Source);
                }
                else
                {
                    imageGif.imageGif.Source = null;
                }
            }
        }

        /// <summary>
        /// 输入Uri开始播放Gif图片
        /// </summary>
        /// <param name="Uri"></param>
        private async void Start(string uri)
        {
            var reg = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?";
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            RandomAccessStreamReference rass = null;
            if (regex.IsMatch(this.Source))
            {
                rass = RandomAccessStreamReference.CreateFromUri(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            else
            {
                try
                {
                    //本地的Uri
                    //rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.BaseUri, uri));
                    rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, uri));
                    //var packageLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
                    //var ss = await packageLocation.GetBasicPropertiesAsync();
                    //var imagesFolder = await packageLocation.GetFolderAsync("Images");
                    //StorageFile file = await imagesFolder.GetFileAsync("amy.jpg");
                    //rass = RandomAccessStreamReference.CreateFromFile(file);
                }
                catch(Exception)
                {
                    Debug.WriteLine("不是一个有效的Uri");
                }
            }
            try
            {
                IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
                Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
                //
                MemoryStream ms = new MemoryStream();
                await tempStream.CopyToAsync(ms);
                byte[] bytes = ms.ToArray();
                tempStream = new MemoryStream(bytes);
                //
                var randomAccessStream = new InMemoryRandomAccessStream();
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);
                /*
                try
                {
                    await CreateGifBitFrame(randomAccessStream);
                    PlayGif();
                }
                catch
                {
                    //var stop = this.Stop;
                    //this.Stop = true;
                    //bitFrame.Clear();
                    //randomAccessStream.Seek(0);
                    //randomAccessStream.FlushAsync().GetResults();
                    //bmp.SetSource(randomAccessStream);
                    //imageGif.Source = bmp;
                    //this.Stop = stop;

                    JpegAndPng(randomAccessStream);
                }
                */

                //判断并设置图片类型
                ImageType type = ImageTypeCheck.CheckImageType(bytes);
                switch (type)
                {
                    case ImageType.GIF:
                        {
                            await CreateGifBitFrame(randomAccessStream);
                            PlayGif();
                            break;
                        }
                    case ImageType.JPG:
                        {
                            JpegAndPng(randomAccessStream);
                            break;
                        }
                    case ImageType.PNG:
                        {
                            JpegAndPng(randomAccessStream);
                            break;
                        }
                    default:
                        {
                            JpegAndPng(randomAccessStream);
                            break;
                        }
                }
            }
            catch
            {
                BitmapImage bi = new BitmapImage();
                if (regex.IsMatch(this.Source))
                {
                    bi.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    bi.UriSource = new Uri(this.baseUri, uri);
                }
                imageGif.Source = bi;
            }
        }

        /// <summary>
        /// 获取Jpg和Png图像
        /// </summary>
        /// <param name="randomAccessStream"></param>
        private async void JpegAndPng(InMemoryRandomAccessStream randomAccessStream)
        {
            var stop = this.Stop;
            this.Stop = true;
            bitFrame.Clear();
            await Task.Delay(TimeSpan.FromMilliseconds(15));
            randomAccessStream.Seek(0);
            await randomAccessStream.FlushAsync();
            BitmapImage bi = new BitmapImage();
            imageGif.Source = null;
            await Task.Delay(TimeSpan.FromMilliseconds(15));
            //bi.SetSource(randomAccessStream);
            await bi.SetSourceAsync(randomAccessStream);
            imageGif.Stretch = this.Stretch;
            imageGif.Width = this.width;
            imageGif.Height = this.height;
            imageGif.Source = bi;
            this.Stop = stop;
        }

        /// <summary>
        /// 获取Jpg和Png图像
        /// </summary>
        /// <param name="randomAccessStream"></param>
        private async void JpegAndPng(BitmapImage bi)
        {
            var stop = this.Stop;
            this.Stop = true;
            bitFrame.Clear();
            imageGif.Source = null;
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            imageGif.Source = bi;
            this.Stop = stop;
        }

        /// <summary>
        /// 播放所有的帧
        /// </summary>
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

                        /*if (double.IsNaN(this.width))
                        {
                            imageGif.Width = frame.Width;
                        }
                        else
                        {
                            imageGif.Width = this.width;
                        }

                        if (double.IsNaN(this.height))
                        {
                            imageGif.Height = frame.Height;
                        }
                        else
                        {
                            imageGif.Height = this.height;
                        }*/

                        if (double.IsNaN(this.width))
                        {
                            imageGif.Width = frame.Width;
                        }
                        else
                        {
                            if (frame.Width > this.width)
                            {
                                imageGif.Width = this.width;
                            }
                            else
                            {
                                imageGif.Width = frame.Width;
                            }
                        }

                        if (double.IsNaN(this.height))
                        {
                            imageGif.Height = frame.Height;
                        }
                        else
                        {
                            if (frame.Height > this.height)
                            {
                                imageGif.Height = this.height;
                            }
                            else
                            {
                                imageGif.Height = frame.Height;
                            }
                        }

                        if (frame.Height > this.height || frame.Width > this.width)
                        {
                            imageGif.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
                        }

                        if (frame.Height <= this.height && frame.Width <= this.width)
                        {
                            imageGif.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                        }

                        imageGif.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        imageGif.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                        imageGif.Margin = new Thickness(frame.Left, frame.Top, 0, 0);

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,async () =>
                            {
                                bmp.SetSource(frame.MemoryStream);
                                imageGif.Source = bmp;
                                //imageGif.Source = frame.wb;
                                //WriteableBitmap wb = new WriteableBitmap(Convert.ToInt32(frame.Width), Convert.ToInt32(frame.Height));
                                //wb.SetSource(frame.MemoryStream);
                                //wb.Invalidate();
                                //imageGif.Source = wb;
                                frame.MemoryStream.Seek(0);
                                await frame.MemoryStream.FlushAsync();
                            });
                    }
                    i++;

                    if (this.Stop)
                    {
                        if (imageGif.Source == null)
                        {
                            //做一些特殊处理
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 拆解所有的Gif帧
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private async Task CreateGifBitFrame(IRandomAccessStream fileStream)
        {
            var decoderId = BitmapDecoder.GifDecoderId;
            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, fileStream);
            //bitFrame = new List<BitFrame>();
            var stop = this.Stop;
            this.Stop = true;
            bitFrame.Clear();
            for (int i = 0; i < bitDecoder.FrameCount; i++)
            {
                var frame = await bitDecoder.GetFrameAsync(Convert.ToUInt32(i));

                var frameProperties = await frame.BitmapProperties.GetPropertiesAsync(new List<string>());
                var imageDescriptionProperties = await (frameProperties["/imgdesc"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Top", "/Left", "/Width", "/Height" });
                //int top = Int32.Parse(imageDescriptionProperties["/Top"].Value.ToString());
                //int left = Int32.Parse(imageDescriptionProperties["/Left"].Value.ToString());
                //int width = Int32.Parse(imageDescriptionProperties["/Width"].Value.ToString());
                //int height = Int32.Parse(imageDescriptionProperties["/Height"].Value.ToString());
                double top = System.Convert.ToDouble(imageDescriptionProperties["/Top"].Value.ToString());
                double left = System.Convert.ToDouble(imageDescriptionProperties["/Left"].Value.ToString());
                double width = System.Convert.ToDouble(imageDescriptionProperties["/Width"].Value.ToString());
                double height = System.Convert.ToDouble(imageDescriptionProperties["/Height"].Value.ToString());
                var gifControlExtensionProperties = await (frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay", "/UserInputFlag" });
                TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
                bool userInputFlag = bool.Parse(gifControlExtensionProperties["/UserInputFlag"].Value.ToString());

                //http://img.t.sinajs.cn/t35/style/images/common/face/ext/normal/49/hatea_org.gif
                var decoderInformation = bitDecoder.DecoderInformation;
                var codecId = decoderInformation.CodecId;
                
                //var bitmapAlpha = frame.BitmapAlphaMode;
                //var pixels = await frame.GetPixelDataAsync(frame.BitmapPixelFormat,
                //bitmapAlpha,
                //new BitmapTransform()
                //{
                //    ScaledHeight = 0,
                //    ScaledWidth = 0,
                //    InterpolationMode = BitmapInterpolationMode.NearestNeighbor,
                //    //Bounds = new BitmapBounds() { Height = Convert.ToUInt32(height), Width = Convert.ToUInt32(width), X = Convert.ToUInt32(left), Y = Convert.ToUInt32(top) },
                //    Flip = BitmapFlip.None,
                //    Rotation = BitmapRotation.None
                //},
                //ExifOrientationMode.RespectExifOrientation,
                //ColorManagementMode.ColorManageToSRgb);

                var pixels = await frame.GetPixelDataAsync();

                var encoderId = BitmapEncoder.PngEncoderId;
                InMemoryRandomAccessStream tmemoryStream = new InMemoryRandomAccessStream();
                //var encoder = await BitmapEncoder.CreateForTranscodingAsync(tmemoryStream, bitDecoder);
                var encoder = await BitmapEncoder.CreateAsync(encoderId, tmemoryStream);
                var bytes = pixels.DetachPixelData();
                //encoder.IsThumbnailGenerated = true;
                //var bitmapAlphaMode = frame.BitmapAlphaMode;
                encoder.SetPixelData(
                    frame.BitmapPixelFormat,
                    BitmapAlphaMode.Premultiplied,
                    frame.PixelWidth,
                    frame.PixelHeight,
                    frame.DpiX,
                    frame.DpiY,
                    bytes);
                await encoder.FlushAsync();
                tmemoryStream.Seek(0);

                /*WriteableBitmap wb = new WriteableBitmap(Convert.ToInt32(frame.PixelWidth), Convert.ToInt32(frame.PixelHeight));
                //InMemoryRandomAccessStream inStream = new InMemoryRandomAccessStream();
                //DataWriter datawriter = new DataWriter(inStream.GetOutputStreamAt(0));
                //datawriter.WriteBytes(pixels.DetachPixelData());
                //await datawriter.StoreAsync();

                
                wb.SetSource(tmemoryStream);
                wb.Invalidate();*/

                bitFrame.Add(new BitFrame() { Delay = delay, MemoryStream = tmemoryStream, Height = height, Width = width, Top = top, Left = left, UserInputFlag = userInputFlag });//, wb = wb });
            }
            this.Stop = stop;
        }

        /// <summary>
        /// 清理资源的方法
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }
    }
}
