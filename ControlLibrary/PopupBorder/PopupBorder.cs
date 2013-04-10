using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class PopupBorder : Control
    {
        public delegate void AnimationOpenStartHandler();
        public event AnimationOpenStartHandler AnimationOpenStart;

        public delegate void AnimationOpenCompleteHandler();
        public event AnimationOpenCompleteHandler AnimationOpenComplete;

        public delegate void AnimationClosedCompleteHandler();
        public event AnimationClosedCompleteHandler AnimationClosedComplete;

        public delegate void ClickCompleteHandler();
        public event ClickCompleteHandler ClickComplete;

        public delegate void OverlayClickCompleteHandler();
        /// <summary>
        /// 点击后面的遮罩事件
        /// </summary>
        public event OverlayClickCompleteHandler OverlayClickComplete;

        private PopupBorderBase popupBorderBase = null;
        private Popup popup = null;

        //private static PopupGrid popupGrid = null;

        public UIElement ChildrenAddGrid
        {
            get { return (UIElement)GetValue(ChildrenAddGridProperty); }
            set { SetValue(ChildrenAddGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddGridProperty = DependencyProperty.Register("ChildrenAddGrid", typeof(UIElement), typeof(PopupBorder), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddGridPropertyChanged)));

        private static void onChildrenAddGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {

            }
        }

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }

        public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register("OverlayBrush", typeof(Brush), typeof(PopupBorder), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(onOverlayBrushPropertyChanged)));

        private static void onOverlayBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {
                popupBorder.popupBorderBase.OverlayBrush = popupBorder.OverlayBrush;
            }
        }

        public Visibility OverlayVisibility
        {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PopupBorder), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(onOverlayVisibilityPropertyChanged)));

        private static void onOverlayVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {

            }
        }

        public double OverlayOpacity
        {
            get { return (double)GetValue(OverlayOpacityProperty); }
            set { SetValue(OverlayOpacityProperty, value); }
        }

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register("OverlayOpacity", typeof(double), typeof(PopupBorder), new PropertyMetadata(0.7, new PropertyChangedCallback(onOverlayOpacityPropertyChanged)));

        private static void onOverlayOpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {

            }
        }

        public double OverlayWidth
        {
            get { return (double)GetValue(OverlayWidthProperty); }
            set { SetValue(OverlayWidthProperty, value); }
        }

        public static readonly DependencyProperty OverlayWidthProperty = DependencyProperty.Register("OverlayWidth", typeof(double), typeof(PopupBorder), new PropertyMetadata(1366.0, new PropertyChangedCallback(onOverlayWidthPropertyChanged)));

        private static void onOverlayWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {

            }
        }

        public double OverlayHeight
        {
            get { return (double)GetValue(OverlayHeightProperty); }
            set { SetValue(OverlayHeightProperty, value); }
        }

        public static readonly DependencyProperty OverlayHeightProperty = DependencyProperty.Register("OverlayHeight", typeof(double), typeof(PopupBorder), new PropertyMetadata(768.0, new PropertyChangedCallback(onOverlayHeightPropertyChanged)));

        private static void onOverlayHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {

            }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupBorder), new PropertyMetadata(false, new PropertyChangedCallback(onIsOpenPropertyChanged)));

        private static void onIsOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {
                if (popupBorder.IsOpen)
                    popupBorder.AnimationOpen();
                else
                    popupBorder.AnimationClose();
            }
        }

        public bool IsZoom
        {
            get { return (bool)GetValue(IsZoomProperty); }
            set { SetValue(IsZoomProperty, value); }
        }

        public static readonly DependencyProperty IsZoomProperty = DependencyProperty.Register("IsZoom", typeof(bool), typeof(PopupBorder), new PropertyMetadata(false, new PropertyChangedCallback(onIsZoomPropertyChanged)));

        private static void onIsZoomPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popupBorderBase != null)
            {
                popupBorder.SettingsIsZoom();
            }
        }

        public UIElement ChildrenAddHeadGrid
        {
            get { return (UIElement)GetValue(ChildrenAddHeadGridProperty); }
            set { SetValue(ChildrenAddHeadGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddHeadGridProperty = DependencyProperty.Register("ChildrenAddHeadGrid", typeof(UIElement), typeof(PopupBorder), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddHeadGridPropertyChanged)));

        private static void onChildrenAddHeadGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null && popupBorder.popup != null)
            {
                
            }
        }

        public UIElement ChildrenAddFootGrid
        {
            get { return (UIElement)GetValue(ChildrenAddFootGridProperty); }
            set { SetValue(ChildrenAddFootGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddFootGridProperty = DependencyProperty.Register("ChildrenAddFootGrid", typeof(UIElement), typeof(PopupBorder), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddFootGridPropertyChanged)));

        private static void onChildrenAddFootGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupBorder = sender as PopupBorder;
            if (popupBorder != null)
            {
                
            }
        }

        private void SettingsIsZoom()
        {
            if (this.IsZoom)
            {
                //popupGrid = popupGrid;

                //this.popupGridBase.Tapped -= popupGridBase_Tapped;
                //this.popupGridBase.Tapped += popupGridBase_Tapped;

                this.ChildrenAddGrid.Tapped -= popupGridBase_Tapped;
                this.ChildrenAddGrid.Tapped += popupGridBase_Tapped;

                this.popupBorderBase.MovingBeyondTheBorderComplete -= popupGridBase_MovingBeyondTheBorderComplete;
                this.popupBorderBase.MovingBeyondTheBorderComplete += popupGridBase_MovingBeyondTheBorderComplete;
            }
            else
            {
                this.popupBorderBase.MovingBeyondTheBorderComplete -= popupGridBase_MovingBeyondTheBorderComplete;
                //this.popupGridBase.Tapped -= popupGridBase_Tapped;
                this.ChildrenAddGrid.Tapped -= popupGridBase_Tapped;
            }
        }

        void popupGridBase_MovingBeyondTheBorderComplete()
        {
            this.IsOpen = false;
        }

        private void popupGridBase_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.IsOpen = false;
            if (this.ClickComplete != null)
            {
                this.ClickComplete();
            }
        }

        public PopupBorder()
        {
            this.DefaultStyleKey = typeof(PopupBorder);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            popupBorderBase = this.GetTemplateChild("popupBorderBase") as PopupBorderBase;

            //try
            //{
            //    popupBorderBase.ApplyTemplate();
            //}
            //catch { }

            popupBorderBase.ApplyTemplateInitComplete += () =>
                {
                    popup = this.GetTemplateChild("popup") as Popup;
                    try
                    {
                        this.popupBorderBase.OverlayBrush = this.OverlayBrush;
                    }
                    catch { }
                    popupBorderBase.Height = this.Height;
                    popupBorderBase.Width = this.Width;
                    popupBorderBase.AnimationOpenStart -= popupGridBase_AnimationOpenStart;
                    popupBorderBase.AnimationOpenStart += popupGridBase_AnimationOpenStart;
                    popupBorderBase.AnimationOpenComplete -= PopupGrid_AnimationOpenComplete;
                    popupBorderBase.AnimationOpenComplete += PopupGrid_AnimationOpenComplete;
                    SettingsIsZoom();
                    Window.Current.SizeChanged -= Current_SizeChanged;
                    Window.Current.SizeChanged += Current_SizeChanged;
                    this.OverlayHeight = Window.Current.Content.RenderSize.Height;
                    this.OverlayWidth = Window.Current.Content.RenderSize.Width;

                    popupBorderBase.OverlayClickComplete -= popupBorderBase_OverlayClickComplete;
                    popupBorderBase.OverlayClickComplete += popupBorderBase_OverlayClickComplete;
                    //添加键盘
                    //this.KeyDown -= popupBorderBase_KeyDown;
                    //this.KeyDown += popupBorderBase_KeyDown;
                    //this.popupBorderBase.KeyDown -= popupBorderBase_KeyDown;
                    //this.popupBorderBase.KeyDown += popupBorderBase_KeyDown;
                    //this.popupBorderBase.PointerEntered -= PopupBorder_PointerEntered;
                    //this.popupBorderBase.PointerEntered += PopupBorder_PointerEntered;
                };
        }

        //添加键盘
        void PopupBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.popupBorderBase.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }

        //添加键盘
        void popupBorderBase_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape || e.Key == VirtualKey.Space)
            {
                if (this.IsOpen)
                {
                    this.IsOpen = false;
                }
            }
        }

        void popupBorderBase_OverlayClickComplete()
        {
            if (OverlayClickComplete != null)
            {
                this.OverlayClickComplete();
            }
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            //this.OverlayHeight = e.Size.Height;
            //this.OverlayWidth = e.Size.Width;
            if (popupBorderBase != null)
            {
                popupBorderBase.AddClipMask();
            }
            switch (ApplicationView.Value)
            {
                case ApplicationViewState.Filled:
                case ApplicationViewState.FullScreenLandscape:
                case ApplicationViewState.FullScreenPortrait:
                    {
                        break;
                    }
                case ApplicationViewState.Snapped:
                    {
                        if (this.IsOpen)
                        {
                            this.IsOpen = false;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        void PopupGrid_AnimationOpenComplete()
        {
            //添加键盘
            this.popupBorderBase.Focus(Windows.UI.Xaml.FocusState.Pointer);
            if (AnimationOpenComplete != null)
            {
                this.AnimationOpenComplete();
            }
        }
        
        void popupGridBase_AnimationOpenStart()
        {
            if (AnimationOpenStart != null)
            {
                this.AnimationOpenStart();
            }
        }

        private void AnimationOpen()
        {
            popup.IsOpen = true;
            popupBorderBase.AnimationOpen();
        }

        private void AnimationClose()
        {
            popupBorderBase.AnimationClose();
            popupBorderBase.AnimationClosedComplete -= popupGridBase_AnimationClosedComplete;
            popupBorderBase.AnimationClosedComplete += popupGridBase_AnimationClosedComplete;
        }

        void popupGridBase_AnimationClosedComplete()
        {
            popup.IsOpen = false;
            if (AnimationClosedComplete != null)
            {
                this.AnimationClosedComplete();
            }
        }

        #region 拉伸属性
        private HorizontalAlignment HorizontalAlignmentWhole
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentWholeProperty); }
            set { SetValue(HorizontalAlignmentWholeProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentWholeProperty = DependencyProperty.Register("HorizontalAlignmentWhole", typeof(HorizontalAlignment), typeof(PopupBorder), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentWhole
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentWholeProperty); }
            set { SetValue(VerticalAlignmentWholeProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentWholeProperty = DependencyProperty.Register("VerticalAlignmentWhole", typeof(VerticalAlignment), typeof(PopupBorder), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginWhole
        {
            get { return (Thickness)GetValue(MarginWholeProperty); }
            set { SetValue(MarginWholeProperty, value); }
        }
        private static readonly DependencyProperty MarginWholeProperty = DependencyProperty.Register("MarginWhole", typeof(Thickness), typeof(PopupBorder), new PropertyMetadata(new Thickness(0)));

        private HorizontalAlignment HorizontalAlignmentHead
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentHeadProperty); }
            set { SetValue(HorizontalAlignmentHeadProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentHeadProperty = DependencyProperty.Register("HorizontalAlignmentHead", typeof(HorizontalAlignment), typeof(PopupBorder), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentHead
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentHeadProperty); }
            set { SetValue(VerticalAlignmentHeadProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentHeadProperty = DependencyProperty.Register("VerticalAlignmentHead", typeof(VerticalAlignment), typeof(PopupBorder), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginHead
        {
            get { return (Thickness)GetValue(MarginHeadProperty); }
            set { SetValue(MarginHeadProperty, value); }
        }
        private static readonly DependencyProperty MarginHeadProperty = DependencyProperty.Register("MarginHead", typeof(Thickness), typeof(PopupBorder), new PropertyMetadata(new Thickness(0)));

        private HorizontalAlignment HorizontalAlignmentBody
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentBodyProperty); }
            set { SetValue(HorizontalAlignmentBodyProperty, value); }
        }
        private static readonly DependencyProperty HorizontalAlignmentBodyProperty = DependencyProperty.Register("HorizontalAlignmentBody", typeof(HorizontalAlignment), typeof(PopupBorder), new PropertyMetadata(HorizontalAlignment.Stretch));

        private VerticalAlignment VerticalAlignmentBody
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentBodyProperty); }
            set { SetValue(VerticalAlignmentBodyProperty, value); }
        }
        private static readonly DependencyProperty VerticalAlignmentBodyProperty = DependencyProperty.Register("VerticalAlignmentBody", typeof(VerticalAlignment), typeof(PopupBorder), new PropertyMetadata(VerticalAlignment.Stretch));

        private Thickness MarginBody
        {
            get { return (Thickness)GetValue(MarginBodyProperty); }
            set { SetValue(MarginBodyProperty, value); }
        }
        private static readonly DependencyProperty MarginBodyProperty = DependencyProperty.Register("MarginBody", typeof(Thickness), typeof(PopupBorder), new PropertyMetadata(new Thickness(0)));
        #endregion
    }
}
