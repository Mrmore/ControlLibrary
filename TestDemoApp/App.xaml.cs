using ControlLibrary;
using ControlLibrary.Common;
using ControlLibrary.SettingsManagement;
using ControlLibrary.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestDemoApp.Helper;
using TestDemoApp.Helper.System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TestDemoApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static VisualElement VisualElements;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //初始化缓存文件夹
            CacheImageManageHelper.CacheImageManageInitialization();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            AppSettings.Current.AddCommand<SettingsContent>("第一项", new SolidColorBrush(Colors.Red), SettingsFlyout.SettingsFlyoutWidth.Wide);
            AppSettings.Current.AddCommand<SettingsContent>("第二项", SettingsFlyout.SettingsFlyoutWidth.Wide);
            AppSettings.Current.AddCommand<SettingsContent>("第五项", new SolidColorBrush(Colors.Green), SettingsFlyout.SettingsFlyoutWidth.Wide);
            //AppSettings.Current.ResetConfigureSettings();

            //测试放在一起一个时机
            SystemSettingHelper.Instance.Init();

            VisualElements = await AppManifestHelper.GetManifestVisualElementsAsync();

            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            var localsettings = ApplicationData.Current.LocalSettings;
            var tile = new NotificationTileUpdateTaskRegistration();
            tile.CreateTileUpdateTasks(localsettings.CreateContainer(NotificationTileConstants.TaskSettingsContainer,
                                                                     ApplicationDataCreateDisposition.Always));

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            if (!rootFrame.Navigate(typeof(BlankPage)))
            {
                throw new Exception("Failed to create initial page");
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
