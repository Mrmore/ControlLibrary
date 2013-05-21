using System;
using System.Collections.Generic;
using System.Linq;
using ControlLibrary.Tools;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.System;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class PopupBorderBase : Control
    {
        public delegate void AnimationOpenStartHandler();
        public event AnimationOpenStartHandler AnimationOpenStart;

        public delegate void AnimationOpenCompleteHandler();
        public event AnimationOpenCompleteHandler AnimationOpenComplete;

        public delegate void AnimationClosedCompleteHandler();
        public event AnimationClosedCompleteHandler AnimationClosedComplete;

        public delegate void MovingBeyondTheBorderCompleteHandler();
        public event MovingBeyondTheBorderCompleteHandler MovingBeyondTheBorderComplete;

        public delegate void OverlayClickCompleteHandler();
        /// <summary>
        /// 点击后面的遮罩事件
        /// </summary>
        public event OverlayClickCompleteHandler OverlayClickComplete;

        public delegate void ApplyTemplateInitCompleteHandler();
        /// <summary>
        /// 皮肤初始化完成事件
        /// </summary>
        public event ApplyTemplateInitCompleteHandler ApplyTemplateInitComplete;

        private Canvas gridOverlay = null;
        private Grid gridPopup = null;
        private Storyboard gridPopupOpen = null;
        private Storyboard gridPopupClosed = null;
        private CompositeTransform ct = null;
        private bool forceManipulationsToEnd = false;
        public Border borderHead = null;
        public Border borderTail = null;
        public Grid gridTail = null;
        public Border borderFoot = null;
        //添加键盘
        private bool isCtrlKeyPressed = false;

        //重绘高宽
        private double childrenAddGridH = double.NaN;
        private double childrenAddGridW = double.NaN;
        private double topOrBottom = double.NaN;
        private double leftOrRight = double.NaN;
        private double movingInterpolationH = double.NaN;
        private double movingInterpolationW = double.NaN;

        public PopupBorderBase()
        {
            this.DefaultStyleKey = typeof(PopupBorderBase);
        }

        private void MovingWithAndHeight()
        {
            childrenAddGridH = (this.ChildrenAddGrid as FrameworkElement).DesiredSize.Height * ct.ScaleY;
            childrenAddGridW = (this.ChildrenAddGrid as FrameworkElement).DesiredSize.Width * ct.ScaleX;
            //topOrBottom = (this.OverlayHeight - (this.ChildrenAddHeadGrid as FrameworkElement).ActualHeight - childrenAddGridH) / 2;
            //leftOrRight = (this.OverlayWidth - childrenAddGridW) / 2;
            topOrBottom = (Window.Current.Bounds.Height - (this.ChildrenAddHeadGrid as FrameworkElement).ActualHeight - childrenAddGridH) / 2;
            leftOrRight = (Window.Current.Bounds.Width - childrenAddGridW) / 2;
            movingInterpolationH = childrenAddGridH + topOrBottom;
            movingInterpolationW = childrenAddGridW + leftOrRight;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gridPopup = this.GetTemplateChild("gridPopup") as Grid;
            gridOverlay = this.GetTemplateChild("gridOverlay") as Canvas;
            borderHead = this.GetTemplateChild("borderHead") as Border;
            borderTail = this.GetTemplateChild("borderTail") as Border;
            gridTail = this.GetTemplateChild("gridTail") as Grid;
            borderFoot = this.GetTemplateChild("borderFoot") as Border;

            if (gridPopup != null)
            {
                gridPopupOpen = gridPopup.Resources["gridPopupOpen"] as Storyboard;
                gridPopupClosed = gridPopup.Resources["gridPopupClosed"] as Storyboard;
                if (this.ChildrenAddGrid != null)
                {
                    this.ct = new CompositeTransform();
                    this.AddChildrenIsZoom();
                }

                if (this.borderHead != null && this.borderFoot != null)
                {
                    this.borderHead.Child = this.ChildrenAddHeadGrid;
                    this.borderHead.SizeChanged -= borderHead_SizeChanged;
                    this.borderHead.SizeChanged += borderHead_SizeChanged;
                    this.borderFoot.Child = this.ChildrenAddFootGrid;
                    Window.Current.SizeChanged -= Current_SizeChanged;
                    Window.Current.SizeChanged += Current_SizeChanged;
                }

                //if (gridTail != null)
                //{
                //    gridTail.Background = new SolidColorBrush(Colors.Transparent);
                //    gridTail.Tapped -= gridTail_Tapped;
                //    gridTail.Tapped += gridTail_Tapped;
                //}

                if (gridOverlay != null)
                {
                    gridOverlay.Tapped -= gridTail_Tapped;
                    gridOverlay.Tapped += gridTail_Tapped;
                }

                if (ApplyTemplateInitComplete != null)
                {
                    this.ApplyTemplateInitComplete();
                }
            }
        }

        void gridTail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (OverlayClickComplete != null)
            {
                this.OverlayClickComplete();
            }
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.Filled:
                case ApplicationViewState.FullScreenLandscape:
                case ApplicationViewState.FullScreenPortrait:
                    {
                        this.OverlayHeight = Window.Current.Bounds.Height;
                        this.OverlayWidth = Window.Current.Bounds.Width;
                        this.Height = Window.Current.Bounds.Height;
                        this.Width = Window.Current.Bounds.Width;
                        this.borderHead.Width = Window.Current.Bounds.Width;
                        break;
                    }
                case ApplicationViewState.Snapped:
                    {
                        this.OverlayHeight = Window.Current.Content.DesiredSize.Height;
                        this.OverlayWidth = Window.Current.Content.DesiredSize.Width;
                        break;
                    }
                default:
                    break;
            }
        }

        private void AddChildrenIsZoom()
        {
            this.borderTail.Child = this.ChildrenAddGrid;
            GenerateCompositeTransformUIElement();
        }

        void borderHead_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AddClipMask();

        }

        public UIElement ChildrenAddGrid
        {
            get { return (UIElement)GetValue(ChildrenAddGridProperty); }
            set { SetValue(ChildrenAddGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddGridProperty = DependencyProperty.Register("ChildrenAddGrid", typeof(UIElement), typeof(PopupBorderBase), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddGridPropertyChanged)));

        private static void onChildrenAddGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {
                popupBorderBase.AddChildrenIsZoom();
            }
        }

        public UIElement ChildrenAddHeadGrid
        {
            get { return (UIElement)GetValue(ChildrenAddHeadGridProperty); }
            set { SetValue(ChildrenAddHeadGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddHeadGridProperty = DependencyProperty.Register("ChildrenAddHeadGrid", typeof(UIElement), typeof(PopupBorderBase), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddHeadGridPropertyChanged)));

        private static void onChildrenAddHeadGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null && popupBorderBase.borderHead != null)
            {
                popupBorderBase.borderHead.Child = popupBorderBase.ChildrenAddHeadGrid;
                popupBorderBase.AddClipMask();
            }
        }

        public UIElement ChildrenAddFootGrid
        {
            get { return (UIElement)GetValue(ChildrenAddFootGridProperty); }
            set { SetValue(ChildrenAddFootGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddFootGridProperty = DependencyProperty.Register("ChildrenAddFootGrid", typeof(UIElement), typeof(PopupBorderBase), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddFootGridPropertyChanged)));

        private static void onChildrenAddFootGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null && popupBorderBase.borderFoot != null)
            {
                popupBorderBase.borderFoot.Child = popupBorderBase.ChildrenAddFootGrid;
            }
        }

        //根据大小剪裁
        public void AddClipMask()
        {
            gridTail.Height = Window.Current.Bounds.Height - this.borderHead.ActualHeight;
            gridTail.Width = Window.Current.Bounds.Width;
            gridTail.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), new Size(Window.Current.Bounds.Width, (Window.Current.Bounds.Height - this.borderHead.ActualHeight))) };
        }

        public bool IsZoom
        {
            get { return (bool)GetValue(IsZoomProperty); }
            set { SetValue(IsZoomProperty, value); }
        }

        public static readonly DependencyProperty IsZoomProperty = DependencyProperty.Register("IsZoom", typeof(bool), typeof(PopupBorderBase), new PropertyMetadata(false, new PropertyChangedCallback(onIsZoomPropertyChanged)));

        private static void onIsZoomPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {
                popupBorderBase.AddChildrenIsZoom();
            }
        }

        void ChildrenAddGrid_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            var wheelPoint = e.GetCurrentPoint(this.ChildrenAddGrid).Position;
            var pointerProp = e.GetCurrentPoint(this.ChildrenAddGrid).Properties;
            if (pointerProp != null)
            {
                int wheelDelta = pointerProp.MouseWheelDelta;
                double delta = 0.0;
                if (wheelDelta > 0)
                {
                    delta = 1.25;
                }
                else
                {
                    delta = 0.75;
                }

                MovingWithAndHeight();

                //放大
                ct.ScaleX = ct.ScaleY *= delta;

                if (System.Math.Round(ct.ScaleX, 1) <= 1.0)
                {
                    ct.ScaleX = 1.0;
                    ct.ScaleY = 1.0;
                }

                if (System.Math.Abs(ct.TranslateY) >= movingInterpolationH || System.Math.Abs(ct.TranslateX) >= movingInterpolationW)
                {
                    if (MovingBeyondTheBorderComplete != null)
                    {
                        this.MovingBeyondTheBorderComplete();
                    }
                }
            }
        }

        //添加键盘
        private void ChildrenAddGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.KeyDown -= ChildrenAddGrid_KeyDown;
            this.KeyUp -= ChildrenAddGrid_KeyUp;
        }

        private void ChildrenAddGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.KeyDown += ChildrenAddGrid_KeyDown;
            this.KeyUp += ChildrenAddGrid_KeyUp;
            this.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }

        private void ChildrenAddGrid_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                isCtrlKeyPressed = false;
            }
        }

        //键盘的放大缩小
        private void ChildrenAddGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                isCtrlKeyPressed = true;
            }
            else if (isCtrlKeyPressed)
            {
                switch (e.Key.ToString())
                {
                    case "189":
                        {
                            //缩小
                            KeyZoomSmall();
                            break;
                        }
                    case "187":
                        {
                            //放大
                            KeyZoomBig();
                            break;
                        }
                }
            }

        }

        /// <summary>
        /// 键盘快捷键放大
        /// </summary>
        public void KeyZoomBig()
        {
            //放大
            double delta = 1.25;
            MovingWithAndHeight();
            //放大
            ct.ScaleX = ct.ScaleY *= delta;
            if (System.Math.Round(ct.ScaleX, 1) <= 0.25)
            {
                ct.ScaleX = 0.25;
                ct.ScaleY = 0.25;
            }
            if (System.Math.Abs(ct.TranslateY) >= movingInterpolationH || System.Math.Abs(ct.TranslateX) >= movingInterpolationW)
            {
                if (MovingBeyondTheBorderComplete != null)
                {
                    this.MovingBeyondTheBorderComplete();
                }
            }
        }

        /// <summary>
        /// 键盘快捷键缩小
        /// </summary>
        public void KeyZoomSmall()
        {
            //缩小
            double delta = 0.75;
            MovingWithAndHeight();
            //放大
            ct.ScaleX = ct.ScaleY *= delta;
            if (System.Math.Round(ct.ScaleX, 1) <= 0.25)
            {
                ct.ScaleX = 0.25;
                ct.ScaleY = 0.25;
            }
            if (System.Math.Abs(ct.TranslateY) >= movingInterpolationH || System.Math.Abs(ct.TranslateX) >= movingInterpolationW)
            {
                if (MovingBeyondTheBorderComplete != null)
                {
                    this.MovingBeyondTheBorderComplete();
                }
            }
        }

        private void GenerateCompositeTransformUIElement()
        {
            ManipulationEventsDel();
            if (this.IsZoom)
            {
                this.ChildrenAddGrid.ManipulationMode = ManipulationModes.All;
                ManipulationEventsAdd();
                InitManipulationTransforms();
                this.ChildrenAddGrid.RenderTransform = ct;
            }
            else
            {
                this.ChildrenAddGrid.ManipulationMode = ManipulationModes.System;
                this.ChildrenAddGrid.RenderTransform = null;
            }
        }

        private void InitManipulationTransforms()
        {
            ChildrenAddGrid.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            ct.ScaleX = 1;
            ct.ScaleY = 1;
            //ct.CenterX = (this.ChildrenAddGrid as FrameworkElement).ActualWidth / 2;
            //ct.CenterY = (this.ChildrenAddGrid as FrameworkElement).ActualHeight / 2;
            ct.Rotation = 0;
            ct.TranslateX = 0;
            ct.TranslateY = 0;
        }

        private void ManipulationEventsAdd()
        {
            this.ChildrenAddGrid.ManipulationStarted += ChildrenAddGrid_ManipulationStarted;
            this.ChildrenAddGrid.ManipulationStarting += ChildrenAddGrid_ManipulationStarting;
            this.ChildrenAddGrid.ManipulationDelta += ChildrenAddGrid_ManipulationDelta;
            this.ChildrenAddGrid.ManipulationCompleted += ChildrenAddGrid_ManipulationCompleted;
            this.ChildrenAddGrid.PointerWheelChanged += ChildrenAddGrid_PointerWheelChanged;
            //添加键盘
            this.ChildrenAddGrid.PointerEntered += ChildrenAddGrid_PointerEntered;
            this.ChildrenAddGrid.PointerExited += ChildrenAddGrid_PointerExited;
        }

        private void ManipulationEventsDel()
        {
            this.ChildrenAddGrid.ManipulationStarted -= ChildrenAddGrid_ManipulationStarted;
            this.ChildrenAddGrid.ManipulationStarting -= ChildrenAddGrid_ManipulationStarting;
            this.ChildrenAddGrid.ManipulationDelta -= ChildrenAddGrid_ManipulationDelta;
            this.ChildrenAddGrid.ManipulationCompleted -= ChildrenAddGrid_ManipulationCompleted;
            this.ChildrenAddGrid.PointerWheelChanged -= ChildrenAddGrid_PointerWheelChanged;
            //添加键盘
            this.ChildrenAddGrid.PointerEntered -= ChildrenAddGrid_PointerEntered;
            this.ChildrenAddGrid.PointerExited -= ChildrenAddGrid_PointerExited;
        }

        void ChildrenAddGrid_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            forceManipulationsToEnd = false;
            e.Handled = true;
        }

        void ChildrenAddGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ChildrenAddGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        void ChildrenAddGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MovingWithAndHeight();
            if (forceManipulationsToEnd)
            {
                e.Complete();
                return;
            }

            //放大
            ct.ScaleX = ct.ScaleY *= e.Delta.Scale;
            //旋转 
            //e.Delta.Rotation 原来是弧度 需要(e.Delta.Rotation * 180 / Math.PI)
            //ct.Rotation += (e.Delta.Rotation * 180 / Math.PI);
            //现在是角度
            //ct.Rotation += e.Delta.Rotation;
            //ct.CenterX = (this.ChildrenAddGrid as FrameworkElement).ActualWidth / 2;
            //ct.CenterY = (this.ChildrenAddGrid as FrameworkElement).ActualHeight / 2;
            //平移
            ct.TranslateX += e.Delta.Translation.X;
            ct.TranslateY += e.Delta.Translation.Y;

            if (System.Math.Round(ct.ScaleX, 1) <= 1.0)
            {
                ct.ScaleX = 1.0;
                ct.ScaleY = 1.0;
            }

            if (System.Math.Abs(ct.TranslateY) >= movingInterpolationH || System.Math.Abs(ct.TranslateX) >= movingInterpolationW)
            {
                if (MovingBeyondTheBorderComplete != null)
                {
                    this.MovingBeyondTheBorderComplete();
                }
            }
            e.Handled = true;
        }

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }

        public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register("OverlayBrush", typeof(Brush), typeof(PopupBorderBase), new PropertyMetadata(new SolidColorBrush(new Color() { A = 128, R = 0, G = 0, B = 0 }), new PropertyChangedCallback(onOverlayBrushPropertyChanged)));

        private static void onOverlayBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {

            }
        }

        public Visibility OverlayVisibility
        {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PopupBorderBase), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(onOverlayVisibilityPropertyChanged)));

        private static void onOverlayVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {

            }
        }

        public double OverlayOpacity
        {
            get { return (double)GetValue(OverlayOpacityProperty); }
            set { SetValue(OverlayOpacityProperty, value); }
        }

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register("OverlayOpacity", typeof(double), typeof(PopupBorderBase), new PropertyMetadata(1.0, new PropertyChangedCallback(onOverlayOpacityPropertyChanged)));

        private static void onOverlayOpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {

            }
        }

        public double OverlayWidth
        {
            get { return (double)GetValue(OverlayWidthProperty); }
            set { SetValue(OverlayWidthProperty, value); }
        }

        public static readonly DependencyProperty OverlayWidthProperty = DependencyProperty.Register("OverlayWidth", typeof(double), typeof(PopupBorderBase), new PropertyMetadata(1366.0, new PropertyChangedCallback(onOverlayWidthPropertyChanged)));

        private static void onOverlayWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {

            }
        }

        public double OverlayHeight
        {
            get { return (double)GetValue(OverlayHeightProperty); }
            set { SetValue(OverlayHeightProperty, value); }
        }

        public static readonly DependencyProperty OverlayHeightProperty = DependencyProperty.Register("OverlayHeight", typeof(double), typeof(PopupBorderBase), new PropertyMetadata(768.0, new PropertyChangedCallback(onOverlayHeightPropertyChanged)));

        private static void onOverlayHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorderBase = sender as PopupBorderBase;
            if (popupBorderBase != null && popupBorderBase.gridPopup != null)
            {

            }
        }

        #region 拉伸属性
        private HorizontalAlignment HorizontalAlignmentWhole
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentWholeProperty); }
            set { SetValue(HorizontalAlignmentWholeProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentWholeProperty = DependencyProperty.Register("HorizontalAlignmentWhole", typeof(HorizontalAlignment), typeof(PopupBorderBase), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentWhole
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentWholeProperty); }
            set { SetValue(VerticalAlignmentWholeProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentWholeProperty = DependencyProperty.Register("VerticalAlignmentWhole", typeof(VerticalAlignment), typeof(PopupBorderBase), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginWhole
        {
            get { return (Thickness)GetValue(MarginWholeProperty); }
            set { SetValue(MarginWholeProperty, value); }
        }
        private static readonly DependencyProperty MarginWholeProperty = DependencyProperty.Register("MarginWhole", typeof(Thickness), typeof(PopupBorderBase), new PropertyMetadata(new Thickness(0)));

        private HorizontalAlignment HorizontalAlignmentHead
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentHeadProperty); }
            set { SetValue(HorizontalAlignmentHeadProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentHeadProperty = DependencyProperty.Register("HorizontalAlignmentHead", typeof(HorizontalAlignment), typeof(PopupBorderBase), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentHead
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentHeadProperty); }
            set { SetValue(VerticalAlignmentHeadProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentHeadProperty = DependencyProperty.Register("VerticalAlignmentHead", typeof(VerticalAlignment), typeof(PopupBorderBase), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginHead
        {
            get { return (Thickness)GetValue(MarginHeadProperty); }
            set { SetValue(MarginHeadProperty, value); }
        }
        private static readonly DependencyProperty MarginHeadProperty = DependencyProperty.Register("MarginHead", typeof(Thickness), typeof(PopupBorderBase), new PropertyMetadata(new Thickness(0)));

        private HorizontalAlignment HorizontalAlignmentBody
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentBodyProperty); }
            set { SetValue(HorizontalAlignmentBodyProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentBodyProperty = DependencyProperty.Register("HorizontalAlignmentBody", typeof(HorizontalAlignment), typeof(PopupBorderBase), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentBody
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentBodyProperty); }
            set { SetValue(VerticalAlignmentBodyProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentBodyProperty = DependencyProperty.Register("VerticalAlignmentBody", typeof(VerticalAlignment), typeof(PopupBorderBase), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginBody
        {
            get { return (Thickness)GetValue(MarginBodyProperty); }
            set { SetValue(MarginBodyProperty, value); }
        }
        private static readonly DependencyProperty MarginBodyProperty = DependencyProperty.Register("MarginBody", typeof(Thickness), typeof(PopupBorderBase), new PropertyMetadata(new Thickness(0)));
        #endregion

        public void AnimationOpen()
        {
            if (AnimationOpenStart != null)
            {
                this.AnimationOpenStart();
            }
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gridPopupClosed.Stop();
            gridPopupOpen.Stop();
            gridPopupOpen.Begin();
            gridPopupOpen.Completed += gridPopupOpen_Completed;
        }

        void gridPopupOpen_Completed(object sender, object e)
        {
            MovingWithAndHeight();

            if (AnimationOpenComplete != null)
            {
                this.AnimationOpenComplete();
            }
        }

        public void AnimationClose()
        {
            //gridPopupOpen.Stop();
            gridPopupClosed.Stop();
            gridPopupClosed.Begin();
            gridPopupClosed.Completed -= gridPopupClosed_Completed;
            gridPopupClosed.Completed += gridPopupClosed_Completed;
        }

        void gridPopupClosed_Completed(object sender, object e)
        {
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //if(this.IsZoom)
            {
                forceManipulationsToEnd = true;
                InitManipulationTransforms();
            }
            if (AnimationClosedComplete != null)
            {
                this.AnimationClosedComplete();
            }
        }
    }
}
