using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlLibrary.AnimationMat;
using ControlLibrary.Primitives.Gestures;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary
{
    /// <summary>
    /// Represents a <see cref="SlideViewManipulationBehavior"/> that uses a Slide transition to navigate through items.
    /// </summary>
    public class SlideManipulationBehavior : SlideViewManipulationBehavior
    {
        private DoubleAnimation slideXAnimation;
        private DoubleAnimation slideYAnimation;
        private DoubleAnimation scaleXAnimation;
        private DoubleAnimation scaleYAnimation;
        private Storyboard storyboard;
        private CompositeTransform transform;
        private IDataSourceItem itemToSelect;
        //private IEasingFunction animationEasing;
        private EasingFunctionBase animationEasing;
        private Duration animationDuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlideManipulationBehavior"/> class.
        /// </summary>
        public SlideManipulationBehavior()
        {
            this.transform = new CompositeTransform();

            ExponentialEase exponentialEasing = new ExponentialEase();
            exponentialEasing.Exponent = 2 * Math.Log(PhysicsConstants.MotionParameters.Friction);
            exponentialEasing.EasingMode = EasingMode.EaseIn;
            this.animationEasing = exponentialEasing;

            this.animationDuration = new Duration(TimeSpan.FromMilliseconds(250));

            this.slideXAnimation = new DoubleAnimation();
            this.InitAnimation(this.slideXAnimation);

            this.slideYAnimation = new DoubleAnimation();
            this.InitAnimation(this.slideYAnimation);

            this.scaleXAnimation = new DoubleAnimation();
            this.InitAnimation(this.scaleXAnimation);

            this.scaleYAnimation = new DoubleAnimation();
            this.InitAnimation(this.scaleYAnimation);

            this.storyboard = new Storyboard();
            this.storyboard.Completed += this.OnSlideStoryboardCompleted;

            Storyboard.SetTarget(this.slideXAnimation, this.transform);
            Storyboard.SetTargetProperty(this.slideXAnimation, "(CompositeTransform.TranslateX)");
            Storyboard.SetTarget(this.slideYAnimation, this.transform);
            Storyboard.SetTargetProperty(this.slideYAnimation, "(CompositeTransform.TranslateY)");

            Storyboard.SetTarget(this.scaleXAnimation, this.transform);
            Storyboard.SetTargetProperty(this.scaleXAnimation, "(CompositeTransform.ScaleXProperty)");
            Storyboard.SetTarget(this.scaleYAnimation, this.transform);
            Storyboard.SetTargetProperty(this.scaleYAnimation, "(CompositeTransform.ScaleYProperty)");
        }

        /// <summary>
        /// Gets or sets the <see cref="IEasingFunction"/> instance used by the built-in slide animation.IEasingFunction
        /// </summary>
        public EasingFunctionBase AnimationEasing
        {
            get
            {
                return this.animationEasing;
            }
            set
            {
                this.animationEasing = value;
                this.InitAnimations();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Duration"/> instance used by the built-in slide animation.
        /// </summary>
        public Duration AnimationDuration
        {
            get
            {
                return this.animationDuration;
            }
            set
            {
                this.animationDuration = value;
                this.InitAnimations();
            }
        }

        internal override void ResetTransform()
        {
            base.ResetTransform();

            this.transform.TranslateX = 0;
            this.transform.TranslateY = 0;

            this.transform.ScaleX = 1;
            this.transform.ScaleY = 1;
        }

        internal override void MoveToNextItem()
        {
            if (AnimationState != SlideViewAnimationState.None)
            {
                return;
            }

            this.PerformFlick(-1, true);
        }

        internal override void MoveToPreviousItem()
        {
            if (AnimationState != SlideViewAnimationState.None)
            {
                return;
            }

            this.PerformFlick(1, true);
        }

        /// <summary>
        /// Called by the owning <see cref="RadSlideView"/> instance when the control is comletely loaded on the visual scene.
        /// </summary>
        protected internal override void OnLoaded()
        {
            base.OnLoaded();

            if (this.Panel != null)
            {
                this.Panel.RenderTransform = this.transform;
            }
        }

        /// <summary>
        /// Occurs upon a completion of a handled gesture.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnGestureCompleted(Gesture gesture)
        {
            base.OnGestureCompleted(gesture);

            if (gesture.GestureType == KnownGesture.Pan && this.GestureHandled)
            {
                // we had a pan gesture, without a flick one
                // remove the current pan offset with an animation
                double panLength = this.OffsetLength;
                if (panLength != 0)
                {
                    this.PerformFlick(Math.Sign(panLength), Math.Abs(panLength) >= this.Panel.ItemLength / 2);
                }
            }
            else if (gesture.GestureType == KnownGesture.Flick && this.AnimationState == SlideViewAnimationState.None && !this.GestureHandled)
            {
                // we have a flick gesture, preceded by a Pan one and we have not handled the Flick, we need to clear the Pan offset
                this.EndAnimate();
            }
        }

        /// <summary>
        /// Occurs when the behavior has been detached from a previously attached <see cref="RadSlideView"/> instance.
        /// </summary>
        /// <param name="oldView"></param>
        protected override void OnDetached(SlideView oldView)
        {
            base.OnDetached(oldView);

            if (oldView.Panel != null)
            {
                oldView.Panel.RenderTransform = null;
            }
        }

        /// <summary>
        /// Occurs upon a valid <see cref="PanGesture"/>.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnPan(PanGesture gesture)
        {
            if (this.View.Orientation == Orientation.Horizontal)
            {
                this.transform.TranslateX = this.Offset.X;
            }
            else
            {
                this.transform.TranslateY = this.Offset.Y;
            }
        }

        /// <summary>
        /// Occurs upon a valid <see cref="PanGesture"/> when the end (or beginning) of the sequence is reached and navigation to next/previous item may not be completed.
        /// </summary>
        /// <param name="panGesture"></param>
        protected override void OnPanStretch(PanGesture panGesture)
        {
            base.OnPanStretch(panGesture);

            double scaleFactor = 0.2;

            double offset = this.View.Orientation == Orientation.Horizontal ? panGesture.CumulativeTranslation.X : panGesture.CumulativeTranslation.Y;
            if (offset > 0)
            {
                offset = Math.Min(this.Panel.ItemLength * scaleFactor, offset);
            }
            else
            {
                offset = Math.Max(-this.Panel.ItemLength * scaleFactor, offset);
            }

            double absOffset = Math.Abs(offset);
            double scale = 1 - (absOffset / (this.Panel.ItemLength + (absOffset / scaleFactor)));

            if (this.View.Orientation == Orientation.Horizontal)
            {
                this.transform.CenterX = offset > 0 ? this.Panel.ItemLength : 0;
                this.transform.TranslateX = offset;
                this.transform.ScaleX = scale;
            }
            else
            {
                this.transform.CenterY = offset > 0 ? this.Panel.ItemLength : 0;
                this.transform.TranslateY = offset;
                this.transform.ScaleY = scale;
            }
        }

        /// <summary>
        /// Resets the Stretch transformations (if any).
        /// </summary>
        protected override void ResetStretch()
        {
            base.ResetStretch();

            if (this.AnimationState == SlideViewAnimationState.None)
            {
                this.AnimateStretch(0, 1);
            }
        }

        /// <summary>
        /// Occurs upon a valid <see cref="FlickGesture"/>.
        /// </summary>
        /// <param name="gesture"></param>
        protected override void OnFlick(FlickGesture gesture)
        {
            double velocity = this.View.Orientation == Orientation.Horizontal ? gesture.Velocity.X : gesture.Velocity.Y;
            int sign = Math.Sign(velocity);
            this.PerformFlick(sign, sign == Math.Sign(this.OffsetLength));
        }

        private void PerformFlick(int sign, bool changeSelection)
        {
            double to = 0;
            if (changeSelection)
            {
                // we have made enough offset to move to the previous/next item
                this.itemToSelect = sign < 0 ? this.View.GetNextDataSourceItem() : this.View.GetPreviousDataSourceItem();
                to = this.Panel.ItemLength * sign;
                this.Panel.OnSelectionAnimationStarted(sign);
            }

            this.AnimateSlide(to);
        }

        private void AnimateSlide(double to)
        {
            DoubleAnimation animation;

            if (this.View.Orientation == Orientation.Horizontal)
            {
                animation = this.slideXAnimation;
                animation.From = this.transform.TranslateX;
            }
            else
            {
                animation = this.slideYAnimation;
                animation.From = this.transform.TranslateY;
            }

            animation.To = to;

            ExponentialEase ee = new ExponentialEase();
            ee.Exponent = 2 * Math.Log(PhysicsConstants.MotionParameters.Friction);
            ee.EasingMode = EasingMode.EaseIn;
            animation.EasingFunction = ee;

            this.storyboard.Children.Add(animation);

            this.BeginAnimate(SlideViewAnimationState.Flick);
        }

        private void AnimateStretch(double translate, double scale)
        {
            if (this.View.Orientation == Orientation.Horizontal)
            {
                this.slideXAnimation.From = this.transform.TranslateX;
                this.slideXAnimation.To = translate;

                this.scaleXAnimation.From = this.transform.ScaleX;
                this.scaleXAnimation.To = scale;

                this.storyboard.Children.Add(this.slideXAnimation);
                this.storyboard.Children.Add(this.scaleXAnimation);
            }
            else
            {
                this.slideYAnimation.From = this.transform.TranslateY;
                this.slideYAnimation.To = translate;

                this.scaleYAnimation.From = this.transform.ScaleY;
                this.scaleYAnimation.To = scale;

                this.storyboard.Children.Add(this.slideYAnimation);
                this.storyboard.Children.Add(this.scaleYAnimation);
            }

            this.BeginAnimate(SlideViewAnimationState.Stretch);
        }

        private void BeginAnimate(SlideViewAnimationState state)
        {
            this.storyboard.Begin();
            this.AnimationState = state;
        }

        private void EndAnimate()
        {
            bool resetTransform = true;
            if (this.itemToSelect != null)
            {
                this.View.ChangeSelectedItem(this.itemToSelect, false);
                this.itemToSelect = null;

                // the owning panel will tell whether we will reset the transform or the panel itself will (valid when only the viewport item is realized)
                resetTransform = !this.Panel.OnSelectionAnimationCompleted();
            }

            if (resetTransform)
            {
                this.ResetTransform();
            }

            this.storyboard.Stop();
            this.storyboard.Children.Clear();
            this.AnimationState = SlideViewAnimationState.None;
        }

        private void InitAnimation(DoubleAnimation animation)
        {
            animation.Duration = this.animationDuration;
            animation.EasingFunction = this.animationEasing;
        }

        private void InitAnimations()
        {
            this.InitAnimation(this.slideXAnimation);
            this.InitAnimation(this.slideYAnimation);
            this.InitAnimation(this.scaleXAnimation);
            this.InitAnimation(this.scaleYAnimation);
        }

        private void OnSlideStoryboardCompleted(object sender, object e)
        {
            this.EndAnimate();
        }
    }
}
