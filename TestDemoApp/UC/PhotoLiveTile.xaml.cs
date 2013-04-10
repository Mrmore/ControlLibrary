using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ControlLibrary.Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Razorfish.Common.Control
{
    public sealed partial class PhotoLiveTile : UserControl
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(String), typeof(PhotoLiveTile), new PropertyMetadata(null, new PropertyChangedCallback(onSourcePropertyChanged)));

        private static void onSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as PhotoLiveTile;
            var str = e.NewValue as String;
            if (!string.IsNullOrWhiteSpace(str))
            {
                var photos = str.Split(new string[] { Constants.STRING_SEPARATOR }, StringSplitOptions.None);
                tile.loadImages(photos);
            }
        }

        public String Source
        {
            get { return (String)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static Random random = new Random();

        private Size _actualSize;
        private string[] _photoList;
        private int _currentPhotoIndex = -1;
        private Image _currentImg;
        private Image _hiddenImg;

        public PhotoLiveTile()
        {
            this.InitializeComponent();
            this.Loaded += LiveTile_Loaded;
        }

        void LiveTile_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= LiveTile_Loaded;

            var parent = this.Parent as FrameworkElement;
            _actualSize = new Size(this.ActualWidth, this.ActualHeight);
            addMask();
        }

        private void loadImages(string[] photoList)
        {
            _photoList = photoList;
            if (photoList != null && photoList.Length > 0)
            {
                _currentImg = generateImage();
                _currentImg.ImageOpened += Image_ImageOpened;
                _currentPhotoIndex = 0;
                this.LayoutRoot.Children.Add(_currentImg);
                //_currentImg.Source = new BitmapImage(new Uri(photoList[0]));
                UIUtil.TrySetImageSource(_currentImg, _photoList[0]);
            }
        }

        void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            var img = sender as Image;
            img.ImageOpened -= Image_ImageOpened;

            // init the scale and x position of the image
            var bmp = img.Source as BitmapImage;
            var newSize = _actualSize;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, newSize);
            var st = UIUtil.GetTransform<ScaleTransform>(img);
            st.ScaleX = st.ScaleY = scale;
            var tt = UIUtil.GetTransform<TranslateTransform>(img);
            tt.X = -(originalSize.Width * scale - newSize.Width) / 2;
            
            if (img == _currentImg)
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
            var bmp = _currentImg.Source as BitmapImage;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, _actualSize);

            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.BeginTime = TimeSpan.FromSeconds(random.NextDouble() * 5);
            double speed = 1;
            da.To = _actualSize.Height - originalSize.Height * scale;
            da.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(da.To.Value) / 10 * speed));
            Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(_currentImg));
            Storyboard.SetTargetProperty(da, "Y");
            sb.Children.Add(da);
            sb.Completed += animation_Completed;
            sb.Begin();
            
        }

        private void reverseAnimateCurrentImage()
        {
            var bmp = _currentImg.Source as BitmapImage;
            var originalSize = new Size(bmp.PixelWidth, bmp.PixelHeight);
            double scale = getUniformToFillScale(originalSize, _actualSize);

            double offset = _actualSize.Height - originalSize.Height * scale;
            double currentY = UIUtil.GetTransform<TranslateTransform>(_currentImg).Y;
            double to = currentY < 0 ? 0 : offset;
            var sb = new Storyboard();
            var da = new DoubleAnimation();
            da.BeginTime = TimeSpan.FromSeconds(2 + random.NextDouble() * 3);
            double speed = random.NextDouble() * .5 + .5;
            da.To = to;
            da.Duration = new Duration(TimeSpan.FromSeconds(Math.Abs(offset) / 10 * speed));
            Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(_currentImg));
            Storyboard.SetTargetProperty(da, "Y");
            sb.Children.Add(da);
            sb.Completed += animation_Completed;
            sb.Begin();
        }

        private void switchImage()
        {
            var sb = new Storyboard();
            double beginTime = random.NextDouble() * 2;
            sb.Children.Add(generateSwitchAnimation(_currentImg, beginTime));
            sb.Children.Add(generateSwitchAnimation(_hiddenImg, beginTime));
            sb.Completed += switchAnimation_Completed;
            sb.Begin();
        }

        private DoubleAnimation generateSwitchAnimation(Image img, double beginTime)
        {
            var da = new DoubleAnimation();
            da.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut };
            da.BeginTime = TimeSpan.FromSeconds(beginTime);
            da.Duration = new Duration(TimeSpan.FromSeconds(.8));
            da.By = -_actualSize.Height;
            Storyboard.SetTarget(da, UIUtil.GetTransform<TranslateTransform>(img));
            Storyboard.SetTargetProperty(da, "Y");
            return da;
        }

        private void switchAnimation_Completed(object sender, object e)
        {
            var img = _currentImg;
            _currentImg = _hiddenImg;
            _hiddenImg = img;

            (sender as Storyboard).Completed -= switchAnimation_Completed;
            animateCurrentImage();
        }

        private void animation_Completed(object sender, object e)
        {
            (sender as Storyboard).Completed -= animation_Completed;
            int index = getNextIndex();
            if (index > -1)
            {
                if (_photoList.Length > 1)
                {
                    _currentPhotoIndex = index;

                    if (_hiddenImg == null)
                    {
                        _hiddenImg = generateImage();
                        this.LayoutRoot.Children.Add(_hiddenImg);
                    }
                    _hiddenImg.ImageOpened += Image_ImageOpened;
                    var translate = UIUtil.GetTransform<TranslateTransform>(_hiddenImg);
                    translate.Y = _actualSize.Height;
                    //_hiddenImg.Source = new BitmapImage(new Uri(_photoList[index]));
                    UIUtil.TrySetImageSource(_hiddenImg, _photoList[index]);
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
            double scale = newSize.Width / originalSize.Width;
            if (originalSize.Height * scale < newSize.Height) scale = newSize.Height / originalSize.Height;
            return scale;
        }

        private int getNextIndex()
        {
            if (_photoList == null) return -1;

            int pending = _currentPhotoIndex + 1;
            if (pending == _photoList.Length) pending = 0;
            return pending;
        }

        private Image generateImage()
        {
            var img = new Image();
            img.Stretch = Stretch.None;
            var tg = new TransformGroup();
            tg.Children.Add(new ScaleTransform());
            tg.Children.Add(new TranslateTransform());
            img.RenderTransform = tg;
            return img;
        }
    }
}
