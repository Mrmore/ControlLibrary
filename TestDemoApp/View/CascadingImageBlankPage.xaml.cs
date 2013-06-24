using ControlLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CascadingImageBlankPage : Page
    {
        public CascadingImageBlankPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ((CascadingImageControl)sender).Cascade();
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox != null)
            {
                var ss = combobox.SelectedItem as ComboBoxItem;
                cascadingImageControl.Stretch = (Stretch)Enum.Parse(typeof(Stretch), ss.Content.ToString());
                //cascadingImageControl.ImageStretch = (Stretch)Enum.Parse(typeof(Stretch), ss.Content.ToString());
            }
        }

        private void comboboxCascadeDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxCascadeDirection != null)
            {
                var ss = comboboxCascadeDirection.SelectedItem as ComboBoxItem;
                cascadingImageControl.CascadeDirection = (CascadeDirection)Enum.Parse(typeof(CascadeDirection), ss.Content.ToString());
            }
        }

        private void comboboxCascadeSequence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxCascadeSequence != null)
            {
                var ss = comboboxCascadeSequence.SelectedItem as ComboBoxItem;
                cascadingImageControl.CascadeSequence = (CascadeSequence)Enum.Parse(typeof(CascadeSequence), ss.Content.ToString());
            }
        }

        private void comboboxIsClip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxIsClip != null)
            {
                var ss = comboboxIsClip.SelectedItem as ComboBoxItem;
                cascadingImageControl.IsClip = (bool)bool.Parse(ss.Content.ToString());
            }
        }

        private void comboboxCascadeAanimation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxCascadeAanimation != null)
            {
                var ss = comboboxCascadeAanimation.SelectedItem as ComboBoxItem;
                cascadingImageControl.CascadeAanimation = (CascadeAanimation)Enum.Parse(typeof(CascadeAanimation), ss.Content.ToString());
            }
        }
    }
}
