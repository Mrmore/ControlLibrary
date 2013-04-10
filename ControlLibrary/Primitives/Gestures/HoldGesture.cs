using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a Hold gesture.
    /// </summary>
    public class HoldGesture : SingleTouchPointGesture
    {
        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        /// <value></value>
        public override KnownGesture GestureType
        {
            get
            {
                return KnownGesture.Hold;
            }
        }
    }
}
