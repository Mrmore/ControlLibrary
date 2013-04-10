using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.Primitives.Gestures
{
    internal class GestureBehavior : IPinchListener
    {
        /// <summary>
        /// Determines whether the Handled property of the manipulation events is honored or not.
        /// </summary>
        public bool CheckForHandled = true;

        internal bool isInHold;
        internal bool manipulating;
        internal UIElement manipulationContainer;
        internal object originalSource;
        internal FrameworkElement owner;
        internal Point primaryTouchPoint;
        internal bool started;

        internal Action<object, GestureEventArgs> PreviewGesture;
        internal Action<GestureBehavior, Gesture> NotifyGesture;
        internal Action NotifyGestureStart;
        internal Action<Gesture> NotifyGestureComplete;
        internal Action<Gesture> NotifyGestureCompleting;

        private bool gestureHandled;
        private bool mouseCaptureReleased;
        private ApplicationViewState currentOrientation;
        private Gesture lastGesture;

        internal GestureBehavior(FrameworkElement owner)
        {
            this.owner = owner;
        }

        event RoutedEventHandler IPinchListener.Unloaded
        {
            add
            {
                if (this.owner != null)
                {
                    this.owner.Unloaded += value;
                }
            }
            remove
            {
                if (this.owner != null)
                {
                    this.owner.Unloaded -= value;
                }
            }
        }

        public static DragDirection GetDragDirection(Point delta)
        {
            double offsetX = Math.Abs(delta.X);
            double offsetY = Math.Abs(delta.Y);

            if (offsetX > offsetY)
            {
                return DragDirection.Horizontal;
            }
            else if (offsetX < offsetY)
            {
                return DragDirection.Vertical;
            }

            return DragDirection.Indeterminate;
        }

        void IPinchListener.OnPinch(PinchGesture gesture)
        {
            if (this.HandleGesture(gesture))
            {
                this.gestureHandled = true;
            }
        }

        void IPinchListener.OnPinchComplete()
        {
            this.HandleGesture(new PinchCompleteGesture());
        }

        internal void Start(bool listenForHandledEvents)
        {
            if (listenForHandledEvents)
            {
                this.owner.AddHandler(UIElement.ManipulationStartedEvent, new EventHandler<Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs>(this.OnManipulationStarted), true);
                this.owner.AddHandler(UIElement.ManipulationCompletedEvent, new EventHandler<Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs>(this.OnManipulationCompleted), true);
                this.owner.AddHandler(UIElement.ManipulationDeltaEvent, new EventHandler<Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs>(this.OnManipulationDelta), true);
                this.owner.AddHandler(UIElement.HoldingEvent, new EventHandler<HoldingRoutedEventArgs>(this.OnHold), true);
                this.owner.AddHandler(UIElement.TappedEvent, new EventHandler<TappedRoutedEventArgs>(this.OnTap), true);
                this.owner.AddHandler(UIElement.DoubleTappedEvent, new EventHandler<DoubleTappedRoutedEventArgs>(this.OnDoubleTap), true);
            }
            else
            {
                this.owner.ManipulationStarted += this.OnManipulationStarted;
                this.owner.ManipulationCompleted += this.OnManipulationCompleted;
                this.owner.ManipulationDelta += this.OnManipulationDelta;
                this.owner.Holding += this.OnHold;
                this.owner.Tapped += this.OnTap;;
                this.owner.DoubleTapped += this.OnDoubleTap;
            }

            this.started = true;
        }

        internal void Start()
        {
            this.Start(false);
        }

        internal void Stop()
        {
            this.owner.ManipulationStarted -= this.OnManipulationStarted;
            this.owner.ManipulationCompleted -= this.OnManipulationCompleted;
            this.owner.ManipulationDelta -= this.OnManipulationDelta;
            this.owner.Holding -= this.OnHold;
            this.owner.DoubleTapped -= this.OnDoubleTap;
            this.owner.Tapped -= this.OnTap;

            this.started = false;
        }

        /// <summary>
        /// Creates gesture event args. Can be overridden by inheritors to create a new type of args.
        /// </summary>
        /// <param name="gesture">The gesture contained in the args.</param>
        /// <param name="primaryLocation">The location at which the gesture occurred.</param>
        /// <returns>A new instance of the GestureEventArgs class. The return value should never be null.</returns>
        protected virtual GestureEventArgs CreateGestureEventArgs(Gesture gesture, Point primaryLocation)
        {
            return new GestureEventArgs(gesture, primaryLocation);
        }

        private void OnManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            PinchProcessor.Instance.AddListener(this);

            this.mouseCaptureReleased = false;
            this.manipulating = true;
            this.isInHold = false;
            this.manipulationContainer = e.Container;
            this.originalSource = e.OriginalSource;
            this.primaryTouchPoint = this.manipulationContainer.TransformToVisual(this.owner).TransformPoint(e.Position);

            Frame frame = Window.Current.Content as Frame;
            if (frame != null)
            {
                //this.currentOrientation = frame.Orientation;
                this.currentOrientation = ApplicationView.Value;
            }

            if (this.NotifyGestureStart != null)
            {
                this.NotifyGestureStart();
            }
        }

        private void OnManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            if (this.CheckForHandled && e.Handled)
            {
                return;
            }

            bool handled = this.HandleManipulationDelta(e) || this.isInHold;
            if (!this.gestureHandled)
            {
                this.gestureHandled = handled;
            }

            if (this.gestureHandled && !this.mouseCaptureReleased)
            {
                // release the mouse capture - e.g. we may have a button pressed but to perform a pan gesture
                this.ReleaseMouseCapture(e.Container);
                this.mouseCaptureReleased = true;
            }

            e.Handled = this.gestureHandled;
        }

        private void OnManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            if (!this.manipulating)
            {
                // If manipulating is false this means
                // that manipulation completed is called without ever
                // starting the manipulation.
                // For example if a gesture behavior is created after the manipulation over an
                // element has started, it will not receive the started event, but will
                // receive the completed event.
                return;
            }

            if (this.NotifyGestureCompleting != null)
            {
                this.NotifyGestureCompleting(this.lastGesture);
            }

            this.manipulating = false;
            if (this.gestureHandled || !this.CheckForHandled || !e.Handled)
            {
                e.Handled = this.HandleManipulationCompleted(e) || this.gestureHandled;
            }

            this.isInHold = false;

            if (this.NotifyGestureComplete != null)
            {
                this.NotifyGestureComplete(this.lastGesture);
            }

            this.gestureHandled = false;
            this.manipulationContainer = null;
            this.originalSource = null;
            this.lastGesture = null;
        }

        private void OnHold(object sender, HoldingRoutedEventArgs e)
        {
            this.isInHold = true;

            // raise the Hold gesture explicitly as the TouchPanel gestures are handled on another thread and synchronization is not verified
            HoldGesture gesture = new HoldGesture() { Position = this.primaryTouchPoint };
            this.HandleGesture(gesture);
        }

        private void OnTap(object sender, TappedRoutedEventArgs e)
        {
            this.HandleGesture(new TapGesture() { Position = this.primaryTouchPoint });
        }

        private void OnDoubleTap(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.HandleGesture(new DoubleTapGesture() { Position = this.primaryTouchPoint });
        }

        private bool HandleManipulationDelta(Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            Point translation = e.Delta.Translation;
            if (translation.X != 0 || translation.Y != 0)
            {
                return this.HandleGesture(new PanGesture() { Position = this.primaryTouchPoint, CumulativeTranslation = e.Cumulative.Translation, DeltaTranslation = translation });
            }

            return false;
        }

        private bool HandleManipulationCompleted(Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            Point velocity = e.Velocities.Linear;
            if (velocity.X != 0 || velocity.Y != 0)
            {
                return this.HandleGesture(new FlickGesture() { Position = this.primaryTouchPoint, Velocity = velocity });
            }

            return false;
        }

        private bool HandleGesture(Gesture gesture)
        {
            this.lastGesture = gesture;
            if (this.RaisePreviewGesture(gesture))
            {
                return true;
            }

            if (this.NotifyGesture != null)
            {
                this.NotifyGesture(this, gesture);
                return gesture.Handled;
            }

            return false;
        }

        private bool RaisePreviewGesture(Gesture gesture)
        {
            if (this.PreviewGesture == null)
            {
                return false;
            }

            GestureEventArgs args = this.CreateGestureEventArgs(gesture, this.primaryTouchPoint);
            this.PreviewGesture(this, args);

            return gesture.Handled;
        }

        private void ReleaseMouseCapture(UIElement container)
        {
            UIElement current = container;
            while (current != null)
            {
                if (current == this.owner)
                {
                    break;
                }

                //current.ReleasePointerCapture(null);
                current.ReleasePointerCaptures();
                current = VisualTreeHelper.GetParent(current) as UIElement;
            }
        }
    }
}
