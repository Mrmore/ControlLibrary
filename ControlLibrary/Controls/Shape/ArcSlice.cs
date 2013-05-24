using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ControlLibrary
{
    /// <summary>
    /// A Path that represents a ring slice with a given
    /// (outer) Radius,
    /// Width,
    /// Height,
    /// StartAngle,
    /// EndAngle and
    /// Center.
    /// </summary>
    public class ArcSlice : Path
    {
        private bool _isUpdating;

        #region StartAngle
        /// <summary>
        /// The start angle property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(ArcSlice),
                new PropertyMetadata(
                    0d,
                    OnStartAngleChanged));

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>
        /// The start angle.
        /// </value>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        private static void OnStartAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ArcSlice)sender;
            var oldStartAngle = (double)e.OldValue;
            var newStartAngle = (double)e.NewValue;
            target.OnStartAngleChanged(oldStartAngle, newStartAngle);
        }

        private void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
        {
            UpdatePath();
        }
        #endregion

        #region EndAngle
        /// <summary>
        /// The end angle property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(
                "EndAngle",
                typeof(double),
                typeof(ArcSlice),
                new PropertyMetadata(
                    0d,
                    OnEndAngleChanged));

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        /// <value>
        /// The end angle.
        /// </value>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ArcSlice)sender;
            var oldEndAngle = (double)e.OldValue;
            var newEndAngle = (double)e.NewValue;
            target.OnEndAngleChanged(oldEndAngle, newEndAngle);
        }

        private void OnEndAngleChanged(double oldEndAngle, double newEndAngle)
        {
            UpdatePath();
        }
        #endregion

        #region Center
        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(
                "Center",
                typeof(Point?),
                typeof(ArcSlice),
                new PropertyMetadata(null, OnCenterChanged));

        /// <summary>
        /// Gets or sets the Center property. This dependency property 
        /// indicates the center point.
        /// Center point is calculated based on Radius and StrokeThickness if not specified.    
        /// </summary>
        public Point? Center
        {
            get { return (Point?)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Center property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCenterChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ArcSlice)d;
            Point? oldCenter = (Point?)e.OldValue;
            Point? newCenter = target.Center;
            target.OnCenterChanged(oldCenter, newCenter);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Center property.
        /// </summary>
        /// <param name="oldCenter">The old Center value</param>
        /// <param name="newCenter">The new Center value</param>
        private void OnCenterChanged(
            Point? oldCenter, Point? newCenter)
        {
            UpdatePath();
        }
        #endregion

        #region ArcThickness
        /// <summary>
        /// Center Dependency Property
        /// </summary>
        public static readonly DependencyProperty ArcThicknessProperty =
            DependencyProperty.Register(
                "ArcThickness",
                typeof(double),
                typeof(ArcSlice),
                new PropertyMetadata(0d, OnArcThicknessChanged));

        /// <summary>
        /// Gets or sets the Center property. This dependency property 
        /// indicates the center point.
        /// Center point is calculated based on Radius and StrokeThickness if not specified.    
        /// </summary>
        public double ArcThickness
        {
            get { return (double)GetValue(ArcThicknessProperty); }
            set { SetValue(ArcThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Center property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnArcThicknessChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ArcSlice)d;
            double oldArcThickness = (double)e.OldValue;
            double newArcThickness = target.ArcThickness;
            target.OnArcThicknessChanged(oldArcThickness, newArcThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Center property.
        /// </summary>
        /// <param name="oldCenter">The old Center value</param>
        /// <param name="newCenter">The new Center value</param>
        private void OnArcThicknessChanged(
            double oldCenter, double newCenter)
        {
            UpdatePath();
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ArcSlice" /> class.
        /// </summary>
        public ArcSlice()
        {
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdatePath();
        }

        /// <summary>
        /// Suspends path updates until EndUpdate is called;
        /// </summary>
        public void BeginUpdate()
        {
            _isUpdating = true;
        }

        /// <summary>
        /// Resumes immediate path updates every time a component property value changes. Updates the path.
        /// </summary>
        public void EndUpdate()
        {
            _isUpdating = false;
            UpdatePath();
        }

        private void UpdatePath()
        {
            if (_isUpdating)
            {
                return;
            }

            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();
            pathFigure.IsClosed = true;

            var center =
                this.Center ??
                new Point(
                    (this.Width + this.StrokeThickness) / 2,
                    (this.Height + this.StrokeThickness) / 2);

            // Starting Point
            pathFigure.StartPoint =
                new Point(
                    center.X + Math.Sin(StartAngle * Math.PI / 180) * (this.Width / 2 - this.ArcThickness),
                    center.Y - Math.Cos(StartAngle * Math.PI / 180) * (this.Height / 2 - this.ArcThickness));

            // Inner Arc
            var innerArcSegment = new ArcSegment();
            innerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            innerArcSegment.Point =
                new Point(
                    center.X + Math.Sin(EndAngle * Math.PI / 180) * (this.Width / 2 - this.ArcThickness),
                    center.Y - Math.Cos(EndAngle * Math.PI / 180) * (this.Height / 2 - this.ArcThickness));
            innerArcSegment.Size = new Size((this.Width / 2 - this.ArcThickness), (this.Height / 2 - this.ArcThickness));
            innerArcSegment.SweepDirection = SweepDirection.Clockwise;

            var lineSegment =
                new LineSegment
                {
                    Point = new Point(
                        center.X + Math.Sin(EndAngle * Math.PI / 180) * this.Width / 2,
                        center.Y - Math.Cos(EndAngle * Math.PI / 180) * this.Height / 2)
                };

            // Outer Arc
            var outerArcSegment = new ArcSegment();
            outerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            outerArcSegment.Point =
                new Point(
                        center.X + Math.Sin(StartAngle * Math.PI / 180) * this.Width / 2,
                        center.Y - Math.Cos(StartAngle * Math.PI / 180) * this.Height / 2);
            outerArcSegment.Size = new Size(this.Width / 2, this.Height / 2);
            outerArcSegment.SweepDirection = SweepDirection.Counterclockwise;

            pathFigure.Segments.Add(innerArcSegment);
            pathFigure.Segments.Add(lineSegment);
            pathFigure.Segments.Add(outerArcSegment);
            pathGeometry.Figures.Add(pathFigure);
            this.InvalidateArrange();
            this.Data = pathGeometry;
        }
    }
}
