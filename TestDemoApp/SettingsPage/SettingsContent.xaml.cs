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
using ControlLibrary.Extensions;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace TestDemoApp
{
    public sealed partial class SettingsContent : UserControl
    {
        public SettingsContent()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsFlyout sf = this.Ancestors<SettingsFlyout>().FirstOrDefault() as SettingsFlyout;
            sf.IsOpen = false;
        }
    }
}
