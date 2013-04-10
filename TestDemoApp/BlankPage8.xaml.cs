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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using ControlLibrary;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage8 : Page
    {
        Uri[] uris;
        ObservableCollection<MatUriModel> matUriModel = null;
        public BlankPage8()
        {
            this.InitializeComponent();
            this.Loaded += BlankPage8_Loaded;
            matUriModel = new ObservableCollection<MatUriModel>();
            uris = new Uri[8];

            for (int i = 0; i < 8; i++)
            {
                uris[i] = new Uri(this.BaseUri,"/Images/SlideView/Images/Transitions/transitions-" + (i + 1) + ".jpg");
                matUriModel.Add(new MatUriModel(new Uri(this.BaseUri,"/Images/SlideView/Images/Transitions/transitions-" + (i + 1) + ".jpg")));
            }

            image.Source = new BitmapImage(new Uri(this.BaseUri,"/Images/SlideView/Images/Transitions/transitions-2.jpg"));
        }

        void BlankPage8_Loaded(object sender, RoutedEventArgs e)
        {
            this.slideView.DataContext = uris;
            this.slideView.TransitionMode = ControlLibrary.SlideViewTransitionMode.Flip;
            slideView.ItemsSource = uris;
            slideView.ItemsSource = matUriModel;
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

        private void imageS_ImageOpened_1(object sender, RoutedEventArgs e)
        {
            var ss = ((sender as Image).Source as BitmapImage).UriSource;
        }
    }

    public class MatUriModel
    {
        public MatUriModel()
        { 
            
        }

        public MatUriModel(Uri uri)
        {
            this.Uri = uri;
        }
        Uri Uri
        { get; set; }
    }
}
