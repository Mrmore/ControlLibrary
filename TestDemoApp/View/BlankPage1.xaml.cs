using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ControlLibrary.Tools;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BlankPage1 : Page
    {
        private List<string> albumImage = null;
        private List<string> albumImage2 = null;
        private bool isOne = false;
        public BlankPage1()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage1_Loaded;
        }

        void BlankPage1_Loaded(object sender, RoutedEventArgs e)
        {
            albumImage = new List<string>();
            albumImage.Add("http://ww4.sinaimg.cn/bmiddle/61d542e4jw1dqp5xqb2nkj.jpg");
            albumImage.Add("http://ww1.sinaimg.cn/bmiddle/63631fd9jw1dqp5zdeqouj.jpg");

            albumImage2 = new List<string>();
            albumImage2.Add("http://ww4.sinaimg.cn/bmiddle/8c74f3dcjw1dr7lu81zmoj.jpg");
            albumImage2.Add("http://ww2.sinaimg.cn/bmiddle/633e8cf8tw1drtv2q9j5dj.jpg");

            albumLiveTile.AlbumList = albumImage;
            isOne = true;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isOne)
            {
                albumLiveTile.AlbumList = albumImage2;
                isOne = false;
            }
            else
            {
                albumLiveTile.AlbumList = albumImage;
                isOne = true;
            }
            
        }

        private void bt_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
