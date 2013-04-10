using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ControlLibrary.Primitives.Gestures
{
    internal class PinchProcessor
    {
        private static readonly PinchProcessor instance = new PinchProcessor();
        private List<IPinchListener> listeners;
        private PinchGesture gesture = new PinchGesture();
        private bool pinching;
        private bool eventHooked;

        private PinchProcessor()
        {
            this.listeners = new List<IPinchListener>(8);
        }

        public static PinchProcessor Instance
        {
            get
            {
                return instance;
            }
        }

        public void AddListener(IPinchListener listener)
        {
            this.listeners.Add(listener);
            listener.Unloaded += this.OnListenerUnloaded;

            if (!this.eventHooked)
            {
                //Windows.UI.Xaml.Input.Pointer
                //Touch.FrameReported += this.OnFrameReported;
                this.eventHooked = true;
            }
        }

        public void RemoveListener(IPinchListener listener)
        {
            if (this.listeners.Count == 0)
            {
                return;
            }

            this.listeners.Remove(listener);
            listener.Unloaded -= this.OnListenerUnloaded;

            if (this.listeners.Count == 0)
            {
                //Touch.FrameReported -= this.OnFrameReported;
                this.eventHooked = false;
                this.pinching = false;
            }
        }

        private static double GetPointDistance(Point pt1, Point pt2)
        {
            double dx = pt1.X - pt2.X;
            double dy = pt1.Y - pt2.Y;

            return Math.Max(1, Math.Sqrt((dx * dx) + (dy * dy)));
        }

        /*private void OnFrameReported(object sender, TouchFrameEventArgs e)
        {
            if (this.listeners.Count == 0)
            {
                Debug.Assert(false, "Event hooked without listeners.");
                return;
            }

            TouchPointCollection points = e.GetTouchPoints(null);
            if (points.Count != 2)
            {
                // we are interested in Pinch gesture only
                if (points.Count == 1 && points[0].Action == TouchAction.Up)
                {
                    // last touch point is released, automatically remove all the listeners
                    // we assume that each listener registers itself upon a ManipulationStarted event
                    this.ClearListeners();
                }
                return;
            }

            TouchPoint firstPoint = points[0];
            TouchPoint secondPoint = points[1];

            if (firstPoint.Action == TouchAction.Up || secondPoint.Action == TouchAction.Up)
            {
                // we are completing the Pinch gesture
                this.pinching = false;
                this.listeners.Apply<IPinchListener>(listener => listener.OnPinchComplete());
                return;
            }

            this.UpdateGesture(firstPoint, secondPoint);

            if (!this.pinching)
            {
                this.pinching = true;
                this.gesture.Scale = 1;
                this.gesture.StartDistance = this.gesture.Distance;
            }
            else
            {
                this.gesture.Scale = this.gesture.Distance / this.gesture.StartDistance;
                foreach (IPinchListener listener in this.listeners)
                {
                    if (ElementTreeHelper.IsParentChainHitTestVisible(listener as UIElement))
                    {
                        listener.OnPinch(this.gesture);
                    }
                }
            }
        }*/

        private void ClearListeners()
        {
            for (int i = this.listeners.Count - 1; i >= 0; i--)
            {
                this.RemoveListener(this.listeners[i]);
            }
        }

        /*private void UpdateGesture(TouchPoint pt1, TouchPoint pt2)
        {
            this.gesture.Handled = false;
            this.gesture.Position = pt1.Position;
            this.gesture.Position2 = pt2.Position;

            double dx = pt1.Position.X - pt2.Position.X;
            double dy = pt1.Position.Y - pt2.Position.Y;

            this.gesture.Distance = Math.Max(1, Math.Sqrt((dx * dx) + (dy * dy)));
        }*/

        private void OnListenerUnloaded(object sender, RoutedEventArgs e)
        {
            IPinchListener listener = e.OriginalSource as IPinchListener;
            if (listener != null)
            {
                this.RemoveListener(listener);
            }
        }
    }
}
