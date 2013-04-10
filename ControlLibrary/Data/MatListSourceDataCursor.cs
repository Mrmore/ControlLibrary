using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    internal partial class MatListSource
    {
        /// <summary>
        /// Gets the first item in the collection.
        /// </summary>
        /// <returns></returns>
        public IDataSourceItem GetFirstItem()
        {
            if (this.items.Count > 0)
            {
                return this.items[0];
            }

            return null;
        }

        /// <summary>
        /// Gets the item that is right after the specified one.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IDataSourceItem GetItemAfter(IDataSourceItem item)
        {
            if (item == null)
            {
                return null;
            }

            return item.Next;
        }

        /// <summary>
        /// Gets the item that is just before the specified one.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IDataSourceItem GetItemBefore(IDataSourceItem item)
        {
            if (item == null)
            {
                return null;
            }

            return item.Previous;
        }

        /// <summary>
        /// Gets the last item in the collection.
        /// </summary>
        /// <returns></returns>
        public virtual IDataSourceItem GetLastItem()
        {
            if (this.items.Count > 0)
            {
                return this.items[this.items.Count - 1];
            }

            return null;
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual IDataSourceItem GetItemAt(int index)
        {
            if (index >= 0 && index < this.items.Count)
            {
                return this.items[index];
            }

            return null;
        }

        /// <summary>
        /// Retrieves the index of the specified item within the collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(IDataSourceItem item)
        {
            if (item == null)
            {
                return -1;
            }

            return item.Index;
        }
    }
}
