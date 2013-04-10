using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace ControlLibrary
{
    /// <summary>
    /// Initial values for Drag Drop Manager
    /// </summary>
    public class InitialValues
    {
        /// <summary>
        /// This property contains the Containing Layout Panel, used to correctly position DragSources
        /// when hovering and to make sure they are always on top.  You'd typically set this once, in your
        /// Page constructor, to the surrounding LayoutRoot of your application.
        /// </summary>
        public static Panel ContainingLayoutPanel { get; set; }

    }
}
