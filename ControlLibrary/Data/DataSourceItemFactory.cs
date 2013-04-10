using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    internal class DataSourceItemFactory : IDataSourceItemFactory
    {
        public IDataSourceItem CreateItem(MatListSource owner, object value)
        {
            return new DataSourceItem(owner, value);
        }

        public IDataSourceGroup CreateGroup(MatListSource owner, DataGroup group)
        {
            return new DataSourceGroup(owner, group);
        }

        public virtual void OnOwningListSourceCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
        }
    }
}
