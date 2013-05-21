using System;
using System.Collections;
using System.Collections.Generic;

namespace ControlLibrary
{
    /// <summary>
    /// Defines extensions to IEnumerable specific to Telerik functionality.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates a new sequence of animations that have reverse behavior.
        /// </summary>
        /// <param name="animations">The animations to reverse.</param>
        /// <returns>Returns a new sequence of reversed animations.</returns>
        //public static IEnumerable<RadAnimation> ReverseAnimations(this IEnumerable<RadAnimation> animations)
        //{
        //    foreach (RadAnimation animation in animations)
        //    {
        //        yield return animation.CreateOpposite();
        //    }
        //}

        /// <summary>
        /// Applies the given function to the objects in this IEnumerable instance.
        /// </summary>
        /// <param name="objects">The objects to which to apply the function argument.</param>
        /// <param name="function">The function to apply.</param>
        /// <param name="predicate">The function argument will be applied only if this predicate returns true or if it is null.</param>
        public static void Apply<T>(this IEnumerable<T> objects, Action<T> function, Predicate<T> predicate = null)
        {
            foreach (T obj in objects)
            {
                bool shouldApply = predicate == null ? true : predicate(obj);

                if (shouldApply)
                {
                    function(obj);
                }
            }
        }

        /// <summary>
        /// Gets the number of items in an IEnumerable.
        /// </summary>
        /// <param name="enumerable">The IEnumerable intance from which to get the item count.</param>
        /// <returns>Returns the number of items in an IEnumerable.</returns>
        public static int Count(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return 0;
            }

            ICollection collection = enumerable as ICollection;
            if (collection != null)
            {
                return collection.Count;
            }

            IEnumerator enumerator = enumerable.GetEnumerator();
            int result = 0;
            while (enumerator.MoveNext())
            {
                result++;
            }

            return result;
        }
    }
}
