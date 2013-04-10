using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Specifies the available modes used by the data item realization mechanism in <see cref="RadSlideView"/> instance.
    /// </summary>
    public enum SlideViewItemRealizationMode
    {
        /// <summary>
        /// The pivot as well as the two adjacent items are realized.
        /// </summary>
        Default,

        /// <summary>
        /// Only the viewport item is realized.
        /// </summary>
        ViewportItem
    }
}
