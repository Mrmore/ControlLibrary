using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    /// <summary>
    /// Implements a wrapper for DateTime that provides formatted strings for DatePicker.
    /// </summary>
    public sealed class DateTimeWrapper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime dateTime;

        /// <summary>
        /// Gets the DateTime being wrapped.
        /// </summary>
        public DateTime DateTime
        {
            get { return this.dateTime; }
            set
            {
                if (this.dateTime == value)
                    return;
                this.dateTime = value;

                if (this.PropertyChanged == null) return;
                
                // Raise Date changed
                this.PropertyChanged(this, new PropertyChangedEventArgs("DateTime"));

                // Raise on Day Name because it's not the same day on differents month / years
                this.PropertyChanged(this, new PropertyChangedEventArgs("DayName"));

                // Raise on Day Name because it's not the same day on differents month / years
                this.PropertyChanged(this, new PropertyChangedEventArgs("DayNumber"));
            }
        }

        /// <summary>
        /// Gets the 4-digit year as a string.
        /// </summary>
        public string YearNumber { get { return DateTime.ToString("yyyy", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the 2-digit month as a string.
        /// </summary>
        public string MonthNumber { get { return DateTime.ToString("MM", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the month name as a string.
        /// </summary>
        public string MonthName { get { return DateTime.ToString("MMMM", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the 2-digit day as a string.
        /// </summary>
        public string DayNumber { get { return DateTime.ToString("dd", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the day name as a string.
        /// </summary>
        public string DayName { get { return DateTime.ToString("dddd", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the hour as a string.
        /// </summary>
        public string HourNumber { get { return DateTime.ToString(CurrentCultureUsesTwentyFourHourClock() ? "%H" : "%h", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the 2-digit minute as a string.
        /// </summary>
        public string MinuteNumber { get { return DateTime.ToString("mm", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the 2-digit seconds as a string.
        /// </summary>
        public string SecondNumber { get { return DateTime.ToString("%s", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Gets the AM/PM designator as a string.
        /// </summary>
        public string AmPmString { get { return DateTime.ToString("tt", CultureInfo.CurrentCulture); } }

        /// <summary>
        /// Initializes a new instance of the DateTimeWrapper class.
        /// </summary>
        /// <param name="dateTime">DateTime to wrap.</param>
        public DateTimeWrapper(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        /// <summary>
        /// Returns a value indicating whether the current culture uses a 24-hour clock.
        /// </summary>
        /// <returns>True if it uses a 24-hour clock; false otherwise.</returns>
        public static bool CurrentCultureUsesTwentyFourHourClock()
        {
            return !CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.Contains("t");
        }
    }

    internal static class DateTimeWrapperExtensions
    {
        public static DateTimeWrapper ToDateTimeWrapper(this DateTime dateTime)
        {
            return new DateTimeWrapper(dateTime);
        }
    }
}
