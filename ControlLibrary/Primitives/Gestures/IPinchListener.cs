using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;

namespace ControlLibrary.Primitives.Gestures
{
    internal interface IPinchListener
    {
        event RoutedEventHandler Unloaded;
        void OnPinch(PinchGesture gesture);
        void OnPinchComplete();
    }
}
