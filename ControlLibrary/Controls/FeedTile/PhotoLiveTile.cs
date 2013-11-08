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

        public event Action<object, RoutedEventArgs> ImageOpened = null;
        public event Action AnimationCompleted = null;

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
            if (this.photoLiveTileBase != null)
            {
                photoLiveTileBase.ImageOpened -= photoLiveTileBase_ImageOpened;
                photoLiveTileBase.ImageOpened += photoLiveTileBase_ImageOpened;
                photoLiveTileBase.AnimationCompleted -= photoLiveTileBase_AnimationCompleted;
                photoLiveTileBase.AnimationCompleted += photoLiveTileBase_AnimationCompleted;
                if (this.Source != null && this.Source != string.Empty)
                {
                    if (!string.IsNullOrWhiteSpace(this.Source))
                    {
                        this.photoLiveTileBase.Source = this.Source;
                    }
                }
            }
        }

        private void photoLiveTileBase_AnimationCompleted()
        {
            if (AnimationCompleted != null)
            {
                AnimationCompleted();
            }
        }

        private void photoLiveTileBase_ImageOpened(object arg1, RoutedEventArgs arg2)
        {
            if (ImageOpened != null)
            {
                ImageOpened(arg1, arg2);
            }
        }
    }
}
