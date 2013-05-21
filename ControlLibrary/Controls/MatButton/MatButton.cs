using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace ControlLibrary
{
    [TemplateVisualState(Name = NormalState, GroupName = VisualStateGroupCommonStates)]
    [TemplateVisualState(Name = PointerOverState, GroupName = VisualStateGroupCommonStates)]
    [TemplateVisualState(Name = PressedState, GroupName = VisualStateGroupCommonStates)]
    [TemplateVisualState(Name = ReleasedState, GroupName = VisualStateGroupCommonStates)]
    [TemplateVisualState(Name = DisabledState, GroupName = VisualStateGroupCommonStates)]
    [TemplateVisualState(Name = FocusedState, GroupName = VisualStateGroupFocusStates)]
    [TemplateVisualState(Name = UnfocusedState, GroupName = VisualStateGroupFocusStates)]
    [TemplateVisualState(Name = PointerFocusedState, GroupName = VisualStateGroupFocusStates)]
    public sealed class MatButton : ContentControl //: Control 为了能让自定义按钮里面能放Ui对象所以继承ContentControl
    {
        #region Static StateResouce
        private const string VisualStateGroupCommonStates = "CommonStates";
        private const string NormalState = "Normal";
        private const string PointerOverState = "PointerOver";
        private const string PressedState = "Pressed";
        private const string ReleasedState = "Released";
        private const string DisabledState = "Disabled";

        private const string VisualStateGroupFocusStates = "FocusStates";
        private const string FocusedState = "Focused";
        private const string UnfocusedState = "Unfocused";
        private const string PointerFocusedState = "PointerFocused";
        #endregion

        private Border border = null;
        public MatButton()
        {
            this.DefaultStyleKey = typeof(MatButton);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MatButton), new PropertyMetadata(new CornerRadius(0), new PropertyChangedCallback(OnCornerRadiusPropertyChanged)));

        private static void OnCornerRadiusPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var matButton = sender as MatButton;
            if (matButton != null && matButton.border != null)
            {
                //matButton.border.CornerRadius = new CornerRadius(0);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            border = this.GetTemplateChild("border") as Border;
            border.PointerMoved += border_PointerMoved;
            border.PointerPressed += border_PointerPressed;
            border.PointerExited += border_PointerExited;
            this.IsEnabledChanged += MyButton_IsEnabledChanged;
            border.GotFocus += border_GotFocus;
            border.LostFocus += border_LostFocus;
            border.PointerReleased += border_PointerReleased;
        }

        void border_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, ReleasedState, true);
        }

        void border_LostFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, UnfocusedState, true);
        }

        void border_GotFocus(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, FocusedState, true);
        }

        void MyButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsEnabled)
            {
                VisualStateManager.GoToState(this, DisabledState, true);
            }
        }

        void border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, NormalState, true);
        }

        void border_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PressedState, true);
        }

        void border_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, PointerOverState, true);
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }
    }
}
