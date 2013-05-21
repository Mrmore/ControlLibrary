using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class BlankPage2 : Page
    {
        public BlankPage2()
        {
            this.InitializeComponent();
            popupGrid.IsZoom = false;
            popupGrid.OverlayClickComplete += popupGrid_OverlayClickComplete;
            popupGrid.IsAnimation = false;
            popupGrid.AnimationOpenComplete += popupGrid_AnimationOpenComplete;
            popupBorder.IsZoom = false;
            popupBorder.OverlayClickComplete += popupBorder_OverlayClickComplete;
        }

        void popupGrid_AnimationOpenComplete()
        {
            btOK.Content = "OK";
        }

        void popupBorder_OverlayClickComplete()
        {
            popupBorder.IsOpen = false;
        }

        void popupGrid_OverlayClickComplete()
        {
            popupGrid.IsOpen = false;
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
            //popupGrid.OverlayBrush = new SolidColorBrush(Colors.Red);
            //popupGrid.AnimationOpen();
            this.popupGrid.IsOpen = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //popupGrid.AnimationClose();
            this.popupGrid.IsOpen = false;
        }

        private void bt_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void bt_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            popupGrid.IsZoom = false;
            popupBorder.IsZoom = false;
        }

        private void bt_Copy1_Click_1(object sender, RoutedEventArgs e)
        {
            popupGrid.IsZoom = true;
            popupBorder.IsZoom = true;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.popupBorder.IsOpen = true;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.popupBorder.IsOpen = false;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            popupGrid.IsAnimation = false;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            popupGrid.IsAnimation = true;
        }

        private void btOK_Click_1(object sender, RoutedEventArgs e)
        {
            popupGrid.IsAnimation = !popupGrid.IsAnimation;
        }
    }
}
