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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage5 : Page
    {
        private bool isOpacity = true;
        private bool isClip = false;

        public BlankPage5()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.Loaded += BlankPage5_Loaded;
        }

        void BlankPage5_Loaded(object sender, RoutedEventArgs e)
        {
            string ss = "cao";
            cacheImage.ImageDownLoadProgress += cacheImage_ImageDownLoadProgress;
        }

        void cacheImage_ImageDownLoadProgress(int downloadValue)
        {
            progressBar.Value = downloadValue;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            clipImage.Source = new BitmapImage(new Uri(tb.Text, UriKind.RelativeOrAbsolute));
            //clipImage.Source = new BitmapImage(new Uri("http://ww3.sinaimg.cn/bmiddle/66e70327jw1dun8wdc8vxj.jpg", UriKind.RelativeOrAbsolute));
            gridImageBrush.ImageSource = new BitmapImage(new Uri(tb.Text, UriKind.RelativeOrAbsolute));
            cacheImage.Source = "http://ww3.sinaimg.cn/bmiddle/66e70327jw1dun8wdc8vxj.jpg";//tb.Text;
            clipImageBrush.Source = tb.Text;
        }

        private void bt1_Click_1(object sender, RoutedEventArgs e)
        {
            if (isOpacity)
            {
                this.grid.Opacity = 1;
                isOpacity = false;
            }
            else
            {
                this.grid.Opacity = 0;
                isOpacity = true;
            }
        }

        private void bt2_Click_1(object sender, RoutedEventArgs e)
        {
            //if (isClip)
            //{
            //    clipImage.IsClipImage = true;
            //    isClip = false;
            //}
            //else
            //{ 
            //    clipImage.IsClipImage = false;
            //    isClip = true;
            //}
            clipImage.IsClipImage = !clipImage.IsClipImage;
        }

        private void bt3_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BP5CeShi));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox != null)
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        clipImageBrush.Stretch = Stretch.None;
                        gridImageBrush.Stretch = Stretch.None;
                        break;
                    case 1:
                        clipImageBrush.Stretch = Stretch.Fill;
                        gridImageBrush.Stretch = Stretch.Fill;
                        break;
                    case 2:
                        clipImageBrush.Stretch = Stretch.Uniform;
                        gridImageBrush.Stretch = Stretch.Uniform;
                        break;
                    case 3:
                        clipImageBrush.Stretch = Stretch.UniformToFill;
                        gridImageBrush.Stretch = Stretch.UniformToFill;
                        break;
                }
            }
        }

        private async void bt7_Click_1(object sender, RoutedEventArgs e)
        {
            if (await cacheImage.SaveImage())
            {
                bt7.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                bt7.Visibility = Windows.UI.Xaml.Visibility.Visible;
            } 
        }

        private void cacheImage_ImageOpened_1(object sender, RoutedEventArgs e)
        {
            bt7.IsEnabled = true;
            bt8.IsEnabled = true;
        }

        private async void bt8_Click_1(object sender, RoutedEventArgs e)
        {
            if (await cacheImage.SaveFileImage())
            {
                bt8.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                bt8.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void bt_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage9));
        }
    }
}
