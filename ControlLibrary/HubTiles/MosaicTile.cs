using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary
{
    /// <summary>
    /// A small rectangle that has two sides and can flip between the two with a swivel animation.
    /// </summary>
    public class MosaicTile : MatControl
    {
        /// <summary>
        /// Identifies the FrontContent dependency property.
        /// </summary>
        public static readonly DependencyProperty FrontContentProperty =
            DependencyProperty.Register("FrontContent", typeof(object), typeof(MosaicTile), new PropertyMetadata(null, OnFrontContentChanged));

        /// <summary>
        /// Identifies the FrontContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(MosaicTile), new PropertyMetadata(null, OnBackContentChanged));

        /// <summary>
        /// Identifies the FrontContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty FrontContentTemplateProperty =
            DependencyProperty.Register("FrontContentTemplate", typeof(DataTemplate), typeof(MosaicTile), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the BackContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BackContentTemplateProperty =
            DependencyProperty.Register("BackContentTemplate", typeof(DataTemplate), typeof(MosaicTile), new PropertyMetadata(null));

        private bool isInFrontState;

        /// <summary>
        /// Initializes a new instance of the MosaicTile class.
        /// </summary>
        public MosaicTile()
        {
            this.DefaultStyleKey = typeof(MosaicTile);
        }

        /// <summary>
        /// Gets or sets the content on the front side of the flip tile.
        /// </summary>
        public object FrontContent
        {
            get
            {
                return this.GetValue(MosaicTile.FrontContentProperty);
            }

            set
            {
                this.SetValue(MosaicTile.FrontContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the content on the back side of the flip tile.
        /// </summary>
        public object BackContent
        {
            get
            {
                return this.GetValue(MosaicTile.BackContentProperty);
            }

            set
            {
                this.SetValue(MosaicTile.BackContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the front content template.
        /// </summary>
        public DataTemplate FrontContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(MosaicTile.FrontContentTemplateProperty);
            }

            set
            {
                this.SetValue(MosaicTile.FrontContentTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the back content template.
        /// </summary>
        public DataTemplate BackContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(MosaicTile.BackContentTemplateProperty);
            }

            set
            {
                this.SetValue(MosaicTile.BackContentTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets a value that determines if the flip tile is in its front state.
        /// </summary>
        public bool IsInFrontState
        {
            get
            {
                return this.isInFrontState;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.GoToBackState(false);
        }

        /// <summary>
        /// Flips the tile to its other side.
        /// </summary>
        public void Flip()
        {
            if (this.isInFrontState)
            {
                this.GoToBackState(true);
            }
            else
            {
                this.GoToFrontState(true);
            }
        }

        /// <summary>
        /// A virtual callback that is invoked when the FrontContent property changes.
        /// </summary>
        /// <param name="newContent">The new FrontContent.</param>
        /// <param name="oldContent">The old FrontContent.</param>
        protected virtual void OnFrontContentChanged(object newContent, object oldContent)
        {
            this.GoToFrontState(true);
        }

        /// <summary>
        /// A virtual callback that is invoked when the BackContent property changes.
        /// </summary>
        /// <param name="newContent">The new BackContent.</param>
        /// <param name="oldContent">The old BackContent.</param>
        protected virtual void OnBackContentChanged(object newContent, object oldContent)
        {
            this.GoToBackState(true);
        }

        private static void OnFrontContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MosaicTile tile = sender as MosaicTile;
            tile.OnFrontContentChanged(args.NewValue, args.OldValue);
        }

        private static void OnBackContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            MosaicTile tile = sender as MosaicTile;
            tile.OnBackContentChanged(args.NewValue, args.OldValue);
        }

        private object CreateRandomlyDesaturatedBackColor()
        {
            //return new Border() { Background = Application.Current.Resources["PhoneAccentBrush"] as Brush };
            return new Border() { Background = new SolidColorBrush(Colors.Red)};
        }

        /*
        private HsvColor RgbToHsv(Color rgbColor)
        {
            int rgbR = rgbColor.R, rgbG = rgbColor.G, rgbB = rgbColor.B;
            int h = 0, s = 0, v = 0;
            int rgbMax = Math.Max(Math.Max(rgbColor.R, rgbColor.G), rgbColor.B);

            // Compute Value
            v = rgbMax;
            if (v == 0)
            {
                s = 0;
                h = 0;
                return new HsvColor() { H = h, S = s, V = v };
            }

            rgbR /= rgbMax;
            rgbG /= rgbMax;
            rgbB /= rgbMax;

            rgbMax = Math.Max(Math.Max(rgbColor.R, rgbColor.G), rgbColor.B);
            int rgbMin = Math.Min(Math.Min(rgbColor.R, rgbColor.G), rgbColor.B);

            // Compute Saturation
            s = rgbMax - rgbMin;

            if (s == 0)
            {
                h = 0;
                return new HsvColor() { H = h, S = s, V = v };
            }

            // Compute Hue
            rgbR = (rgbColor.R - rgbMin) / (rgbMax - rgbMin);
            rgbG = (rgbColor.G - rgbMin) / (rgbMax - rgbMin);
            rgbB = (rgbColor.B - rgbMin) / (rgbMax - rgbMin);

            rgbMax = Math.Max(Math.Max(rgbColor.R, rgbColor.G), rgbColor.B);
            rgbMin = Math.Min(Math.Min(rgbColor.R, rgbColor.G), rgbColor.B);

            if (rgbColor.R == rgbMax)
            {
                h = 60 * (rgbColor.G - rgbColor.B);
                if (h < 0)
                {
                    h += 360;
                }
            }
            else if (rgbColor.G == rgbMax)
            {
                h = 120 + 60 * (rgbColor.B - rgbColor.R);
            }
            else
            {
                h = 240 + 60 * (rgbColor.R - rgbColor.G);
            }

            return new HsvColor() { H = h, S = s, V = v };
        }
        */

        private Color HsvToRgb(HsvColor hsvColor)
        {
            Color rgbColor = Colors.Black;

            return rgbColor;
        }

        private Color HsvToRgb()
        {
            Color rgbColor = Colors.Black;

            return rgbColor;
        }
        
        private void GoToFrontState(bool animate)
        {
            //this.Dispatcher.InvokeAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, (e, s) =>
            //            {
            //                VisualStateManager.GoToState(this, "Front", animate);
            //            }, this, null);
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            VisualStateManager.GoToState(this, "Front", animate);
                        });
            this.isInFrontState = true;
        }

        private void GoToBackState(bool animate)
        {
            //this.Dispatcher.InvokeAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, (e, s) =>
            //{
            //    VisualStateManager.GoToState(this, "Back", animate);
            //}, this, null);
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                VisualStateManager.GoToState(this, "Back", animate);
            });
            this.isInFrontState = false;
        }
    }
}
