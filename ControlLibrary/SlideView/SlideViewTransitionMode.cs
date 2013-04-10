using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Defines the possible transitions modes for browsing the items of a <see cref="RadSlideView"/> instance.
    /// </summary>
    public enum SlideViewTransitionMode
    {
        /// <summary>
        /// Slide transition is used.
        /// </summary>
        Slide,

        /// <summary>
        /// Flip (book-like) transition is used.
        /// </summary>
        Flip
    }
}
