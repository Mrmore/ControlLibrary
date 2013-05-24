using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary
{
    public class LoopItemsPanel : Panel
    {
        // hack to animate a value easy
        private Slider sliderVertical;
        private readonly TimeSpan animationDuration = TimeSpan.FromMilliseconds(600);
        // separating offset
        private double offsetSeparator;
        // item height. must be 1d to fire first arrangeoverride
        private double itemHeight = 1d;
        // true when arrange override is ok
        private bool templateApplied;

        public LoopItemsPanel()
        {
            this.ManipulationMode = (ManipulationModes.TranslateY | ManipulationModes.TranslateInertia);
            this.ManipulationDelta += OnManipulationDelta;
            this.Tapped += OnTapped;
            sliderVertical = new Slider
            {
                SmallChange = 0.0000000001,
                Minimum = double.MinValue,
                Maximum = double.MaxValue,
                StepFrequency = 0.0000000001
            };
            sliderVertical.ValueChanged += OnVerticalOffsetChanged;
        }

        private void OnVerticalOffsetChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.UpdatePositions(e.NewValue - e.OldValue);
        }

        private void OnTapped(object sender, TappedRoutedEventArgs args)
        {
            if (this.Children == null || this.Children.Count == 0)
                return;

            var positionY = args.GetPosition(this).Y;

            foreach (var child in this.Children)
            {
                var rect = child.TransformToVisual(this).TransformBounds(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));
                if (!(positionY >= rect.Y) || !(positionY <= (rect.Y + rect.Height))) continue;
                // scroll to Selected
                this.ScrollToSelectedIndex(child, rect);
                break;
            }
        }

        /// <summary>
        /// Get Items Source count items
        /// </summary>
        /// <returns></returns>
        private int GetItemsCount()
        {
            return this.Children != null ? this.Children.Count : 0;
        }

        /// <summary>
        /// On manipulation delta
        /// </summary>
        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e == null)
                return;
            var translation = e.Delta.Translation;
            this.UpdatePositions(translation.Y / 2);
        }

        private void ScrollToSelectedIndex(UIElement selectedItem, Rect rect)
        {
            if (!templateApplied)
                return;

            // Apply Transform
            TranslateTransform compositeTransform = (TranslateTransform)selectedItem.RenderTransform;
            if (compositeTransform == null)
                return;
            var centerTopOffset = (this.ActualHeight / 2d) - (itemHeight) / 2d;
            var deltaOffset = centerTopOffset - rect.Y;
            this.UpdatePositionsWithAnimation(compositeTransform.Y, compositeTransform.Y + deltaOffset);
        }

        /// <summary>
        /// Updating with an animation (after a tap)
        /// </summary>
        private void UpdatePositionsWithAnimation(Double fromOffset, Double toOffset)
        {
            var storyboard = new Storyboard();
            var animationSnap = new DoubleAnimation
            {
                EnableDependentAnimation = true,
                From = fromOffset,
                To = toOffset,
                Duration = animationDuration,
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut }
            };
            storyboard.Children.Add(animationSnap);

            Storyboard.SetTarget(animationSnap, sliderVertical);
            Storyboard.SetTargetProperty(animationSnap, "Value");

            sliderVertical.ValueChanged -= OnVerticalOffsetChanged;
            sliderVertical.Value = fromOffset;
            sliderVertical.ValueChanged += OnVerticalOffsetChanged;

            storyboard.Begin();
        }

        /// <summary>
        /// Updating position
        /// </summary>
        private void UpdatePositions(Double offsetDelta)
        {
            Double maxLogicalHeight = this.GetItemsCount() * itemHeight;
            // Reaffect correct offsetSeparator
            this.offsetSeparator = (this.offsetSeparator + offsetDelta) % maxLogicalHeight;
            // Get the correct number item
            Int32 itemNumberSeparator = (Int32)(Math.Abs(this.offsetSeparator) / itemHeight);
            Int32 itemIndexChanging;
            Double offsetAfter;
            Double offsetBefore;

            if (this.offsetSeparator > 0)
            {
                itemIndexChanging = this.GetItemsCount() - itemNumberSeparator - 1;
                offsetAfter = this.offsetSeparator;

                if (this.offsetSeparator % maxLogicalHeight == 0)
                    itemIndexChanging++;

                offsetBefore = offsetAfter - maxLogicalHeight;
            }
            else
            {
                itemIndexChanging = itemNumberSeparator;
                offsetBefore = this.offsetSeparator;
                offsetAfter = maxLogicalHeight + offsetBefore;
            }
            // items that must be before
            this.UpdatePosition(itemIndexChanging, this.GetItemsCount(), offsetBefore);
            // items that must be after
            this.UpdatePosition(0, itemIndexChanging, offsetAfter);
        }

        /// <summary>
        /// Translate items to a new offset
        /// </summary>
        private void UpdatePosition(Int32 startIndex, Int32 endIndex, Double offset)
        {
            for (Int32 i = startIndex; i < endIndex; i++)
            {
                UIElement loopListItem = this.Children[i];
                // Apply Transform
                TranslateTransform compositeTransform = (TranslateTransform)loopListItem.RenderTransform;

                if (compositeTransform == null)
                    continue;
                compositeTransform.Y = offset;
            }
        }

        /// <summary>
        /// Arrange all items
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Clip to ensure items dont override container
            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, finalSize.Width, finalSize.Height) };
            Double positionTop = 0d;
            // Must Create looping items count
            foreach (UIElement item in this.Children)
            {
                if (item == null)
                    continue;
                Size desiredSize = item.DesiredSize;
                if (double.IsNaN(desiredSize.Width) || double.IsNaN(desiredSize.Height)) continue;
                // Get rect position
                var rect = new Rect(0, positionTop, desiredSize.Width, desiredSize.Height);
                item.Arrange(rect);
                // set internal CompositeTransform to handle movement
                TranslateTransform compositeTransform = new TranslateTransform();
                item.RenderTransform = compositeTransform;
                positionTop += desiredSize.Height;
            }
            templateApplied = true;
            return finalSize;
        }

        /// <summary>
        /// Measure items 
        /// </summary>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size s = base.MeasureOverride(availableSize);
            // set good cliping
            this.Clip = new RectangleGeometry { Rect = new Rect(0, 0, s.Width, s.Height) };
            // Measure all items
            foreach (UIElement container in this.Children)
            {
                container.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                if (itemHeight != container.DesiredSize.Height)
                    itemHeight = container.DesiredSize.Height;
            }
            return (s);
        }
    }
}
