using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace ControlLibrary.Primitives.Gestures
{
    /// <summary>
    /// Contains gesture information provided by GestureBehavior when a gesture occurs.
    /// </summary>
    public class GestureEventArgs : EventArgs
    {
        private Gesture gesture;
        private Point primaryLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEventArgs"/> class.
        /// </summary>
        /// <param name="gesture">The gesture.</param>
        /// <param name="primaryLocation">The location at which the gesture occurred.</param>
        public GestureEventArgs(Gesture gesture, Point primaryLocation)
        {
            this.gesture = gesture;
            this.primaryLocation = primaryLocation;
        }

        /// <summary>
        /// Gets the location (in coordinates, relative to the chart surface) of the primary touch point of the gesture.
        /// </summary>
        public Point PrimaryLocation
        {
            get
            {
                return this.primaryLocation;
            }
        }

        /// <summary>
        /// Gets the structure that describes the gesture.
        /// </summary>
        public Gesture Gesture
        {
            get
            {
                return this.gesture;
            }
        }
    }
}
