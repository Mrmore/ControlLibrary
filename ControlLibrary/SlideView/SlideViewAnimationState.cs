using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Defines the possible animation states of a <see cref="SlideViewManipulationBehavior"/> instance.
    /// </summary>
    public enum SlideViewAnimationState
    {
        /// <summary>
        /// No animation is running.
        /// </summary>
        None,

        /// <summary>
        /// An animation, generated upon a Flick gesture, is running.
        /// </summary>
        Flick,

        /// <summary>
        /// An animation, generated upon a Stretch transformation, is running.
        /// </summary>
        Stretch
    }
}
