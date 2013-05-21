using System.ComponentModel;
using System.Windows;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary
{
    /// <summary>
    /// Used in RadMosaicHubTile to display the small flipping images.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CustomMosaicTile : MosaicTile
    {
        private Canvas frontCanvas;
        private Canvas backCanvas;
        private Image frontImage;
        private Image backImage;
        private FrameCorner? frameCorner = null;

        public Brush MosaicTileBrush
        {
            get { return (Brush)GetValue(MosaicTileBrushProperty); }
            set { SetValue(MosaicTileBrushProperty, value); }
        }

        public static readonly DependencyProperty MosaicTileBrushProperty = DependencyProperty.Register("MosaicTileBrush", typeof(Brush), typeof(CustomMosaicTile), new PropertyMetadata(new SolidColorBrush(Colors.Transparent), new PropertyChangedCallback(onMosaicTileBrushPropertyChanged)));

        private static void onMosaicTileBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var customMosaicTile = sender as CustomMosaicTile;
            if (customMosaicTile != null)
            {

            }
        }

        /// <summary>
        /// Initializes a new instance of the CustomMosaicTile class.
        /// </summary>
        public CustomMosaicTile()
        {
            this.DefaultStyleKey = typeof(CustomMosaicTile);
            this.SizeChanged += this.OnSizeChanged;
        }
        
        /// <summary>
        /// Determines which corner of an image the tile is displaying if it is a part
        /// of a picture frame inside the mosaic hub tile.
        /// </summary>
        public FrameCorner? FrameCorner
        {
            get
            {
                return this.frameCorner;
            }

            set
            {
                FrameCorner? previousValue = this.frameCorner;
                this.frameCorner = value;

                this.OnFrameCornerChanged(value, previousValue);
            }
        }

        /// <summary>
        /// Returns true if the tile is currently displaying an image and false
        /// otherwise.
        /// </summary>
        public bool IsContentDisplayed
        {
            get
            {
                return (this.IsInFrontState && this.FrontContent != null) ||
                       (!this.IsInFrontState && this.BackContent != null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.frontCanvas = this.GetTemplateChild("PART_FrontCanvas") as Canvas;
            this.backCanvas = this.GetTemplateChild("PART_BackCanvas") as Canvas;

            this.frontImage = this.frontCanvas.Children[0] as Image;
            this.backImage = this.backCanvas.Children[0] as Image;
        }

        /// <summary>
        /// Flipes the tile and displays the provided image on the new side.
        /// </summary>
        /// <param name="newSource">The new image.</param>
        public void Flip(ImageSource newSource)
        {
            if (this.IsInFrontState)
            {
                if (newSource == this.BackContent)
                {
                    this.Flip();
                }
                else
                {
                    this.BackContent = newSource;
                }
            }
            else
            {
                if (newSource == this.FrontContent)
                {
                    this.Flip();
                }
                else
                {
                    this.FrontContent = newSource;
                }
            }
        }

        private void OnFrameCornerChanged(FrameCorner? newValue, FrameCorner? oldValue)
        {
            this.UpdateFrameCorner(newValue);
        }

        private void UpdateFrameCorner(FrameCorner? frameCorner)
        {
            double doubleWidth = this.ActualWidth * 2;
            double doubleHeight = this.ActualHeight * 2;
            Image imageToOffset = null;

            if (this.IsInFrontState)
            {
                if (!frameCorner.HasValue)
                {
                    this.backImage.Width = this.ActualWidth;
                    this.backImage.Height = this.ActualHeight;
                    Canvas.SetLeft(this.backImage, 0);
                    Canvas.SetTop(this.backImage, 0);
                    return;
                }

                this.backImage.Width = doubleWidth;
                this.backImage.Height = doubleHeight;
                imageToOffset = this.backImage;
            }
            else
            {
                if (!frameCorner.HasValue)
                {
                    this.frontImage.Width = this.ActualWidth;
                    this.frontImage.Height = this.ActualHeight;

                    Canvas.SetLeft(this.frontImage, 0);
                    Canvas.SetTop(this.frontImage, 0);
                    return;
                }

                this.frontImage.Width = doubleWidth;
                this.frontImage.Height = doubleHeight;
                imageToOffset = this.frontImage;
            }

            if (frameCorner.Value == ControlLibrary.FrameCorner.TopLeft)
            {
                Canvas.SetTop(imageToOffset, 0);
                Canvas.SetLeft(imageToOffset, 0);
            }

            if (frameCorner.Value == ControlLibrary.FrameCorner.TopRight)
            {
                Canvas.SetTop(imageToOffset, 0);
                Canvas.SetLeft(imageToOffset, -this.ActualWidth);
            }

            if (frameCorner.Value == ControlLibrary.FrameCorner.BottomLeft)
            {
                Canvas.SetTop(imageToOffset, -this.ActualHeight);
                Canvas.SetLeft(imageToOffset, 0);
            }

            if (frameCorner.Value == ControlLibrary.FrameCorner.BottomRight)
            {
                Canvas.SetTop(imageToOffset, -this.ActualHeight);
                Canvas.SetLeft(imageToOffset, -this.ActualWidth);
            }
        }

        private void OnSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            RectangleGeometry frontClip = new RectangleGeometry() { Rect = new Rect(new Point(), e.NewSize) };
            RectangleGeometry backClip = new RectangleGeometry() { Rect = new Rect(new Point(), e.NewSize) };

            this.frontCanvas.Clip = frontClip;
            this.backCanvas.Clip = backClip;

            this.UpdateFrameCorner(this.frameCorner);
        }
    }
}
