using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Extensions
{
    //扩展方法必须为静态类
    public static class IEnumerableToObservableCollection
    {
        /// <summary>
        /// 将IEnumerable T转换为ObservableCollection T
        /// </summary>
        /// <typeparam name="T">为模糊类型</typeparam>
        /// <param name="source">为传入的类型</param>
        /// <returns>ObservableCollection T = ObservableCollection类型集合</returns>
        public static ObservableCollection<T> ToObservableCollection<T>
            (this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return new ObservableCollection<T>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static ObservableCollection<object> Convert(this IEnumerable original)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            return new ObservableCollection<object>(original.Cast<object>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static ObservableCollection<T> Convert<T>(this IEnumerable<T> original)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            return new ObservableCollection<T>(original);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static ObservableCollection<T> Convert<T>(this IEnumerable original)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            return new ObservableCollection<T>(original.Cast<T>());
        }
    }
}
