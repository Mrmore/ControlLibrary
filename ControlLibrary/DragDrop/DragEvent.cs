using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;

namespace ControlLibrary
{
    /// <summary>
    /// Delegate for creating drag events
    /// </summary>
    /// <param name="sender">The dragging sender.</param>
    /// <param name="args">Drag event args.</param>
    public delegate void DragEventHandler(object sender, DragEventArgs args);

    /// <summary>
    /// Class defining the drag event arguments
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// Blank Constuctor
        /// </summary>
        public DragEventArgs()
        {
        }

        /// <summary>
        /// Contstructor with bits
        /// </summary>
        /// <param name="horizontalChange">Horizontal change</param>
        /// <param name="verticalChange">Vertical change</param>
        /// <param name="mouseEventArgs">The mouse event args</param>
        //PointerEventArgs DragEventArgs
        public DragEventArgs(double horizontalChange, double verticalChange, PointerRoutedEventArgs mouseEventArgs)
        {
            this.HorizontalChange = horizontalChange;
            this.VerticalChange = verticalChange;
            this.MouseEventArgs = mouseEventArgs;
        }


        /// <summary>
        /// Gets or sets the horizontal change of the drag
        /// </summary>
        public double HorizontalChange { get; set; }

        /// <summary>
        /// Gets or sets the vertical change of the drag
        /// </summary>
        public double VerticalChange { get; set; }

        /// <summary>
        /// Gets or sets the mouse event args
        /// </summary>
        //public PointerEventArgs MouseEventArgs { get; set; }
        public PointerRoutedEventArgs MouseEventArgs { get; set; }

    }
}
