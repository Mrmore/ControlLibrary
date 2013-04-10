using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Creates instances of the <see cref="IDataSourceItem"/> type which are used by a <see cref="RadListSource"/> instance.
    /// </summary>
    internal interface IDataSourceItemFactory
    {
        /// <summary>
        /// Creates a <see cref="IDataSourceItem"/> instance.
        /// </summary>
        IDataSourceItem CreateItem(MatListSource owner, object value);

        /// <summary>
        /// Creates a group item for the specified data group.
        /// </summary>
        IDataSourceGroup CreateGroup(MatListSource owner, DataGroup dataGroup);

        /// <summary>
        /// Called when the items in the owning <see cref="RadListSource"/> instance change.
        /// </summary>
        /// <param name="args"></param>
        void OnOwningListSourceCollectionChanged(NotifyCollectionChangedEventArgs args);
    }
}
