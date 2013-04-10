using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// A gesture that is triggered by one touch point (one touch device).
    /// </summary>
    public abstract class SingleTouchPointGesture : Gesture
    {
        /// <summary>
        /// Gets the position, relative to the manipulation container of the touch point.
        /// </summary>
        public Point Position
        {
            get;
            internal set;
        }
    }
}
