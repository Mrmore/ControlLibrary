using System;
using Windows.UI.Xaml.Data;

namespace ControlLibrary.Converter
{
    public class ParallaxConverter : IValueConverter
    {
        const double _factor = -0.10;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return (double)value * _factor;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is double)
            {
                return (double)value / _factor;
            }
            return 0;
        }
    }
}
