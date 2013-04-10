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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestDemoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThemeAnimationPage : Page
    {
        public ThemeAnimationPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            downAnimation.Begin();
        }

        private void rectangle_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            upAnimation.Begin();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (cb.SelectedIndex)
            {
                case 0:
                    swipeHintThemeAnimation.Begin();
                    break;
                case 1:
                    swipeBackThemeAnimation.Begin();
                    break;
                case 2:
                    splitCloseThemeAnimation.Begin();
                    break;
                case 3:
                    splitOpenThemeAnimation.Begin();
                    break;
                case 4:
                    repositionThemeAnimation.Begin();
                    break;
                case 5:
                    popOutThemeAnimation.Begin();
                    break;
                case 6:
                    popInThemeAnimation.Begin();
                    break;
                case 7:
                    fadeOutThemeAnimation.Begin();
                    break;
                case 8:
                    fadeInThemeAnimation.Begin();
                    break;
                case 9:
                    dropTargetItemThemeAnimation.Begin();
                    break;
                case 10:
                    dragOverThemeAnimation.Begin();
                    break;
                case 11:
                    dragItemThemeAnimation.Begin();
                    break;
            }

            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
