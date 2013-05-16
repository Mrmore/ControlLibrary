using ControlLibrary.Effects;
using ControlLibrary.GifSynthesis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
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
        private WriteableBitmap wbOriginal = null;
        public MoveOutControlBlankPage()
        {
            this.InitializeComponent();
            this.Loaded += MoveOutControlBlankPage_Loaded;
        }

        private async void MoveOutControlBlankPage_Loaded(object sender, RoutedEventArgs e)
        {
            RandomAccessStreamReference rass = RandomAccessStreamReference.CreateFromUri(new Uri("http://ww1.sinaimg.cn/bmiddle/643be833jw1e4nzv4dc12j20dc0hsq4g.jpg", UriKind.RelativeOrAbsolute));
            IRandomAccessStreamWithContentType streamRandom = await rass.OpenReadAsync();
            var cloneStream = streamRandom.CloneStream();
            wb = new WriteableBitmap(1, 1);
            //wb = await (new WriteableBitmap(1, 1).FromStream(streamRandom));
            await wb.SetSourceAsync(streamRandom);
            //this.image.Source = wb;
            wbOriginal = WriteableBitmapExpansion.CopyWriteableBitmap(wb);
            var bi = new BitmapImage();
            await bi.SetSourceAsync(cloneStream);
            this.imageOriginal.Source = bi;

            this.image.Source = BlurEffect.WriteableBitmapBlur(wb, Convert.ToInt32(sliderRadius.Value), sliderSigma.Value);
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
            this.image.Source = BlurEffect.WriteableBitmapBlur(wb, null);
            //this.image.Source = BlurEffect.WriteableBitmapBlur(wb, Convert.ToInt32(this.tb.Text));       
            //this.image.Source = BlurEffect.WriteableBitmapBlur(wb);
        }

        private void sliderRadius_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderRadius != null && sliderSigma != null)
            {
                this.image.Source = BlurEffect.WriteableBitmapBlur(wb, Convert.ToInt32(e.NewValue), sliderSigma.Value);
                wb.Invalidate();
            }
        }

        private void sliderSigma_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderRadius != null && sliderSigma != null)
            {
                this.image.Source = BlurEffect.WriteableBitmapBlur(wb, Convert.ToInt32(sliderSigma.Value), e.NewValue);
                wb.Invalidate();
            }
        }

        private void btOriginal_Click(object sender, RoutedEventArgs e)
        {
            this.image.Source = this.wbOriginal;
        }

        private void btGray_Click(object sender, RoutedEventArgs e)
        {
            this.image.Source = GrayEffect.GrayProcess(this.wb);
        }

        private void sliderMosaic_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderMosaic != null)
            {
                this.image.Source = MosaicEffects.MosaicProcess(this.wb, System.Convert.ToInt32(e.NewValue));
            }
        }

        private void btNoctilucent_Click(object sender, RoutedEventArgs e)
        {
            this.image.Source = NoctilucentEffect.NoctilucentProcess(this.wb);
        }

        private void sliderMotionblurOffset_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.image.Source = MotionblurEffect.MotionblurProcess(this.wb, System.Convert.ToInt32(e.NewValue), 1);
        }

        private void comboBoxMotionblurDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxMotionblurDirection != null)
            {
                int direction = comboBoxMotionblurDirection.SelectedIndex + 1;
                this.image.Source = MotionblurEffect.MotionblurProcess(this.wb, System.Convert.ToInt32(sliderMotionblurOffset.Value), direction);
            }
        }
    }
}
