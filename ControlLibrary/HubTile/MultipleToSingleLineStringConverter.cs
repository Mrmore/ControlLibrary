using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ControlLibrary
{
    /// <summary>
    /// Converts a multi-line string into a single line string.
    /// </summary>
    internal class MultipleToSingleLineStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return ((string)value).Replace(Environment.NewLine, " ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }

    /// <summary>
    /// If there is a new notification (value)
    /// Returns a Visible value for the notification block (parameter).
    /// Or a Collapsed value for the message block (parameter).
    /// Returns a opposite values otherwise.
    /// </summary>
    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if ((bool)value ^ (bool)parameter)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}
