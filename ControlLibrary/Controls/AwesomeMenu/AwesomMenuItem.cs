using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary
{
    public class AwesomMenuItem : Grid
    {
        #region Public Property
        public Point StratPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point NearPoint { get; set; }
        public Point FarPoint { get; set; }
        public CompositeTransform ItemTransfrom { get; set; } 
        #endregion

        #region Private Fileds
        private Image _contentImage;
        private BitmapImage bi = null;
        private string _backgroundUri;
        private string _contentImageUri;
        private Uri baseUri = new Uri("ms-appx://");
        private string reg = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?";
        #endregion

        #region Clik Event Handle
        public delegate void TouchItemEnd(AwesomMenuItem item);
        public event TouchItemEnd ClickMenuItem; 
        #endregion

        public AwesomMenuItem(string imgUri, string backgrounUrl)
        {
            this._contentImageUri = imgUri;
            this._backgroundUri = backgrounUrl;
            IntiLayout(_contentImageUri);
        }

        #region Private Methods
        private void IntiLayout(string strImageUri)
        {
            SetImage(strImageUri);
            this.Width = 50;
            this.Height = 50;
            this.Children.Add(_contentImage);
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            if (regex.IsMatch(_backgroundUri))
            {
                bi = new BitmapImage(new System.Uri(_backgroundUri, System.UriKind.RelativeOrAbsolute));
            }
            else
            {
                bi = new BitmapImage(new System.Uri(this.baseUri, _backgroundUri));
            }
            this.Background = new ImageBrush { ImageSource = bi };
            this.Tapped -= AwesomMenuItem_Tapped;
            this.Tapped += AwesomMenuItem_Tapped;

            ItemTransfrom = new CompositeTransform();
            this.RenderTransform = ItemTransfrom;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        void AwesomMenuItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ClickMenuItem != null)
            {
                ClickMenuItem(this);
            }
            e.Handled = true;
        }

        private static Rect ScaleRect(Rect rect, float n)
        {
            return new Rect((rect.Width - rect.Width * n) / 2, (rect.Height - rect.Height * n) / 2, rect.Width * n, rect.Height * n);
        }

        private void SetImage(string imgUri)
        {
            if (bi != null)
            {
                bi = null;
            }
            
            Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
            if (regex.IsMatch(imgUri))
            {
                bi = new BitmapImage(new System.Uri(imgUri, System.UriKind.RelativeOrAbsolute));
            }
            else
            {
                bi = new BitmapImage(new System.Uri(this.baseUri, imgUri));
            }

            if (_contentImage != null)
                _contentImage.Source = null;
            else
                _contentImage = new Image();
            _contentImage.Stretch = Stretch.None;
            _contentImage.Source = bi;
        }
        #endregion

    }
}
