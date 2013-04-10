using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// Indentifies all the touch gestures, known by the framework.
    /// </summary>
    public enum KnownGesture
    {
        /// <summary>
        /// A Tap gesture.
        /// </summary>
        Tap,

        /// <summary>
        /// A Double-tap gesture.
        /// </summary>
        DoubleTap,

        /// <summary>
        /// A Pan (drag) gesture.
        /// </summary>
        Pan,

        /// <summary>
        /// A flick gesture.
        /// </summary>
        Flick,

        /// <summary>
        /// A pinch gesture.
        /// </summary>
        Pinch,

        /// <summary>
        /// A special gesture that notifies for a successfully completed Pinch gesture.
        /// </summary>
        PinchComplete,

        /// <summary>
        /// A Hold gesture.
        /// </summary>
        Hold,
    }
}
