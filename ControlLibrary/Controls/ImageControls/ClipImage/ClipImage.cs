using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ControlLibrary.Helper;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public class ClipImage : Control
    {
        public delegate void ClipImageOpened(object sender);
        public event ClipImageOpened ImageOpened;

        //图片打开失败的代理
        public delegate void ExceptionRoutedEventHandler(object sender, ExceptionRoutedEventArgs e);
        /// <summary>
        /// 图片打开失败的事件
        /// </summary>
        public event ExceptionRoutedEventHandler ImageFailed;

        //图片下载的实时代理
        public delegate void ImageDownLoadProgressHandler(int downloadValue);
        /// <summary>
        /// 图片下载的实时事件
        /// </summary>
        public event ImageDownLoadProgressHandler ImageDownLoadProgress;


        private bool isImageComplete = false;
        /// <summary>
        /// 判断图片是否已经完成
        /// </summary>
        public bool IsImageComplete
        {
            get
            {
                return isImageComplete;
            }
            set
            {
                isImageComplete = value;
            }
        }

        private Grid gridRoot = null;
        //private Image imageClip = null;
        private CacheImage imageClip = null;
        private CompositeTransform ct = null;
        private double desiredSizeHeight = double.NaN;
        private double desiredSizeWidth = double.NaN;
        private Size size;
        private string oldUri = string.Empty;

        private Storyboard sbVisible = null;
        private Storyboard sbNotVisible = null;
        private bool isSbComplete = false;

        private Uri baseUri = new Uri("ms-appx://");

        public CacheImageDateType CacheType
        {
            get { return (CacheImageDateType)GetValue(CacheTypeProperty); }
            set { SetValue(CacheTypeProperty, value); }
        }

        public static readonly DependencyProperty CacheTypeProperty = DependencyProperty.Register("CacheType", typeof(CacheImageDateType), typeof(ClipImage), new PropertyMetadata(CacheImageDateType.CachePhoto, new PropertyChangedCallback(onCacheTypePropertyChanged)));

        private static void onCacheTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clipImage = sender as ClipImage;
            if (clipImage != null && clipImage.imageClip != null)
            {

            }
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ClipImage), new PropertyMetadata(null, new PropertyChangedCallback(onSourcePropertyChanged)));

        /*public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ClipImage), new PropertyMetadata(null, new PropertyChangedCallback(onSourcePropertyChanged)));*/

        private static void onSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clipImage = sender as ClipImage;
            if (clipImage != null && clipImage.imageClip != null)
            {
                clipImage.desiredSizeHeight = clipImage.desiredSizeWidth = double.NaN;
                //clipImage.imageClip.Visibility = Visibility.Collapsed;

                /*clipImage.imageClip.Source = clipImage.Source;*/
                if (clipImage.Source != null)
                {
                    if (clipImage.oldUri != ((BitmapImage)clipImage.Source).UriSource.AbsoluteUri)
                    {
                        clipImage.isSbComplete = false;
                        clipImage.imageClip.Opacity = 0.0;
                        clipImage.imageClip.Source = clipImage.oldUri = ((BitmapImage)clipImage.Source).UriSource.AbsoluteUri;
                    }
                }

                //clipImage.imageClip.ImageOpened -= clipImage.imageClip_ImageOpened;
                //clipImage.imageClip.ImageOpened += clipImage.imageClip_ImageOpened;

                /*clipImage.imageClip.ImageOpened -= clipImage.imageClip_ImageOpened;
                if (clipImage.IsClipImage)
                {

                    clipImage.imageClip.ImageOpened += clipImage.imageClip_ImageOpened;
                    if (clipImage.desiredSizeHeight != double.NaN && clipImage.desiredSizeWidth != double.NaN)
                    {
                        clipImage.ScaleXY(clipImage.imageClip);
                    }
                    else
                    {
                        clipImage.imageClip.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    clipImage.InitImageTransforms();
                }*/

                clipImage.IsClipImage = true;
            }
        }

        public bool IsClipImage
        {
            get { return (bool)GetValue(IsClipImageProperty); }
            set { SetValue(IsClipImageProperty, value); }
        }

        public static readonly DependencyProperty IsClipImageProperty = DependencyProperty.Register("IsClipImage", typeof(bool), typeof(ClipImage), new PropertyMetadata(true, new PropertyChangedCallback(onIsClipImagePropertyChanged)));

        private static void onIsClipImagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clipImage = sender as ClipImage;
            if (clipImage != null && clipImage.imageClip != null)
            {
                clipImage.imageClip.ImageOpened -= clipImage.imageClip_ImageOpened;
                if (clipImage.IsClipImage)
                {

                    clipImage.imageClip.ImageOpened += clipImage.imageClip_ImageOpened;
                    if (clipImage.desiredSizeHeight != double.NaN && clipImage.desiredSizeWidth != double.NaN)
                    {
                        clipImage.ScaleXY(clipImage.imageClip.GetImage());
                    }
                    else
                    {
                        //clipImage.imageClip.Visibility = Visibility.Collapsed;
                        clipImage.imageClip.Opacity = 0.0;
                    }
                }
                else
                {
                    clipImage.InitImageTransforms();
                }
            }
        }

        public bool IsCacheImage
        {
            get { return (bool)GetValue(IsCacheImageProperty); }
            set { SetValue(IsCacheImageProperty, value); }
        }

        public static readonly DependencyProperty IsCacheImageProperty = DependencyProperty.Register("IsCacheImage", typeof(bool), typeof(ClipImage), new PropertyMetadata(false));

        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register("IsAnimation", typeof(bool), typeof(ClipImage), new PropertyMetadata(false, new PropertyChangedCallback(onIsAnimationPropertyChanged)));

        private static void onIsAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clipImage = sender as ClipImage;
            if (clipImage != null && clipImage.imageClip != null)
            {
                if (clipImage.IsAnimation)
                {
                    clipImage.imageClip.Opacity = 0;
                    clipImage.CreateAnimationBegin();
                }
                else
                {
                    clipImage.imageClip.Opacity = 1.0;
                }
            }
        }

        public TimeSpan AnimationTime
        {
            get { return (TimeSpan)GetValue(AnimationTimeProperty); }
            set { SetValue(AnimationTimeProperty, value); }
        }

        public static readonly DependencyProperty AnimationTimeProperty = DependencyProperty.Register("AnimationTime", typeof(TimeSpan), typeof(ClipImage), new PropertyMetadata(TimeSpan.FromSeconds(2), new PropertyChangedCallback(onAnimationTimePropertyChanged)));

        private static void onAnimationTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var clipImage = sender as ClipImage;
            if (clipImage != null && clipImage.imageClip != null)
            {
                if (clipImage.IsAnimation)
                {
                    clipImage.imageClip.Opacity = 0;
                    clipImage.CreateAnimationBegin();
                }
                else
                {
                    clipImage.imageClip.Opacity = 1.0;
                }
            }
        }

        //简写动画方法(淡入)
        protected virtual void CreateAnimationBegin()
        {
            sbVisible = new Storyboard();
            DoubleAnimationUsingKeyFrames keyFramesOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesOpacity, imageClip);
            Storyboard.SetTargetProperty(keyFramesOpacity, "(UIElement.Opacity)");
            KeyTime ktOpacity1 = KeyTime.FromTimeSpan(new TimeSpan(0, 0, 0));
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity1, Value = 0 });
            KeyTime ktOpacity2 = KeyTime.FromTimeSpan(this.AnimationTime);
            keyFramesOpacity.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktOpacity2, Value = 1 });
            sbVisible.Children.Add(keyFramesOpacity);
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

        //简写动画方法(浅出)
        protected virtual void CreateAnimationEnd()
        {
            sbNotVisible = new Storyboard();
            DoubleAnimationUsingKeyFrames keyFramesOpacity = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(keyFramesOpacity, imageClip);
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

        public ClipImage()
        {
            this.DefaultStyleKey = typeof(ClipImage);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            gridRoot = this.GetTemplateChild("gridRoot") as Grid;
            //imageClip = this.GetTemplateChild("clipImage") as Image;
            imageClip = this.GetTemplateChild("clipImage") as CacheImage;
            if (gridRoot != null && imageClip != null)
            {
                if (this.IsAnimation)
                {
                    this.imageClip.Opacity = 0;
                    this.CreateAnimationBegin();
                }
                else
                {
                    this.imageClip.Opacity = 1.0;
                }

                this.ct = new CompositeTransform();
                this.InitImageTransforms();
                /*this.imageClip.Visibility = Windows.UI.Xaml.Visibility.Collapsed;*/
                this.imageClip.Opacity = 0;
                this.imageClip.Stretch = Stretch.Uniform;
                //初始化需要
                this.desiredSizeHeight = this.desiredSizeWidth = double.NaN;
                /*this.imageClip.Source = this.Source;*/
                if (this.Source != null)
                {
                    this.imageClip.Source = ((BitmapImage)this.Source).UriSource.AbsoluteUri;
                }
                //this.SizeChanged += ClipImage_SizeChanged;
                this.imageClip.ImageOpened -= this.imageClip_ImageOpened;
                if (this.IsClipImage)
                {
                    this.imageClip.ImageOpened += this.imageClip_ImageOpened;
                    if (this.desiredSizeHeight != double.NaN && this.desiredSizeWidth != double.NaN)
                    {
                        this.ScaleXY(this.imageClip.GetImage());
                    }
                }

                this.imageClip.ImageFailed -= imageClip_ImageFailed;
                this.imageClip.ImageFailed += imageClip_ImageFailed;

                this.imageClip.ImageDownLoadProgress -= imageClip_ImageDownLoadProgress;
                this.imageClip.ImageDownLoadProgress += imageClip_ImageDownLoadProgress;
                this.IsImageComplete = imageClip.IsImageComplete;
            }
        }

        void imageClip_ImageDownLoadProgress(int downloadValue)
        {
            if (this.ImageDownLoadProgress != null)
            {
                this.ImageDownLoadProgress(downloadValue);
            }
        }

        void imageClip_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (this.ImageFailed != null)
            {
                this.ImageFailed(sender, e);
            }
        }

        void ClipImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            size = e.NewSize;
            Init(e.NewSize);
            //后续可能会加入，时时改变大小重绘
            this.imageClip.ImageOpened -= this.imageClip_ImageOpened;
            if (this.IsClipImage)
            {
                this.imageClip.ImageOpened += this.imageClip_ImageOpened;
                //if (this.desiredSizeHeight != double.NaN && this.desiredSizeWidth != double.NaN)
                //{
                //    this.ScaleXY(this.imageClip);
                //}
                if (!double.IsNaN(this.desiredSizeHeight) && !double.IsNaN(this.desiredSizeWidth))
                {
                    this.ScaleXY(this.imageClip.GetImage());
                }
            }
        }

        private void ScaleXY(object sender)
        {
            try
            {
                if (sender != null)
                {
                    InitImageTransforms();
                    var interpolationX = 1.0;
                    var interpolationY = 1.0;

                    this.desiredSizeHeight = (sender as Image).ActualHeight;
                    this.desiredSizeWidth = (sender as Image).ActualWidth;

                    /*this.desiredSizeHeight = (sender as CacheImage).ActualHeight;
                    this.desiredSizeWidth = (sender as CacheImage).ActualWidth;*/

                    var interpolationH = this.imageClip.Height - desiredSizeHeight;
                    var interpolationW = this.imageClip.Width - desiredSizeWidth;
                    if (interpolationH <= 0)
                    {
                        interpolationH = 0;
                    }
                    else
                    {
                        //interpolationH = interpolationH / this.imageClip.Height;
                        interpolationH = interpolationH / this.desiredSizeHeight;
                    }

                    if (interpolationW <= 0)
                    {
                        interpolationW = 0;
                    }
                    else
                    {
                        //interpolationW = interpolationW / this.imageClip.Width;
                        interpolationW = interpolationW / this.desiredSizeWidth;
                    }

                    if (interpolationH > interpolationW)
                    {
                        //interpolationX = interpolationX + interpolationH;
                        interpolationX = interpolationX + System.Math.Round(interpolationH, 15, MidpointRounding.AwayFromZero) + 0.005;
                    }
                    else
                    {
                        //interpolationY = interpolationY + interpolationW;
                        interpolationY = interpolationY + System.Math.Round(interpolationW, 15, MidpointRounding.AwayFromZero) + 0.005;
                    }

                    if (interpolationX > interpolationY)
                    {
                        ct.ScaleX = ct.ScaleY = interpolationX;
                    }
                    else
                    {
                        ct.ScaleX = ct.ScaleY = interpolationY;
                    }

                    /*this.imageClip.Visibility = Windows.UI.Xaml.Visibility.Visible;*/
                    if (!this.isSbComplete)
                    {
                        if (this.IsAnimation)
                        {
                            this.isSbComplete = true;
                            this.AnimationBeginStop();
                            this.AnimationBeginStart();
                        }
                        else
                        {
                            this.AnimationBeginStop();
                            this.imageClip.Opacity = 1.0;
                        }
                    }

                    //this.imageClip.Opacity = 1.0;

                    if (this.ImageOpened != null)
                    {
                        this.ImageOpened(sender);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void imageClip_ImageOpened(object sender, RoutedEventArgs e)
        {
            ScaleXY(sender);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            size = finalSize;
            Init(finalSize);
            return base.ArrangeOverride(finalSize);
        }

        private void Init(Size size)
        {
            gridRoot.Width = imageClip.Width = size.Width;
            gridRoot.Height = imageClip.Height = size.Height;
            AddMask();
        }

        private void InitImageTransforms()
        {
            ct.ScaleX = 1.0;
            ct.ScaleY = 1.0;
            this.imageClip.RenderTransform = ct;
        }

        private void AddMask()
        {
            gridRoot.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), size) };
        }

        /// <summary>
        /// 保存图片到系统的文件夹
        /// </summary>
        /// <returns>保存成功返回true,失败返回false</returns>
        public async Task<bool> SaveImage(string fileName = "")
        {
            if (imageClip != null)
            {
                return await imageClip.SaveImage(fileName);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存图片到任意的文件夹
        /// </summary>
        /// <returns>保存成功返回true,失败返回false</returns>
        public async Task<bool> SaveFileImage()
        {
            if (imageClip != null)
            {
                return await imageClip.SaveFileImage();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 占停下载
        /// </summary>
        public void Pause()
        {
            if (this.imageClip != null)
            {
                this.imageClip.Pause();
            }
        }

        /// <summary>
        /// 继续下载
        /// </summary>
        public void Resume()
        {
            if (this.imageClip != null)
            {
                this.imageClip.Resume();
            }
        }

        /// <summary>
        /// 取消下载
        /// </summary>
        public void Cancel()
        {
            if (this.imageClip != null)
            {
                this.imageClip.Cancel();
            }
        }
    }
}
