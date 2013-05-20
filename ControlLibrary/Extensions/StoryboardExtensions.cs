using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary.Extensions
{
    /// <summary>
    /// Extension methods to add animations to a storyboard
    /// </summary>
    public static class StoryboardExtensions
    {
        public static Timeline CreateDoubleAnimation(this Storyboard storyboard,
            Duration duration, double from, double to)
        {
            return storyboard.CreateDoubleAnimation(duration, from, to,
                new SineEase
                {
                    EasingMode = EasingMode.EaseInOut
                });
        }

        public static Timeline CreateDoubleAnimation(this Storyboard storyboard,
            Duration duration, double from, double to, EasingFunctionBase easingFunction)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = duration,
                EasingFunction = easingFunction
            };
            return animation;
        }

        public static void AddAnimation(this Storyboard storyboard,
            DependencyObject item, Timeline t, string property)
        {
            if (string.IsNullOrWhiteSpace(property)) throw new ArgumentNullException("property");
            Storyboard.SetTarget(t, item);
            Storyboard.SetTargetProperty(t, property);
            storyboard.Children.Add(t);
        }

        public static void AddTranslationAnimation(this Storyboard storyboard,
            FrameworkElement fe, Point from, Point to, Duration duration)
        {
            storyboard.AddTranslationAnimation(fe, from, to, duration, null);
        }

        public static void AddTranslationAnimation(this Storyboard storyboard,
            FrameworkElement fe, Point from, Point to, Duration duration,
            EasingFunctionBase easingFunction)
        {
            storyboard.AddAnimation(fe.RenderTransform,
                         storyboard.CreateDoubleAnimation(duration, from.X, to.X, easingFunction),
                            "TranslateX");
            storyboard.AddAnimation(fe.RenderTransform,
                        storyboard.CreateDoubleAnimation(duration, from.Y, to.Y, easingFunction),
                           "TranslateY");
        }

        public static void AddTranslationAnimation(this Storyboard storyboard,
        FrameworkElement fe, double dx, double dy, Duration duration,
        EasingFunctionBase easingFunction)
        {
            var from = fe.GetTranslatePoint();
            var to = new Point(from.X + dx, from.Y + dy);

            storyboard.AddAnimation(fe.RenderTransform,
                         storyboard.CreateDoubleAnimation(duration, from.X, to.X, easingFunction),
                            "TranslateX");
            storyboard.AddAnimation(fe.RenderTransform,
                        storyboard.CreateDoubleAnimation(duration, from.Y, to.Y, easingFunction),
                           "TranslateY");
        }
    }
}
