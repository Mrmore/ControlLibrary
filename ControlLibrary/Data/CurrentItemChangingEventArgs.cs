using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlLibrary
{
    /// <summary>
    /// Encapsulates the data associated with a <see cref="RadListSource.CurrentItemChanging"/> event.
    /// </summary>
    public class CurrentItemChangingEventArgs : EventArgs
    {
        private bool cancelable;
        private bool cancel;
        private object newCurrentItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentItemChangingEventArgs"/> class.
        /// </summary>
        /// <param name="newCurrent">The item that is about to be come the new current one.</param>
        /// <param name="cancelable">True if the event may be canceled, false otherwise.</param>
        public CurrentItemChangingEventArgs(object newCurrent, bool cancelable)
        {
            this.newCurrentItem = newCurrent;
            this.cancelable = cancelable;
        }

        /// <summary>
        /// Gets the item that is about to become the new current one.
        /// </summary>
        public object NewCurrentItem
        {
            get
            {
                return this.newCurrentItem;
            }
        }

        /// <summary>
        /// Determines whether the event may be canceled.
        /// </summary>
        public bool Cancelable
        {
            get
            {
                return this.cancelable;
            }
        }

        /// <summary>
        /// Determines whether the event should be canceled.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                if (value && !this.cancelable)
                {
                    throw new InvalidOperationException("Event is not cancelable");
                }

                this.cancel = value;
            }
        }
    }
}
