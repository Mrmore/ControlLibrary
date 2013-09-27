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
using System.Diagnostics;
using Windows.Storage;
using ControlLibrary.GifSynthesis;

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
            var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "Video/mp322.flv"));
            //var rass = RandomAccessStreamReference.CreateFromUri(new Uri(this.baseUri, "Video/aac.flv"));
            var streamRandom = await rass.OpenReadAsync();

            using (YouTubeFlvToMp3OrAac youTubeFlvToMp3OrAac = new YouTubeFlvToMp3OrAac("mp322", streamRandom))
            {
                youTubeFlvToMp3OrAac.ExtractStreams();
                youTubeFlvToMp3OrAac.Dispose();
            }

            /*
            RandomAccessStreamReference ras = RandomAccessStreamReference.CreateFromUri(new Uri(baseUri,"Video/mp322.flv"));
            var streamRandom = await ras.OpenReadAsync();
           
            var localFolder = KnownFolders.MusicLibrary;
            var file = await localFolder.CreateFileAsync("mp322.flv", CreationCollisionOption.OpenIfExists);
            byte[] bytes = streamRandom.ConvertIRandomAccessStreamToByte();
            await FileIO.WriteBytesAsync(file, bytes);
            using (YouTubeFlvToMp3OrAac youTubeFlvToMp3OrAac = new YouTubeFlvToMp3OrAac(file, false))
            {
                youTubeFlvToMp3OrAac.ExtractStreams();
                youTubeFlvToMp3OrAac.Dispose();
            }
            */ 
        }

        private async void BtYouTube_Click(object sender, RoutedEventArgs e)
        {
            var youtubeurl = YouTube.GetYouTubeId("http://www.youtube.com/watch?v=eAX85PXl408");
            try
            {
                //获取相对应源的Uri地址
                //普通 视频测试
                //var url = await YouTube.GetVideoUriAsync(youtubeurl, YouTubeQuality.QualityMP3_FLV_44KHZ);
                //长视频 例如金瓶梅
                //var url = await YouTube.GetVideoUriAsync("rtD-8mQnzRs", YouTubeQuality.Quality720P_MP4);
                //vevo 视频测试
                var url = await YouTube.GetVideoUriAsync("LrUvu1mlWco", YouTubeQuality.Quality720P_MP4);
                me.Source = url.Uri;
                me.Play();
                
                //获取所有源的Uri地址列表
                var uriAllList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.All);
                var uriFlvOrMp3List = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Flv);
                var uriMp3OrFlvList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Mp3);
                var uriMp4List = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.Mp4);
                var uriQualityList = await YouTube.GetVideoAllUrisAsync(youtubeurl, YouTubeFormat.YouTubeQuality);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "无法连接到远程服务" + "Unable to connect to the remote serve");
            }
        }

        private void BtShare_Click(object sender, RoutedEventArgs e)
        {
            SystemShareHelper.Instance.Init();
            //http://www.youtube.com/channel/HCp-Rdqh3z4Uc  http://blogs.msdn.com/b/b8/
            SystemShareHelper.Instance.ShowShare("你好啊小陈er", "http://www.youtube.com/channel/HCp-Rdqh3z4Uc", "http://ww3.sinaimg.cn/bmiddle/67c1cd54jw1e5gz0kz5hij20zz14r0xi.jpg", "测试测试测试");

            //NotificationTileHelper.UpdateTileWithImages(GetNotificationTileList2());      
        }

        private void UpdateTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
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

        private List<NotificationTile> GetNotificationTileList()
        {
            List<NotificationTile> ntList = new List<NotificationTile>();
            NotificationTile nt = new NotificationTile();
            nt.ImageUri = "http://ww3.sinaimg.cn/bmiddle/643be833jw1e5jg5horgij20dc0hsabq.jpg";
            nt.ImageAltName = "美女";
            nt.NotificationTileTag = "mat";
            nt.TextHeading = "标题";
            nt.TextBodyWrap = "内容内容内容内容内容内容内容内容";
            ntList.Add(nt);

            NotificationTile nt1 = new NotificationTile();
            nt1.ImageUri = "http://ww4.sinaimg.cn/bmiddle/6cef4748jw1e5l83hufyvj21kw2cn4qq.jpg";
            nt1.ImageAltName = "美女1";
            nt1.NotificationTileTag = "mat1";
            nt1.TextHeading = "标题1";
            nt1.TextBodyWrap = "内容1内容1内容1内容1内容1内容1内容1内容1";
            ntList.Add(nt1);

            NotificationTile nt2 = new NotificationTile();
            nt2.ImageUri = "http://ww3.sinaimg.cn/bmiddle/7f811e76jw1e4zjsf7ksuj20qa0yotcw.jpg";
            nt2.ImageAltName = "美女2";
            nt2.NotificationTileTag = "mat2";
            nt2.TextHeading = "标题2";
            nt2.TextBodyWrap = "内容2内容2内容2内容2内容2内容2内容2内容2";
            ntList.Add(nt2);

            NotificationTile nt3 = new NotificationTile();
            nt3.ImageUri = "http://ww2.sinaimg.cn/bmiddle/7f811e76jw1e4xmts5ay0j20dc0hs75c.jpg";
            nt3.ImageAltName = "美女3";
            nt3.NotificationTileTag = "mat3";
            nt3.TextHeading = "标题3";
            nt3.TextBodyWrap = "内容3内容3内容3内容3内容3内容3内容3内容3";
            ntList.Add(nt3);

            NotificationTile nt4 = new NotificationTile();
            nt4.ImageUri = "http://ww4.sinaimg.cn/bmiddle/7f811e76jw1e4rcc8f54oj20lb0sg0vi.jpg";
            nt4.ImageAltName = "美女4";
            nt4.NotificationTileTag = "mat4";
            nt4.TextHeading = "标题4";
            nt4.TextBodyWrap = "内容4内容4内容4内容4内容4内容4内容4内容4";
            ntList.Add(nt4);

            return ntList;
        }

        private List<NotificationTile> GetNotificationTileList2()
        {
            List<NotificationTile> ntList = new List<NotificationTile>();
            NotificationTile nt = new NotificationTile();
            nt.ImageUri = "http://ww2.sinaimg.cn/bmiddle/6a03cfedjw1e6i31q4a5jj20d40hsq4x.jpg";
            nt.ImageAltName = "美女";
            nt.NotificationTileTag = "mat";
            nt.TextHeading = "标题";
            nt.TextBodyWrap = "内容内容内容内容内容内容内容内容";
            ntList.Add(nt);

            NotificationTile nt1 = new NotificationTile();
            nt1.ImageUri = "http://ww2.sinaimg.cn/bmiddle/49ce04b7jw1e6i2u2v5swj20k00qo408.jpg";
            nt1.ImageAltName = "美女1";
            nt1.NotificationTileTag = "mat1";
            nt1.TextHeading = "标题1";
            nt1.TextBodyWrap = "内容1内容1内容1内容1内容1内容1内容1内容1";
            ntList.Add(nt1);

            NotificationTile nt2 = new NotificationTile();
            nt2.ImageUri = "http://ww2.sinaimg.cn/bmiddle/636b6f59jw1e6i2gxhco8j20hs0nptc0.jpg";
            nt2.ImageAltName = "美女2";
            nt2.NotificationTileTag = "mat2";
            nt2.TextHeading = "标题2";
            nt2.TextBodyWrap = "内容2内容2内容2内容2内容2内容2内容2内容2";
            ntList.Add(nt2);

            NotificationTile nt3 = new NotificationTile();
            nt3.ImageUri = "http://ww3.sinaimg.cn/bmiddle/6ac1a4b8jw1e6i2wfucy8j20xc18ggz9.jpg";
            nt3.ImageAltName = "美女3";
            nt3.NotificationTileTag = "mat3";
            nt3.TextHeading = "标题3";
            nt3.TextBodyWrap = "内容3内容3内容3内容3内容3内容3内容3内容3";
            ntList.Add(nt3);

            NotificationTile nt4 = new NotificationTile();
            nt4.ImageUri = "http://ww3.sinaimg.cn/bmiddle/6ac1a4b8jw1e6i2wje4suj20xc18gtlr.jpg";
            nt4.ImageAltName = "美女4";
            nt4.NotificationTileTag = "mat4";
            nt4.TextHeading = "标题4";
            nt4.TextBodyWrap = "内容4内容4内容4内容4内容4内容4内容4内容4";
            ntList.Add(nt4);

            return ntList;
        }


        private NotificationTile GetNotificationTile()
        {
            NotificationTile nt = new NotificationTile();
            nt.ImageUri = "http://ww3.sinaimg.cn/bmiddle/643be833jw1e5jg5horgij20dc0hsabq.jpg";
            nt.ImageAltName = "美女";
            nt.NotificationTileTag = "mat";
            nt.TextHeading = "标题";
            nt.TextBodyWrap = "内容内容内容内容内容内容内容内容";
            return nt;
        }

        private void BtTile_Click(object sender, RoutedEventArgs e)
        {
            //UpdateTile();
            //NotificationTileHelper.UpdateTileWithImage(GetNotificationTile());
            NotificationTileHelper.UpdateTileWithImages(GetNotificationTileList());           
        }

        private async void BtPin_Click(object sender, RoutedEventArgs e)
        {
            //await NotificationSecondaryTileHelper.PinSecondaryTileWithImage("mat1", GetNotificationTile());
            await NotificationSecondaryTileHelper.PinSecondaryTileWithImages("mats", GetNotificationTileList());
        }

        private async void BtUnPin_Click(object sender, RoutedEventArgs e)
        {
            //await NotificationSecondaryTileHelper.UnPinSecondaryTileWithImage("mat1");
            await NotificationSecondaryTileHelper.UnPinSecondaryTileWithImage("mats");
        }

        private void BtCasImage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CascadingImagePage));
        }
    }
}
