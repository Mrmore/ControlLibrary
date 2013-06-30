using ControlLibrary.Tools;
using ControlLibrary.Tools.Async;
using RenrenCoreWrapper.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace ControlLibrary
{
    public sealed class ImageGif : Control, IDisposable
    {
        private AsyncLock asyncLock = new AsyncLock();
        //private SemaphoreSlim semaphore = null;
        private BitmapImage bmp = null;
        private List<BitFrame> bitFrame = null;
        //private Image imageGif = null;
        private Grid imageGifGrid = null;
        private Viewbox viewBox = null;
        private double width = double.NaN;
        private double height = double.NaN;
        private Uri baseUri = new Uri("ms-appx://");

        //淡入
        private Storyboard sbVisible = null;
        //浅出
        private Storyboard sbNotVisible = null;
        //为了能保存GIF 把byte[] bytes 提成全局的
        private byte[] bytes = null;

        //开始播放gif
        public delegate void BeginPlayGif();
        public BeginPlayGif BeginPlay = null;

        //优化
        private List<Image> imageList = null;
        private List<BitmapImage> bitmapImageList = null;

        //图片下载的实时代理
        public delegate void ImageDownLoadProgressHandler(int downloadValue);
        /// <summary>
        /// 图片下载的实时事件
        /// </summary>
        public event ImageDownLoadProgressHandler ImageDownLoadProgress;

        //图片打开成功的代理
        public delegate void RoutedEventHandler(object sender, RoutedEventArgs e);
        /// <summary>
        /// 图片打开成功的事件 sender 对象是ImageGif类型对象
        /// </summary>
        public event RoutedEventHandler ImageOpened;

        //图片打开失败的代理
        public delegate void ExceptionRoutedEventHandler(object sender, ExceptionRoutedEventArgs e);
        /// <summary>
        /// 图片打开失败的事件
        /// </summary>
        public event ExceptionRoutedEventHandler ImageFailed;

        //动画完成的实时代理
        public delegate void AnimationCompleteHandler();
        /// <summary>
        /// 动画完成的实时事件
        /// </summary>
        public event AnimationCompleteHandler AnimationComplete;

        /// <summary>
        /// 避免被释放多次的标记
        /// </summary>
        private bool disposed = false;

        public ImageGif()
        {
            this.DefaultStyleKey = typeof(ImageGif);
            sbVisible = new Storyboard();
            sbNotVisible = new Storyboard();
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
            //imageGif = this.GetTemplateChild("imageGif") as Image;
            imageGifGrid = this.GetTemplateChild("imageGifGrid") as Grid;
            viewBox = this.GetTemplateChild("viewBox") as Viewbox;
            bitFrame = new List<BitFrame>();
            bmp = new BitmapImage();
            //优化
            imageList = new List<Image>();
            bitmapImageList = new List<BitmapImage>();
            //semaphore = new SemaphoreSlim(1, 1);

            this.SizeChanged -= ImageGif_SizeChanged;
            this.SizeChanged += ImageGif_SizeChanged;
            if (this.imageGifGrid != null)
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    this.Start(this.Source);
                }
                else
                {
                    //this.imageGif.Source = null;
                    this.imageGifGrid.Children.Clear();
                    //优化
                    this.imageList.Clear();
                    this.bitmapImageList.Clear();
                }
            }
        }

        public Storyboard GetStart()
        {
            return sbVisible;
        }

        //简写动画方法(淡入)
        private void CreateAnimationBegin()
        {
            //sbVisible = new Storyboard();
            sbVisible.Stop();
            sbVisible.Children.Clear();
            sbVisible.Completed -= sbVisible_Completed;
            sbVisible.Completed += sbVisible_Completed;
            DoubleAnimationUsingKeyFrames keyFramesOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesOpacity, imageGifGrid);
            Storyboard.SetTargetProperty(keyFramesOpacity, "(UIElement.Opacity)");
            KeyTime ktOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity1, Value = 0 });
            KeyTime ktOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity2, Value = 1 });
            sbVisible.Children.Add(keyFramesOpacity);
        }

        private void sbVisible_Completed(object sender, object e)
        {
            if (this.AnimationComplete != null)
            {
                this.AnimationComplete();
            }
        }

        private void AnimationBeginStart()
        {
            if (sbVisible != null)
            {
                sbVisible.Begin();
            }
        }

        private void AnimationBeginStop()
        {
            if (sbVisible != null)
            {
                sbVisible.Stop();
            }
        }

        public Storyboard GetEnd()
        {
            return sbNotVisible;
        }

        //简写动画方法(浅出)
        private void CreateAnimationEnd()
        {
            //sbNotVisible = new Storyboard();
            sbNotVisible.Stop();
            sbNotVisible.Children.Clear();
            DoubleAnimationUsingKeyFrames keyFramesOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesOpacity, imageGifGrid);
            Storyboard.SetTargetProperty(keyFramesOpacity, "(UIElement.Opacity)");
            KeyTime ktOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity1, Value = 1 });
            KeyTime ktOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity2, Value = 0 });
            sbNotVisible.Children.Add(keyFramesOpacity);
        }

        private void AnimationEndStart()
        {
            if (sbNotVisible != null)
            {
                sbNotVisible.Begin();
            }
        }

        private void AnimationEndStop()
        {
            if (sbNotVisible != null)
            {
                sbNotVisible.Stop();
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
            if (imageGif != null && imageGif.imageGifGrid != null)
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
            if (imageGif != null && imageGif.imageGifGrid != null)
            {
                if (!string.IsNullOrEmpty(imageGif.Source))
                {
                    //Start方法体有清空Grid的方法，如果不在停止循环的Stop属性，并入清空Grid的方法，
                    //这样多次调用会使界面造成闪烁的残影。
                    //imageGif.imageGifGrid.Children.Clear();
                    imageGif.Start(imageGif.Source);
                }
                else
                {
                    //imageGif.imageGif.Source = null;
                    imageGif.imageGifGrid.Children.Clear();
                }
            }
        }

        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register("IsAnimation", typeof(bool), typeof(ImageGif), new PropertyMetadata(false, new PropertyChangedCallback(onIsAnimationPropertyChanged)));

        private static void onIsAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var imageGif = sender as ImageGif;
            if (imageGif != null && imageGif.imageGifGrid != null && !string.IsNullOrEmpty(imageGif.Source))
            {
                imageGif.Start(imageGif.Source);
            }
        }

        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register("AnimationTime", typeof(TimeSpan), typeof(ImageGif), new PropertyMetadata(TimeSpan.FromSeconds(2), new PropertyChangedCallback(onAnimationTimePropertyChanged)));

        private static void onAnimationTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var imageGif = sender as ImageGif;
            if (imageGif != null && imageGif.imageGifGrid != null && !string.IsNullOrEmpty(imageGif.Source))
            {
                imageGif.Start(imageGif.Source);
            }
        }

        /// <summary>
        /// 输入Uri开始播放Gif图片
        /// </summary>
        /// <param name="Uri"></param>
        private async void Start(string uri)
        {
            InMemoryRandomAccessStream randomAccessStream = null;
            if (this.ImageDownLoadProgress != null)
            {
                this.ImageDownLoadProgress(0);
            }

            if (this.IsAnimation)
            {
                this.imageGifGrid.Opacity = 0;
                CreateAnimationBegin();
            }
            else
            {
                this.imageGifGrid.Opacity = 1.0;
            }

            //优化网络流量和用户体验
            if (GifCacheDictionaryHelper.Instance.SelectUriToBytes(this.Source) == null)
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
                    catch (Exception)
                    {
                        Debug.WriteLine("不是一个有效的Uri");
                    }
                }
                if (this.ImageDownLoadProgress != null)
                {
                    for (int i = 0; i <= 7; i++)
                    {
                        this.ImageDownLoadProgress(i * 10);
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                    }
                }
                try
                {
                    IRandomAccessStreamWithContentType streamRandom = null;
                    try
                    {
                        streamRandom = await rass.OpenReadAsync();
                    }
                    catch
                    {
                        if (ImageFailed != null)
                        {
                            ImageFailed(this, null);
                        }
                        return;
                    }
                    Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
                    //为了能判断文件头做了一个流拷贝，保存了一份字节数组
                    MemoryStream ms = new MemoryStream();
                    await tempStream.CopyToAsync(ms);
                    //byte[] bytes = ms.ToArray();
                    bytes = ms.ToArray();
                    tempStream = new MemoryStream(bytes);
                    //
                    randomAccessStream = new InMemoryRandomAccessStream();
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
                    if (this.ImageDownLoadProgress != null)
                    {
                        this.ImageDownLoadProgress(80);
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                    }
                    switch (type)
                    {
                        case ImageType.GIF:
                            {
                                using (await asyncLock.LockAsync())
                                {
                                    //*优化网络流量和用户体验
                                    GifCacheDictionaryHelper.Instance.AddUriToBytes(this.Source, bytes);
                                    //*
                                    viewBox.Stretch = this.Stretch;
                                    await CreateGifBitFrame(randomAccessStream);
                                    PlayGif();
                                }
                                break;
                            }
                        case ImageType.JPG:
                            {
                                viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                                JpegAndPng(randomAccessStream);
                                break;
                            }
                        case ImageType.PNG:
                            {
                                viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                                JpegAndPng(randomAccessStream);
                                break;
                            }
                        default:
                            {
                                viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                                JpegAndPng(randomAccessStream);
                                break;
                            }
                    }
                }
                catch
                {
                    //GIF错误数据的流程
                    GifCacheDictionaryHelper.Instance.DelectUriToBytes(this.Source);
                    var ss = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.DownloadProgress -= bi_DownloadProgress;
                        bi.DownloadProgress += bi_DownloadProgress;
                        bi.ImageOpened -= bi_ImageOpened;
                        bi.ImageOpened += bi_ImageOpened;
                        bi.ImageFailed -= bi_ImageFailed;
                        bi.ImageFailed += bi_ImageFailed;
                        //if (regex.IsMatch(this.Source))
                        //{
                        //    bi.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                        //}
                        //else
                        //{
                        //    bi.UriSource = new Uri(this.baseUri, uri);
                        //}

                        randomAccessStream.Seek(0);
                        randomAccessStream.FlushAsync();
                        //await bi.SetSourceAsync(randomAccessStream);
                        bi.SetSource(randomAccessStream);

                        this.imageGifGrid.Children.Clear();
                        //优化
                        this.imageList.Clear();
                        this.bitmapImageList.Clear();
                        this.bitFrame.Clear();

                        viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                        Image imageGif = new Image();
                        imageGif.Source = null;
                        imageGif.Stretch = this.Stretch;
                        imageGif.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                        imageGif.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                        if (!double.IsNaN(this.width) && this.width != 0.0)
                        {
                            imageGif.Width = this.width;
                        }
                        if (!double.IsNaN(this.height) && this.height != 0.0)
                        {
                            imageGif.Height = this.height;
                        }
                        imageGif.Source = bi;
                        this.imageGifGrid.Children.Add(imageGif);
                    });
                    //ss.Completed = delegate(IAsyncAction asyncAction, AsyncStatus asyncStatus)
                    //{
                    //    ss.Close();
                    //};
                    this.Stop = catchStop;
                }
            }
            else
            {
                using (await asyncLock.LockAsync())
                {
                    if (this.ImageDownLoadProgress != null)
                    {
                        this.ImageDownLoadProgress(80);
                    }
                    bytes = GifCacheDictionaryHelper.Instance.SelectUriToBytes(this.Source).Bytes;
                    Stream tempStream = new MemoryStream(bytes);
                    var inMemoryRandomAccessStream = new InMemoryRandomAccessStream();
                    var outputStream = inMemoryRandomAccessStream.GetOutputStreamAt(0);
                    await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);
                    await CreateGifBitFrame(inMemoryRandomAccessStream);
                    PlayGif();
                }
            }
        }

        private void bi_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (this.ImageFailed != null)
            {
                this.ImageFailed(sender, e);
            }
        }

        private void bi_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (this.ImageOpened != null)
            {
                //this.ImageOpened(sender, e);
                this.ImageOpened(this, e);
            }
        }

        private void bi_DownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if (e.Progress >= 80)
            {
                if (this.ImageDownLoadProgress != null)
                {
                    this.ImageDownLoadProgress(e.Progress);
                }
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
            await Task.Delay(TimeSpan.FromMilliseconds(15));
            randomAccessStream.Seek(0);
            await randomAccessStream.FlushAsync();
            this.imageGifGrid.Children.Clear();
            //优化
            this.imageList.Clear();
            this.bitmapImageList.Clear();
            this.bitFrame.Clear();
            Image imageGif = new Image();
            BitmapImage bi = new BitmapImage();
            imageGif.Source = null;
            await Task.Delay(TimeSpan.FromMilliseconds(15));
            //bi.SetSource(randomAccessStream);
            await bi.SetSourceAsync(randomAccessStream);
            imageGif.Stretch = this.Stretch;
            imageGif.Width = this.width;
            imageGif.Height = this.height;
            imageGif.Source = bi;
            this.imageGifGrid.Children.Add(imageGif);
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
            this.imageGifGrid.Children.Clear();
            //优化
            this.imageList.Clear();
            this.bitmapImageList.Clear();
            this.bitFrame.Clear();

            Image imageGif = new Image();
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
            //semaphore.WaitOne(TimeSpan.FromMilliseconds(300));
            //做优化
            if (imageList.Count > 0 || bitmapImageList.Count > 0)
            {
                imageList.Clear();
                bitmapImageList.Clear();
            }

            for (int i = 0; i <= bitFrame.Count; i++)
            {
                Image imageGif = new Image();
                imageList.Add(imageGif);
                BitmapImage BitmapImage = new BitmapImage();
                bitmapImageList.Add(BitmapImage);
            }

            if (this.ImageDownLoadProgress != null)
            {
                for (int i = 0; i <= 10; i++)
                {
                    this.ImageDownLoadProgress(90 + i);
                    await Task.Delay(TimeSpan.FromMilliseconds(30));
                }
            }

            if (this.IsAnimation)
            {
                AnimationBeginStop();
                AnimationBeginStart();
            }
            else
            {
                AnimationBeginStop();
                imageGifGrid.Opacity = 1.0;
            }

            if (this.ImageOpened != null)
            {
                this.ImageOpened(this, new RoutedEventArgs());
            }

            if (bitFrame != null && bitFrame.Count > 0)
            {
                if (BeginPlay != null)
                {
                    BeginPlay();
                }
                for (int i = 0; i <= bitFrame.Count; )
                {
                    if (i == bitFrame.Count)
                    {
                        //正常
                        i = 0;
                        //await Task.Delay(TimeSpan.FromMilliseconds(15));
                    }

                    var frame = bitFrame.ElementAt(i);
                    //Image imageGif = new Image();
                    var imageGif = imageList.ElementAt(i);
                    imageGif.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    if (frame != null)
                    {
                        //修改
                        //await Task.Delay(frame.Delay);

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

                        //正常流程
                        if (double.IsNaN(this.width) || this.width == 0)
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

                        if (double.IsNaN(this.height) || this.height == 0)
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

                        //后思路 2012-12-27
                        imageGif.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
                        //imageGif.Width = frame.Width;
                        //imageGif.Height = frame.Height;

                        //new
                        //if (i == 0)
                        //{
                        //    imageGifGrid.Width = imageGif.Width;
                        //    imageGifGrid.Height = imageGif.Height;
                        //}

                        //加入了缩放，缩放比例的Gif
                        /*if (!double.IsNaN(this.height))
                        {
                            imageGif.Height = this.height;
                            imageGif.Margin = new Thickness(frame.Left * (this.width / frame.Width), frame.Top * (this.height / frame.Height), 0, 0);
                        }
                        else
                        {
                            imageGif.Height = frame.Height;
                            imageGif.Margin = new Thickness(frame.Left, frame.Top, 0, 0);
                        }
                        if (!double.IsNaN(this.width))
                        {
                            imageGif.Width = this.width;
                            imageGif.Margin = new Thickness(frame.Left * (this.width / frame.Width), frame.Top * (this.height / frame.Height), 0, 0);
                        }
                        else
                        {
                            imageGif.Width = frame.Width;
                            imageGif.Margin = new Thickness(frame.Left, frame.Top, 0, 0);
                        }
                        imageGif.Stretch = this.Stretch;
                        imageGif.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        imageGif.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;*/

                        imageGif.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                        imageGif.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                        imageGif.Margin = new Thickness(frame.Left, frame.Top, 0, 0);

                        await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            //BitmapImage bmp = new BitmapImage();
                            BitmapImage bmp = bitmapImageList.ElementAt(i);
                            bmp.SetSource(frame.MemoryStream);
                            imageGif.Source = bmp;

                            //如果前两帧的大小一样，那么每次播放完一帧都删除。否则只在播完每一次后删除
                            //如果前两帧一样大小，就认为后面所有帧都一样大小，然后每次播放一帧不会乱。
                            //如果不一样，则不能删除，通常第一帧为场景图，尺寸最大
                            if (i == 0)
                            {
                                this.imageGifGrid.Children.Clear();
                                this.imageGifGrid.Width = imageGif.Width;
                                this.imageGifGrid.Height = imageGif.Height;
                            }
                            else
                            {
                                //if (i > 0 && imageList[0].Width > 0 && imageList[0].Width == imageList[1].Width)
                                //{
                                //    this.imageGifGrid.Children.Clear();
                                //}

                                //new
                                //if (i >= 1 && imageList[0].Width > 0 && imageList[0].Width == imageList[1].Width
                                //        && imageList[0].Height > 0 && imageList[0].Height == imageList[1].Height)
                                //{
                                //    this.imageGifGrid.Children.Clear();
                                //}

                                if (bitFrame.Count > 0)
                                {
                                    if (!bitFrame.ElementAt(i - 1).Disposal)
                                    {
                                        imageList[i - 1].Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                                    }
                                }
                            }
                            try
                            {
                                imageGifGrid.Children.Add(imageGif);
                            }
                            catch (Exception) { }
                            frame.MemoryStream.Seek(0);
                            await frame.MemoryStream.FlushAsync();
                        });
                        await Task.Delay(frame.Delay);
                    }
                    i++;

                    if (this.Stop)
                    {
                        this.imageGifGrid.Children.Clear();
                        //做优化
                        this.imageList.Clear();
                        this.bitmapImageList.Clear();
                        this.bitFrame.Clear();
                        //try
                        //{
                        //    semaphore.Release(1);
                        //    return;
                        //}
                        //catch (Exception)
                        //{
                        //    this.Stop = false;
                        //}
                        this.Stop = false;
                        return;
                    }
                }
            }
        }

        private bool catchStop = false;
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
            var stop = catchStop = this.Stop;
            this.Stop = true;
            //await semaphore.WaitAsync(TimeSpan.FromMilliseconds(300));
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
                var gifControlExtensionProperties = await (frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay", "/UserInputFlag", "/Disposal" });
                var delayTemp = gifControlExtensionProperties["/Delay"].Value.ToString();
                if (string.IsNullOrEmpty(delayTemp) || delayTemp == "0")
                {
                    delayTemp = "10";
                }
                TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(delayTemp) / 100); 
                //TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
                bool userInputFlag = bool.Parse(gifControlExtensionProperties["/UserInputFlag"].Value.ToString());
                //共享图层(如果是2为不共享为False,其余不为2的为共享为True)
                var disposalSource = gifControlExtensionProperties["/Disposal"].Value.ToString();
                bool disposal = true;
                if (disposalSource == "2")
                {
                    disposal = false;
                }
                else
                {
                    disposal = true;
                }

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

                bitFrame.Add(new BitFrame() { Delay = delay, MemoryStream = tmemoryStream, Height = height, Width = width, Top = top, Left = left, UserInputFlag = userInputFlag, Disposal = disposal });//, wb = wb });
            }
            if (this.ImageDownLoadProgress != null)
            {
                this.ImageDownLoadProgress(90);
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            this.Stop = stop;
        }

        /// <summary>
        /// 保存图片到任意的文件夹
        /// </summary>
        /// <returns>保存成功返回true,失败返回false</returns>
        public async Task<bool> SaveFileImage()
        {
            try
            {
                StorageFile storageFile = null;
                storageFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(ApiHelper.ComputeMD5(this.Source), CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBytesAsync(storageFile, bytes);

                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                savePicker.FileTypeChoices.Add("图片类型", new List<string>() { ".gif" });

                savePicker.SuggestedFileName = storageFile.Name;
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    await storageFile.CopyAndReplaceAsync(file);//, file.Name, NameCollisionOption.GenerateUniqueName);
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 清理资源的方法
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                this.imageGifGrid.Children.Clear();
                //做优化
                this.imageList.Clear();
                this.bitmapImageList.Clear();
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }

        /// <summary>
        /// 通过传入数据流来显示GIF呈现
        /// </summary>
        /// <param name="iRandomAccessStream">传入一个图片的数据流</param>
        public async Task SetSourceAsync(IRandomAccessStream iRandomAccessStream)
        {
            if (this.ImageDownLoadProgress != null)
            {
                this.ImageDownLoadProgress(0);
            }

            if (this.IsAnimation)
            {
                this.imageGifGrid.Opacity = 0;
                CreateAnimationBegin();
            }
            else
            {
                this.imageGifGrid.Opacity = 1.0;
            }

            if (this.ImageDownLoadProgress != null)
            {
                for (int i = 0; i <= 7; i++)
                {
                    this.ImageDownLoadProgress(i * 10);
                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                }
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            try
            {
                Stream tempStream = iRandomAccessStream.GetInputStreamAt(0).AsStreamForRead();
                //为了能判断文件头做了一个流拷贝，保存了一份字节数组
                MemoryStream ms = new MemoryStream();
                await tempStream.CopyToAsync(ms);
                bytes = ms.ToArray();
                tempStream = new MemoryStream(bytes);
                //
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

                //判断并设置图片类型
                ImageType type = ImageTypeCheck.CheckImageType(bytes);
                if (this.ImageDownLoadProgress != null)
                {
                    this.ImageDownLoadProgress(80);
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                }
                switch (type)
                {
                    case ImageType.GIF:
                        {
                            using (await asyncLock.LockAsync())
                            {
                                viewBox.Stretch = this.Stretch;
                                await CreateGifBitFrame(randomAccessStream);
                                PlayGif();
                            }
                            break;
                        }
                    case ImageType.JPG:
                        {
                            viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                            JpegAndPng(randomAccessStream);
                            break;
                        }
                    case ImageType.PNG:
                        {
                            viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                            JpegAndPng(randomAccessStream);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            catch
            {
                var ss = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    BitmapImage bi = new BitmapImage();
                    bi.DownloadProgress -= bi_DownloadProgress;
                    bi.DownloadProgress += bi_DownloadProgress;
                    bi.ImageOpened -= bi_ImageOpened;
                    bi.ImageOpened += bi_ImageOpened;
                    bi.ImageFailed -= bi_ImageFailed;
                    bi.ImageFailed += bi_ImageFailed;
                    randomAccessStream.Seek(0);
                    randomAccessStream.FlushAsync();
                    //await bi.SetSourceAsync(randomAccessStream);
                    bi.SetSource(randomAccessStream);

                    this.imageGifGrid.Children.Clear();
                    //优化
                    this.imageList.Clear();
                    this.bitmapImageList.Clear();
                    this.bitFrame.Clear();

                    viewBox.Stretch = Windows.UI.Xaml.Media.Stretch.None;
                    Image imageGif = new Image();
                    imageGif.Source = null;
                    imageGif.Stretch = this.Stretch;
                    imageGif.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                    imageGif.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                    if (!double.IsNaN(this.width) && this.width != 0.0)
                    {
                        imageGif.Width = this.width;
                    }
                    if (!double.IsNaN(this.height) && this.height != 0.0)
                    {
                        imageGif.Height = this.height;
                    }
                    imageGif.Source = bi;
                    this.imageGifGrid.Children.Add(imageGif);
                });
                this.Stop = catchStop;
            }
        }
    }
}
