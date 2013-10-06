using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] source, int start)
        {
            int len = source.Length - start;

            var res = new T[len];

            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }

            return res;
        }
    }
}
