using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a Pinch gesture.
    /// </summary>
    public class PinchGesture : TwoTouchPointGesture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PinchGesture"/> class.
        /// </summary>
        public PinchGesture()
        {
            this.Scale = 1;
        }

        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        /// <value></value>
        public override KnownGesture GestureType
        {
            get
            {
                return KnownGesture.Pinch;
            }
        }

        /// <summary>
        /// Gets a value representing the scale ration between the current and the start distance of the two touch points.
        /// </summary>
        public double Scale
        {
            get;
            internal set;
        }
    }
}
