using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a two-touch point gesture - e.g. Pinch.
    /// </summary>
    public abstract class TwoTouchPointGesture : SingleTouchPointGesture
    {
        /// <summary>
        /// Gets the <see cref="Point"/> that represents the position of the second touch point.
        /// </summary>
        public Point Position2
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the initial (starting) distance between the two touch points.
        /// </summary>
        public double StartDistance
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the current distance between the two touch points.
        /// </summary>
        public double Distance
        {
            get;
            internal set;
        }
    }
}
