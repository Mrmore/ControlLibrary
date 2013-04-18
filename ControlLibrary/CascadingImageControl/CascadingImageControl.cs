using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using ControlLibrary.Extensions;
using System.Collections.Concurrent;
using Windows.Foundation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace ControlLibrary
{
    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    [TemplatePart(Name = LayoutViewboxName, Type = typeof(Viewbox))]
    public sealed class CascadingImageControl : Control
    {
        #region Private Variable
        private const string LayoutGridName = "PART_LayoutGrid";

        private const string LayoutViewboxName = "LayoutViewbox";

        private Grid _layoutGrid;

        private bool _isLoaded;

        private static readonly Random Random = new Random();

        private Viewbox viewbox = null;

        private double H, W = double.NaN;
        private double RH, RW = double.NaN;
        private SizeChangedEventHandler sizeChanged = null;
        private Size size;
        #endregion

        #region Columns
        /// <summary>
        /// Columns Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(int),
                typeof(CascadingImageControl),
                new PropertyMetadata(3, OnColumnsChanged));

        /// <summary>
        /// Gets or sets the Columns property. This dependency property 
        /// indicates the number of columns.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Columns property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnColumnsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            int oldColumns = (int)e.OldValue;
            int newColumns = target.Columns;
            target.OnColumnsChanged(oldColumns, newColumns);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Columns property.
        /// </summary>
        /// <param name="oldColumns">The old Columns value</param>
        /// <param name="newColumns">The new Columns value</param>
        private void OnColumnsChanged(
            int oldColumns, int newColumns)
        {
            Cascade();
        }
        #endregion

        #region Rows
        /// <summary>
        /// Rows Dependency Property
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                "Rows",
                typeof(int),
                typeof(CascadingImageControl),
                new PropertyMetadata(3, OnRowsChanged));

        /// <summary>
        /// Gets or sets the Rows property. This dependency property 
        /// indicates the number of rows.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Rows property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRowsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            int oldRows = (int)e.OldValue;
            int newRows = target.Rows;
            target.OnRowsChanged(oldRows, newRows);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Rows property.
        /// </summary>
        /// <param name="oldRows">The old Rows value</param>
        /// <param name="newRows">The new Rows value</param>
        private void OnRowsChanged(
            int oldRows, int newRows)
        {
            Cascade();
        }
        #endregion

        #region ImageSource
        /// <summary>
        /// ImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(
                "ImageSource",
                typeof(ImageSource),
                typeof(CascadingImageControl),
                new PropertyMetadata(null, OnImageSourceChanged));

        /// <summary>
        /// Gets or sets the ImageSource property. This dependency property 
        /// indicates the image to display in a cascade.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            ImageSource oldImageSource = (ImageSource)e.OldValue;
            ImageSource newImageSource = target.ImageSource;
            target.OnImageSourceChanged(oldImageSource, newImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ImageSource property.
        /// </summary>
        /// <param name="oldImageSource">The old ImageSource value</param>
        /// <param name="newImageSource">The new ImageSource value</param>
        private void OnImageSourceChanged(
            ImageSource oldImageSource, ImageSource newImageSource)
        {
            Cascade();
        }
        #endregion

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(CascadingImageControl),
            new PropertyMetadata(Stretch.Uniform, new PropertyChangedCallback(OnStretchPropertyChanged)));

        private static void OnStretchPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var cascadingImageControl = sender as CascadingImageControl;
            if (cascadingImageControl != null && cascadingImageControl._layoutGrid != null)
            {
                cascadingImageControl.Cascade();
            }
        }
        #endregion

        #region ImageStretch
        private Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register("ImageStretch", typeof(Stretch), typeof(CascadingImageControl),
            new PropertyMetadata(Stretch.Fill, new PropertyChangedCallback(OnImageStretchPropertyChanged)));

        private static void OnImageStretchPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var cascadingImageControl = sender as CascadingImageControl;
            if (cascadingImageControl != null && cascadingImageControl.viewbox != null)
            {
                cascadingImageControl.viewbox.Stretch = cascadingImageControl.ImageStretch;
                cascadingImageControl.Cascade();
            }
        }
        #endregion

        #region ColumnDelay
        /// <summary>
        /// ColumnDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColumnDelayProperty =
            DependencyProperty.Register(
                "ColumnDelay",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.025), OnColumnDelayChanged));

        /// <summary>
        /// Gets or sets the ColumnDelay property. This dependency property 
        /// indicates the delay of the cascade for each column.
        /// </summary>
        public TimeSpan ColumnDelay
        {
            get { return (TimeSpan)GetValue(ColumnDelayProperty); }
            set { SetValue(ColumnDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ColumnDelay property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnColumnDelayChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldColumnDelay = (TimeSpan)e.OldValue;
            TimeSpan newColumnDelay = target.ColumnDelay;
            target.OnColumnDelayChanged(oldColumnDelay, newColumnDelay);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ColumnDelay property.
        /// </summary>
        /// <param name="oldColumnDelay">The old ColumnDelay value</param>
        /// <param name="newColumnDelay">The new ColumnDelay value</param>
        private void OnColumnDelayChanged(
            TimeSpan oldColumnDelay, TimeSpan newColumnDelay)
        {
            Cascade();
        }
        #endregion

        #region RowDelay
        /// <summary>
        /// RowDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty RowDelayProperty =
            DependencyProperty.Register(
                "RowDelay",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.05), OnRowDelayChanged));

        /// <summary>
        /// Gets or sets the RowDelay property. This dependency property 
        /// indicates the delay of the cascade for each row.
        /// </summary>
        public TimeSpan RowDelay
        {
            get { return (TimeSpan)GetValue(RowDelayProperty); }
            set { SetValue(RowDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RowDelay property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRowDelayChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldRowDelay = (TimeSpan)e.OldValue;
            TimeSpan newRowDelay = target.RowDelay;
            target.OnRowDelayChanged(oldRowDelay, newRowDelay);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RowDelay property.
        /// </summary>
        /// <param name="oldRowDelay">The old RowDelay value</param>
        /// <param name="newRowDelay">The new RowDelay value</param>
        private void OnRowDelayChanged(
            TimeSpan oldRowDelay, TimeSpan newRowDelay)
        {
            Cascade();
        }
        #endregion

        #region TileDuration
        /// <summary>
        /// TileDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty TileDurationProperty =
            DependencyProperty.Register(
                "TileDuration",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(2.0), OnTileDurationChanged));

        /// <summary>
        /// Gets or sets the TileDuration property. This dependency property 
        /// indicates the duration of an item's animation.
        /// </summary>
        public TimeSpan TileDuration
        {
            get { return (TimeSpan)GetValue(TileDurationProperty); }
            set { SetValue(TileDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TileDuration property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTileDurationChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldTileDuration = (TimeSpan)e.OldValue;
            TimeSpan newTileDuration = target.TileDuration;
            target.OnTileDurationChanged(oldTileDuration, newTileDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the TileDuration property.
        /// </summary>
        /// <param name="oldTileDuration">The old TileDuration value</param>
        /// <param name="newTileDuration">The new TileDuration value</param>
        private void OnTileDurationChanged(
            TimeSpan oldTileDuration, TimeSpan newTileDuration)
        {
        }
        #endregion

        #region CascadeDirection
        /// <summary>
        /// CascadeDirection Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeDirectionProperty =
            DependencyProperty.Register(
                "CascadeDirection",
                typeof(CascadeDirection),
                typeof(CascadingImageControl),
                new PropertyMetadata(CascadeDirection.Shuffle, new PropertyChangedCallback(OnCascadeDirectionChanged)));

        /// <summary>
        /// Gets or sets the CascadeDirection property. This dependency property 
        /// indicates the direction of the cascade animation.
        /// </summary>
        public CascadeDirection CascadeDirection
        {
            get { return (CascadeDirection)GetValue(CascadeDirectionProperty); }
            set { SetValue(CascadeDirectionProperty, value); }
        }

        private static void OnCascadeDirectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var cascadingImageControl = sender as CascadingImageControl;
            if (cascadingImageControl != null && cascadingImageControl._layoutGrid != null)
            {
                cascadingImageControl.Cascade();
            }
        }
        #endregion

        #region CascadeInEasingFunction
        /// <summary>
        /// CascadeInEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInEasingFunctionProperty =
            DependencyProperty.Register(
                "CascadeInEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingImageControl),
                new PropertyMetadata(new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 3, Springiness = 0.0 }));

        /// <summary>
        /// Gets or sets the CascadeInEasingFunction property. This dependency property 
        /// indicates the easing function to use in the cascade in transition.
        /// </summary>
        public EasingFunctionBase CascadeInEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(CascadeInEasingFunctionProperty); }
            set { SetValue(CascadeInEasingFunctionProperty, value); }
        }
        #endregion

        #region CascadeSequence
        /// <summary>
        /// CascadeSequence Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeSequenceProperty =
            DependencyProperty.Register(
                "CascadeSequence",
                typeof(CascadeSequence),
                typeof(CascadingImageControl),
                new PropertyMetadata(CascadeSequence.EqualDuration, new PropertyChangedCallback(OnCascadeSequencePropertyChanged)));

        /// <summary>
        /// Gets or sets the CascadeSequence property. This dependency property 
        /// indicates how cascade animations are sequenced.
        /// </summary>
        public CascadeSequence CascadeSequence
        {
            get { return (CascadeSequence)GetValue(CascadeSequenceProperty); }
            set { SetValue(CascadeSequenceProperty, value); }
        }

        private static void OnCascadeSequencePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var cascadingImageControl = sender as CascadingImageControl;
            if (cascadingImageControl != null && cascadingImageControl._layoutGrid != null)
            {
                cascadingImageControl.Cascade();
            }
        }
        #endregion

        #region IsClip
        public static readonly DependencyProperty IsClipProperty =
            DependencyProperty.Register(
                "IsClip",
                typeof(bool),
                typeof(CascadingImageControl),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsClipPropertyChanged)));

        public bool IsClip
        {
            get { return (bool)GetValue(IsClipProperty); }
            set { SetValue(IsClipProperty, value); }
        }

         private static void OnIsClipPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var cascadingImageControl = sender as CascadingImageControl;
            if (cascadingImageControl != null && cascadingImageControl._layoutGrid != null)
            {
                if (cascadingImageControl.IsClip)
                {
                    cascadingImageControl.AddMask();
                }
                else
                {
                    cascadingImageControl._layoutGrid.Clip = null;
                }
            }
        }
        
        #endregion

        public CascadingImageControl()
        {
            this.DefaultStyleKey = typeof(CascadingImageControl);
            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _layoutGrid = GetTemplateChild(LayoutGridName) as Grid;

            //viewbox = GetTemplateChild(LayoutViewboxName) as Viewbox;

            if (_layoutGrid == null)
            {
                Debug.WriteLine("CascadingImageControl requires a Grid called PART_LayoutGrid in its template.");
            }

            this.SizeChanged -= CascadingImageControl_SizeChanged;
            this.SizeChanged += CascadingImageControl_SizeChanged;
            Cascade();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            Cascade();
        }

        public void Cascade()
        {
            RH = RW = double.NaN;
            if (!_isLoaded ||
                _layoutGrid == null)
            {
                return;
            }

            if (Rows < 1)
                Rows = 1;
            if (Columns < 1)
                Columns = 1;

            _layoutGrid.Children.Clear();
            _layoutGrid.RowDefinitions.Clear();
            _layoutGrid.ColumnDefinitions.Clear();

            for (int row = 0; row < Rows; row++)
            {
                _layoutGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int column = 0; column < Columns; column++)
            {
                _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var sb = new Storyboard();

            var totalDurationInSeconds = RowDelay.TotalSeconds * (Rows - 1) +
                                         ColumnDelay.TotalSeconds * (Columns - 1) +
                                         TileDuration.TotalSeconds;

            CascadeDirection direction = this.CascadeDirection;

            if (direction == CascadeDirection.Random)
            {
                direction = (CascadeDirection)Random.Next((int)CascadeDirection.Random);
            }

            int startColumn;
            int exclusiveEndColumn;
            int columnIncrement;

            int startRow;
            int exclusiveEndRow;
            int rowIncrement;

            switch (direction)
            {
                case CascadeDirection.Shuffle:
                case CascadeDirection.TopLeft:
                    startColumn = 0;
                    exclusiveEndColumn = Columns;
                    columnIncrement = 1;
                    startRow = 0;
                    exclusiveEndRow = Rows;
                    rowIncrement = 1;
                    break;
                case CascadeDirection.TopRight:
                    startColumn = Columns - 1;
                    exclusiveEndColumn = -1;
                    columnIncrement = -1;
                    startRow = 0;
                    exclusiveEndRow = Rows;
                    rowIncrement = 1;
                    break;
                case CascadeDirection.BottomRight:
                    startColumn = Columns - 1;
                    exclusiveEndColumn = -1;
                    columnIncrement = -1;
                    startRow = Rows - 1;
                    exclusiveEndRow = -1;
                    rowIncrement = -1;
                    break;
                case CascadeDirection.BottomLeft:
                    startColumn = 0;
                    exclusiveEndColumn = Columns;
                    columnIncrement = 1;
                    startRow = Rows - 1;
                    exclusiveEndRow = -1;
                    rowIncrement = -1;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            List<Tuple<int, int>> rectCoords = new List<Tuple<int, int>>(Rows * Columns);
            List<Rectangle> rects = new List<Rectangle>(Rows * Columns);
            //List<PlaneProjection> projs = new List<PlaneProjection>(Rows * Columns);
            //List<CompositeTransform> ct = new List<CompositeTransform>(Rows * Columns);

            //**********************开始的位置
            /*
            List<double> translateXInfo = new List<double>();
            List<double> translateYInfo = new List<double>();
            */

            for (int row = startRow; row != exclusiveEndRow; row = row + rowIncrement)
                for (int column = startColumn; column != exclusiveEndColumn; column = column + columnIncrement)
                {
                    var rect = new Rectangle();
                    rects.Add(rect);

                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, column);
                    rectCoords.Add(new Tuple<int, int>(column, row));

                    var brush = new ImageBrush();
                    brush.ImageSource = this.ImageSource;
                    brush.Stretch = this.Stretch;
                    rect.Fill = brush;

                    //设置图片的笔刷位置
                    var transform = new CompositeTransform();
                    transform.TranslateX = -column;
                    transform.ScaleX = Columns;
                    transform.TranslateY = -row;
                    transform.ScaleY = Rows;
                    brush.RelativeTransform = transform;

                    //设置填充图片笔刷矩形的透视投影
                    var projection = new PlaneProjection();
                    projection.CenterOfRotationY = 0;
                    rect.Projection = projection;
                    //projs.Add(projection);

                    //设置填充图片笔刷矩形的呈现位置
                    var rectTransform = new CompositeTransform();
                    //rectTransform.CenterX = rectTransform.CenterY = 0.5;
                    rectTransform.TranslateX = column;
                    rectTransform.TranslateY = row;
                    rectTransform.Rotation = 0;
                    rect.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
                    rect.RenderTransform = rectTransform;
                    //ct.Add(rectTransform);

                    //if (!double.IsNaN(RH) && !double.IsNaN(RW))
                    //{
                    //    rect.Margin = new Thickness(column * 52, row * 50, 0, 0);
                    //}

                    _layoutGrid.Children.Add(rect);
                    
                }
            //GetRHAndRW();
            if (double.IsNaN(RH) || double.IsNaN(RW))
            {
                rects[0].SizeChanged -= sizeChanged;
                rects[0].SizeChanged += sizeChanged = (ss, ee) =>
                {
                    this.RH = ee.NewSize.Height;
                    this.RW = ee.NewSize.Width;

                    var indices = new List<int>(Rows * Columns);

                    for (int i = 0; i < Rows * Columns; i++)
                    {
                        indices.Add(i);
                        
                        var transform = rects[i].RenderTransform as CompositeTransform;
                        transform.TranslateX = transform.TranslateX * RW;
                        transform.TranslateY = transform.TranslateY * RH;

                        //*****************************恶心点，随便弄个起始位置，到时候可以根据行列信息或者奇偶信息来设置起始位置
                        //transform.TranslateX = i * RW;
                        //transform.TranslateY = i * RH;

                        /*
                        if (i < Rows * Columns / 2)
                        {
                            transform.TranslateX = -RW;
                            transform.TranslateY = -RH;
                        }
                        else
                        {
                            transform.TranslateX = RW;
                            transform.TranslateY = RH;
                        }

                        translateXInfo.Add(transform.TranslateX);
                        translateYInfo.Add(transform.TranslateY);
                        System.Diagnostics.Debug.WriteLine("TX = " + transform.TranslateX + " TY = " + transform.TranslateY + "\n");
                        */
                    }

                    if (direction == CascadeDirection.Shuffle)
                    {
                        indices = indices.Shuffle();
                    }

                    for (int ii = 0; ii < indices.Count; ii++)
                    {
                        var i = indices[ii];
                        //var projection = projs[i];
                        var projection = rects[i].Projection;                     
                        var rect = rects[i];
                        var column = rectCoords[ii].Item1;
                        var row = rectCoords[ii].Item2;
                        //*******************拿到当前的transform
                        var transfrom = rect.RenderTransform as CompositeTransform;
                        //Debug.WriteLine("i: {0}, p: {1}, rect: {2}, c: {3}, r: {4}", i, projection.GetHashCode(), rect.GetHashCode(), column, row);
                        var rotationAnimation = new DoubleAnimationUsingKeyFrames();
                        Storyboard.SetTarget(rotationAnimation, projection);
                        Storyboard.SetTargetProperty(rotationAnimation, "RotationX");

                        var endKeyTime =
                            this.CascadeSequence == CascadeSequence.EndTogether
                                ? TimeSpan.FromSeconds(totalDurationInSeconds)
                                : TimeSpan.FromSeconds(
                                    (double)row * RowDelay.TotalSeconds +
                                    (double)column * ColumnDelay.TotalSeconds +
                                    TileDuration.TotalSeconds);

                        rotationAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.Zero,
                                Value = 90
                            });
                        rotationAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                                Value = 90
                            });
                        rotationAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = endKeyTime,
                                EasingFunction = CascadeInEasingFunction,
                                Value = 0
                            });

                        sb.Children.Add(rotationAnimation);

                        //****************************y,z的效果可以研究下如何加比较好
                        var rotationAnimationY = new DoubleAnimationUsingKeyFrames();
                        Storyboard.SetTarget(rotationAnimationY, projection);
                        Storyboard.SetTargetProperty(rotationAnimationY, "RotationY");

                        rotationAnimationY.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.Zero,
                                Value = 180
                            });
                        rotationAnimationY.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                                Value = 90
                            });
                        rotationAnimationY.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = endKeyTime,
                                EasingFunction = CascadeInEasingFunction,
                                Value = 0
                            });

                        sb.Children.Add(rotationAnimationY);

                        var rotationAnimationZ = new DoubleAnimationUsingKeyFrames();
                        Storyboard.SetTarget(rotationAnimationZ, projection);
                        Storyboard.SetTargetProperty(rotationAnimationZ, "RotationZ");

                        rotationAnimationZ.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.Zero,
                                Value = 180
                            });
                        rotationAnimationZ.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                                Value = 90
                            });
                        rotationAnimationZ.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = endKeyTime,
                                EasingFunction = CascadeInEasingFunction,
                                Value = 0
                            });

                        sb.Children.Add(rotationAnimationZ);
                        //****************************

                        var opacityAnimation = new DoubleAnimationUsingKeyFrames();
                        Storyboard.SetTarget(opacityAnimation, rect);
                        Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

                        opacityAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.Zero,
                                Value = 0
                            });
                        opacityAnimation.KeyFrames.Add(
                            new DiscreteDoubleKeyFrame
                            {
                                KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                                Value = 0
                            });
                        opacityAnimation.KeyFrames.Add(
                            new EasingDoubleKeyFrame
                            {
                                KeyTime = endKeyTime,
                                EasingFunction = CascadeInEasingFunction,
                                Value = 1
                            });

                        sb.Children.Add(opacityAnimation);

                        //******************** 效果差不错出来了,主要是起始点的设置，还有，这里的动画要不要弄成关键祯？
                        var translateXanimation = new DoubleAnimation();
                        /*translateXanimation.From = translateXInfo[i];*/
                        translateXanimation.From = transfrom.TranslateX;
                        translateXanimation.To = 0;
                        translateXanimation.Duration = endKeyTime;
                        translateXanimation.EasingFunction = CascadeInEasingFunction;
                        //BackEase(缓动函数) BounceEase(弹跳效果) CircleEase(加速和/或减速) CubicEase( f(t) = t3 创建加速和/或减速) 
                        //ElasticEase(弹簧来回振动直到停止) ExponentialEase(指数公式创建加速和/或减速) PowerEase(f(t) = tp 创建加速和/或减速)
                        //QuadraticEase(f(t) = t2 创建加速和/或减速) QuarticEase(f(t) = t4 创建加速和/或减速) QuinticEase(f(t) = t5 创建加速和/或减速) 
                        //SineEase(正弦方程式（见下面的备注）创建加速和/或减速)
                        Storyboard.SetTarget(translateXanimation, transfrom);
                        Storyboard.SetTargetProperty(translateXanimation, "TranslateX");
                        sb.Children.Add(translateXanimation);

                        var translateYanimation = new DoubleAnimation();
                        /*translateYanimation.From = translateYInfo[i];*/
                        translateYanimation.From = transfrom.TranslateY;
                        translateYanimation.To = 0;
                        translateYanimation.Duration = endKeyTime;
                        translateXanimation.EasingFunction = CascadeInEasingFunction;
                        Storyboard.SetTarget(translateYanimation, transfrom);
                        Storyboard.SetTargetProperty(translateYanimation, "TranslateY");
                        sb.Children.Add(translateYanimation);
                        //****************************
                    }

                    sb.Begin();
                };
            }       
        }

        protected override Windows.Foundation.Size MeasureOverride(Windows.Foundation.Size availableSize)
        {          
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.H = finalSize.Height;
            this.W = finalSize.Width;
            this.size = finalSize;
            if (IsClip)
            {
                AddMask();
            }
            return base.ArrangeOverride(finalSize);
        }

        private void GetRHAndRW()
        {
            if (!double.IsNaN(this.H) && !double.IsNaN(this.W))
            {
                this.RH = (int)(this.H / this.Rows + 0.5);
                this.RW = (int)(this.W / this.Columns + 0.5);
            }
        }

        //剪裁显示大小
        private void AddMask()
        {
            if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height))
            {
                var ss = _layoutGrid.Clip;
                _layoutGrid.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), new Size(this.Width, this.Height)) };
            }
            else
            {
                _layoutGrid.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), size) };
            }
        }

        private void CascadingImageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsClip)
            {
                AddMask();
            }
        }
    }
}
