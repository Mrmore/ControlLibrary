using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Represents a group view model within a collection view source.
    /// </summary>
    public interface IDataSourceGroup : IDataSourceItem
    {
        /// <summary>
        /// Gets the <see cref="IDataSourceItem"/> instances owned by this group.
        /// </summary>
        IList<IDataSourceItem> ChildItems
        {
            get;
        }

        /// <summary>
        /// Determines whether the group is expanded (its child items are visible and may be enumerated).
        /// </summary>
        bool IsExpanded
        {
            get;
            set;
        }

        /// <summary>
        /// Determines whether the group has nested groups or data items only.
        /// </summary>
        bool HasChildGroups
        {
            get;
        }

        /// <summary>
        /// Gets the zero-based level of this group.
        /// </summary>
        int Level
        {
            get;
        }

        /// <summary>
        /// Gets the last child item in this group.
        /// </summary>
        IDataSourceItem LastChildItem
        {
            get;
        }
    }
}
