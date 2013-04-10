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
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Media3D;
using ControlLibrary.CacheManagement;
using ControlLibrary;
using Windows.Storage.Streams;
using ControlLibrary.Tools.CompressionGif;
using WinRt.Net;
using System.Threading;
using WinRtHttpHelper;
using Windows.UI;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CeShiGifPage : Page
    {
        private Uri baseUri = new Uri("ms-appx://");

        public CeShiGifPage()
        {
            this.InitializeComponent();
            this.Loaded += CeShiGifPage_Loaded;
        }

        async void CeShiGifPage_Loaded(object sender, RoutedEventArgs e)
        {
            colorPicker1.Color = (grid.Background as SolidColorBrush).Color;
            TextBlock tb = new TextBlock();
            tb.IsTextSelectionEnabled = true;
            Image image = new Image();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BitmapImage bi = new BitmapImage(new Uri(this.tb.Text, UriKind.RelativeOrAbsolute));
            image.Source = bi;
            image.ImageOpened += async (ss, ee) =>
            {
                //while (true)
                //{
                //    await Task.Delay(TimeSpan.FromSeconds(0.2));
                //    image.PlayToSource.PlayNext();
                //}
            };

            imageGif.Source = this.tb.Text;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            imageGif.Stop = true;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "http://ww1.sinaimg.cn/bmiddle/86b379b4jw1dw6kt25rgrg.gif";
            imageGif.Source = this.tb.Text;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "http://ww1.sinaimg.cn/bmiddle/69cb4e25jw1dwdgxhaurlg.gif";
            imageGif.Source = this.tb.Text;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            imageGif.Stop = false;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "http://ww1.sinaimg.cn/bmiddle/91620e28jw1dwdhndaf8cj.jpg";
            imageGif.Source = this.tb.Text;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "Images/amy.jpg";
            imageGif.Source = this.tb.Text;
            image.Source = new BitmapImage(new Uri(this.BaseUri, this.tb.Text));
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            this.tb.Text = "/Images/6dc470d7jw1dse92lrac2g.gif";
            imageGif.Source = this.tb.Text;
        }

        private void colorPicker1_ColorChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (colorPicker1 != null)
            {
                (grid.Background as SolidColorBrush).Color = colorPicker1.Color;
            }
        }

        private void tb_KeyDown_1(object sender, KeyRoutedEventArgs e)
        {
            //- 189  = 187
            var ss = e.Key.ToString();
            if (e.Key == Windows.System.VirtualKey.Add)
            {
                var ss1 = e.Key.ToString();
            }
        }

        private async void btSaveGif_Click(object sender, RoutedEventArgs e)
        {
            var ss = await imageGif.SaveFileImage();
        }

        private void colorBox_ColorChanged_1()
        {
            (grid.Background as SolidColorBrush).Color = colorBox.SelectedColor;
        }

        private async void btFolderSize_Click(object sender, RoutedEventArgs e)
        {
            var list = await CacheManagement.Instance.GetImageLocalFolderFileSizeString();
            if (list.Count > 0)
            {
                btFolderSize.Content = "已占用：" + (await CacheManagement.Instance.GetImageLocalFolderFileSizeString())[0];
            }
            //btFolderSize.Content = "已占用：" + (await CacheManagement.Instance.GetImageLocalFolderAllFileSizeString());
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            CacheBitmapImage.Instance.AddCacheList();
        }

        private void btLoad_Click(object sender, RoutedEventArgs e)
        {
            CacheBitmapImage.Instance.LoadAndSynchronousDictionary();
        }

        private void btceshi_Click(object sender, RoutedEventArgs e)
        {
            imageGif.Source = "/Images/ceshi123.gif";
        }

        private async void Button_Click_10(object sender, RoutedEventArgs e)
        {
            var dtf = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("dayofweek day month year");
            DateTime now = DateTime.Now;
            tbTime.Text = dtf.Format(now);
            //var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "/Images/6dc470d7jw1dse92lrac2g.gif"));
            //var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "Images/amy.jpg"));
            var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "/Images/ceshi123.gif"));
            var streamRandom = await rass.OpenReadAsync();
            await imageGif.SetSourceAsync(streamRandom);
        }

        HttpDownLoadContinueHelper hdlch = null;
        //压缩Gif
        private async void btyasuo_Click(object sender, RoutedEventArgs e)
        {
            List<int> listA = new List<int> { 1, 2, 3, 5, 7, 9 };
            List<int> listB = new List<int> { 13, 4, 17, 29, 2 };
            List<int> ResultA = listA.Union(listB).ToList<int>(); //剔除重复项
            List<int> ResultB = listA.Concat(listB).ToList<int>();//保留重复项
            List<int> ResultC = listA.Union(listB).Except(listB).ToList<int>();
            List<int> ResultD = listA.Union(listB).Except(listA).ToList<int>();

            await Task.Run(async () =>
            {
                //var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "/Gif/123.gif"));
                var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "/Gif/234.gif"));
                var streamRandom = await rass.OpenReadAsync();
                var streamRandomCopy = streamRandom.CloneStream();
                var stream = await CompressionHelper.CompressionGif(streamRandom, 400, 300);
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await imageGif.SetSourceAsync(stream);
                    await gifYaSuo.SetSourceAsync(streamRandomCopy);
                });
            });
            Windows.Networking.BackgroundTransfer.BackgroundDownloader bgd = new Windows.Networking.BackgroundTransfer.BackgroundDownloader();
            var sss = await GetAsyncTaskSample();

            //BitmapImage bi = new BitmapImage();
            //HttpDownLoadHelper hdlh = new HttpDownLoadHelper();
            //var ss = await hdlh.StartDownload(new Uri("http://ww4.sinaimg.cn/bmiddle/636b6f59jw1e2exg8yk5pj.jpg", UriKind.RelativeOrAbsolute));
            //await bi.SetSourceAsync(ss);
            //ceshiImage.Source = bi;
            hdlch = new HttpDownLoadContinueHelper();
            hdlch.DownLoadChanging += new EventHandler<WinRtHttpHelper.Data.DownLoadChangingEventArgs>(DownLoadChanging);
            var ss = await hdlch.StartDownload(new Uri("http://ww4.sinaimg.cn/bmiddle/636b6f59jw1e2exg8yk5pj.jpg", UriKind.RelativeOrAbsolute));
        }

        private async void DownLoadChanging(object sender, WinRtHttpHelper.Data.DownLoadChangingEventArgs e)
        {
            pbar.Foreground = new SolidColorBrush(Colors.Yellow);
            pbar.Value = e.Progress;
            if (e.Progress == 100)
            {
                InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
                datawriter.WriteBytes(e.Bytes);
                await datawriter.StoreAsync();
                BitmapImage bi = new BitmapImage();
                await bi.SetSourceAsync(memoryStream);
                ceshiImage.Source = bi;
            }
        }

        private void btPause_Click(object sender, RoutedEventArgs e)
        {
            if (hdlch != null)
            {
                var ss = hdlch.SuspendDownload();
            }
        }

        private async void btContinue_Click(object sender, RoutedEventArgs e)
        {
            if (hdlch != null)
            {
                await hdlch.ContinueDownload(new Uri("http://ww4.sinaimg.cn/bmiddle/636b6f59jw1e2exg8yk5pj.jpg", UriKind.RelativeOrAbsolute));
            }
        }

        public static Task<string> GetAsyncTaskSample(CancellationToken cancellationToken = default(CancellationToken))
        {
            var httpHelper = new HttpHelper("http://bbs.a9vg.com/forum.php");

            return httpHelper
                .OpenReadTaskAsync(cancellationToken)
                .ContinueWith(t =>
                {
                    // propagate previous task exceptions correctly.
                    if (t.IsFaulted || t.IsCanceled) t.Wait();

                    using (var stream = t.Result)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                });
        }
    }
}
