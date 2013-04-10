using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ControlLibrary;
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
    public sealed partial class BlankPage7 : Page
    {
        public BlankPage7()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage7_Loaded;
        }

        void BlankPage7_Loaded(object sender, RoutedEventArgs e)
        {
            //this.pictureRotatorHubTile.UpdateInterval = TimeSpan.FromSeconds(3);
            titleImageRotatorHubTile.UpdateInterval = new TimeSpan(0, 0, 0, 3);
            ObservableCollection<TitleImage> imageSources = new ObservableCollection<TitleImage>();
            TitleImage titleImage1 = new TitleImage() { Title = "美女1", Content = "是个大漂亮姑娘1", ImageUri = "http://ww4.sinaimg.cn/bmiddle/6cacb4ebjw1drurw8kjftj.jpg" };
            imageSources.Add(titleImage1);
            TitleImage titleImage2 = new TitleImage() { Title = "美女2", Content = "是个大漂亮姑娘2", ImageUri = "http://ww4.sinaimg.cn/bmiddle/a1eadd4agw1drvj66k58dj.jpg" };
            imageSources.Add(titleImage2);
            TitleImage titleImage3 = new TitleImage() { Title = "美女3", Content = "是个大漂亮姑娘3", ImageUri = "http://ww1.sinaimg.cn/bmiddle/7ee46e25gw1drvj5po6nfj.jpg" };
            imageSources.Add(titleImage3);
            TitleImage titleImage4 = new TitleImage() { Title = "美女4", Content = "是个大漂亮姑娘4", ImageUri = "http://ww4.sinaimg.cn/bmiddle/6cef4748jw1drvil3vyw4j.jpg" };
            imageSources.Add(titleImage4);
            TitleImage titleImage5 = new TitleImage() { Title = "美女5", Content = "是个大漂亮姑娘5", ImageUri = "http://ww1.sinaimg.cn/bmiddle/8a52b9a0jw1drxyc384o7j.jpg" };
            imageSources.Add(titleImage5);
            TitleImage titleImage6 = new TitleImage() { Title = "美女6", Content = "是个大漂亮姑娘6", ImageUri = "http://ww3.sinaimg.cn/bmiddle/4ada9d17gw1drxy77g740j.jpg" };
            imageSources.Add(titleImage6);

            //TitleImage titleImage1 = new TitleImage() { Title = "美女1", Content = "是个大漂亮姑娘1", ImageUri = "/Images/HubTile/comment1.png" };
            //imageSources.Add(titleImage1);
            //TitleImage titleImage2 = new TitleImage() { Title = "美女2", Content = "是个大漂亮姑娘2", ImageUri = "/Images/HubTile/comment2.png" };
            //imageSources.Add(titleImage2);
            //TitleImage titleImage3 = new TitleImage() { Title = "美女3", Content = "是个大漂亮姑娘3", ImageUri = "/Images/HubTile/comment3.png" };
            //imageSources.Add(titleImage3);
            this.titleImageRotatorHubTile.ImageSources = imageSources;
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
    }
}
