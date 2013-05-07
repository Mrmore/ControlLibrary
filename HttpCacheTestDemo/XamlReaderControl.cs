using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace HttpCacheTestDemo
{
    public sealed class XamlReaderControl : Control
    {
        Grid grid = null;
        private const string _contentTemplate
            = @"<ControlTemplate " +
              "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' " +
              "xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>" +
              "<Grid x:Name='LayoutRoot' Background='{TemplateBinding Background}'>" +
              "<Grid.RenderTransform>" +
              "<MatrixTransform x:Name='MatrixTransform'/>" +
              "</Grid.RenderTransform>" +
              "</Grid>" +
              "</ControlTemplate>";

        public XamlReaderControl()
        {
            Template = (ControlTemplate)XamlReader.Load(_contentTemplate);
            ApplyTemplate();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.grid = (Grid)GetTemplateChild("LayoutRoot");
        }
    }
}
