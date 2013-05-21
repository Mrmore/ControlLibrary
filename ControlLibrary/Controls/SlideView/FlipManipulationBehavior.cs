using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ControlLibrary.Primitives.Gestures;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

//检查检查检查检查
namespace ControlLibrary
{
    /// <summary>
    /// Represents a <see cref="SlideViewManipulationBehavior"/> that uses a Flip transition to navigate through items.
    /// </summary>
    public class FlipManipulationBehavior : SlideViewManipulationBehavior
    {
        /// <summary>
        /// Identifies the <see cref="EmptyBackground"/> property.
        /// </summary>
        public static readonly DependencyProperty EmptyBackgroundProperty =
            DependencyProperty.Register("EmptyBackground", typeof(Brush), typeof(FlipManipulationBehavior), new PropertyMetadata(null));

        private static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(FlipManipulationBehavior), new PropertyMetadata(0d, OnRotationChanged));

        private const int AnimationDiration = 300;
        private const double ProjectionFlipAngleH = 83.25;
        private const double ProjectionFlipDifferenceH = 2 * (90 - ProjectionFlipAngleH);
        private const double ProjectionFlipAngleV = 79.25;
        private const double ProjectionFlipDifferenceV = 2 * (90 - ProjectionFlipAngleV);

        private Rectangle animationBack;
        private Rectangle animationFront;
        private Rectangle partialItem;
        private PlaneProjection backProjection;
        private PlaneProjection frontProjection;
        private SnapshotState snapshotState;
        private DoubleAnimation rotationAnimation;
        private Storyboard storyboard;
        private double rotation;
        private bool waitingForFlick;
        private int panSign;
        private int previousPanSign = int.MinValue;
        private int selectionChangedSign;
        private int flickSign;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlipManipulationBehavior"/> class.
        /// </summary>
        public FlipManipulationBehavior()
        {
            this.animationBack = new Rectangle();
            this.animationFront = new Rectangle();
            this.partialItem = new Rectangle();

            this.rotationAnimation = new DoubleAnimation();
            this.rotationAnimation.EasingFunction = new CubicEase();

            this.storyboard = new Storyboard();
            this.storyboard.Children.Add(this.rotationAnimation);
            this.storyboard.Completed += this.OnStoryboardCompleted;
            Storyboard.SetTarget(this.rotationAnimation, this);
            Storyboard.SetTargetProperty(this.rotationAnimation, "(FlipManipulationBehavior.Rotation)");
        }

        private enum SnapshotState
        {
            None,
            Generating,
            Ready
        }

        /// <summary>
        /// Gets or sets the <see cref="Brush"/> instance to be used when generating the Stretch snapshots.
        /// </summary>
        public Brush EmptyBackground
        {
            get
            {
                return this.GetValue(EmptyBackgroundProperty) as Brush;
            }
            set
            {
                this.SetValue(EmptyBackgroundProperty, value);
            }
        }

        private double MaxRotationAngle
        {
            get
            {
                if (this.panSign < 0)
                {
                    return 180;
                }

                if (this.View == null)
                {
                    return 180;
                }

                if (this.View.Orientation == Orientation.Horizontal)
                {
                    return 180 - ProjectionFlipDifferenceH;
                }

                return 180 - ProjectionFlipDifferenceV;
            }
        }

        internal override void MoveToNextItem()
        {
            if (this.snapshotState != SnapshotState.None)
            {
                return;
            }

            // force the snapshots to be regenerated
            this.previousPanSign = int.MinValue;

            this.panSign = this.flickSign = -1;
            this.PrepareSnapshots();
            this.waitingForFlick = true;
        }

        internal override void MoveToPreviousItem()
        {
            if (this.snapshotState != SnapshotState.None)
            {
                return;
            }

            // force the snapshots to be regenerated
            this.previousPanSign = int.MinValue;

            this.panSign = this.flickSign = 1;
            this.PrepareSnapshots();
            this.waitingForFlick = true;
        }

        internal override void ResetTransform()
        {
            base.ResetTransform();

            this.previousPanSign = this.panSign;
            this.panSign = 0;
            this.flickSign = 0;
            this.rotation = 0;

            this.View.TransitionLayer.Opacity = 0;
            this.snapshotState = SnapshotState.None;
        }

        internal override void ResetCache()
        {
            base.ResetCache();

            this.previousPanSign = Int32.MinValue;
            this.selectionChangedSign = 0;
        }

        /// <summary>
        /// Called by the owning <see cref="RadSlideView"/> instance when the control is comletely loaded on the visual scene.
        /// </summary>
        protected internal override void OnLoaded()
        {
            base.OnLoaded();

            if (this.View.TransitionLayer != null)
            {
                this.View.TransitionLayer.Children.Add(this.partialItem);
                this.View.TransitionLayer.Children.Add(this.animationFront);
                this.View.TransitionLayer.Children.Add(this.animationBack);
            }
        }

        /// <summary>
        /// Occurs when the behavior has been detached from a previously attached <see cref="RadSlideView"/> instance.
        /// </summary>
        /// <param name="oldView"></param>
        protected override void OnDetached(SlideView oldView)
        {
            base.OnDetached(oldView);

            oldView.TransitionLayer.Children.Clear();
        }

        /// <summary>
        /// Determines whether all the condinitions, needed to process a gesture, are met.
        /// </summary>
        /// <param name="gesture">The gesture to examine.</param>
        /// <returns></returns>
        protected override bool CanHandleGesture(Gesture gesture)
        {
            if (this.waitingForFlick || this.snapshotState == SnapshotState.Generating)
            {
                return false;
            }

            return base.CanHandleGesture(gesture);
        }

        /// <summary>
        /// Occurs upon a valid <see cref="PanGesture"/>.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnPan(PanGesture gesture)
        {
            base.OnPan(gesture);

            this.PreparePan(this.OffsetLength);
            if (this.snapshotState == SnapshotState.Ready)
            {
                this.UpdateProjections();
            }
        }

        /// <summary>
        /// Occurs upon a completion of a handled gesture.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnGestureCompleted(Gesture gesture)
        {
            base.OnGestureCompleted(gesture);

            if (this.AnimationState != SlideViewAnimationState.None)
            {
                return;
            }

            if (gesture.GestureType == KnownGesture.Pan)
            {
                if (this.snapshotState != SnapshotState.Ready || !gesture.Handled || this.OffsetLength == 0)
                {
                    this.ResetTransform();
                    return;
                }

                // we had a pan gesture, without a flick one
                // remove the current pan offset with an animation
                bool changeSelection = this.rotation > ProjectionFlipAngleH;
                double to = changeSelection ? this.MaxRotationAngle : 0;
                this.AnimateRotation(to, SlideViewAnimationState.Flick, AnimationDiration);
            }
            else if (gesture.GestureType == KnownGesture.Flick && !gesture.Handled)
            {
                this.ResetTransform();
            }
        }

        /// <summary>
        /// Occurs upon a valid <see cref="FlickGesture"/>.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnFlick(FlickGesture gesture)
        {
            base.OnFlick(gesture);

            if (this.snapshotState == SnapshotState.None)
            {
                if (this.waitingForFlick)
                {
                    Debug.Assert(false, "Wrong state.");
                    this.waitingForFlick = false;
                }
                return;
            }

            double velocityLength = this.View.Orientation == Orientation.Horizontal ? gesture.Velocity.X : gesture.Velocity.Y;
            this.flickSign = Math.Sign(velocityLength);

            if (this.snapshotState == SnapshotState.Generating)
            {
                this.waitingForFlick = true;
            }
            else
            {
                double to = this.flickSign == this.panSign ? this.MaxRotationAngle : 0;
                this.AnimateRotation(to, SlideViewAnimationState.Flick, AnimationDiration);
            }
        }

        /// <summary>
        /// Occurs upon a valid <see cref="PanGesture"/> when the end (or beginning) of the sequence is reached and navigation to next/previous item may not be completed.
        /// </summary>
        /// <param name="panGesture"></param>
        protected override void OnPanStretch(PanGesture panGesture)
        {
            base.OnPanStretch(panGesture);

            double length = this.View.Orientation == Orientation.Horizontal ? panGesture.CumulativeTranslation.X : panGesture.CumulativeTranslation.Y;
            this.PreparePan(length);
            this.rotation = Math.Min(this.rotation, 60);

            this.UpdateProjections();
        }

        /// <summary>
        /// Resets the Stretch transformations (if any).
        /// </summary>
        protected override void ResetStretch()
        {
            base.ResetStretch();

            if (this.snapshotState == SnapshotState.Ready)
            {
                this.AnimateRotation(0, SlideViewAnimationState.Stretch, AnimationDiration);
            }
            else
            {
                this.ResetTransform();
            }
        }

        private static void OnRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlipManipulationBehavior strategy = d as FlipManipulationBehavior;
            strategy.rotation = (double)e.NewValue;
            strategy.UpdateProjections();
        }

        private void PrepareSnapshots()
        {
            if (this.snapshotState == SnapshotState.Generating)
            {
                return;
            }

            this.snapshotState = SnapshotState.Generating;

            // prepare all snapshots
            this.PreparePartialSnapshot();
            this.PrepareBackSnapshot();
            this.PrepareFrontSnapshot();

            //// TODO: Revisit this logic for the official Q1
            ////if (this.selectionChangedSign == 0)
            ////{
            ////    if (this.previousPanSign == this.panSign)
            ////    {
            ////        // we already have generated snapshots for this pan direction
            ////    }
            ////    else
            ////    {
            ////        // prepare all snapshots
            ////        this.PreparePartialSnapshot();
            ////        this.PrepareBackSnapshot();
            ////        this.PrepareFrontSnapshot();
            ////    }
            ////}
            ////else
            ////{
            ////    Rectangle temp;

            ////    // check which snapshots may be reused
            ////    if (this.selectionChangedSign == this.panSign)
            ////    {
            ////        // we may reuse the partial item - it is currently the Front one
            ////        temp = this.animationFront;
            ////        this.animationFront = this.partialItem;
            ////        this.partialItem = temp;

            ////        this.PrepareBackSnapshot();
            ////    }
            ////    else
            ////    {
            ////        // we may reuse the front/back snapshots and re-generate only the partial one
            ////        temp = this.animationFront;
            ////        this.animationFront = this.animationBack;
            ////        this.animationBack = temp;
            ////    }

            ////    this.PreparePartialSnapshot();
            ////    this.selectionChangedSign = 0;
            ////}

            this.animationFront.Projection = this.frontProjection = this.CreateProjection();
            this.animationBack.Projection = this.backProjection = this.CreateProjection();

            this.UpdateProjections();
            this.LayoutSnapshots();

            this.View.TransitionLayer.Opacity = 1;
            CompositionTarget.Rendering += this.OnCompositionTargetRendering;
        }

        private void PrepareFrontSnapshot()
        {
            TranslateTransform translate = null;
            if (this.panSign < 0)
            {
                if (this.View.Orientation == Orientation.Horizontal)
                {
                    translate = new TranslateTransform() { X = -this.Panel.ItemLength / 2 };
                }
                else
                {
                    translate = new TranslateTransform() { Y = -this.Panel.ItemLength / 2 };
                }
            }

            this.UpdateSnapshot(this.animationFront, this.Panel.PivotItem, translate);
        }

        private void PrepareBackSnapshot()
        {
            TranslateTransform translate = null;
            ContentControl item;
            if (this.panSign < 0)
            {
                item = this.Panel.NextItem;
            }
            else
            {
                item = this.Panel.PreviousItem;
                if (this.View.Orientation == Orientation.Horizontal)
                {
                    translate = new TranslateTransform() { X = -this.Panel.ItemLength / 2 };
                }
                else
                {
                    translate = new TranslateTransform() { Y = -this.Panel.ItemLength / 2 };
                }
            }

            this.UpdateSnapshot(this.animationBack, item, translate);
        }

        private void PreparePartialSnapshot()
        {
            TranslateTransform translate = null;
            ContentControl item;
            if (this.panSign < 0)
            {
                item = this.Panel.NextItem;
                if (this.View.Orientation == Orientation.Horizontal)
                {
                    translate = new TranslateTransform() { X = -this.Panel.ItemLength / 2 };
                }
                else
                {
                    translate = new TranslateTransform() { Y = -this.Panel.ItemLength / 2 };
                }
            }
            else
            {
                item = this.Panel.PreviousItem;
            }

            this.UpdateSnapshot(this.partialItem, item, translate);
        }

        private void UpdateSnapshot(Rectangle visual, ContentControl snapshotSource, TranslateTransform transform)
        {
            Size size = this.GetSnapshotSize(snapshotSource);

            if (snapshotSource.Content == null)
            {
                Brush emptyBackground = this.EmptyBackground;
                if (emptyBackground == null)
                {
                    emptyBackground = Application.Current.Resources["PhoneBackgroundBrush"] as Brush;
                }
                visual.Fill = emptyBackground;
            }
            else
            {
                WriteableBitmap source = this.GetSnapshot(snapshotSource, transform, size);
                visual.Fill = new ImageBrush() { ImageSource = source };
            }

            visual.Width = size.Width;
            visual.Height = size.Height;
        }

        private void UpdateProjections()
        {
            if (this.View.Orientation == Orientation.Horizontal)
            {
                this.UpdateProjectionsHorizontally();
            }
            else
            {
                this.UpdateProjectionsVertically();
            }
        }

        private void UpdateProjectionsHorizontally()
        {
            if (this.panSign < 0)
            {
                this.frontProjection.RotationY = this.rotation;
                if (this.rotation <= ProjectionFlipAngleH)
                {
                    this.animationBack.Opacity = 0;
                    this.animationFront.Opacity = 1;
                    this.backProjection.RotationY = this.rotation;
                    this.backProjection.LocalOffsetX = 0;
                }
                else
                {
                    this.animationBack.Opacity = 1;
                    this.animationFront.Opacity = 0;
                    this.backProjection.LocalOffsetX = -this.animationBack.Width;
                    this.backProjection.RotationY = -(this.MaxRotationAngle - this.rotation);
                }
            }
            else
            {
                this.frontProjection.GlobalOffsetX = this.animationFront.Width;
                this.frontProjection.LocalOffsetX = -this.animationFront.Width;
                this.frontProjection.RotationY = -this.rotation;

                if (this.rotation <= ProjectionFlipAngleH)
                {
                    this.animationFront.Opacity = 1;
                    this.animationBack.Opacity = 0;
                }
                else
                {
                    this.animationBack.Opacity = 1;
                    this.animationFront.Opacity = 0;
                    this.backProjection.RotationY = this.MaxRotationAngle - this.rotation;
                }
            }
        }

        private void UpdateProjectionsVertically()
        {
            if (this.panSign < 0)
            {
                if (this.rotation <= ProjectionFlipAngleV)
                {
                    this.animationBack.Opacity = 0;
                    this.animationFront.Opacity = 1;
                    this.frontProjection.RotationX = -this.rotation;
                }
                else
                {
                    this.animationBack.Opacity = 1;
                    this.animationFront.Opacity = 0;
                    this.backProjection.LocalOffsetY = -this.animationBack.Height;
                    this.backProjection.RotationX = this.MaxRotationAngle - this.rotation;
                }
            }
            else
            {
                if (this.rotation <= ProjectionFlipAngleV)
                {
                    this.animationFront.Opacity = 1;
                    this.animationBack.Opacity = 0;

                    this.frontProjection.GlobalOffsetY = this.animationFront.Height;
                    this.frontProjection.LocalOffsetY = -this.animationFront.Height;
                    this.frontProjection.RotationX = this.rotation;
                }
                else
                {
                    this.animationBack.Opacity = 1;
                    this.animationFront.Opacity = 0;
                    this.backProjection.RotationX = 180 + this.rotation + ProjectionFlipDifferenceV;
                }
            }
        }

        private PlaneProjection CreateProjection()
        {
            PlaneProjection projection = new PlaneProjection();
            projection.CenterOfRotationY = 0;
            projection.CenterOfRotationX = 0;
            projection.CenterOfRotationZ = 0;
            projection.RotationX = 0;
            projection.RotationZ = 0;

            return projection;
        }

        private void LayoutSnapshots()
        {
            Canvas.SetZIndex(this.partialItem, 0);
            Canvas.SetZIndex(this.animationBack, 1);
            Canvas.SetZIndex(this.animationFront, 2);

            double middle = Math.Round(this.Panel.ItemLength / 2);
            double pixelOffset = 0.5 * this.panSign;

            if (this.View.Orientation == Orientation.Horizontal)
            {
                Canvas.SetTop(this.animationBack, 0);
                Canvas.SetTop(this.animationFront, 0);
                Canvas.SetTop(this.partialItem, 0);

                double left = this.panSign < 0 ? middle : 0;

                Canvas.SetLeft(this.animationBack, middle - pixelOffset);
                Canvas.SetLeft(this.animationFront, left + pixelOffset);
                Canvas.SetLeft(this.partialItem, left);
            }
            else
            {
                Canvas.SetLeft(this.animationBack, 0);
                Canvas.SetLeft(this.animationFront, 0);
                Canvas.SetLeft(this.partialItem, 0);

                double top = this.panSign < 0 ? middle : 0;

                Canvas.SetTop(this.animationBack, middle - pixelOffset);
                Canvas.SetTop(this.animationFront, top - pixelOffset);
                Canvas.SetTop(this.partialItem, top);
            }
        }

        private void OnCompositionTargetRendering(object sender, object e)
        {
            CompositionTarget.Rendering -= this.OnCompositionTargetRendering;
            this.snapshotState = SnapshotState.Ready;

            if (this.waitingForFlick)
            {
                this.waitingForFlick = false;
                double to = this.panSign == this.flickSign ? this.MaxRotationAngle : 0;
                this.AnimateRotation(to, SlideViewAnimationState.Flick, 2 * AnimationDiration);
            }
            else if (!this.IsManipulating)
            {
                this.ResetTransform();
            }
        }

        private Size GetSnapshotSize(FrameworkElement item)
        {
            double width = 0;
            double height = 0;

            if (this.View.Orientation == Orientation.Horizontal)
            {
                width = (item.ActualWidth / 2) + 1;
                height = item.ActualHeight;
            }
            else
            {
                width = item.ActualWidth;
                height = (item.ActualHeight / 2) + 1;
            }

            return new Size(width, height);
        }

        private WriteableBitmap GetSnapshot(FrameworkElement item, Transform transform, Size size)
        {
            WriteableBitmap wbmp = new WriteableBitmap((int)size.Width, (int)size.Height);
            //wbmp.Render(item, transform);
            wbmp.Invalidate();

            return wbmp;
        }

        private void PreparePan(double offset)
        {
            int sign = Math.Sign(offset);
            if (this.snapshotState == SnapshotState.None)
            {
                this.panSign = sign;
                this.PrepareSnapshots();
            }

            if (sign != this.panSign)
            {
                offset = 0;
            }

            double factor = Math.Abs(offset) / this.Panel.ItemLength;
            this.rotation = factor * 180;
        }

        private void AnimateRotation(double to, SlideViewAnimationState state, double milliseconds)
        {
            if (this.AnimationState != SlideViewAnimationState.None)
            {
                return;
            }

            this.AnimationState = state;
            this.rotationAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(milliseconds));
            this.rotationAnimation.From = this.rotation;
            this.rotationAnimation.To = to;
            this.storyboard.Begin();
            this.Panel.OnSelectionAnimationStarted(this.panSign);
        }

        private void OnStoryboardCompleted(object sender, object e)
        {
            bool changeSeletion = this.rotation == this.MaxRotationAngle;
            bool resetTransform = true;

            if (changeSeletion)
            {
                // we have made enough offset to move to the previous/next item
                IDataSourceItem item = this.panSign < 0 ? this.View.GetNextDataSourceItem() : this.View.GetPreviousDataSourceItem();
                if (item != null)
                {
                    this.View.ChangeSelectedItem(item, false);
                }

                // the owning panel will tell whether we will reset the transform or the panel itself will (valid when only the viewport item is realized)
                resetTransform = !this.Panel.OnSelectionAnimationCompleted();
                this.selectionChangedSign = this.panSign;
            }
            else
            {
                this.selectionChangedSign = 0;
            }

            if (resetTransform)
            {
                this.ResetTransform();
            }

            this.storyboard.Stop();
            this.AnimationState = SlideViewAnimationState.None;
        }
    }
}
