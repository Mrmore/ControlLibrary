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
using Windows.Storage.Streams;
using ControlLibrary.Tools.Multimedia;
using TestDemoApp.Helper.System;
using NotificationsExtensions.TileContent;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        private Uri baseUri = new Uri("ms-appx:///");

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
            NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder); 
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
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Default);
                        break;
                    case 1:
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Mail);
                        break;
                    case 2:
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.SMS);
                        break;
                    case 3:
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.IM);
                        break;
                    case 4:
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder);
                        break;
                    default:
                        NotificationToastHelper.DisplayWebImageToast("http://ww1.sinaimg.cn/bmiddle/74dcdb86jw1dr58k5x0n6j.jpg", "测试", "测试测试", ToastAudioContent.Reminder);
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

        private void BtMoveOutControl_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MoveOutControlBlankPage));
        }

        private void BtSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void BtRating_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RatingPage));
        }

        private void BtTriggers_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TriggersPage));
        }

        private void BtCropImageControl_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CropImageControlPage));
        }

        private void BtDatePicker_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DatePickerMenuPage));
        }

        private async void BtFlvToMp3OrAac_Click(object sender, RoutedEventArgs e)
        {
            //var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "Video/mp322.flv"));
            var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "Video/aac.flv"));
            var streamRandom = await rass.OpenReadAsync();

            using (YouTubeFlvToMp3OrAac youTubeFlvToMp3OrAac = new YouTubeFlvToMp3OrAac("aac", streamRandom))
            {
                youTubeFlvToMp3OrAac.ExtractStreams();
                youTubeFlvToMp3OrAac.Dispose();
            }
        }

        private async void BtYouTube_Click(object sender, RoutedEventArgs e)
        {
            var youtubeurl = YouTube.GetYouTubeId("http://www.youtube.com/watch?v=eAX85PXl408");
            //获取相对应源的Uri地址
            var url = await YouTube.GetVideoUriAsync(youtubeurl, YouTubeQuality.QualityMP3_FLV_44KHZ);

            //获取所有源的Uri地址列表
            var uriAllList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.All);
            var uriFlvOrMp3List = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Flv);
            var uriMp3OrFlvList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Mp3);
            var uriMp4List = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Mp4);
            var uriQualityList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.YouTubeQuality);
        }

        private void BtShare_Click(object sender, RoutedEventArgs e)
        {
            SystemShareHelper.Instance.Init();
            //http://www.youtube.com/channel/HCp-Rdqh3z4Uc  http://blogs.msdn.com/b/b8/
            SystemShareHelper.Instance.ShowShare("你好啊小陈er", "http://www.youtube.com/channel/HCp-Rdqh3z4Uc", "http://ww3.sinaimg.cn/bmiddle/67c1cd54jw1e5gz0kz5hij20zz14r0xi.jpg", "测试测试测试");
        }

        private void UpdateTile()
        {
            ITileWidePeekImage05 tileContent = TileContentFactory.CreateTileWidePeekImage05();

            tileContent.ImageMain.Src = "http://ww3.sinaimg.cn/bmiddle/643be833jw1e5jg5horgij20dc0hsabq.jpg";
            tileContent.ImageSecondary.Src = "http://ww1.sinaimg.cn/bmiddle/643be833jw1e5jg5ioqhpj20dc0hsmyk.jpg";
            tileContent.TextHeading.Text = "可可曾";
            tileContent.TextBodyWrap.Text = "Mat"+ ":" + "马童";

            /*
            ITileSquareImage squareContent = TileContentFactory.CreateTileSquareImage();

            squareContent.Image.Src = "http://ww4.sinaimg.cn/bmiddle/643be833jw1e5jg5l8dapj20hs0dcwga.jpg";
            squareContent.Image.Alt = "Web image";
            */ 

            ITileSquarePeekImageAndText02 squareImageAndTextContent = TileContentFactory.CreateTileSquarePeekImageAndText02();

            squareImageAndTextContent.Image.Src = "http://ww4.sinaimg.cn/bmiddle/643be833jw1e5jg5l8dapj20hs0dcwga.jpg";
            squareImageAndTextContent.Image.Alt = "Web image";
            squareImageAndTextContent.TextHeading.Text = string.Format("{0:t}", DateTime.Now);
            squareImageAndTextContent.TextBodyWrap.Text = "Here is some text that is displayed on the peek";

            // include the square template.
            //tileContent.SquareContent = squareContent;
            tileContent.SquareContent = squareImageAndTextContent;

            // send the notification to the app's application tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
        }

        private void BtTile_Click(object sender, RoutedEventArgs e)
        {
            UpdateTile();
        }
    }
}
