using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class BlankPage6 : Page
    {
        public BlankPage6()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage6_Loaded;
        }

        void BlankPage6_Loaded(object sender, RoutedEventArgs e)
        {
            //this.pictureRotatorHubTile.UpdateInterval = TimeSpan.FromSeconds(3);
            pictureRotatorHubTile.UpdateInterval = new TimeSpan(0, 0, 0, 3);
            ObservableCollection<string> imageSources = new ObservableCollection<string>();
            //imageSources.Add("http://ww4.sinaimg.cn/bmiddle/6cacb4ebjw1drurw8kjftj.jpg");
            //imageSources.Add("http://ww4.sinaimg.cn/bmiddle/a1eadd4agw1drvj66k58dj.jpg");
            //imageSources.Add("http://ww1.sinaimg.cn/bmiddle/7ee46e25gw1drvj5po6nfj.jpg");
            //imageSources.Add("http://ww4.sinaimg.cn/bmiddle/6cef4748jw1drvil3vyw4j.jpg");
            //imageSources.Add("http://ww1.sinaimg.cn/bmiddle/8a52b9a0jw1drxyc384o7j.jpg");
            //imageSources.Add("http://ww3.sinaimg.cn/bmiddle/4ada9d17gw1drxy77g740j.jpg");
            imageSources.Add("/Images/HubTile/comment1.png");
            imageSources.Add("/Images/HubTile/comment2.png");
            imageSources.Add("/Images/HubTile/comment3.png");
            this.pictureRotatorHubTile.ImageSources = imageSources;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void bt_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void bt1_Click_1(object sender, RoutedEventArgs e)
        {
            NewGuide.CreateXmlFile();
        }

        private async void bt2_Click_1(object sender, RoutedEventArgs e)
        {
            tbk.Text = (await NewGuide.SelectXmlFile()).ToString();
        }

        private void bt3_Click_1(object sender, RoutedEventArgs e)
        {
            NewGuide.EditXmlFile();
        }

        private void bt4_Click_1(object sender, RoutedEventArgs e)
        {
            NewGuide.ResetEditXmlFile();
        }
    }
}
