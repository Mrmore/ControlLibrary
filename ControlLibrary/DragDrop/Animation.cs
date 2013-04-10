using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace ControlLibrary
{
    /// <summary>
    /// Class defining animations
    /// </summary>
    internal class Animation
    {
        /// <summary>
        /// Creates an animation on the drop target to show "hover" effect when hovering in
        /// </summary>
        /// <param name="objectToBeAnimated"></param>
        /// <returns></returns>
        internal static Storyboard CreateDropTargetHoverIn(DependencyObject objectToBeAnimated)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimationUsingKeyFrames animOpacity = new DoubleAnimationUsingKeyFrames();

            animOpacity.BeginTime = new TimeSpan(0);
            animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.0, 0));
            animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.2, 1));
            sb.Children.Add(animOpacity);

            Storyboard.SetTarget(animOpacity, objectToBeAnimated);
            Storyboard.SetTargetProperty(animOpacity, "(UIElement.Opacity)");

            return sb;
        }

        /// <summary>
        /// Creates an animation on the drop target to show "hover" effect when hovering in
        /// </summary>
        /// <param name="objectToBeAnimated"></param>
        /// <returns></returns>
        internal static Storyboard CreateDropTargetHoverOut(DependencyObject objectToBeAnimated)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimationUsingKeyFrames animOpacity = new DoubleAnimationUsingKeyFrames();

            animOpacity.BeginTime = new TimeSpan(0);
            animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.0, 1));
            animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.2, 0));
            sb.Children.Add(animOpacity);

            Storyboard.SetTarget(animOpacity, objectToBeAnimated);
            Storyboard.SetTargetProperty(animOpacity, "(UIElement.Opacity)");

            return sb;
        }

        /// <summary>
        /// Animation to return a dragged element to its original position
        /// </summary>
        /// <param name="objectToBeAnimated"></param>
        /// <param name="CurrentPosition"></param>
        /// <param name="FinalPosition"></param>
        /// <param name="duration"></param>
        /// <param name="easingfunction"></param>
        /// <returns></returns>
        internal static Storyboard ReturnDragToOriginalPosition(DependencyObject objectToBeAnimated,
            Point CurrentPosition, Point FinalPosition, double duration, EasingFunctionBase easingfunction)
        {

            if (easingfunction == null)
            {
                Storyboard sb = new Storyboard();

                DoubleAnimationUsingKeyFrames animTop = new DoubleAnimationUsingKeyFrames();
                DoubleAnimationUsingKeyFrames animLeft = new DoubleAnimationUsingKeyFrames();

                animTop.BeginTime = new TimeSpan(0);
                animTop.KeyFrames.Add(getSplineDoubleKeyFrame(0.0, CurrentPosition.Y));
                animTop.KeyFrames.Add(getSplineDoubleKeyFrame(duration, FinalPosition.Y));
                sb.Children.Add(animTop);

                Storyboard.SetTarget(animTop, objectToBeAnimated);
                Storyboard.SetTargetProperty(animTop, "(Canvas.Top)");

                animLeft.BeginTime = new TimeSpan(0);
                animLeft.KeyFrames.Add(getSplineDoubleKeyFrame(0.0, CurrentPosition.X));
                animLeft.KeyFrames.Add(getSplineDoubleKeyFrame(duration, FinalPosition.X));
                sb.Children.Add(animLeft);

                Storyboard.SetTarget(animLeft, objectToBeAnimated);
                Storyboard.SetTargetProperty(animLeft, "(Canvas.Left)");

                return sb;
            }
            else
            {
                Storyboard sb = new Storyboard();

                DoubleAnimationUsingKeyFrames animTop = new DoubleAnimationUsingKeyFrames();
                DoubleAnimationUsingKeyFrames animLeft = new DoubleAnimationUsingKeyFrames();

                animTop.BeginTime = new TimeSpan(0);
                animTop.KeyFrames.Add(getEasingDoubleKeyFrame(0.0, CurrentPosition.Y, easingfunction));
                animTop.KeyFrames.Add(getEasingDoubleKeyFrame(duration, FinalPosition.Y, easingfunction));
                sb.Children.Add(animTop);

                Storyboard.SetTarget(animTop, objectToBeAnimated);
                Storyboard.SetTargetProperty(animTop, "(Canvas.Top)");

                animLeft.BeginTime = new TimeSpan(0);
                animLeft.KeyFrames.Add(getEasingDoubleKeyFrame(0.0, CurrentPosition.X, easingfunction));
                animLeft.KeyFrames.Add(getEasingDoubleKeyFrame(duration, FinalPosition.X, easingfunction));
                sb.Children.Add(animLeft);

                Storyboard.SetTarget(animLeft, objectToBeAnimated);
                Storyboard.SetTargetProperty(animLeft, "(Canvas.Left)");

                return sb;
            }
        }

        /// <summary>
        /// Creates an animation on the drop target to show "hover" effect when hovering in
        /// </summary>
        /// <param name="objectToBeAnimated"></param>
        /// <returns></returns>
        internal static Storyboard CreateDropTargetHoverOutFromAnyAnimationPosition(DependencyObject objectToBeAnimated)
        {
            Storyboard sb = new Storyboard();

            DoubleAnimationUsingKeyFrames animOpacity = new DoubleAnimationUsingKeyFrames();

            animOpacity.BeginTime = new TimeSpan(0);
            // animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.0, 1));
            animOpacity.KeyFrames.Add(getSplineDoubleKeyFrame(0.2, 0));
            sb.Children.Add(animOpacity);

            Storyboard.SetTarget(animOpacity, objectToBeAnimated);
            Storyboard.SetTargetProperty(animOpacity, "(UIElement.Opacity)");

            return sb;
        }

        /// <summary>
        /// Create SplineDoubleKeyFrame, based on seconds for duration and value to animate to
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static SplineDoubleKeyFrame getSplineDoubleKeyFrame(double seconds, double value)
        {
            return new SplineDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(seconds)),
                Value = value
            };
        }

        /// <summary>
        /// Create EasingDoubleKeyFrame, based on seconds, duration and easing function
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="value"></param>
        /// <param name="easingfunction"></param>
        /// <returns></returns>
        private static EasingDoubleKeyFrame getEasingDoubleKeyFrame(double seconds, double value, EasingFunctionBase easingfunction)
        {
            return new EasingDoubleKeyFrame()
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(seconds)),
                Value = value,
                EasingFunction = easingfunction
            };
        }

    }
}
