using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Extensions
{
    public static class CollectionExtensions
    {
        private const int defaultCapacity = 4;

        public static void AddRange<T>(this ICollection<T> destination,
                                       IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                destination.Add(item);
            }
        }

        public static void InsertRange<T>(this ICollection<T> destination, int index,
                                       IEnumerable<T> source)
        {
            if (destination != null && source != null && (uint)index < (uint)(destination.Count - 1))
            {
                var list = destination.ToList();
                list.InsertRange(index, source);
                destination.Clear();
                destination.AddRange(list);

                //ObservableCollection<T> newValue = destination.ToObservableCollection();
                //for (int i = source.Count() - 1; i >= 0; i--)
                //{
                //    newValue.Insert(index, source.ElementAt(i));
                //}
                //destination.Clear();
                //destination.AddRange(list);
            }
        }

        public static void InsertOCRange<T>(this ObservableCollection<T> destination, int index,
                                       IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            if (destination == null)
            {
                throw new ArgumentNullException();
            }
            if ((uint)index >= (uint)destination.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = source.Count() - 1; i >= 0; i--)
            {
                destination.Insert(index, source.ElementAt(i));
            }
        }

        public static int RemoveAll<T>(this ObservableCollection<T> coll, Func<T, bool> condition)
        {
            var itemsToRemove = coll.Where(condition).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                coll.Remove(itemToRemove);
            }
            return itemsToRemove.Count;
        }

        public static void RemoveRange<T>(this ObservableCollection<T> destination, int index, int count)
        {
            if (destination == null)
            {
                throw new ArgumentNullException();
            }
            if ((uint)index >= (uint)destination.Count || (uint)destination.Count < (uint)(index + count))
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < count; i++)
            {
                destination.RemoveAt(index);
            }
        }

        public static int FirstIndexMatch<T>(this IEnumerable<T> items, Func<T, bool> matchCondition)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (matchCondition.Invoke(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        /*
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            for (int i = collection.Count - 1; i >= 0; i--)
            {
                if (condition(collection[i]))
                {
                    collection.RemoveAt(i);
                }
            }
        }
        */

        /*
        public static void InsertRange<T>(this ICollection<T> destination, int index,
                                       IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            if (destination == null)
            {
                throw new ArgumentNullException();
            }
            ICollection<T> c = destination as ICollection<T>;
            int size = c.Count;
            T[] items = new T[size];
            c.CopyTo(items, 0);
            if ((uint)index > (uint)size)
            {
                throw new ArgumentOutOfRangeException();
            }
            ICollection<T> s = source as ICollection<T>;          
            if (s != null)
            {
                int count = s.Count;
                if (count > 0)
                {
                    var min = size + count;
                    if (items.Length < min)
                    {
                        int newCapacity = items.Length == 0 ? defaultCapacity : items.Length * 2;
                        if (newCapacity < min) newCapacity = min;
                    }

                    if (index < size)
                    {
                        Array.Copy(items, index, items, index + count, size - index);
                    }
                    if (destination == s)
                    {
                        Array.Copy(items, 0, items, index, index);
                        Array.Copy(items, index + count, items, index * 2, size - index);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(items, index);
                    }
                    size += count;
                }
            }
            else
            {
                using (IEnumerator<T> en = source.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
        }
        */
    }
}
