using ControlLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
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
            settingswidth.SelectedIndex = 0;
            SettingsPane.GetForCurrentView().CommandsRequested += BlankPage_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= BlankPage_CommandsRequested;
            base.OnNavigatedFrom(e);
        }

        private void BlankPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand cmd = new SettingsCommand("第三项", "第三项", (x) =>
            {
                SettingsFlyout settings = new SettingsFlyout();
                settings.FlyoutWidth = (SettingsFlyout.SettingsFlyoutWidth)Enum.Parse(typeof(SettingsFlyout.SettingsFlyoutWidth), settingswidth.SelectionBoxItem.ToString());
                settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
                settings.HeaderText = string.Format("{0} 第三项", App.VisualElements.DisplayName);

                // provide some logo (preferrably the smallogo the app uses)
                BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
                settings.SmallLogoImageSource = bmp;

                // set the content for the flyout
                settings.Content = new SettingsContent();

                // open it
                settings.IsOpen = true;
            });

            args.Request.ApplicationCommands.Add(cmd);
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            SettingsPane.Show();
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
