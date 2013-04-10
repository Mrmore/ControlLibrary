using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace ControlLibrary
{
    /// <summary>
    /// Represents the panel that hosts the slides within a <see cref="SlideView"/> instance.
    /// </summary>
    public class SlideViewPanel : Panel
    {
        private const int ContainerCount = 3;
        private const double PreviewRatio = 0.1;

        private SlideView view;

        private bool invalidateScheduled;
        private Size itemSize;
        private bool loadingData;
        private bool waitingForLayout;
        private bool animateViewportItem;

        internal SlideView View
        {
            get
            {
                return this.view;
            }
        }

        internal double ItemLength
        {
            get
            {
                if (this.view == null)
                {
                    return 0;
                }

                if (this.view.Orientation == Orientation.Horizontal)
                {
                    return this.itemSize.Width;
                }
                return this.itemSize.Height;
            }
        }

        /// <summary>
        /// A special flag, valid when the RealizationMode equals ViewportItem.
        /// Indicates that we are currently realizing the pivot item and waiting for the LayoutUpdated event.
        /// </summary>
        internal bool WaitingForLayout
        {
            get
            {
                return this.waitingForLayout;
            }
        }

        internal bool IsUIInvalidated
        {
            get
            {
                return this.invalidateScheduled;
            }
        }

        internal SlideViewItem PivotItem
        {
            get
            {
                if (this.Children.Count == 0)
                {
                    return null;
                }

                return this.Children[this.Children.Count / 2] as SlideViewItem;
            }
        }

        internal SlideViewItem PreviousItem
        {
            get
            {
                int itemCount = this.Children.Count;
                if (itemCount == 0)
                {
                    return null;
                }

                return this.Children[(itemCount / 2) - 1] as SlideViewItem;
            }
        }

        internal SlideViewItem NextItem
        {
            get
            {
                int itemCount = this.Children.Count;
                if (itemCount == 0)
                {
                    return null;
                }

                return this.Children[(itemCount / 2) + 1] as SlideViewItem;
            }
        }

        internal void Attach(SlideView view)
        {
            this.view = view;
        }

        internal void OnSelectionAnimationStarted(int sign)
        {
            if (this.Children.Count == 0 || this.view.ItemRealizationMode == SlideViewItemRealizationMode.Default)
            {
                return;
            }

            // the sign indicates the direction of the animation
            if (sign == 1)
            {
                // load previous item
                this.PreviousItem.BeginLoad();
            }
            else
            {
                // load next item
                this.NextItem.BeginLoad();
            }

            this.loadingData = true;
        }

        internal bool OnSelectionAnimationCompleted()
        {
            this.animateViewportItem = this.loadingData;

            this.loadingData = false;
            this.RefreshUI();

            return this.animateViewportItem;
        }

        /// <summary>
        /// Forces a synchronous realization of the containers.
        /// </summary>
        internal void RefreshUI()
        {
            this.UpdateContainers();
            this.RealizeContainers();
        }

        /// <summary>
        /// Schedules asynchronous update of the UI.
        /// </summary>
        internal async void InvalidateUI()
        {
            if (this.invalidateScheduled || this.view == null)
            {
                return;
            }

            //this.Dispatcher.InvokeAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.OnUIInvalidated, this, null);
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, this.OnUIInvalidated);
            this.invalidateScheduled = true;
        }

        internal void Reset()
        {
            this.Children.Clear();
            this.InvalidateMeasure();

            if (this.view.ManipulationBehavior != null)
            {
                this.view.ManipulationBehavior.ResetCache();
            }
        }

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity (<see cref="F:System.Double.PositiveInfinity"/>) can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.RefreshUI();

            Size baseMeasure = base.MeasureOverride(availableSize);
            Thickness margin = this.view.Orientation == Orientation.Horizontal ?
                this.CalculateItemOffsetHorizontally(availableSize) :
                this.CalculateItemOffsetVertically(availableSize);

            availableSize.Width -= margin.Left + margin.Right;
            availableSize.Height -= margin.Top + margin.Bottom;

            foreach (FrameworkElement item in this.Children)
            {
                item.Measure(availableSize);
            }

            return baseMeasure;
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size that is used after the element is arranged in layout.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size baseSize = base.ArrangeOverride(finalSize);

            int offscreenItemCount = this.Children.Count / 2;
            Thickness margin;
            double left, top;
            if (this.view.Orientation == Orientation.Horizontal)
            {
                margin = this.CalculateItemOffsetHorizontally(finalSize);
                finalSize.Width -= margin.Left + margin.Right;
                left = -(finalSize.Width * offscreenItemCount) + margin.Left;
                top = 0;
            }
            else
            {
                margin = this.CalculateItemOffsetVertically(finalSize);
                finalSize.Height -= margin.Top + margin.Bottom;
                top = -(finalSize.Height * offscreenItemCount) + margin.Top;
                left = 0;
            }

            foreach (SlideViewItem item in this.Children)
            {
                item.Arrange(new Rect(left, top, finalSize.Width, finalSize.Height));

                if (this.view.Orientation == Orientation.Horizontal)
                {
                    left += finalSize.Width;
                }
                else
                {
                    top += finalSize.Height;
                }
            }

            this.itemSize = finalSize;

            return baseSize;
        }

        private void OnUIInvalidated()
        {
            if (!this.invalidateScheduled)
            {
                System.Diagnostics.Debug.Assert(false, "Must be invalidated at this point.");
                return;
            }

            this.invalidateScheduled = false;
            this.RefreshUI();
        }

        private void RealizeContainers()
        {
            if (this.view.SelectedDataSourceItem == null)
            {
                return;
            }

            if (this.view.ItemRealizationMode == SlideViewItemRealizationMode.ViewportItem || this.view.ListSource.Count == 1)
            {
                this.RealizeViewport();
            }
            else
            {
                this.RealizeDefault();
            }
        }

        private void RealizeViewport()
        {
            if (this.loadingData)
            {
                return;
            }

            DataTemplate previewTemplate = this.view.ItemPreviewTemplate;

            this.RealizeItem(this.PreviousItem, previewTemplate == null ? null : this.view.GetPreviousDataSourceItem());
            this.RealizeItem(this.PivotItem, this.view.SelectedDataSourceItem);
            this.RealizeItem(this.NextItem, previewTemplate == null ? null : this.view.GetNextDataSourceItem());

            this.LayoutUpdated += this.OnLayoutUpdated;
            this.waitingForLayout = true;
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            this.LayoutUpdated -= this.OnLayoutUpdated;
            this.waitingForLayout = false;

            if (this.view.ManipulationBehavior != null)
            {
                this.view.ManipulationBehavior.ResetTransform();
            }

            this.PreviousItem.EndLoad();
            this.NextItem.EndLoad();

            // animate the viewport item, using its LoadAnimation property
            if (this.animateViewportItem)
            {
                CompositionTarget.Rendering += this.OnCompositionTargetRendering;
                this.animateViewportItem = false;
            }
        }

        private void OnCompositionTargetRendering(object sender, object e)
        {
            CompositionTarget.Rendering -= this.OnCompositionTargetRendering;
            //RadAnimation animation = this.PivotItem.LoadAnimation;
            //if (animation != null)
            //{
            //    RadAnimationManager.Play(this.PivotItem, animation);
            //}

            Timeline animation = this.PivotItem.LoadAnimation;
            if (animation != null)
            {
                Storyboard sb = new Storyboard();
                Storyboard.SetTarget(animation, this.PivotItem);
                sb.Children.Add(animation);
                sb.Begin();
            }
        }

        private void RealizeDefault()
        {
            IDataSourceItem pivotItem = this.view.SelectedDataSourceItem;
            this.RealizeItem(this.PivotItem, this.view.SelectedDataSourceItem);

            int prevIndex = (this.Children.Count / 2) - 1;
            IDataSourceItem prevItem = this.view.GetPreviousDataSourceItem(pivotItem);
            while (prevIndex >= 0)
            {
                this.RealizeItem(this.Children[prevIndex] as SlideViewItem, prevItem);
                prevIndex--;
                prevItem = this.view.GetPreviousDataSourceItem(prevItem);
            }

            int nextIndex = (this.Children.Count / 2) + 1;
            IDataSourceItem nextItem = this.view.GetNextDataSourceItem(pivotItem);
            while (nextIndex < this.Children.Count)
            {
                this.RealizeItem(this.Children[nextIndex] as SlideViewItem, nextItem);
                nextIndex++;
                nextItem = this.view.GetNextDataSourceItem(nextItem);
            }
        }

        private void RealizeItem(SlideViewItem container, IDataSourceItem dataItem)
        {
            object value = dataItem == null ? null : dataItem.Value;

            container.Attach(dataItem);
            container.ContentTemplate = this.view.GetItemTemplate(container, value);
            container.Style = this.view.GetItemContainerStyle(container, value);
        }

        private void UpdateContainers()
        {
            if (this.view.SelectedDataSourceItem == null)
            {
                this.Children.Clear();
            }
            else if (this.Children.Count == 0)
            {
                int itemCount = ContainerCount;
                if (this.view.AdjacentItemsPreviewMode != SlideViewAdjacentItemsPreviewMode.None)
                {
                    // add two more additional containers due to the generated offset on the left and on the right
                    itemCount += 2;
                }

                for (int i = 0; i < itemCount; i++)
                {
                    SlideViewItem container = this.view.CreateContainer();
                    container.SetOwner(this);
                    this.Children.Add(container);
                }
            }
        }

        private Thickness CalculateItemOffsetHorizontally(Size availableSize)
        {
            Thickness offset = new Thickness();
            if ((this.view.AdjacentItemsPreviewMode & SlideViewAdjacentItemsPreviewMode.Previous) == SlideViewAdjacentItemsPreviewMode.Previous)
            {
                offset.Left = PreviewRatio * availableSize.Width;
            }
            if ((this.view.AdjacentItemsPreviewMode & SlideViewAdjacentItemsPreviewMode.Next) == SlideViewAdjacentItemsPreviewMode.Next)
            {
                offset.Right = PreviewRatio * availableSize.Width;
            }

            return offset;
        }

        private Thickness CalculateItemOffsetVertically(Size availableSize)
        {
            Thickness offset = new Thickness();
            if ((this.view.AdjacentItemsPreviewMode & SlideViewAdjacentItemsPreviewMode.Previous) == SlideViewAdjacentItemsPreviewMode.Previous)
            {
                offset.Top = PreviewRatio * availableSize.Height;
            }
            if ((this.view.AdjacentItemsPreviewMode & SlideViewAdjacentItemsPreviewMode.Next) == SlideViewAdjacentItemsPreviewMode.Next)
            {
                offset.Bottom = PreviewRatio * availableSize.Height;
            }

            return offset;
        }
    }
}
