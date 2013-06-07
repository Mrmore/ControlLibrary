using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace TestDemoApp.Helper.System
{
    public class SystemSettingHelper
    {
        private volatile static SystemSettingHelper _instance = null;
        private static readonly object lockObject = new object();

        public static SystemSettingHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new SystemSettingHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        private SettingsPane appSettings = null;
        private SettingsPaneCommandsRequestedEventArgs settingsPaneCommands = null;
        private SettingsCommand cmd = null;
        private SettingsCommand cmd1 = null;

        //手动添加(现在暂时不用这种方式)
        public async void Init()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(5));
            appSettings = SettingsPane.GetForCurrentView();
            appSettings.CommandsRequested -= appSettings_CommandsRequested;
            appSettings.CommandsRequested += appSettings_CommandsRequested;
        }

        public void Reset()
        {
            appSettings.CommandsRequested -= appSettings_CommandsRequested;
        }

        private void appSettings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            //设置面板打开就会进,所以外面做按钮调用删除,也不起作用,
            //除非在App设置跳转内部写(CommandsRequested)里实现.
            settingsPaneCommands = args;
            cmd = new SettingsCommand("Mat", "Mat", (x) =>
            {
                SettingsFlyout settings = new SettingsFlyout();
                settings.FlyoutWidth = SettingsFlyout.SettingsFlyoutWidth.Narrow;
                settings.HeaderBrush = new SolidColorBrush(Colors.Orange);
                settings.HeaderText = string.Format("{0} Mat", App.VisualElements.DisplayName);
                BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
                settings.SmallLogoImageSource = bmp;
                settings.Content = new SettingsContent();
                settings.IsOpen = true;
            });

            settingsPaneCommands.Request.ApplicationCommands.Add(cmd);

            cmd1 = new SettingsCommand("Matong", "Matong", (x) =>
            {
                SettingsFlyout settings = new SettingsFlyout();
                settings.FlyoutWidth = SettingsFlyout.SettingsFlyoutWidth.Narrow;
                settings.HeaderBrush = new SolidColorBrush(Colors.Blue);
                settings.HeaderText = string.Format("{0} Matong", App.VisualElements.DisplayName);
                BitmapImage bmp = new BitmapImage(App.VisualElements.SmallLogoUri);
                settings.SmallLogoImageSource = bmp;
                settings.Content = new SettingsContent();
                settings.IsOpen = true;
            });

            settingsPaneCommands.Request.ApplicationCommands.Add(cmd1);
        }
    }
}
