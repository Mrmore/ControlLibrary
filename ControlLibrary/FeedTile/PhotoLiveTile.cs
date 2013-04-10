using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControlLibrary
{
    public class PhotoLiveTile : Control
    {
        private PhotoLiveTileBase photoLiveTileBase = null;
        public int CurrentPhotoIndex
        {
            get
            {
                if (photoLiveTileBase != null)
                {
                    return photoLiveTileBase.CurrentPhotoIndex;
                }

                return -1;
            }
        }

        public String Source
        {
            get { return (String)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(String), typeof(PhotoLiveTile), new PropertyMetadata(null, new PropertyChangedCallback(onSourcePropertyChanged)));

        private static void onSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var photoLiveTile = sender as PhotoLiveTile;
            if (photoLiveTile != null && photoLiveTile.photoLiveTileBase != null)
            {
                var str = e.NewValue as String;
                if (!string.IsNullOrWhiteSpace(str))
                {
                    photoLiveTile.photoLiveTileBase.Source = photoLiveTile.Source;
                }
            }
        }

        public PhotoLiveTile()
        {
            this.DefaultStyleKey = typeof(PhotoLiveTile);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            photoLiveTileBase = this.GetTemplateChild("photoLiveTileBase") as PhotoLiveTileBase;
            if (this.photoLiveTileBase != null && this.Source != null && this.Source != string.Empty)
            {
                if (!string.IsNullOrWhiteSpace(this.Source))
                {
                    this.photoLiveTileBase.Source = this.Source;
                }
            }
        }
    }
}
