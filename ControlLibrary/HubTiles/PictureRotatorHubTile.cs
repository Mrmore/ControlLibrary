using System;
using System.Collections.Generic;
using System.Linq;
using ControlLibrary.Exceptions;
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
    public class PictureRotatorHubTile : PictureHubTile
    {
        private static readonly DependencyProperty FirstContentProperty =
            DependencyProperty.Register("FirstContent", typeof(Image), typeof(PictureRotatorHubTile), new PropertyMetadata(null));

        private static readonly DependencyProperty SecondContentProperty =
            DependencyProperty.Register("SecondContent", typeof(Image), typeof(PictureRotatorHubTile), new PropertyMetadata(null));

        private UIElement panel;
        private FrameworkElement firstContentContainer;

        private Storyboard globalContentAnimation = new Storyboard();
        private DoubleAnimation moveUpGlobalAnimation;

        private Storyboard localContentAnimation = new Storyboard();
        private DoubleAnimation moveUpLocalAnimation;

        private int? lastPictureIndex = 0;

        /// <summary>
        /// Initializes a new instance of the RadPictureRotatorHubTile class.
        /// </summary>
        public PictureRotatorHubTile()
        {
            this.DefaultStyleKey = typeof(PictureRotatorHubTile);

            this.moveUpGlobalAnimation = new DoubleAnimation();
            this.moveUpGlobalAnimation.Duration = TimeSpan.FromSeconds(0.4);
            this.moveUpGlobalAnimation.EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseInOut };
            this.moveUpGlobalAnimation.From = 0;
            Storyboard.SetTargetProperty(this.moveUpGlobalAnimation, "(Canvas.Top)");

            this.moveUpLocalAnimation = new DoubleAnimation();
            this.moveUpLocalAnimation.BeginTime = TimeSpan.FromSeconds(0.3);
            this.moveUpLocalAnimation.Duration = TimeSpan.FromSeconds(8);
            this.moveUpLocalAnimation.From = 0;
            Storyboard.SetTargetProperty(this.moveUpLocalAnimation, "(Canvas.Top)");

            this.localContentAnimation.Children.Add(this.moveUpLocalAnimation);

            this.globalContentAnimation.Children.Add(this.moveUpGlobalAnimation);
            this.globalContentAnimation.Completed += this.OnCurrentAnimationCompleted;
        }

        internal object FirstContent
        {
            get
            {
                return this.GetValue(PictureRotatorHubTile.FirstContentProperty);
            }

            set
            {
                this.SetValue(PictureRotatorHubTile.FirstContentProperty, value);
            }
        }

        internal object SecondContent
        {
            get
            {
                return this.GetValue(PictureRotatorHubTile.SecondContentProperty);
            }

            set
            {
                this.SetValue(PictureRotatorHubTile.SecondContentProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that determines whether a rectangle clip is set on the LayoutRoot.
        /// </summary>
        protected override bool ShouldClip
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.panel = this.GetTemplateChild("PART_Panel") as UIElement;
            if (this.panel == null)
            {
                throw new MissingTemplatePartException(typeof(UIElement), "PART_Panel");
            }

            this.firstContentContainer = this.GetTemplateChild("PART_FirstContent") as FrameworkElement;

            Storyboard.SetTarget(this.moveUpLocalAnimation, this.firstContentContainer);
            Storyboard.SetTarget(this.moveUpGlobalAnimation, this.panel);
        }

        /// <summary>
        /// Should be overridden in descendant classes to indicate if the same image can be displayed
        /// many times in a row.
        /// </summary>
        /// <param name="index">The index of the new image.</param>
        /// <returns>Returns true if the image can be repeated and false otherwise.</returns>
        protected override bool IsNewIndexValid(int index)
        {
            if (this.lastPictureIndex == null)
            {
                this.lastPictureIndex = index;
                return true;
            }

            return index != this.lastPictureIndex.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);
            CreateImageBingUI();
        }

        private async void CreateImageBingUI()
        {
            ImageSource imageSource = await this.GetImageSource();
            if (imageSource != null)
            {
                //Image image = new Image() { Stretch = Stretch.UniformToFill };
                //image.Source = imageSource;
                //this.FirstContent = image;

                //image = new Image() { Stretch = Stretch.UniformToFill };
                //image.Source = imageSource;
                //this.SecondContent = image;

                ClipImage clipImage = new ClipImage() { Width = this.Width, Height = this.Height };
                clipImage.Source = imageSource;
                this.FirstContent = clipImage;

                clipImage = new ClipImage() { Width = this.Width, Height = this.Height };
                clipImage.Source = imageSource;
                this.SecondContent = clipImage;
            }
        }

        protected override void OnUpLoadSources()
        {
            base.OnUpLoadSources();
            CreateImageBingUI();
        }

        /// <summary>
        /// A virtual callback that is called periodically when the tile is no frozen. It can be used to
        /// update the tile visual states or other necessary operations.
        /// </summary>
        protected override void Update()
        {
            if (this.BackContent != null)
            {
                base.Update();
            }

            if (this.FirstContent == null)
            {
                return;
            }

            this.moveUpGlobalAnimation.To = -this.Height;
            this.globalContentAnimation.Begin();
        }

        private async void OnCurrentAnimationCompleted(object sender, object e)
        {
            this.firstContentContainer.LayoutUpdated += this.OnFirstContentLayoutUpdated;

            Canvas.SetTop(this.panel, 0);
            Canvas.SetTop(this.firstContentContainer, 0);

            //Image firstImage = this.FirstContent as Image;
            //Image secondImage = this.SecondContent as Image;
            ClipImage firstImage = this.FirstContent as ClipImage;
            ClipImage secondImage = this.SecondContent as ClipImage;

            ImageSource tmp = secondImage.Source;

            secondImage.Source = null;
            firstImage.Source = tmp;

            secondImage.Source = await this.GetImageSource();
        }

        private void OnFirstContentLayoutUpdated(object sender, object e)
        {
            this.firstContentContainer.LayoutUpdated -= this.OnFirstContentLayoutUpdated;

            //double heightDiff = this.firstContentContainer.ActualHeight - this.Height;
            double heightDiff = this.firstContentContainer.DesiredSize.Height - this.Height;
            if (heightDiff < 0)
            {
                return;
            }

            if (heightDiff > this.Height)
            {
                heightDiff = this.Height;
            }

            this.moveUpLocalAnimation.To = -heightDiff;
            this.localContentAnimation.Begin();
        }
    }
}
