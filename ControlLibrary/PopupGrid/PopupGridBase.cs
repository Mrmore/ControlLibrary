using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class PopupGridBase : Control
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
        //添加键盘
        private bool isCtrlKeyPressed = false;

        //重绘高宽
        private double childrenAddGridH = double.NaN;
        private double childrenAddGridW = double.NaN;
        private double topOrBottom = double.NaN;
        private double leftOrRight = double.NaN;
        private double movingInterpolationH = double.NaN;
        private double movingInterpolationW = double.NaN;

        public PopupGridBase()
        {
            this.DefaultStyleKey = typeof(PopupGridBase);
        }

        private void MovingWithAndHeight()
        {
            if (ct == null)
            {
                ct = new CompositeTransform();
            }
            childrenAddGridH = (this.ChildrenAddGrid as FrameworkElement).DesiredSize.Height * ct.ScaleY;
            childrenAddGridW = (this.ChildrenAddGrid as FrameworkElement).DesiredSize.Width * ct.ScaleX;
            //topOrBottom = (this.OverlayHeight - childrenAddGridH) / 2;
            //leftOrRight = (this.OverlayWidth - childrenAddGridW) / 2;
            topOrBottom = (Window.Current.Bounds.Height - childrenAddGridH) / 2;
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
            //Application.Current.
            if (gridPopup != null)
            {
                gridPopupOpen = gridPopup.Resources["gridPopupOpen"] as Storyboard;
                gridPopupClosed = gridPopup.Resources["gridPopupClosed"] as Storyboard;
                if (this.ChildrenAddGrid != null)
                {
                    this.ct = new CompositeTransform();
                    this.AddChildrenIsZoom();
                }

                Window.Current.SizeChanged -= Current_SizeChanged;
                Window.Current.SizeChanged += Current_SizeChanged;
            }

            if (gridOverlay != null)
            {
                gridOverlay.Tapped -= gridOverlay_Tapped;
                gridOverlay.Tapped += gridOverlay_Tapped;
            }

            if (ApplyTemplateInitComplete != null)
            {
                this.ApplyTemplateInitComplete();
            }
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
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

        void gridOverlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (OverlayClickComplete != null)
            {
                this.OverlayClickComplete();
            }
        }

        public UIElement ChildrenAddGrid
        {
            get { return (UIElement)GetValue(ChildrenAddGridProperty); }
            set { SetValue(ChildrenAddGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddGridProperty = DependencyProperty.Register("ChildrenAddGrid", typeof(UIElement), typeof(PopupGridBase), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddGridPropertyChanged)));

        private static void onChildrenAddGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {
                popupGrid.AddChildrenIsZoom();
            }
        }

        public bool IsZoom
        {
            get { return (bool)GetValue(IsZoomProperty); }
            set { SetValue(IsZoomProperty, value); }
        }

        public static readonly DependencyProperty IsZoomProperty = DependencyProperty.Register("IsZoom", typeof(bool), typeof(PopupGridBase), new PropertyMetadata(false, new PropertyChangedCallback(onIsZoomPropertyChanged)));

        private static void onIsZoomPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {
                popupGrid.AddChildrenIsZoom();
            }
        }

        //是否显示动画
        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register("IsAnimation", typeof(bool), typeof(PopupGridBase), new PropertyMetadata(true, new PropertyChangedCallback(onIsAnimationPropertyChanged)));

        private static void onIsAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        private void AddChildrenIsZoom()
        {
            this.gridPopup.Children.Clear();
            this.gridPopup.Children.Add(this.ChildrenAddGrid);
            //if (IsZoom)
            {
                GenerateCompositeTransformUIElement();
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
            ct.Rotation += e.Delta.Rotation;
            //ct.CenterX = (this.ChildrenAddGrid as FrameworkElement).ActualWidth / 2;
            //ct.CenterY = (this.ChildrenAddGrid as FrameworkElement).ActualHeight / 2;
            //平移
            ct.TranslateX += e.Delta.Translation.X;
            ct.TranslateY += e.Delta.Translation.Y;

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
            e.Handled = true;
        }

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }

        public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register("OverlayBrush", typeof(Brush), typeof(PopupGridBase), new PropertyMetadata(new SolidColorBrush(new Color() { A = 128, R = 0, G = 0, B = 0 }), new PropertyChangedCallback(onOverlayBrushPropertyChanged)));

        private static void onOverlayBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        public Visibility OverlayVisibility
        {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PopupGridBase), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(onOverlayVisibilityPropertyChanged)));

        private static void onOverlayVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        public double OverlayOpacity
        {
            get { return (double)GetValue(OverlayOpacityProperty); }
            set { SetValue(OverlayOpacityProperty, value); }
        }

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register("OverlayOpacity", typeof(double), typeof(PopupGridBase), new PropertyMetadata(1.0, new PropertyChangedCallback(onOverlayOpacityPropertyChanged)));

        private static void onOverlayOpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        public double OverlayWidth
        {
            get { return (double)GetValue(OverlayWidthProperty); }
            set { SetValue(OverlayWidthProperty, value); }
        }

        public static readonly DependencyProperty OverlayWidthProperty = DependencyProperty.Register("OverlayWidth", typeof(double), typeof(PopupGridBase), new PropertyMetadata(1366.0, new PropertyChangedCallback(onOverlayWidthPropertyChanged)));

        private static void onOverlayWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        public double OverlayHeight
        {
            get { return (double)GetValue(OverlayHeightProperty); }
            set { SetValue(OverlayHeightProperty, value); }
        }

        public static readonly DependencyProperty OverlayHeightProperty = DependencyProperty.Register("OverlayHeight", typeof(double), typeof(PopupGridBase), new PropertyMetadata(768.0, new PropertyChangedCallback(onOverlayHeightPropertyChanged)));

        private static void onOverlayHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGridBase;
            if (popupGrid != null && popupGrid.gridPopup != null)
            {

            }
        }

        public void AnimationOpen()
        {
            if (AnimationOpenStart != null)
            {
                this.AnimationOpenStart();
            }
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gridPopupOpen.Completed -= gridPopupOpen_Completed;

            if (this.IsAnimation)
            {
                gridPopupClosed.Stop();
                gridPopupOpen.Stop();
                gridPopupOpen.Begin();
                gridPopupOpen.Completed += gridPopupOpen_Completed;
            }
            else
            {
                gridPopupClosed.Stop();
                gridPopup.Visibility = Windows.UI.Xaml.Visibility.Visible;
                gridPopup.Opacity = 1.0;
                MovingWithAndHeight();
                if (AnimationOpenComplete != null)
                {
                    this.AnimationOpenComplete();
                }
            }
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
            gridPopupClosed.Completed -= gridPopupClosed_Completed;
            if (this.IsAnimation)
            {
                //gridPopupOpen.Stop();
                gridPopupClosed.Stop();
                gridPopupClosed.Begin();
                gridPopupClosed.Completed += gridPopupClosed_Completed;
            }
            else
            {

                gridPopup.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                gridPopup.Opacity = 0.0;
                this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                forceManipulationsToEnd = true;
                InitManipulationTransforms();
                if (AnimationClosedComplete != null)
                {
                    this.AnimationClosedComplete();
                }
            }
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
