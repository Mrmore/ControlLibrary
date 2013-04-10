using ControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage4 : Page
    {
        public BlankPage4()
        {
            this.InitializeComponent();
            //mosaicTile.UpdateInterval = new TimeSpan(0, 0, 0, 5);
            mosaicTile.UpdateInterval = TimeSpan.FromSeconds(3);
            ObservableCollection<string> imageSources = new ObservableCollection<string>();
            imageSources.Add("http://ww4.sinaimg.cn/bmiddle/6cacb4ebjw1drurw8kjftj.jpg");
            imageSources.Add("http://ww4.sinaimg.cn/bmiddle/a1eadd4agw1drvj66k58dj.jpg");
            imageSources.Add("http://ww1.sinaimg.cn/bmiddle/7ee46e25gw1drvj5po6nfj.jpg");
            imageSources.Add("http://ww4.sinaimg.cn/bmiddle/6cef4748jw1drvil3vyw4j.jpg");
            imageSources.Add("http://ww1.sinaimg.cn/bmiddle/8a52b9a0jw1drxyc384o7j.jpg");
            imageSources.Add("http://ww3.sinaimg.cn/bmiddle/4ada9d17gw1drxy77g740j.jpg");         
            mosaicTile.ImageSources = imageSources;

            this.Loaded += BlankPage4_Loaded;
        }

        private void BlankPage4_Loaded(object sender, RoutedEventArgs e)
        {
            InitPathMenu();
        }

        private void InitPathMenu()
        {
            Rect rc = new Rect { Width = 400, Height = 400 };
            var items = new List<AwesomMenuItem>();

            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/AwesomMenu/icon-star.png", "Images/AwesomMenu/bg-menuitem.png"));

            //构造的时候可以设置指定方法也可以通过方法来设置，都可以
            var menu = new AwesomeMenu(rc, items, "Images/AwesomMenu/icon-plus.png", "Images/AwesomMenu/bg-addbutton.png", AwesomeMenuType.AwesomeMenuTypeUpAndRight);

            menu.SetType(AwesomeMenuType.AwesomeMenuTypeUpAndRight);
            menu.SetStartPoint(new Point(-50, 50));
            menu.AwesomeMenuRadianType = AwesomeMenuRadianType.AwesomeMenuRadianNone;
            menu.MenuItemSpacing = 0;
            menu.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            menu.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            menu.Margin = new Thickness(50, 400, 0, 0);
            grid.Children.Add(menu);
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
