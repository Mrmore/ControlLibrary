using ControlLibrary.Effects;
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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MoveOutControlBlankPage : Page
    {
        private WriteableBitmap wb = null;
        public MoveOutControlBlankPage()
        {
            this.InitializeComponent();
            this.Loaded += MoveOutControlBlankPage_Loaded;
        }

        private async void MoveOutControlBlankPage_Loaded(object sender, RoutedEventArgs e)
        {
            RandomAccessStreamReference rass = RandomAccessStreamReference.CreateFromUri(new Uri("http://ww1.sinaimg.cn/bmiddle/643be833jw1e4nzv4dc12j20dc0hsq4g.jpg", UriKind.RelativeOrAbsolute));
            IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
            //wb = new WriteableBitmap(1, 1);
            wb = await (new WriteableBitmap(1, 1).FromStream(streamRandom));
            //await wb.SetSourceAsync(streamRandom);
            this.image.Source = wb;
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

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            //BlurEffect.WriteableBitmapBlur(wb, null);
            BlurEffect.WriteableBitmapBlur(wb, 55);
        }
    }
}
