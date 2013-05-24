using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary
{
    public class CircleItemsPanel : Panel
    {
        public Double Radius
        {
            get { return (Double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(Double), typeof(CircleItemsPanel), new PropertyMetadata(200d, PropertyChangedCallback));


        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            CircleItemsPanel radialPanel = (CircleItemsPanel)dependencyObject;
            radialPanel.InvalidateArrange();
        }

        // Overrides
        protected override Size MeasureOverride(Size availableSize)
        {
            Size s = base.MeasureOverride(availableSize);
            foreach (UIElement element in this.Children)
                element.Measure(availableSize);
            return s;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // Clip to ensure items dont override container
            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };
            // Size and position the child elements
            int i = 0;
            double degreesOffset = 360.0 / this.Children.Count;
            foreach (FrameworkElement element in this.Children)
            {
                double centerX = element.DesiredSize.Width / 2.0;
                double centerY = element.DesiredSize.Height / 2.0;

                // calculate the good angle
                double degreesAngle = degreesOffset * i++;

                RotateTransform transform = new RotateTransform();
                transform.CenterX = centerX;
                transform.CenterY = centerY;
                // must be degrees. It's a shame it's not in radian :)
                transform.Angle = degreesAngle;
                element.RenderTransform = transform;

                // calculate radian angle
                var radianAngle = (Math.PI * degreesAngle) / 180.0;

                // get x and y
                double x = this.Radius * Math.Cos(radianAngle);
                double y = this.Radius * Math.Sin(radianAngle);

                // get real X and Y (because 0;0 is on top left and not middle of the circle)
                var rectX = x + (finalSize.Width / 2.0) - centerX;
                var rectY = y + (finalSize.Height / 2.0) - centerY;

                // arrange element
                element.Arrange(new Rect(rectX, rectY, element.DesiredSize.Width, element.DesiredSize.Height));
            }
            return finalSize;
        }
    }

    public enum ItemOrientationOptions
    {
        Normal, Rotated
    }
}
