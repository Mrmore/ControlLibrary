using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// A gesture which occurs when a Pinch gesture has been performed and one finger is up while the other is still down.
    /// </summary>
    public class PinchCompleteGesture : Gesture
    {
        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        /// <value></value>
        public override KnownGesture GestureType
        {
            get
            {
                return KnownGesture.PinchComplete;
            }
        }
    }
}
