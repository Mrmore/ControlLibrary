using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using ControlLibrary.Extensions;

namespace ControlLibrary
{
    [TemplateVisualState(Name = "UnFocused", GroupName = "Picker")]
    [TemplateVisualState(Name = "Focused", GroupName = "Picker")]
    [TemplateVisualState(Name = "Selected", GroupName = "Picker")]
    public sealed class PickerSelectorItem : ContentControl
    {
        // Parent picker selector
        internal PickerSelector PickerSelector { get; set; }

        public Rect RectPosition { get; set; }

        public Double GetVerticalPosition()
        {
            return this.RectPosition.Y + this.GetTranslateTransform().Y;
        }

        public TranslateTransform GetTranslateTransform()
        {
            return (TranslateTransform)this.RenderTransform;
        }

        /// <summary>
        /// ctor
        /// </summary>
        public PickerSelectorItem()
        {
            this.DefaultStyleKey = typeof(PickerSelectorItem);

            this.RenderTransform = new TranslateTransform();

            this.RectPosition = Rect.Empty;

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // get grid involved in VisualState transitions
            var grid = this.GetVisualDescendent<Grid>();

            if (grid != null && !this.IsFocused)
                grid.Opacity = this.IsSelected ? 1d : 0d;

            this.ManageVisualStates(false);

        }



        public new Object ContentTemplate
        {
            get { return GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static new readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(Object), typeof(PickerSelectorItem), new PropertyMetadata(null));

        public new Object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(PickerSelectorItem), new PropertyMetadata(null));


        public Boolean IsSelected
        {
            get { return (Boolean)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(Boolean), typeof(PickerSelectorItem), new PropertyMetadata(false, IsSelectedChangedCallback));

        private static void IsSelectedChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            PickerSelectorItem pickerSelectorItem = (PickerSelectorItem)dependencyObject;

            pickerSelectorItem.ManageVisualStates();

        }


        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFocused.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register("IsFocused", typeof(bool), typeof(PickerSelectorItem), new PropertyMetadata(false, FocusChangedCallback));


        private static void FocusChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            PickerSelectorItem pickerSelectorItem = (PickerSelectorItem)dependencyObject;

            pickerSelectorItem.ManageVisualStates();

        }

        /// <summary>
        /// Set correct Visual States
        /// </summary>
        internal void ManageVisualStates(bool isAnimated = true)
        {

           if (this.IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", isAnimated);
                //Debug.WriteLine(GetDebugWrite() + " To Selected");
                return;
            }

            if (this.IsFocused)
            {
                VisualStateManager.GoToState(this, "Focused", isAnimated);
                //Debug.WriteLine(GetDebugWrite() + " To Focused");
                return;
            }

            VisualStateManager.GoToState(this, "UnFocused", isAnimated);
            //Debug.WriteLine(GetDebugWrite() + " To UnFocused");
        }

        //private string GetDebugWrite()
        //{
        //    var bottomPosition = this.GetVerticalPosition() + (this.RectPosition.Height * 2);
        //    var topPosition = this.GetVerticalPosition() - (this.RectPosition.Height);

        //    if (!(bottomPosition >= this.PickerSelector.RectPosition.Y) ||
        //        !(topPosition <= (this.PickerSelector.RectPosition.Y + this.PickerSelector.RectPosition.Height)))
        //        return "";

        //    FrameworkElement border = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;

        //    var currentState = String.Empty;

        //    VisualStateGroup vsg = null;

        //    if ((this.DataContext as DateTimeWrapper) != null)
        //    {
        //        switch (this.PickerSelector.DataSourceType)
        //        {
        //            case DataSourceType.Day:
        //                currentState += "Day : ";
        //                break;
        //            case DataSourceType.Month:
        //                currentState += "Month : ";
        //                break;
        //            case DataSourceType.Year:
        //                currentState += "Year : ";
        //                break;
        //        }

        //        currentState += (this.DataContext as DateTimeWrapper).DateTime + " ";
        //    }

        //    if (border != null)
        //    {
        //        IList<VisualStateGroup> vsgCollection =
        //            VisualStateManager.GetVisualStateGroups(border).ToList();

        //        vsg = vsgCollection[0];

        //        if (vsg.CurrentState != null)
        //            currentState += vsg.CurrentState.Name;

        //    }

        //    return currentState;
        //}
    }

}
