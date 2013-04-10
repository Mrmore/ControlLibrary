using System;
using System.Collections.Generic;
using System.Linq;
using ControlLibrary.Tools;
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
    public sealed class PhotoLiveTileBase : Control
    {
        private Canvas LayoutRoot = null;
        private static Random random = new Random();

        private Size actualSize;
        private string[] photoList;
        private int currentPhotoIndex = -1;
        public int CurrentPhotoIndex
        {
            get
            {
                return currentPhotoIndex;
            }
        }
        private Image currentImg;
        private Image hiddenImg;

        public String Source
        {
            get { return (String)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(String), typeof(PhotoLiveTileBase), new PropertyMetadata(null, new PropertyChangedCallback(onSourcePropertyChanged)));

        private static void onSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as PhotoLiveTileBase;
            if (tile != null && tile.LayoutRoot != null)
            {
                var str = e.NewValue as String;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    var photos = str.Split(new string[] { Constants.STRING_SEPARATOR }, StringSplitOptions.None);
                    tile.loadImages(photos);
                }
            }
        }

        public PhotoLiveTileBase()
        {
            this.DefaultStyleKey = typeof(PhotoLiveTileBase);
            this.Loaded += LiveTile_Loaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LayoutRoot = this.GetTemplateChild("LayoutRoot") as Canvas;
            if (this.LayoutRoot != null && this.Source != null && this.Source != string.Empty)
            {
                if (!string.IsNullOrWhiteSpace(this.Source))
                {
                    var photos = this.Source.Split(new string[] { Constants.STRING_SEPARATOR }, StringSplitOptions.None);
                    this.loadImages(photos);
                }
            }
        }

        void LiveTile_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= LiveTile_Loaded;

            var parent = this.Parent as FrameworkElement;
            actualSize = new Size(this.ActualWidth, this.ActualHeight);
            addMask();
        }

        private void loadImages(string[] photoListString)
        {
            if (LayoutRoot != null)
            {
                photoList = photoListString;
                if (photoListString != null && photoListString.Length > 0)
                {

                    //方法1
                    //currentImg = GenerateTransformGroupImage();

                    //方法2
                    currentImg = GenerateCompositeTransformImage();

                    currentImg.ImageOpened += Image_ImageOpened;
                    currentPhotoIndex = 0;
                    this.LayoutRoot.Children.Add(currentImg);
                    //currentImg.Source = new BitmapImage(new Uri(photoList[0]));
                    UIUtil.TrySetImageSource(currentImg, photoList[0]);
                }
            }
        }

        void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            var img = sender as Image;
            img.ImageOpened -= Image_ImageOpened;

            // init the scale and x position of the image
            var bmp = img.Source as BitmapImage;
            var newSize = actualSize;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, newSize);

            //方法1
            //var st = UIUtil.GetTransform<ScaleTransform>(img);
            //st.ScaleX = st.ScaleY = scale;
            //var tt = UIUtil.GetTransform<TranslateTransform>(img);
            //tt.X = -(originalSize.Width * scale - newSize.Width) / 2;

            //方法2
            if (img.RenderTransform != null)
            {
                CompositeTransform ct = img.RenderTransform as CompositeTransform;
                if (ct != null)
                {
                    ct.ScaleX = ct.ScaleY = scale;
                    ct.TranslateX = -(originalSize.Width * scale - newSize.Width) / 2;
                }
            }


            if (img == currentImg)
            {
                animateCurrentImage();
            }
            else
            {
                switchImage();
            }
        }

        private void animateCurrentImage()
        {
            var bmp = currentImg.Source as BitmapImage;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, actualSize);

            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.BeginTime = TimeSpan.FromSeconds(random.NextDouble() * 5);
            double speed = 1;
            da.To = actualSize.Height - originalSize.Height * scale;
            da.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(da.To.Value) / 10 * speed));

            //方法1
            //Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(currentImg));
            //Storyboard.SetTargetProperty(da, "Y");

            //方法2
            Storyboard.SetTarget(da, currentImg);
            Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            sb.Children.Add(da);
            sb.Completed += animation_Completed;
            sb.Begin();

        }

        private void reverseAnimateCurrentImage()
        {
            var bmp = currentImg.Source as BitmapImage;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, actualSize);

            double offset = actualSize.Height - originalSize.Height * scale;

            //方法1
            //double currentY = UIUtil.GetTransform<TranslateTransform>(currentImg).Y;

            //方法2
            double currentY = (currentImg.RenderTransform as CompositeTransform).TranslateY;

            double to = currentY < 0 ? 0 : offset;
            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.BeginTime = TimeSpan.FromSeconds(2 + random.NextDouble() * 3);
            double speed = random.NextDouble() * .5 + .5;
            da.To = to;
            da.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(offset) / 10 * speed));

            //方法1
            //Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(currentImg));
            //Storyboard.SetTargetProperty(da, "Y");

            //方法2
            Storyboard.SetTarget(da, currentImg);
            Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            sb.Children.Add(da);
            sb.Completed += animation_Completed;
            sb.Begin();
        }

        private void switchImage()
        {
            var sb = new Storyboard();
            double beginTime = random.NextDouble() * 2;
            sb.Children.Add(generateSwitchAnimation(currentImg, beginTime));
            sb.Children.Add(generateSwitchAnimation(hiddenImg, beginTime));
            sb.Completed += switchAnimation_Completed;
            sb.Begin();
        }

        private DoubleAnimation generateSwitchAnimation(Image img, double beginTime)
        {
            var da = new DoubleAnimation();
            da.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut };
            da.BeginTime = TimeSpan.FromSeconds(beginTime);
            da.Duration = new Duration(TimeSpan.FromSeconds(.8));
            da.By = -actualSize.Height;

            //方法1
            //Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(img));
            //Storyboard.SetTargetProperty(da, "Y");

            //方法2
            Storyboard.SetTarget(da, img);
            Storyboard.SetTargetProperty(da, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");

            return da;
        }

        private void switchAnimation_Completed(object sender, object e)
        {
            var img = currentImg;
            currentImg = hiddenImg;
            hiddenImg = img;

            (sender as Storyboard).Completed -= switchAnimation_Completed;
            animateCurrentImage();
        }

        private void animation_Completed(object sender, object e)
        {
            (sender as Storyboard).Completed -= animation_Completed;
            int index = getNextIndex();
            if (index > -1)
            {
                if (photoList.Length > 1)
                {
                    currentPhotoIndex = index;

                    if (hiddenImg == null)
                    {
                        //方法1
                        //hiddenImg = GenerateTransformGroupImage();

                        //方法2
                        hiddenImg = GenerateCompositeTransformImage();

                        this.LayoutRoot.Children.Add(hiddenImg);
                    }
                    hiddenImg.ImageOpened += Image_ImageOpened;

                    //方法1
                    //var translate = UIUtil.GetTransform<TranslateTransform>(hiddenImg);
                    //translate.Y = actualSize.Height;

                    //方法2
                    var translate = hiddenImg.RenderTransform as CompositeTransform;
                    translate.TranslateY = actualSize.Height;

                    //hiddenImg.Source = new BitmapImage(new Uri(photoList[index]));
                    UIUtil.TrySetImageSource(hiddenImg, photoList[index]);
                }
                else
                {
                    reverseAnimateCurrentImage();
                }
            }
        }

        private void addMask()
        {
            if (LayoutRoot.Clip != null) return;

            var parent = this.Parent as FrameworkElement;
            if (parent == null) return;

            if (LayoutRoot.Clip == null)
            {
                var clip = new RectangleGeometry();
                clip.Rect = new Rect(0, 0, parent.ActualWidth, parent.ActualHeight);
                LayoutRoot.Clip = clip;
            }
        }

        private double getUniformToFillScale(Size originalSize, Size newSize)
        {
            if (originalSize.Width == 0.0 || originalSize.Height == 0.0)
                return 1.0;
            double scale = newSize.Width / originalSize.Width;
            if (originalSize.Height * scale < newSize.Height) scale = newSize.Height / originalSize.Height;
            return scale;
        }


        private int getNextIndex()
        {
            if (photoList == null) return -1;

            int pending = currentPhotoIndex + 1;
            if (pending == photoList.Length) pending = 0;
            return pending;
        }

        //方法1
        private Image GenerateTransformGroupImage()
        {
            var img = new Image();
            img.Stretch = Stretch.None;
            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform());
            tg.Children.Add(new TranslateTransform());
            img.RenderTransform = tg;
            return img;
        }

        //方法2
        private Image GenerateCompositeTransformImage()
        {
            var img = new Image();
            img.Stretch = Stretch.None;
            CompositeTransform ct = new CompositeTransform();
            img.RenderTransform = ct;
            return img;
        }
    }
}
