using ControlLibrary.Tools.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.Behaviors
{
    public class ClipToBoundsBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += AssociatedObjectSizeChanged;
            AssociatedObject.Loaded += AssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SizeChanged -= AssociatedObjectSizeChanged;
            AssociatedObject.Loaded -= AssociatedObjectLoaded;
            base.OnDetaching();
        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            SetClip();
        }

        void AssociatedObjectSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetClip();
        }

        private void SetClip()
        {
            AssociatedObject.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0,
                    AssociatedObject.ActualWidth, AssociatedObject.ActualHeight)
            };
        }
    }
}
