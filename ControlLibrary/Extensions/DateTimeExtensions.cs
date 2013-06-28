using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;

namespace ControlLibrary.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short date string representation.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortDateString(this DateTime dt)
        {
            return dt.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortDatePattern);
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent short time string representation.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortTimeString(this DateTime dt)
        {
            return dt.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern);
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent long date string representation.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongDateString(this DateTime dt)
        {
            return dt.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.LongDatePattern);
        }

        /// <summary>
        /// Converts the value of the current DateTime object to its equivalent long time string representation.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongTimeString(this DateTime dt)
        {
            return dt.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.LongTimePattern);
        }

        /// <summary>
        /// Creates a DateTimeFormatter object that is initialized by a format template string.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="formatTemplate">A format template string that specifies the requested components. The order of the components is irrelevant.  This can also be a format pattern.</param>
        /// <returns></returns>
        public static string Format(this DateTime dt, string formatTemplate)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(formatTemplate);
            return formatter.Format(dt);
        }

        public static string Format(this DateTime dt, string formatTemplate, IEnumerable<string> languages)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(formatTemplate, languages);
            return formatter.Format(dt);
        }

        /// <summary>
        /// Creates a DateTimeFormatter object that is initialized with hour, minute, and second formats.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="hourFormat">The desired hour format to include in the template.</param>
        /// <param name="minuteFormat">The desired minute format to include in the template.</param>
        /// <param name="secondFormat">The desired second format to include in the template.</param>
        /// <returns></returns>
        public static string Format(this DateTime dt, HourFormat hourFormat, MinuteFormat minuteFormat, SecondFormat secondFormat)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(hourFormat, minuteFormat, secondFormat);
            return formatter.Format(dt);
        }

        /// <summary>
        /// Creates a DateTimeFormatter object that is initialized with year, month, day, and day of week formats.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="yearFormat">The desired year format to include in the template.</param>
        /// <param name="monthFormat">The desired month format to include in the template.</param>
        /// <param name="dayFormat">The desired day format to include in the template.</param>
        /// <param name="dayOfWeekFormat">The desired day of week format to include in the template.</param>
        /// <returns></returns>
        public static string Format(this DateTime dt, YearFormat yearFormat, MonthFormat monthFormat, DayFormat dayFormat, DayOfWeekFormat dayOfWeekFormat)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(yearFormat, monthFormat, dayFormat, dayOfWeekFormat);
            return formatter.Format(dt);
        }

        public static string Format(this DateTime dt, string formatTemplate, IEnumerable<string> languages, string geographicRegion, string calendar, string clock)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(formatTemplate, languages, geographicRegion, calendar, clock);
            return formatter.Format(dt);
        }

        public static string Format(this DateTime dt, YearFormat yearFormat, MonthFormat monthFormat, DayFormat dayFormat, DayOfWeekFormat dayOfWeekFormat, HourFormat hourFormat, MinuteFormat minuteFormat, SecondFormat secondFormat, IEnumerable<string> languages)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(yearFormat, monthFormat, dayFormat, dayOfWeekFormat, hourFormat, minuteFormat, secondFormat, languages);
            return formatter.Format(dt);
        }

        public static string Format(this DateTime dt, YearFormat yearFormat, MonthFormat monthFormat, DayFormat dayFormat, DayOfWeekFormat dayOfWeekFormat, HourFormat hourFormat, MinuteFormat minuteFormat, SecondFormat secondFormat, IEnumerable<string> languages, string geographicRegion, string calendar, string clock)
        {
            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(yearFormat, monthFormat, dayFormat, dayOfWeekFormat, hourFormat, minuteFormat, secondFormat, languages, geographicRegion, calendar, clock);
            return formatter.Format(dt);
        }

        public static string ToUserString(this LaborType type)
        {
            switch (type)
            {
                case LaborType.Hourly:
                    return Catalog.GetString("LaborTypeHourly");
                case LaborType.OverTime:
                    return Catalog.GetString("LaborTypeOverTime");
                case LaborType.HolidayTime:
                    return Catalog.GetString("LaborTypeHolidayTime");
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Helper method to safely convert a string to a double
        /// </summary>
        public static double ToDouble(this string text, IFormatProvider provider)
        {
            double x;
            double.TryParse(text, NumberStyles.Any, provider, out x);
            return x;
        }

        /// <summary>
        /// Helper method to safely convert a string to a decimal
        /// </summary>
        public static decimal ToDecimal(this string text, IFormatProvider provider)
        {
            decimal x;
            decimal.TryParse(text, NumberStyles.Any, provider, out x);
            return x;
        }

        /// <summary>
        /// Helper method to safely convert a string to a int
        /// </summary>
        public static int ToInt(this string text, IFormatProvider provider)
        {
            int value = 0;
            int.TryParse(text, NumberStyles.Any, provider, out value);
            return value;
        }
    }


    /// <summary>
    /// An enumeration for Labor types
    /// </summary>
    public enum LaborType
    {
        /// <summary>
        /// The labor counts as standard hourly labor
        /// </summary>
        Hourly = 0,
        /// <summary>
        /// The labor counts as over time
        /// </summary>
        OverTime = 1,
        /// <summary>
        /// The labor counts as holiday time
        /// </summary>
        HolidayTime = 2,
    }

    /// <summary>
    /// Static class containing strings used throughout the app
    /// - a developer could replace this with *.resx supporting globalization if desired
    /// </summary>
    public class Catalog
    {

        private const string UsernameValidation = "Please enter a username.";
        private const string PasswordValidation = "Please enter a password.";
        private const string LaborTypeHourly = "Hourly";
        private const string LaborTypeOverTime = "Over Time";
        private const string LaborTypeHolidayTime = "Holiday Time";

        public static string GetString(string key, string comment = null)
        {
            switch (key)
            {
                case "UsernameValidation":
                    return UsernameValidation;
                case "PasswordValidation":
                    return PasswordValidation;
                case "LaborTypeHourly":
                    return LaborTypeHourly;
                case "LaborTypeOverTime":
                    return LaborTypeOverTime;
                case "LaborTypeHolidayTime":
                    return LaborTypeHolidayTime;
                default:
                    return string.Empty;
            }
        }
    }
}
