using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Defines a data source view model, used by <see cref="RadListSource"/>.
    /// </summary>
    public interface IDataSourceItem
    {
        /// <summary>
        /// Gets a boolean value determining whether the data item
        /// will appear as checked when the list control shows
        /// check boxes next to each visual item.
        /// </summary>
        bool IsChecked
        {
            get;
        }

        /// <summary>
        /// Gets the raw data wrapped by this instance.
        /// </summary>
        object Value
        {
            get;
        }

        /// <summary>
        /// Gets or sets the previous item in the owning data source.
        /// </summary>
        IDataSourceItem Previous
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the next item in the owning data source.
        /// </summary>
        IDataSourceItem Next
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the index of this item within its owning data source.
        /// </summary>
        int Index
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent group of this instance.
        /// </summary>
        IDataSourceGroup ParentGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display value that will be used when there are no data templates for the item.
        /// </summary>
        object DisplayValue
        {
            get;
            set;
        }

        /// <summary>
        /// Changes the raw data wrapped by this instance.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ChangeValue(object value);
    }
}
