using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Primitives.Pagination
{
    /// <summary>
    /// Defines the public API of a control instance which content may be viewed by pages. Such a control is <see cref="RadSlideView"/> for example.
    /// </summary>
    public interface IPageProvider
    {
        /// <summary>
        /// Occurs when the currently selected index changes.
        /// </summary>
        event EventHandler CurrentIndexChanged;

        /// <summary>
        /// Gets the current items sequence used to populate the control's thumbnails.
        /// </summary>
        IEnumerable ThumbnailsSource
        {
            get;
        }

        /// <summary>
        /// Gets the current count of all the items within the control.
        /// </summary>
        int PageCount
        {
            get;
        }

        /// <summary>
        /// Gets or sets the index of the current page.
        /// </summary>
        int CurrentIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Moves to the previous item in the sequence.
        /// </summary>
        void MovePrevious();

        /// <summary>
        /// Moves to the next item in the sequence.
        /// </summary>
        void MoveNext();
    }
}
