using ControlLibrary.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace TestDemoApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SystemCursorPage : Page
    {
        public SystemCursorPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void bt_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void combobox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (combobox != null)
            {
                switch (combobox.SelectedIndex)
                {
                    case 0:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Arrow);
                            break;
                        }
                    case 1:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Cross);
                            break;
                        }
                    case 2:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Custom);
                            break;
                        }
                    case 3:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Hand);
                            break;
                        }
                    case 4:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Help);
                            break;
                        }
                    case 5:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.IBeam);
                            break;
                        }
                    case 6:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.SizeAll);
                            break;
                        }
                    case 7:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.SizeNortheastSouthwest);
                            break;
                        }
                    case 8:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.SizeNorthSouth);
                            break;
                        }
                    case 9:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.SizeNorthwestSoutheast);
                            break;
                        }
                    case 10:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.SizeWestEast);
                            break;
                        }
                    case 11:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.UniversalNo);
                            break;
                        }
                    case 12:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.UpArrow);
                            break;
                        }
                    case 13:
                        {
                            rectangle.SetValue(FrameworkElementExtensions.SystemCursorProperty, CoreCursorType.Wait);
                            break;
                        }
                }
            }
        }
    }
}
