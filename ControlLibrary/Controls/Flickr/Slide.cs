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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class Slide : Control
    {
        //对象取得完成的事件路由
        /// <summary>
        /// 对象完成的代理
        /// </summary>
        public delegate void LoadedCompletedHandler();
        /// <summary>
        /// 对象完成触发的事件
        /// </summary>
        public event LoadedCompletedHandler LoadedCompleted;
        
        private Grid gridSb = null;
        private Storyboard uxSBPanning = null;
        private Storyboard uxSBZoomming = null;
        public Storyboard uxSBOutro = null;
        private Image uxHorizontalImage = null;

        private double gridSbWidth = double.NaN;
        private double gridSbHeight = double.NaN;

        /// <summary>
        /// The current storyboard that was chosen randomly from storyboardList
        /// </summary>
        private Storyboard _currentStoryboard = new Storyboard();

        /// <summary>
        /// Gets the current time of the currentStoryboard before it was stopped
        /// </summary>
        public TimeSpan TimeStoryboardStopped;

        /// <summary>
        /// Gets the Orientation for which to display the image based on the Image's ratio
        /// </summary>
        protected Orientation ImageOrientation = Orientation.Horizontal;

        /// <summary>
        /// Gets or sets the picture for the Image within the Slide
        /// </summary>
        public void SetImage(ImageSource image)
        {
            //ImageOrientation = (image.PixelWidth > image.PixelHeight)
            //                       ? Orientation.Horizontal
            //                       : Orientation.Vertical;
            //var targetImg = (ImageOrientation == Orientation.Horizontal) ? uxHorizontalImage : uxVerticalImage;

            // Sets the Image Source and Image Visibility based on the Orientation
            if (uxHorizontalImage != null)
            {
                uxHorizontalImage.Visibility = Visibility.Collapsed;
                //uxVerticalImage.Visibility = Visibility.Collapsed;

                uxHorizontalImage.Source = image;

                //addMask();
                uxHorizontalImage.Visibility = Visibility.Visible;
            }
        }

        public ImageSource GetImageSource()
        {
            return uxHorizontalImage.Source;
        }

        public void SetImageWH(double w, double h)
        {
            if (uxHorizontalImage != null)
            {
                uxHorizontalImage.Height = h;
                uxHorizontalImage.Width = w;
            }
        }

        public Image GetImage()
        {
            return uxHorizontalImage;
        }

        private void addMask()
        {
            //uxHorizontalImage.HorizontalAlignment = this.HorizontalAlignment;
            //uxHorizontalImage.VerticalAlignment = this.VerticalAlignment;
            //uxHorizontalImage.Margin = this.Margin;
            if (gridSb.Clip != null) return;

            if (gridSbWidth == double.NaN || gridSbHeight == double.NaN) return;

            if (gridSb.Clip == null)
            {
                var clip = new RectangleGeometry();
                clip.Rect = new Rect(0, 0, gridSbWidth, gridSbHeight);
                gridSb.Clip = clip;
                //uxHorizontalImage.Clip = clip;
            }

            //uxHorizontalImage.Height = this.gridSbHeight;
            //uxHorizontalImage.Width = this.gridSbWidth;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.gridSbHeight = finalSize.Height;
            this.gridSbWidth = finalSize.Width;
            return base.ArrangeOverride(finalSize);
        }
        
        public Slide()
        {
            this.DefaultStyleKey = typeof(Slide);

            if (uxSBOutro != null && uxSBPanning != null && uxSBZoomming != null)
            {
                uxSBOutro.Completed -= uxSBOutro_Completed;
                uxSBOutro.Completed += uxSBOutro_Completed;
                uxSBPanning.Completed -= uxSBSlideDuration_Completed;
                uxSBPanning.Completed += uxSBSlideDuration_Completed;
                uxSBZoomming.Completed -= uxSBSlideDuration_Completed;
                uxSBZoomming.Completed += uxSBSlideDuration_Completed;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            gridSb = this.GetTemplateChild("gridSb") as Grid;
            uxHorizontalImage = this.GetTemplateChild("uxHorizontalImage") as Image;
            if (gridSb != null)
            {
                uxSBOutro = gridSb.Resources["uxSBOutro"] as Storyboard;
                uxSBPanning = gridSb.Resources["uxSBPanning"] as Storyboard;
                uxSBZoomming = gridSb.Resources["uxSBZoomming"] as Storyboard;
                if (uxSBOutro != null && uxSBPanning != null && uxSBZoomming != null)
                {
                    uxSBOutro.Completed -= uxSBOutro_Completed;
                    uxSBOutro.Completed += uxSBOutro_Completed;
                    uxSBPanning.Completed -= uxSBSlideDuration_Completed;
                    uxSBPanning.Completed += uxSBSlideDuration_Completed;
                    uxSBZoomming.Completed -= uxSBSlideDuration_Completed;
                    uxSBZoomming.Completed += uxSBSlideDuration_Completed;
                }

                if (LoadedCompleted != null)
                {
                    this.LoadedCompleted();
                }
            }
        }

        private void SetStoryboardTimes(Storyboard target, TimeSpan duration, TimeSpan hold, bool isZooming)
        {
            target.Duration = duration + hold;

            foreach (var child in
                target.Children.Cast<DoubleAnimationUsingKeyFrames>().Where(child => child.KeyFrames.Count == 2))
            {
                child.KeyFrames[1].KeyTime = duration;

                // randomly flip numbers
                if (!isZooming && FlickrHelp.RandomNumber.Next(0, 2) != 0)
                    continue;

                SwapKeyFrameValues(child);
            }

            // zooming, valid grouping, and should I randomly flip values
            if (isZooming && target.Children.Count % 2 == 0 && FlickrHelp.RandomNumber.Next(0, 2) != 0)
            {
                var groupings = target.Children.Cast<DoubleAnimationUsingKeyFrames>().GroupBy(o => (string)o.ReadLocalValue(Storyboard.TargetNameProperty));

                foreach (var group in groupings)
                {
                    var startValue = group.ElementAt(0).KeyFrames[0].Value;
                    var endValue = group.ElementAt(0).KeyFrames[1].Value;

                    foreach (var item in group)
                    {
                        item.KeyFrames[0].Value = endValue;
                        item.KeyFrames[1].Value = startValue;
                    }
                }
            }
        }

        private static void SwapKeyFrameValues(DoubleAnimationUsingKeyFrames frame)
        {
            var temp = frame.KeyFrames[0].Value;

            frame.KeyFrames[0].Value = frame.KeyFrames[1].Value;
            frame.KeyFrames[1].Value = temp;
        }

        /// <summary>
        /// Event handler that fires when the Slide Duration has completed
        /// Calls the SlideDurationCompleting Event
        /// </summary>
        void uxSBSlideDuration_Completed(object sender, object e)
        {
            if (FlickrHelp.SlideDurationCompleting != null)
                FlickrHelp.SlideDurationCompleting(sender, e);
        }


        /// <summary>
        /// Event handler that fires when the outro has completed
        /// Stops the Slide and removes the usercontrol from the Slide Grid on the player
        /// </summary>
        void uxSBOutro_Completed(object sender, object e)
        {
            ForceRemove();
        }

        public void ForceRemove()
        {
            Stop();
            if (!(Parent is Grid))
                return;

            ((Grid)Parent).Children.Remove(this);
            try
            {
                //GC.Collect();
            }
            catch { }
        }

        /// <summary>
        /// Begins Slide
        /// </summary>
        public void Begin()
        {
            if (_currentStoryboard != null)
            {
                var duration = TimeSpan.FromSeconds(SettingsHelper.SlideDuration);
                var hold = TimeSpan.FromSeconds(SettingsHelper.HoldDuration);

                var isZooming = (FlickrHelp.RandomNumber.Next(0, 3) == 0);  // total 4 different style pans, 2 zoom, 1/3 chance to zoom
                _currentStoryboard = isZooming ? uxSBZoomming : uxSBPanning;

                SetStoryboardTimes(_currentStoryboard, duration, hold, isZooming);

                // Begin Intro and Current Storyboard
                _currentStoryboard.Begin();
            }
        }


        /// <summary>
        /// Pauses all Storyboards
        /// </summary>
        public void Pause()
        {
            TimeStoryboardStopped = _currentStoryboard.GetCurrentTime();
            _currentStoryboard.Stop();

            uxSBOutro.Pause();
        }

        /// <summary>
        /// Resumes all Storyboards
        /// </summary>
        public void Resume()
        {
            _currentStoryboard.Begin();
            _currentStoryboard.Seek(TimeStoryboardStopped);

            uxSBOutro.Resume();
        }

        /// <summary>
        /// Stops all Storyboards
        /// </summary>
        public void Stop()
        {
            _currentStoryboard.Stop();
            uxSBOutro.Stop();
        }
    }
}
