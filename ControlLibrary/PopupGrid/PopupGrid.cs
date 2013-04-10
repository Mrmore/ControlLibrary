using System;
using System.Collections.Generic;
using System.Linq;
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

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class PopupGrid : Control
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

        private PopupGridBase popupGridBase = null;
        private Popup popup = null;

        //private static PopupGrid popupGrid = null;

        public UIElement ChildrenAddGrid
        {
            get { return (UIElement)GetValue(ChildrenAddGridProperty); }
            set { SetValue(ChildrenAddGridProperty, value); }
        }

        public static readonly DependencyProperty ChildrenAddGridProperty = DependencyProperty.Register("ChildrenAddGrid", typeof(UIElement), typeof(PopupGrid), new PropertyMetadata(null, new PropertyChangedCallback(onChildrenAddGridPropertyChanged)));

        private static void onChildrenAddGridPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue(OverlayBrushProperty); }
            set { SetValue(OverlayBrushProperty, value); }
        }

        public static readonly DependencyProperty OverlayBrushProperty = DependencyProperty.Register("OverlayBrush", typeof(Brush), typeof(PopupGrid), new PropertyMetadata(new SolidColorBrush(new Color() { A = 128, R = 0, G = 0, B = 0 }), new PropertyChangedCallback(onOverlayBrushPropertyChanged)));

        private static void onOverlayBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {
                popupGrid.popupGridBase.OverlayBrush = popupGrid.OverlayBrush;
            }
        }

        public Visibility OverlayVisibility
        {
            get { return (Visibility)GetValue(OverlayVisibilityProperty); }
            set { SetValue(OverlayVisibilityProperty, value); }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(PopupGrid), new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(onOverlayVisibilityPropertyChanged)));

        private static void onOverlayVisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public double OverlayOpacity
        {
            get { return (double)GetValue(OverlayOpacityProperty); }
            set { SetValue(OverlayOpacityProperty, value); }
        }

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register("OverlayOpacity", typeof(double), typeof(PopupGrid), new PropertyMetadata(1.0, new PropertyChangedCallback(onOverlayOpacityPropertyChanged)));

        private static void onOverlayOpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public double OverlayWidth
        {
            get { return (double)GetValue(OverlayWidthProperty); }
            set { SetValue(OverlayWidthProperty, value); }
        }

        public static readonly DependencyProperty OverlayWidthProperty = DependencyProperty.Register("OverlayWidth", typeof(double), typeof(PopupGrid), new PropertyMetadata(1366.0, new PropertyChangedCallback(onOverlayWidthPropertyChanged)));

        private static void onOverlayWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public double OverlayHeight
        {
            get { return (double)GetValue(OverlayHeightProperty); }
            set { SetValue(OverlayHeightProperty, value); }
        }

        public static readonly DependencyProperty OverlayHeightProperty = DependencyProperty.Register("OverlayHeight", typeof(double), typeof(PopupGrid), new PropertyMetadata(768.0, new PropertyChangedCallback(onOverlayHeightPropertyChanged)));

        private static void onOverlayHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupGrid), new PropertyMetadata(false, new PropertyChangedCallback(onIsOpenPropertyChanged)));

        private static void onIsOpenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {
                if (popupGrid.IsOpen)
                    popupGrid.AnimationOpen();
                else
                    popupGrid.AnimationClose();
            }
        }

        //是否显示动画
        public bool IsAnimation
        {
            get { return (bool)GetValue(IsAnimationProperty); }
            set { SetValue(IsAnimationProperty, value); }
        }

        public static readonly DependencyProperty IsAnimationProperty = DependencyProperty.Register("IsAnimation", typeof(bool), typeof(PopupGrid), new PropertyMetadata(true, new PropertyChangedCallback(onIsAnimationPropertyChanged)));

        private static void onIsAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popup != null)
            {

            }
        }

        public bool IsZoom
        {
            get { return (bool)GetValue(IsZoomProperty); }
            set { SetValue(IsZoomProperty, value); }
        }

        public static readonly DependencyProperty IsZoomProperty = DependencyProperty.Register("IsZoom", typeof(bool), typeof(PopupGrid), new PropertyMetadata(false, new PropertyChangedCallback(onIsZoomPropertyChanged)));

        private static void onIsZoomPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var popupGrid = sender as PopupGrid;
            if (popupGrid != null && popupGrid.popupGridBase != null && popupGrid.ChildrenAddGrid != null)
            {
                popupGrid.SettingsIsZoom();
            }
        }

        private void SettingsIsZoom()
        {
            if (this.IsZoom)
            {
                //popupGrid = popupGrid;

                this.ChildrenAddGrid.Tapped -= ChildrenAddGrid_Tapped;
                this.ChildrenAddGrid.Tapped += ChildrenAddGrid_Tapped;

                this.popupGridBase.MovingBeyondTheBorderComplete -= popupGridBase_MovingBeyondTheBorderComplete;
                this.popupGridBase.MovingBeyondTheBorderComplete += popupGridBase_MovingBeyondTheBorderComplete;
            }
            else
            {
                this.popupGridBase.MovingBeyondTheBorderComplete -= popupGridBase_MovingBeyondTheBorderComplete;
                this.ChildrenAddGrid.Tapped -= ChildrenAddGrid_Tapped;
            }
        }

        void popupGridBase_MovingBeyondTheBorderComplete()
        {
            this.IsOpen = false;
        }

        private void ChildrenAddGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.IsOpen = false;
            if (this.ClickComplete != null)
            {
                this.ClickComplete();
            }
        }

        public PopupGrid()
        {
            this.DefaultStyleKey = typeof(PopupGrid);
            //Window.Current.SizeChanged += Current_SizeChanged;
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //PopupGrid popupGrid = this;            
            popupGridBase = this.GetTemplateChild("popupGridBase") as PopupGridBase;

            try
            {
                popupGridBase.ApplyTemplate();
            }
            catch { }

            popupGridBase.ApplyTemplateInitComplete += () =>
                {
                    
                };

            popup = this.GetTemplateChild("popup") as Popup;
            try
            {
                this.popupGridBase.OverlayBrush = this.OverlayBrush;
            }
            catch { }

            popupGridBase.Height = this.Height;
            popupGridBase.Width = this.Width;
            popupGridBase.AnimationOpenStart -= popupGridBase_AnimationOpenStart;
            popupGridBase.AnimationOpenStart += popupGridBase_AnimationOpenStart;
            popupGridBase.AnimationOpenComplete -= PopupGrid_AnimationOpenComplete;
            popupGridBase.AnimationOpenComplete += PopupGrid_AnimationOpenComplete;
            if (this.ChildrenAddGrid != null)
            {
                SettingsIsZoom();
            }
            Window.Current.SizeChanged -= Current_SizeChanged;
            Window.Current.SizeChanged += Current_SizeChanged;
            this.OverlayHeight = Window.Current.Content.RenderSize.Height;
            this.OverlayWidth = Window.Current.Content.RenderSize.Width;

            this.popupGridBase.OverlayClickComplete -= popupGridBase_OverlayClickComplete;
            this.popupGridBase.OverlayClickComplete += popupGridBase_OverlayClickComplete;
            //添加键盘
            //this.KeyDown -= PopupGrid_KeyDown;
            //this.KeyDown += PopupGrid_KeyDown;
            //this.popupGridBase.KeyDown -= PopupGrid_KeyDown;
            //this.popupGridBase.KeyDown += PopupGrid_KeyDown;
            //this.popupGridBase.PointerEntered -= PopupGrid_PointerEntered;
            //this.popupGridBase.PointerEntered += PopupGrid_PointerEntered;
        }

        //添加键盘
        void PopupGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            this.popupGridBase.Focus(Windows.UI.Xaml.FocusState.Pointer);
        }

        //添加键盘
        void PopupGrid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape || e.Key == VirtualKey.Space)
            {
                if (this.IsOpen)
                {
                    this.IsOpen = false;
                }
            }
        }

        void popupGridBase_OverlayClickComplete()
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
            this.popupGridBase.Focus(Windows.UI.Xaml.FocusState.Pointer);
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
            popupGridBase.AnimationOpen();
        }

        private void AnimationClose()
        {
            popupGridBase.AnimationClose();
            popupGridBase.AnimationClosedComplete -= popupGridBase_AnimationClosedComplete;
            popupGridBase.AnimationClosedComplete += popupGridBase_AnimationClosedComplete;
        }

        void popupGridBase_AnimationClosedComplete()
        {
            popup.IsOpen = false;
            if (AnimationClosedComplete != null)
            {
                this.AnimationClosedComplete();
            }
        }
    }
}
