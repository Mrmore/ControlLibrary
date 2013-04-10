using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary.CommonControl
{
    /// <summary>
    /// Encapsulates the data, associated with a change in the bit state of a <see cref="DisposableObject"/> instance.
    /// </summary>
    [CLSCompliant(false)]
    public class BitStateChangedEventArgs : EventArgs
    {
        private uint key;
        private bool oldValue;
        private bool newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="key">The key that defines the changed bit.</param>
        /// <param name="oldValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        public BitStateChangedEventArgs(uint key, bool oldValue, bool newValue)
        {
            this.key = key;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        /// Gets the key that defines the changed bit.
        /// </summary>
        public uint Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the previous value.
        /// </summary>
        public bool OldValue
        {
            get
            {
                return this.oldValue;
            }
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public bool NewValue
        {
            get
            {
                return this.newValue;
            }
        }
    }
}
