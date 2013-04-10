using ControlLibrary.GifSynthesis;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BlankPageGifSynthesis : Page
    {
        private Uri baseUri = new Uri("ms-appx:///");
        public BlankPageGifSynthesis()
        {
            this.InitializeComponent();
            this.Loaded += BlankPageGifSynthesis_Loaded;
        }

        private async void BlankPageGifSynthesis_Loaded(object sender, RoutedEventArgs e)
        {
            await ControlLibrary.CacheManagement.CacheManagement.Instance.DelectImageLocalFolder();
            List<Uri> uriList = new List<Uri>();
            uriList.Add(new Uri(this.baseUri, "Gif/6.jpg"));
            uriList.Add(new Uri(this.baseUri, "Gif/0.png"));
            uriList.Add(new Uri(this.baseUri, "Gif/1.png"));
            uriList.Add(new Uri(this.baseUri, "Gif/2.png"));
            uriList.Add(new Uri(this.baseUri, "Gif/3.png"));
            uriList.Add(new Uri(this.baseUri, "Gif/5.jpg"));
            //uriList.Add(new Uri(this.baseUri, "Gif/6.jpg"));

            //image.Source = new BitmapImage(uriList[1]);

            //this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            //{
            //    await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //});

            //Task.Run(async () =>
            //{
            //    //this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            //    //    {
            //    await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //    //});
            //});

            //Task.Factory.StartNew(async () =>
            //    {
            //        await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //    });

            //await GifSynthesisHelper.SaveSynthesisGif(uriList);

            //var UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();
            //Task serverTask = Task.Run(async () =>
            //{
            //    await GifSynthesisHelper.SynthesisGif(uriList);
            //});
            //Task uiTask = serverTask.ContinueWith(async (t) =>
            //{
            //    await GifSynthesisHelper.SaveSynthesisGif();
            //}, UISyncContext);

            //Task.Factory.StartNew(() =>
            //{
            //    this.Dispatcher.RunIdleAsync(async (t) =>
            //        {
            //            await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //        });
            //});

            //Task.Run(async () =>
            //{
            //    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //        CoreDispatcherPriority.Normal, async () => 
            //    {
            //        await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //    });
            //});

            //Task.Factory.StartNew(async () =>
            //{
            //    Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //        CoreDispatcherPriority.High, async () =>
            //    {
            //        await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //    });               
            //});

            //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //        CoreDispatcherPriority.High, async () =>
            //    {
            //        await GifSynthesisHelper.SaveSynthesisGif(uriList);
            //    });

            //await Task.Run(async () =>
            //{
            //    await GifSynthesisHelper.SynthesisGif(uriList);
            //});
            //await GifSynthesisHelper.SaveSynthesisGif();

            await GifSynthesisHelper.SaveSynthesisGif(uriList);
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
