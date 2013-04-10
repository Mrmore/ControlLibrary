using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a double-tap gesture.
    /// </summary>
    public class DoubleTapGesture : SingleTouchPointGesture
    {
        /// <summary>
        /// Gets the <see cref="KnownGesture"/> value for this instance.
        /// </summary>
        /// <value></value>
        public override KnownGesture GestureType
        {
            get
            {
                return KnownGesture.DoubleTap;
            }
        }
    }
}
