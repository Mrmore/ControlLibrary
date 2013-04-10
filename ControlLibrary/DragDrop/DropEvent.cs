using System;
using System.Net;
using System.Windows;
using System.Windows.Input;


namespace ControlLibrary
{
    /// <summary>
    /// Delegate for creating drop events
    /// </summary>
    /// <param name="sender">The drop sender.</param>
    /// <param name="args">Drop event args.</param>
    public delegate void DropEventHandler(object sender, DropEventArgs args);

    /// <summary>
    /// Class defining the drop event arguments
    /// </summary>
    public class DropEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DropEventArgs()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        public DropEventArgs(DragSource source)
        {
            DragSource = source;
        }

        /// <summary>
        /// Contains the dragsource being dropped
        /// </summary>
        public DragSource DragSource { get; set; }
    }
}
