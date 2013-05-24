using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ControlLibrary
{
    internal class DateTimeHelper
    {
        internal static readonly int[] DaysToMonth365 = new[]
        {
          0,
          31,
          59,
          90,
          120,
          151,
          181,
          212,
          243,
          273,
          304,
          334,
          365
        };
        internal static readonly int[] DaysToMonth366 = new[]
        {
          0,
          31,
          60,
          91,
          121,
          152,
          182,
          213,
          244,
          274,
          305,
          335,
          366
        };

        public static int DaysInMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException("month");

            int[] numArray = DateTime.IsLeapYear(year) ? DaysToMonth366 : DaysToMonth365;

            return numArray[month] - numArray[month - 1];
        }
    }

    public sealed class YearDataSource
    {
        const int YearsAvailables = 100;
        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            //DateTime
            // Année bisextile
            if (DateTimeHelper.DaysInMonth(1950, relativeDate.Month) < DateTimeHelper.DaysInMonth(relativeDate.Year, relativeDate.Month))
                return new DateTime(1950, relativeDate.Month, DateTimeHelper.DaysInMonth(1900, relativeDate.Month), 0, 0, 0);

            return new DateTime(1950, relativeDate.Month, relativeDate.Day, relativeDate.Hour, relativeDate.Minute,
                                relativeDate.Second);

        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return YearsAvailables;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            if ((1601 == relativeDate.Year) || (3000 == relativeDate.Year))
                return null;

            // Principe : Incrémenter l'année de 1
            // Attention : Si l'année est Bisextile, alors l'année + delta du mois de février peut ne  pas être bonne !
            int nextYear = relativeDate.Year + delta;

            // Calcul du bon jour
            // Minimum entre le nb de jour aujourd'hui et le nb de jours de l'année d'aprés
            // Si aujourd'hui == 29 et année prochaine == 28 alors return 28
            // si AUjourd'hui == 28 et année prochaine == 29 alors return 28
            int nextDay = Math.Min(originalDate.Day, DateTimeHelper.DaysInMonth(nextYear, relativeDate.Month));

            return new DateTime(nextYear, relativeDate.Month, nextDay, relativeDate.Hour, relativeDate.Minute, relativeDate.Second);


        }


    }

    public sealed class MonthDataSource
    {
        const int MonthsInYear = 12;
        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, 01, relativeDate.Day, relativeDate.Hour, relativeDate.Minute,
                                relativeDate.Second);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return MonthsInYear;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            // Récupère le mois prochain 
            int nextMonth = ((MonthsInYear + relativeDate.Month - 1 + delta) % MonthsInYear) + 1;

            // Récupère le jours suivant 
            int nextDay = Math.Min(relativeDate.Day, DateTimeHelper.DaysInMonth(relativeDate.Year, nextMonth));

            return new DateTime(relativeDate.Year, nextMonth, nextDay, relativeDate.Hour, relativeDate.Minute, relativeDate.Second);


            //newData = DataSource.GetNext(firstWrapper) as DateTimeWrapper;

            //// Jour étalon (31)
            //int currentWrapperDay = wrapper.DateTime.Day;

            //// Jour max du mois en cours (en Avril 31)
            //int currentMaxItemDay = DateTime.DaysInMonth(newData.DateTime.Year, newData.DateTime.Month);

            //// A cause du précédent je suis à 30 là
            //int currentRecalculatedDay = newData.DateTime.Day;

            //// Si le jour en cours 
            //if (currentWrapperDay > currentRecalculatedDay && currentWrapperDay <= currentMaxItemDay)
            //{
            //    newData = new DateTimeWrapper(new DateTime(newData.DateTime.Year, newData.DateTime.Month, currentWrapperDay));
            //}
        }
    }

    public sealed class DayDataSource
    {
        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, 01, relativeDate.Hour, relativeDate.Minute, relativeDate.Second);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return DateTimeHelper.DaysInMonth(relativeDate.Year, relativeDate.Month);
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int daysInMonth = DateTimeHelper.DaysInMonth(relativeDate.Year, relativeDate.Month);
            int nextDay = ((daysInMonth + relativeDate.Day - 1 + delta) % daysInMonth) + 1;
            return new DateTime(relativeDate.Year, relativeDate.Month, nextDay, relativeDate.Hour, relativeDate.Minute, relativeDate.Second);
        }
    }

    public sealed class TwelveHourDataSource
    {
        const int HoursInHalfDay = 24;

        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, 0, relativeDate.Minute, relativeDate.Second, 0);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return HoursInHalfDay;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int nextHour = (HoursInHalfDay + relativeDate.Hour + delta) % HoursInHalfDay;
            nextHour += HoursInHalfDay <= relativeDate.Hour ? HoursInHalfDay : 0;
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, nextHour, relativeDate.Minute, relativeDate.Second, 0);
        }
    }

    public sealed class MinuteDataSource
    {
        const int MinutesInHour = 60;

        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, relativeDate.Hour, 0, relativeDate.Second, 0);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return MinutesInHour;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int nextMinute = (MinutesInHour + relativeDate.Minute + delta) % MinutesInHour;
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, relativeDate.Hour, nextMinute, relativeDate.Second, 0);
        }
    }

    public sealed class SecondDataSource
    {
        const int SecondsInMinute = 60;

        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, relativeDate.Hour, relativeDate.Minute, 0, 0);

        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return SecondsInMinute;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int nextSecond = (SecondsInMinute + relativeDate.Second + delta) % SecondsInMinute;
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, relativeDate.Hour, relativeDate.Minute, nextSecond, 0);
        }
    }

    public sealed class AmPmDataSource
    {
        const int HoursInDay = 24;

        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, 0, relativeDate.Minute, relativeDate.Second, 0);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return HoursInDay;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int nextHour = relativeDate.Hour + (delta * (HoursInDay / 2));
            if ((nextHour < 0) || (HoursInDay <= nextHour))
            {
                return null;
            }
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, nextHour, relativeDate.Minute, relativeDate.Second, 0);
        }
    }

    public sealed class TwentyFourHourDataSource
    {
        const int HoursInDay = 24;
        public DateTime GetFirstAvailableDate(DateTime relativeDate)
        {
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, 0, relativeDate.Minute, relativeDate.Second, 0);
        }

        public int GetNumberOfItems(DateTime relativeDate)
        {
            return HoursInDay;
        }

        public DateTime? GetNext(DateTime originalDate, DateTime relativeDate, int delta)
        {
            int nextHour = (HoursInDay + relativeDate.Hour + delta) % HoursInDay;
            return new DateTime(relativeDate.Year, relativeDate.Month, relativeDate.Day, nextHour, relativeDate.Minute, relativeDate.Second, 0);
        }
    }
}
