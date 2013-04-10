using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a Flick (inertial) gesture.
    /// </summary>
    public class FlickGesture : SingleTouchPointGesture
    {
        /// <summary>
        /// Gets a <see cref="Point"/> value that represents the velocity (inertia) of the flick.
        /// </summary>
        public Point Velocity
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        /// <value></value>
        public override KnownGesture GestureType
        {
            get
            {
                return KnownGesture.Flick;
            }
        }
    }
}
