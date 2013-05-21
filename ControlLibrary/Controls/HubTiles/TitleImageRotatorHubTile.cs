using System;
using System.Collections.Generic;
using System.Linq;
using ControlLibrary.Exceptions;
using Windows.UI;
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
    public sealed class TitleImageRotatorHubTile : TitleImageHubTile
    {
        private static readonly DependencyProperty FirstContentProperty =
            DependencyProperty.Register("FirstContent", typeof(string), typeof(TitleImageRotatorHubTile), new PropertyMetadata(string.Empty));

        private static readonly DependencyProperty FirstTitleProperty =
            DependencyProperty.Register("FirstTitle", typeof(string), typeof(TitleImageRotatorHubTile), new PropertyMetadata(string.Empty));

        private static readonly DependencyProperty FirstImageProperty =
            DependencyProperty.Register("FirstImage", typeof(ImageSource), typeof(TitleImageRotatorHubTile), new PropertyMetadata(null));

        private static readonly DependencyProperty SecondContentProperty =
            DependencyProperty.Register("SecondContent", typeof(string), typeof(TitleImageRotatorHubTile), new PropertyMetadata(string.Empty));

        private static readonly DependencyProperty SecondTitleProperty =
           DependencyProperty.Register("SecondTitle", typeof(string), typeof(TitleImageRotatorHubTile), new PropertyMetadata(string.Empty));

        private static readonly DependencyProperty SecondImageProperty =
            DependencyProperty.Register("SecondImage", typeof(ImageSource), typeof(TitleImageRotatorHubTile), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register("TitleFontSize", typeof(double), typeof(TitleImageRotatorHubTile), new PropertyMetadata(20.0));

        public static readonly DependencyProperty ContentFontSizeProperty =
            DependencyProperty.Register("ContentFontSize", typeof(double), typeof(TitleImageRotatorHubTile), new PropertyMetadata(20.0));

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(TitleImageRotatorHubTile), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty ContentForegroundProperty =
            DependencyProperty.Register("ContentForeground", typeof(Brush), typeof(TitleImageRotatorHubTile), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        internal string FirstContent
        {
            get
            {
                return (string)this.GetValue(FirstContentProperty);
            }

            set
            {
                this.SetValue(FirstContentProperty, value);
            }
        }

        internal string FirstTitle
        {
            get
            {
                return (string)this.GetValue(FirstTitleProperty);
            }

            set
            {
                this.SetValue(FirstTitleProperty, value);
            }
        }

        internal ImageSource FirstImage
        {
            get
            {
                return (ImageSource)this.GetValue(FirstImageProperty);
            }

            set
            {
                this.SetValue(FirstImageProperty, value);
            }
        }

        internal string SecondContent
        {
            get
            {
                return (string)this.GetValue(SecondContentProperty);
            }

            set
            {
                this.SetValue(SecondContentProperty, value);
            }
        }

        internal string SecondTitle
        {
            get
            {
                return (string)this.GetValue(SecondTitleProperty);
            }

            set
            {
                this.SetValue(SecondTitleProperty, value);
            }
        }

        internal ImageSource SecondImage
        {
            get
            {
                return (ImageSource)this.GetValue(SecondImageProperty);
            }

            set
            {
                this.SetValue(SecondImageProperty, value);
            }
        }

        public double TitleFontSize
        {
            get
            {
                return (double)this.GetValue(TitleFontSizeProperty);
            }

            set
            {
                this.SetValue(TitleFontSizeProperty, value);
            }
        }

        public double ContentFontSize
        {
            get
            {
                return (double)this.GetValue(ContentFontSizeProperty);
            }

            set
            {
                this.SetValue(ContentFontSizeProperty, value);
            }
        }

        public Brush TitleForeground
        {
            get
            {
                return (Brush)this.GetValue(TitleForegroundProperty);
            }

            set
            {
                this.SetValue(TitleForegroundProperty, value);
            }
        }

        public Brush ContentForeground
        {
            get
            {
                return (Brush)this.GetValue(ContentForegroundProperty);
            }

            set
            {
                this.SetValue(ContentForegroundProperty, value);
            }
        }

        private UIElement panel;
        private FrameworkElement firstContentContainer;

        private Storyboard globalContentAnimation = new Storyboard();
        private DoubleAnimation moveUpGlobalAnimation;

        private Storyboard localContentAnimation = new Storyboard();
        private DoubleAnimation moveUpLocalAnimation;

        private int? lastPictureIndex = 0;

        //private Image firstImage = null;
        //private Image secondImage = null;
        private ClipImage firstImage = null;
        private ClipImage secondImage = null;
        private TextBlock firstTitleName = null;
        private TextBlock firstContent = null;
        private TextBlock secondTitleName = null;
        private TextBlock secondContent = null;


        /// <summary>
        /// Initializes a new instance of the RadPictureRotatorHubTile class.
        /// </summary>
        public TitleImageRotatorHubTile()
        {
            this.DefaultStyleKey = typeof(TitleImageRotatorHubTile);

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

            //firstImage = this.GetTemplateChild("FirstHeader") as Image;
            //secondImage = this.GetTemplateChild("SecondHeader") as Image;
            firstImage = this.GetTemplateChild("FirstHeader") as ClipImage;
            secondImage = this.GetTemplateChild("SecondHeader") as ClipImage;
            firstTitleName = this.GetTemplateChild("firstTitleName") as TextBlock;
            firstContent = this.GetTemplateChild("firstContent") as TextBlock;
            secondTitleName = this.GetTemplateChild("secondTitleName") as TextBlock;
            secondContent = this.GetTemplateChild("secondContent") as TextBlock;

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

            //Image image = new Image() { Stretch = Stretch.UniformToFill };
            //image.Source = await this.GetImageSource();
            //this.FirstImage = image;

            //image = new Image() { Stretch = Stretch.UniformToFill };
            //image.Source = await this.GetImageSource();
            //this.SecondImage = image;

            CreateImageBingUI();
        }

        private async void CreateImageBingUI()
        {
            TitleImage titleImage = new TitleImage();
            titleImage = await this.GetSource();
            if (titleImage != null)
            {
                //this.FirstImage = titleImage.ImageSource;
                //this.firstTitleName.Text = this.FirstTitle = titleImage.Title;
                //this.firstContent.Text = this.FirstContent = titleImage.Content;
                firstImage.Source = titleImage.ImageSource;
                this.firstTitleName.Text = titleImage.Title;
                this.firstContent.Text = titleImage.Content;

                //this.SecondImage = titleImage.ImageSource;
                //this.secondTitleName.Text = this.SecondTitle = titleImage.Title;
                //this.secondContent.Text = this.SecondContent = titleImage.Content;
                secondImage.Source = titleImage.ImageSource;
                this.secondTitleName.Text = titleImage.Title;
                this.secondContent.Text = titleImage.Content;
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

            //if (string.IsNullOrEmpty(this.FirstContent) || this.FirstImage == null || string.IsNullOrEmpty(this.FirstTitle))
            //{
            //    return;
            //}

            if (string.IsNullOrEmpty(this.firstContent.Text) || this.firstImage.Source == null || string.IsNullOrEmpty(this.firstTitleName.Text))
            {
                return;
            }

            this.moveUpGlobalAnimation.To = -this.Height;
            this.globalContentAnimation.Begin();
        }

        private async void OnCurrentAnimationCompleted(object sender, object e)
        {
            //this.firstContentContainer.LayoutUpdated += this.OnFirstContentLayoutUpdated;

            Canvas.SetTop(this.panel, 0);
            Canvas.SetTop(this.firstContentContainer, 0);

            /*firstImage.Source = this.FirstImage;
            secondImage.Source = this.SecondImage;
            
            ImageSource tmp = secondImage.Source;

            secondImage.Source = null;
            firstImage.Source = tmp;
            this.firstTitleName.Text = this.FirstTitle = this.secondTitleName.Text = this.SecondTitle;
            this.firstContent.Text = this.FirstContent = this.secondContent.Text = this.SecondContent;

            var titleImage = await this.GetSource();
            secondImage.Source = titleImage.ImageSource;
            this.secondTitleName.Text = this.SecondTitle = titleImage.Title;
            this.secondContent.Text = this.SecondContent = titleImage.Content;*/

            ImageSource tmp = secondImage.Source;
            secondImage.Source = null;
            firstImage.Source = tmp;
            this.firstTitleName.Text = this.secondTitleName.Text;
            this.firstContent.Text = this.secondContent.Text;

            var titleImage = await this.GetSource();
            secondImage.Source = titleImage.ImageSource;
            this.secondTitleName.Text = titleImage.Title;
            this.secondContent.Text = titleImage.Content;
        }

        private void OnFirstContentLayoutUpdated(object sender, object e)
        {
            this.firstContentContainer.LayoutUpdated -= this.OnFirstContentLayoutUpdated;

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
