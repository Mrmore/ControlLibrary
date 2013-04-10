using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Defines the possible modes for previewing adjacent to the viewport items visibility in a <see cref="RadSlideView"/> instance.
    /// </summary>
    [Flags]
    public enum SlideViewAdjacentItemsPreviewMode
    {
        /// <summary>
        /// Adjacent items are not visible, only the Viewport one is displayed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The previous item is partially visible, together with the viewport one.
        /// </summary>
        Previous = 1,

        /// <summary>
        /// The next item is partially visible, together with the viewport one.
        /// </summary>
        Next = Previous << 1,

        /// <summary>
        /// Both previous and next items are partially visible.
        /// </summary>
        Both = Previous | Next
    }
}
