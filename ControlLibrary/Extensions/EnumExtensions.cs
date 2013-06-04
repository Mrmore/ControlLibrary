using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Extensions
{
    public static class EnumExtensions
    {
        public static List<int> ConverEnumToList<T>()
        {
            List<int> list = new List<int>();

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                int index = Convert.ToInt32(item);
                list.Add(index);
            }

            return list;
        }

        public static T ConverStringToEnum<T>(this string txt)
        {
            return (T)Enum.Parse(typeof(T), txt);
        }
    }
}
