using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Charting
{
    /// <summary>
    /// Represents an object that implements the <see cref="IDisposable"/> interface and automatically releases any events, attached to this instance.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private static readonly object EventDisposingKey = new object();
        private static readonly object EventDisposedKey = new object();

        private bool isDisposing;
        private bool isDisposed;
        private EventHandlerList events;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        protected DisposableObject()
        {
            this.events = new EventHandlerList();
        }

        /// <summary>
        /// Raised when the object enters its Dispose routine.
        /// </summary>
        public event EventHandler Disposing
        {
            add
            {
                this.events.AddHandler(EventDisposingKey, value);
            }
            remove
            {
                this.events.RemoveHandler(EventDisposingKey, value);
            }
        }

        /// <summary>
        /// Raised when the object has finished its Dispose routine.
        /// </summary>
        public event EventHandler Disposed
        {
            add
            {
                this.events.AddHandler(EventDisposedKey, value);
            }
            remove
            {
                this.events.RemoveHandler(EventDisposedKey, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is currently in a Dispose cycle.
        /// </summary>
        public bool IsDisposing
        {
            get
            {
                return this.isDisposing;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is already disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return this.isDisposed;
            }
        }

        /// <summary>
        /// Gets the list of all events, associated with this instance.
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                return this.events;
            }
        }

        /// <summary>
        /// Releases all resources, used by this instance.
        /// </summary>
        public void Dispose()
        {
            this.DisposeCore(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all managed resources allocated by this instance.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {
            this.events.Clear();
        }

        /// <summary>
        /// Disposes all unmanaged resources allocated by this instance.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        {
            // TODO: Override this method if any unmanaged resources such as file handles are allocated
        }

        private void DisposeCore(bool disposing)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.IsDisposing)
            {
                throw new InvalidOperationException("Object is currently disposing its resources.");
            }

            this.isDisposing = true;

            if (disposing)
            {
                this.DisposeManagedResources();
            }

            this.DisposeUnmanagedResources();

            this.isDisposing = false;
            this.isDisposed = true;
        }
    }
}
