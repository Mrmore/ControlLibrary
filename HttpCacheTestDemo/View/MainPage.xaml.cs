using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRtHttpHelper;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace HttpCacheTestDemo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        HttpDownLoadContinueHelper hdlch = null;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            hdlch = new HttpDownLoadContinueHelper();
            hdlch.DownLoadChanging += new EventHandler<WinRtHttpHelper.Data.DownLoadChangingEventArgs>(DownLoadChanging);
        }

        private async void DownLoadChanging(object sender, WinRtHttpHelper.Data.DownLoadChangingEventArgs e)
        {
            pBar.Value = e.Progress;
            if (e.Progress == 100)
            {
                InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
                datawriter.WriteBytes(e.Bytes);
                await datawriter.StoreAsync();
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(memoryStream);
                image.Source = bi;
            }
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        //开始下载
        private async void btS_Click(object sender, RoutedEventArgs e)
        {
            var ss = await hdlch.StartDownload(new Uri("http://ww4.sinaimg.cn/bmiddle/636b6f59jw1e2exg8yk5pj.jpg", UriKind.RelativeOrAbsolute));
        }

        //暂停下载
        private async void btP_Click(object sender, RoutedEventArgs e)
        {
            if (hdlch != null)
            {
                var ss = await hdlch.SuspendDownload();
            }
        }

        //继续下载
        private async void btC_Click(object sender, RoutedEventArgs e)
        {
            if (hdlch != null)
            {
                await hdlch.ContinueDownload(new Uri("http://ww4.sinaimg.cn/bmiddle/636b6f59jw1e2exg8yk5pj.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        private void btbg_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BackgroundDownloaderPage));
        }
    }
}
