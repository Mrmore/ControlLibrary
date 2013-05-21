using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace ControlLibrary
{
    public sealed class ColorBox : Control
    {
        private ComboBox comboBox = null;
        private int selectedCount = -1;

        //图片下载的实时代理
        public delegate void ColorChangedHandler();
        /// <summary>
        /// 图片下载的实时事件
        /// </summary>
        public event ColorChangedHandler ColorChanged;
        public ColorBox()
        {
            this.DefaultStyleKey = typeof(ColorBox);
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            private set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ColorBox), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));

        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorBox = d as ColorBox;
            if (colorBox != null && colorBox.comboBox != null)
            {

            }
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            private set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorBox), new PropertyMetadata(Colors.Red, new PropertyChangedCallback(OnSelectedColorPropertyChanged)));

        private static void OnSelectedColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var colorBox = d as ColorBox;
            if (colorBox != null && colorBox.comboBox != null)
            {

            }
        }

        protected override Windows.Foundation.Size ArrangeOverride(Windows.Foundation.Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            comboBox = this.GetTemplateChild("comboBox") as ComboBox;
            if (comboBox != null)
            {
                var colors = typeof(Colors).GetTypeInfo().DeclaredProperties;
                foreach (var item in colors)
                {
                    comboBox.Items.Add(item);
                    if (this.SelectedColor.Equals((Color)(item as PropertyInfo).GetValue(null)))
                    {
                        selectedCount = comboBox.Items.Count;
                    }
                }
                comboBox.SelectionChanged -= comboBox_SelectionChanged;
                comboBox.SelectionChanged += comboBox_SelectionChanged;
                comboBox.SelectedIndex = selectedCount - 1;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedIndex = comboBox.SelectedIndex;
            if (comboBox.SelectedIndex != -1)
            {
                var pi = comboBox.SelectedItem as PropertyInfo;
                this.SelectedColor = (Color)pi.GetValue(null);
                if (ColorChanged != null)
                {
                    ColorChanged();
                }
            }
        }
    }
}
