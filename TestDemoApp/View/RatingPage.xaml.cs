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
    public sealed partial class RatingPage : Page
    {
        public RatingPage()
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

        private void Btback_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void rate_ValueChanged_1(object sender, ValueChangedEventArgs<double> e)
        {
            Rating r = sender as Rating;
            if (EventOutput != null)
            {
                EventOutput.Text = string.Format("你选择了 '{0}'", e.NewValue.ToString());
                EventOutput.Text += string.Format("\n旧值: {0}, 新值: {1}", e.OldValue.ToString(), e.NewValue.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //rate.Value = 4;
            initiallyCollapsed.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
