using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DelayTimer();
        }

        /// <summary>
        /// 延迟定时器
        /// </summary>
        private void DelayTimer()
        {
            //设置延迟定时器
            ThreadPoolTimer tptimer = ThreadPoolTimer.CreateTimer(async (timer) =>
            {
                await Dispatcher.RunAsync(
                    CoreDispatcherPriority.High, () =>
                    {
                        this.probar1.Value += 20;
                    });
            }, TimeSpan.FromMilliseconds(3000));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PeriodicTimer();
        }

        /// <summary>
        /// 循环定时器
        /// </summary>
        private void PeriodicTimer()
        {
            //循环定时器
            ThreadPoolTimer tptimer = ThreadPoolTimer.CreatePeriodicTimer(
                async (timer) =>
                {
                    await Dispatcher.RunAsync(
                        CoreDispatcherPriority.High, () =>
                        {
                            this.probar1.Value = this.probar1.Value + 1;
                        });
                },
                TimeSpan.FromMilliseconds(100));
        }
    }
}
