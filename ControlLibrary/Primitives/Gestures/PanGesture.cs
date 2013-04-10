using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// The logical representation of a Pan (drag) gesture.
    /// </summary>
    public class PanGesture : SingleTouchPointGesture
    {
        /// <summary>
        /// Gets the <see cref="Point"/> value that represents the cumulative physical translation of the touch point.
        /// </summary>
        public Point CumulativeTranslation
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the <see cref="Point"/> value that represents the delta translation between the last pan and the current one.
        /// </summary>
        public Point DeltaTranslation
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
                return KnownGesture.Pan;
            }
        }
    }
}
