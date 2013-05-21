using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ControlLibrary.Tools;
using Windows.Foundation;
using Windows.UI;
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
    public sealed class AlbumLiveTile : Control
    {
        private Grid LayoutRoot = null;
        private Storyboard uxSBIntro = null;
        private double x;
        private Grid uxSlides = null;
        //临时得到图片流完成的变量
        private Image imageTemp = null;
        private BitmapImage bitmap = null;
        private string uriStringTemp = string.Empty;
        private bool isManipulation = false;
        //缓存所有的图像源
        //private List<BitmapImage> bitmapList = null;

        private double LayoutRootWidth = double.NaN;
        private double LayoutRootHeight = double.NaN;

        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(AlbumLiveTile), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(onBorderColorPropertyChanged)));

        private static void onBorderColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as AlbumLiveTile;
            if (tile != null && tile.LayoutRoot != null)
            {

            }
        }

        public List<string> AlbumList
        {
            get { return (List<string>)GetValue(AlbumListProperty); }
            set { SetValue(AlbumListProperty, value); }
        }

        public static readonly DependencyProperty AlbumListProperty = DependencyProperty.Register("AlbumList", typeof(List<string>), typeof(AlbumLiveTile), new PropertyMetadata(null, new PropertyChangedCallback(onAlbumListPropertyChanged)));

        private static void onAlbumListPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as AlbumLiveTile;
            if (tile != null && tile.LayoutRoot != null)
            {
                tile.PlayAlbum();
            }
        }

        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register("ImageStretch", typeof(Stretch), typeof(AlbumLiveTile), new PropertyMetadata(Stretch.None, new PropertyChangedCallback(onStretchPropertyChanged)));

        private static void onStretchPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as AlbumLiveTile;
            if (tile != null && tile.LayoutRoot != null)
            {
                if (tile._newSlide != null)
                {
                    Image image = tile._newSlide.GetImage();
                    image.Stretch = tile.ImageStretch;
                }
            }
        }

        public Visibility VisibilityBorder
        {
            get { return (Visibility)GetValue(VisibilityBorderProperty); }
            set { SetValue(VisibilityBorderProperty, value); }
        }

        public static readonly DependencyProperty VisibilityBorderProperty = DependencyProperty.Register("VisibilityBorder", typeof(Visibility), typeof(AlbumLiveTile), new PropertyMetadata(Visibility.Collapsed, new PropertyChangedCallback(onVisibilityBorderPropertyChanged)));

        private static void onVisibilityBorderPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tile = sender as AlbumLiveTile;
            if (tile != null && tile.LayoutRoot != null)
            {

            }
        }

        /// <summary>
        /// Defines the different states that the Player can have
        /// </summary>
        public enum PlayerStates
        {
            Stopped,
            Playing,
            Paused
        }

        public event RoutedEventHandler Stopped;
        public event RoutedEventHandler FailedToLoad;

        private PlayerStates _playerState = PlayerStates.Stopped;
        /// <summary>
        /// Gets or Sets the value for the Player's State
        /// </summary>
        public PlayerStates PlayerState { get { return _playerState; } set { _playerState = value; } }

        /// <summary>
        /// The index of the currently displayed picture of the album
        /// </summary>
        private int _currentPictureIndex;
        public int CurrentPictureIndex
        {
            get
            {
                return _currentPictureIndex;
            }
        }

        /// <summary>
        /// The new slide that will display under the old slide waiting to display
        /// </summary>
        private Slide _newSlide;

        /// <summary>
        /// The old slide that is currently display waiting to transition out
        /// </summary>
        private Slide _oldSlide;


        public AlbumLiveTile()
        {
            this.DefaultStyleKey = typeof(AlbumLiveTile);
            if (LayoutRoot != null)
            {
                this.PlayAlbum();
                if (imageTemp == null)
                {
                    imageTemp = new Image();
                    imageTemp.Opacity = 0;
                }
                if (bitmap == null)
                {
                    bitmap = new BitmapImage();
                }
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            imageTemp = new Image();
            imageTemp.Opacity = 0;
            bitmap = new BitmapImage();
            LayoutRoot = this.GetTemplateChild("LayoutRoot") as Grid;
            if (LayoutRoot != null)
            {
                //LayoutRoot.DoubleTapped += LayoutRoot_DoubleTapped;
                LayoutRoot.ManipulationDelta += LayoutRoot_ManipulationDelta;
                LayoutRoot.ManipulationCompleted += LayoutRoot_ManipulationCompleted;
                LayoutRoot.ManipulationStarting += LayoutRoot_ManipulationStarting;
                uxSBIntro = this.LayoutRoot.Resources["uxSBIntro"] as Storyboard;
                uxSlides = this.GetTemplateChild("uxSlides") as Grid;
                FlickrHelp.SlideDurationCompleting += SlideHandler_SlideCompleting;
                this.PlayAlbum();
            }
        }

        void LayoutRoot_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            x = 0;
            isManipulation = false;
        }

        /// <summary>
        /// Event handler that fires when a slide duration has completed.
        /// Slide to next picture
        /// </summary>
        private void SlideHandler_SlideCompleting(object sender, object e)
        {
            SlidePicture(int.MinValue);
        }

        void LayoutRoot_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //x *= e.Delta.Translation.X;
            x *= e.Delta.Translation.X;
            e.Handled = true;
        }

        void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
            if (!isManipulation)
            {
                if (x > 1.5)
                {
                    if (PlayerState == PlayerStates.Paused)
                        Resume();

                    SlidePicture(x);
                }
                isManipulation = true;
            }
        }

        void LayoutRoot_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (PlayerState == PlayerStates.Playing && uxSBIntro.GetCurrentState() != ClockState.Active)
            {
                Stop();
            }
        }

        /// <summary>
        /// Plays an Album
        /// </summary>
        public void PlayAlbum()
        {

            PlayerState = PlayerStates.Playing;

            _currentPictureIndex = 0;

            uxSBIntro.Begin();

            ShowPicture();
        }

        /// <summary>
        /// Slides to the next or previous picture depending on the angle of the flick gesture passed
        /// </summary>
        /// <param name="angle">Angle of the Flick Gesture</param>
        public void SlidePicture(double angle)
        {
            if (AlbumList != null)
            {
                if (angle != int.MinValue && (angle > SettingsHelper.ForwardGestureGreaterThanAngle || angle < SettingsHelper.ForwardGestureLessThanAngle))
                    _currentPictureIndex = _currentPictureIndex == 0 ? AlbumList.Count - 1 : _currentPictureIndex - 1;
                else
                    _currentPictureIndex = _currentPictureIndex >= AlbumList.Count - 1 ? 0 : _currentPictureIndex + 1;

                ShowPicture();
            }

        }

        /// <summary>
        /// Displays the picture of the currentPictureIndex
        /// </summary>
        private void ShowPicture()
        {
            if (AlbumList == null)
            {
                if (FailedToLoad != null)
                    FailedToLoad.Invoke(this, null);

                return;
            }

            if (_newSlide != null)
            {
                _oldSlide = _newSlide;
            }

            _newSlide = new Slide();
            _newSlide.Opacity = 0;
            this.LayoutRoot.Children.Add(_newSlide);

            _newSlide.LoadedCompleted += () =>
            {
                /*this.Dispatcher.InvokeAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, (e, s) =>
                {
                    //if (!this.LayoutRoot.Children.Contains(imageTemp))
                    //{
                    this.LayoutRoot.Children.Add(imageTemp);
                    //}
                    this.LayoutRoot.Children.Remove(_newSlide);
                    _newSlide.Opacity = 1;

                    var gt = this.Margin;
                    var t = uxSlides.Margin;

                    if (bitmap.UriSource != null)
                    {
                        uriStringTemp = bitmap.UriSource.AbsoluteUri;
                    }

                    if (_currentPictureIndex >= AlbumList.Count)
                    {
                        return;
                    }

                    var source = AlbumList[_currentPictureIndex];
                    //var bitmap = new WriteableBitmap(800, 800);
                    bitmap = new BitmapImage();
                    bitmap.UriSource = new Uri(source, UriKind.Absolute);
                    imageTemp.Source = bitmap;

                    //imageTemp.ImageOpened += (ss, ee) =>
                    //    {
                    //        var img = ss as Image;

                    //        _newSlide.SetImage(bitmap);
                    //        this.LayoutRoot.Children.Remove(this.imageTemp);
                    //        _newSlide.UpdateLayout();
                    //        _newSlide.Begin();

                    //        if (_oldSlide != null)
                    //            _oldSlide.uxSBOutro.Begin();

                    //        if (!uxSlides.Children.Contains(_newSlide))
                    //        {
                    //            uxSlides.Children.Insert(0, _newSlide);
                    //        }
                    //        while (uxSlides.Children.Count > 2)
                    //        {
                    //            ((Slide)uxSlides.Children[uxSlides.Children.Count - 1]).ForceRemove();
                    //        }
                    //    };

                    if (uriStringTemp == source)
                    {
                        SetSlideImageBitmap();
                    }
                    else
                    {
                        imageTemp.ImageOpened += imageTemp_ImageOpened;
                    }
                }, this, null);*/

                this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        if (!this.LayoutRoot.Children.Contains(imageTemp))
                        {
                            this.LayoutRoot.Children.Add(imageTemp);
                        }
                        this.LayoutRoot.Children.Remove(_newSlide);
                        _newSlide.Opacity = 1;

                        var gt = this.Margin;
                        var t = uxSlides.Margin;

                        if (bitmap.UriSource != null)
                        {
                            uriStringTemp = bitmap.UriSource.AbsoluteUri;
                        }

                        if (_currentPictureIndex >= AlbumList.Count)
                        {
                            return;
                        }

                        var source = AlbumList[_currentPictureIndex];
                        //var bitmap = new WriteableBitmap(800, 800);
                        bitmap = new BitmapImage();
                        bitmap.UriSource = new Uri(source, UriKind.Absolute);
                        imageTemp.Source = bitmap;

                        //imageTemp.ImageOpened += (ss, ee) =>
                        //    {
                        //        var img = ss as Image;

                        //        _newSlide.SetImage(bitmap);
                        //        this.LayoutRoot.Children.Remove(this.imageTemp);
                        //        _newSlide.UpdateLayout();
                        //        _newSlide.Begin();

                        //        if (_oldSlide != null)
                        //            _oldSlide.uxSBOutro.Begin();

                        //        if (!uxSlides.Children.Contains(_newSlide))
                        //        {
                        //            uxSlides.Children.Insert(0, _newSlide);
                        //        }
                        //        while (uxSlides.Children.Count > 2)
                        //        {
                        //            ((Slide)uxSlides.Children[uxSlides.Children.Count - 1]).ForceRemove();
                        //        }
                        //    };

                        if (uriStringTemp == source)
                        {
                            SetSlideImageBitmap();
                        }
                        else
                        {
                            imageTemp.ImageOpened += imageTemp_ImageOpened;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            };
        }

        private void imageTemp_ImageOpened(object sender, RoutedEventArgs e)
        {
            var img = sender as Image;
            img.ImageOpened -= imageTemp_ImageOpened;
            SetSlideImageBitmap();
        }

        //设置图片数据源
        private void SetSlideImageBitmap()
        {
            try
            {
                _newSlide.SetImage(bitmap);
                _newSlide.GetImage().Stretch = this.ImageStretch;
                //var w = bitmap.PixelWidth * (LayoutRootWidth / bitmap.PixelWidth);
                //var h = bitmap.PixelHeight * (LayoutRootHeight / bitmap.PixelHeight);
                //_newSlide.SetImageWH(w, h);
                addMask();
                this.LayoutRoot.Children.Remove(this.imageTemp);
                _newSlide.UpdateLayout();
                _newSlide.Begin();


                if (_oldSlide != null)
                {
                    _oldSlide.uxSBOutro.Begin();
                }

                if (!uxSlides.Children.Contains(_newSlide))
                {
                    uxSlides.Children.Insert(0, _newSlide);
                }
                while (uxSlides.Children.Count > 2)
                {
                    ((Slide)uxSlides.Children[uxSlides.Children.Count - 1]).ForceRemove();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.LayoutRootWidth = finalSize.Width;
            this.LayoutRootHeight = finalSize.Height;
            return base.ArrangeOverride(finalSize);
        }

        private void addMask()
        {
            //手动设置
            /*_newSlide.GetImage().HorizontalAlignment = this.HorizontalAlignment;
            _newSlide.GetImage().VerticalAlignment = this.VerticalAlignment;
            _newSlide.GetImage().Margin = this.Margin;*/

            _newSlide.GetImage().HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            _newSlide.GetImage().VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            _newSlide.GetImage().Margin = new Thickness(0);
            if (LayoutRoot.Clip != null) return;

            if (LayoutRootWidth == double.NaN || LayoutRootHeight == double.NaN) return;

            if (LayoutRoot.Clip == null)
            {
                var clip = new RectangleGeometry();
                clip.Rect = new Rect(0, 0, LayoutRootWidth, LayoutRootHeight);
                LayoutRoot.Clip = clip;
            }
        }

        // Stops all Storyboards
        public void Stop()
        {
            //uxSlides.Children.Where(result => result is Slide).ToList().ForEach(slide => ((Slide)slide).Stop());
            foreach (var element in uxSlides.Children.ToList())
            {
                if (element is Slide)
                {
                    (element as Slide).Stop();
                }
            }
            uxSlides.Children.Clear();
            uxSBIntro.Stop();

            PlayerState = PlayerStates.Stopped;

            if (Stopped != null)
            {
                var args = new RoutedEventArgs();

                Stopped.Invoke(this, args);
            }
        }

        /// <summary>
        /// Pauses all Storyboards
        /// </summary>
        public void Pause()
        {
            //uxSlides.Children.Where(result => result is Slide).ToList().ForEach(slide => ((Slide)slide).Pause());

            foreach (var element in uxSlides.Children.ToList())
            {
                if (element is Slide)
                {
                    (element as Slide).Pause();
                }
            }

            PlayerState = PlayerStates.Paused;
        }

        /// <summary>
        /// Resumes all Storyboards
        /// </summary>
        public void Resume()
        {
            //uxSlides.Children.Where(result => result is Slide).ToList().ForEach(slide => ((Slide)slide).Resume());

            foreach (var element in uxSlides.Children.ToList())
            {
                if (element is Slide)
                {
                    (element as Slide).Resume();
                }
            }

            PlayerState = PlayerStates.Playing;
        }
    }
}
