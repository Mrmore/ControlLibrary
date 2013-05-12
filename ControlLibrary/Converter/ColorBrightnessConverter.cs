using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.Converter
{
    public class ColorBrightnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // parameter should be the brightness factor in double.
            var factor = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            var brush = (SolidColorBrush)value;

            var newColor = new Color() { A = brush.Color.A, B = System.Convert.ToByte(brush.Color.B * factor, CultureInfo.InvariantCulture), G = System.Convert.ToByte(brush.Color.G * factor, CultureInfo.InvariantCulture), R = System.Convert.ToByte(brush.Color.R * factor, CultureInfo.InvariantCulture) };

            return new SolidColorBrush(newColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
