using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ControlLibrary.Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NotificationsExtensions.ToastContent;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage_Loaded;
        }

        void BlankPage_Loaded(object sender, RoutedEventArgs e)
        {
            StringBuilder photoList = new StringBuilder();
            string photoUrl1 = "http://ww3.sinaimg.cn/bmiddle/7a73a8e1tw1dqdefnrn1gj.jpg";
            photoList.Append(photoUrl1 + Constants.STRING_SEPARATOR);
            string photoUrl2 = "http://ww2.sinaimg.cn/bmiddle/a0f81faajw1dqcbghf844j.jpg";
            photoList.Append(photoUrl2 + Constants.STRING_SEPARATOR);
            string photoUrl3 = "http://ww4.sinaimg.cn/bmiddle/6ad41dc7gw1dqdeje7p4qj.jpg";
            photoList.Append(photoUrl3);
            feedPhotoLiveTileBase.Source = photoList.ToString();
            photoLiveTile.Source = photoList.ToString();
            feedPhotoLiveTile.Source = photoList.ToString();
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
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage2));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage3));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //NotificationHelper.DisplayTextTost("测试", "测试测试", ToastAudioContent.Reminder);
            NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder); 
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage4));
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage5));
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage6));
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage7));
        }

        private void Btclose_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ThemeAnimationPage));
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage8));
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox != null)
            {
                switch (this.comboBox.SelectedIndex)
                {
                    case 0:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Default);
                        break;
                    case 1:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Mail);
                        break;
                    case 2:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.SMS);
                        break;
                    case 3:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.IM);
                        break;
                    case 4:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder);
                        break;
                    default:
                        NotificationHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder);
                        break;
                }
            }
        }

        private void BtSystemCursor_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SystemCursorPage));
        }

        private void BtGifSynthesis_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPageGifSynthesis));
        }

        private void BtCascadingImage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CascadingImageBlankPage));
        }
    }
}
