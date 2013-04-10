using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a touch gesture.
    /// </summary>
    public abstract class Gesture
    {
        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        public abstract KnownGesture GestureType
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the gesture is already handled by another instance.
        /// </summary>
        public bool Handled
        {
            get;
            set;
        }
    }
}
