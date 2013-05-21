using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using ControlLibrary.Primitives.Gestures;
using ControlLibrary.Primitives.Pagination;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    /// <summary>
    /// Represents a data-bound control that navigates through a sequence of items slide-by-slide.
    /// </summary>
    public class SlideView : DataControlBase, IPageProvider, IListSourceProvider
    {
        /// <summary>
        /// Identifies the SelectedItem dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(SlideView), new PropertyMetadata(null, OnSelectedItemChanged));

        /// <summary>
        /// Identifies the <see cref="IsLoopingEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsLoopingEnabledProperty =
            DependencyProperty.Register("IsLoopingEnabled", typeof(bool), typeof(SlideView), new PropertyMetadata(true, OnIsLoopingEnabledChanged));

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SlideView), new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

        /// <summary>
        /// Identifies the <see cref="ItemRealizationMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RealizationModeProperty =
            DependencyProperty.Register("RealizationMode", typeof(SlideViewItemRealizationMode), typeof(SlideView), new PropertyMetadata(SlideViewItemRealizationMode.Default, OnRealizationModeChanged));

        /// <summary>
        /// Identifies the <see cref="AdjacentItemsPreviewMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AdjacentItemsPreviewModeProperty =
            DependencyProperty.Register("AdjacentItemsPreviewMode", typeof(SlideViewAdjacentItemsPreviewMode), typeof(SlideView), new PropertyMetadata(SlideViewAdjacentItemsPreviewMode.None, OnAdjacentItemsPreviewModeChanged));

        /// <summary>
        /// Identifies the <see cref="ItemPreviewTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPreviewTemplateProperty =
            DependencyProperty.Register("ItemPreviewTemplate", typeof(DataTemplate), typeof(SlideView), new PropertyMetadata(null, OnItemPreviewTemplateChanged));

        /// <summary>
        /// Identifies the <see cref="ManipulationBehavior"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TransitionModeProperty = DependencyProperty.Register(
            "TransitionMode",
            typeof(SlideViewTransitionMode),
            typeof(SlideView),
            new PropertyMetadata(SlideViewTransitionMode.Slide, OnTransitionModeChanged));

        private static readonly TimeSpan DefaultSlideShowInterval = TimeSpan.FromSeconds(1);

        private DataTemplate itemPreviewTemplateCache;
        private SlideViewPanel itemsPanel;
        private Canvas transitionLayer;
        private IDataSourceItem selectedDataSourceItem;
        private GestureBehavior behavior;
        private Orientation orientationCache = Orientation.Horizontal;
        private SlideViewItemRealizationMode realizationModeCache = SlideViewItemRealizationMode.Default;
        private SlideViewAdjacentItemsPreviewMode adjacentItemsPreviewModeCache = SlideViewAdjacentItemsPreviewMode.None;
        private SlideViewManipulationBehavior manipulationBehavior;
        private Thickness clipMarginCache;
        private DispatcherTimer slideShowTimer;
        private EventHandler currentIndexChangedDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadSlideView"/> class.
        /// </summary>
        public SlideView()
        {
            this.DefaultStyleKey = typeof(SlideView);
            this.SizeChanged += this.OnSizeChanged;

            this.behavior = new GestureBehavior(this);
            this.behavior.NotifyGesture = this.OnGesture;
            this.behavior.NotifyGestureStart = this.OnGestureStarted;
            this.behavior.NotifyGestureComplete = this.OnGestureCompleted;

            this.slideShowTimer = new DispatcherTimer();
            this.slideShowTimer.Tick += this.OnSlideShowTimerTick;
        }

        /// <summary>
        /// Occurs when the <see cref="SelectedItem"/> property has changed.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        event EventHandler IPageProvider.CurrentIndexChanged
        {
            add
            {
                this.currentIndexChangedDelegate += value;
            }
            remove
            {
                this.currentIndexChangedDelegate -= value;
            }
        }

        /// <summary>
        /// Gets or sets the mode that defines the visibility of the two adjacent to the viewport items.
        /// </summary>
        public SlideViewAdjacentItemsPreviewMode AdjacentItemsPreviewMode
        {
            get
            {
                return this.adjacentItemsPreviewModeCache;
            }
            set
            {
                this.SetValue(AdjacentItemsPreviewModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> instance that defines the appearance of the offscreen items when the RealizationMode property is set to ViewportItem.
        /// </summary>
        public DataTemplate ItemPreviewTemplate
        {
            get
            {
                return this.itemPreviewTemplateCache;
            }
            set
            {
                this.SetValue(ItemPreviewTemplateProperty, value);
            }
        }

        /// <summary>
        /// Gets the current <see cref="SlideViewManipulationBehavior"/> that handles user input and navigates through the items.
        /// </summary>
        public SlideViewManipulationBehavior ManipulationBehavior
        {
            get
            {
                return this.manipulationBehavior;
            }
        }

        /// <summary>
        /// Gets or sets the currently selected item in the view.
        /// </summary>
        public object SelectedItem
        {
            get
            {
                if (this.selectedDataSourceItem != null)
                {
                    return this.selectedDataSourceItem.Value;
                }

                return null;
            }
            set
            {
                this.SetValue(SelectedItemProperty, value);
            }
        }

        /// <summary>
        /// Gets the next item that may be visualized. Returns null if the end of the sequence is reached and IsLoopingEnabled is False.
        /// </summary>
        public object NextItem
        {
            get
            {
                IDataSourceItem nextItem = this.GetNextDataSourceItem();
                if (nextItem == null)
                {
                    return null;
                }

                return nextItem.Value;
            }
        }

        /// <summary>
        /// Gets the next item that may be visualized. Returns null if the end of the sequence is reached and IsLoopingEnabled is False.
        /// </summary>
        public object PreviousItem
        {
            get
            {
                IDataSourceItem prevItem = this.GetPreviousDataSourceItem();
                if (prevItem == null)
                {
                    return null;
                }

                return prevItem.Value;
            }
        }

        /// <summary>
        /// Gets the <see cref="SlideViewItem"/> instance that represents the currently selected item. This is actually the viewport item.
        /// </summary>
        public SlideViewItem SelectedItemContainer
        {
            get
            {
                if (this.itemsPanel == null || this.itemsPanel.Children.Count == 0)
                {
                    return null;
                }

                return this.itemsPanel.PivotItem;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will loop infinitely among the items source.
        /// </summary>
        public bool IsLoopingEnabled
        {
            get
            {
                return (bool)this.GetValue(IsLoopingEnabledProperty);
            }
            set
            {
                this.SetValue(IsLoopingEnabledProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current orientation of the control.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this.orientationCache;
            }
            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current orientation of the control.
        /// </summary>
        public SlideViewItemRealizationMode ItemRealizationMode
        {
            get
            {
                return this.realizationModeCache;
            }
            set
            {
                this.SetValue(RealizationModeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the current strategy that handles the manipulation of the control.
        /// </summary>
        public SlideViewTransitionMode TransitionMode
        {
            get
            {
                return (SlideViewTransitionMode)this.GetValue(TransitionModeProperty);
            }
            set
            {
                this.SetValue(TransitionModeProperty, value);
            }
        }

        /// <summary>
        /// Determines whether a slide-show is currently running.
        /// </summary>
        public bool IsSlideShowRunning
        {
            get
            {
                return this.slideShowTimer.IsEnabled;
            }
        }

        IEnumerable IPageProvider.ThumbnailsSource
        {
            get
            {
                return this.ItemsSource;
            }
        }

        int IPageProvider.PageCount
        {
            get
            {
                return this.ListSource.Count;
            }
        }

        int IPageProvider.CurrentIndex
        {
            get
            {
                if (this.selectedDataSourceItem == null)
                {
                    return -1;
                }

                return this.selectedDataSourceItem.Index;
            }
            set
            {
                if (value < 0 || value > this.ListSource.Count - 1)
                {
                    throw new IndexOutOfRangeException("SelectedIndex");
                }

                IDataSourceItem item = this.ListSource.GetItemAt(value);
                this.ChangeSelectedItem(item, true);
            }
        }

        MatListSource IListSourceProvider.ListSource
        {
            get
            {
                return this.ListSource;
            }
        }

        internal SlideViewPanel Panel
        {
            get
            {
                return this.itemsPanel;
            }
        }

        internal IDataSourceItem SelectedDataSourceItem
        {
            get
            {
                return this.selectedDataSourceItem;
            }
        }

        internal Canvas TransitionLayer
        {
            get
            {
                return this.transitionLayer;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this control is properly templated.
        /// </summary>
        /// <value>
        /// 	<c>True</c> if this instance is properly templated; otherwise, <c>false</c>.
        /// </value>
        protected internal override bool IsProperlyTemplated
        {
            get
            {
                return this.itemsPanel != null && this.transitionLayer != null;
            }
        }

        void IPageProvider.MoveNext()
        {
            this.MoveToNextItem(this.IsLoaded);
        }

        void IPageProvider.MovePrevious()
        {
            this.MoveToPreviousItem(this.IsLoaded);
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate"/>. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.transitionLayer = this.GetTemplateChild("transitionLayer") as Canvas;

            this.itemsPanel = this.GetTemplateChild("itemsPanel") as SlideViewPanel;
            this.itemsPanel.Attach(this);

            this.UpdateSelectedItem();

            if (this.IsLoaded)
            {
                // Caliburn breaks the default sequence of events and Loaded comes BEFORE the OnApplyTemplate method
                this.ResetManipulationBehavior();
            }
        }

        /// <summary>
        /// Advances to the next item in the sequence.
        /// </summary>
        /// <param name="animate">True to perform a transition, false otherwise.</param>
        /// <returns>True if a next item exists, false otherwise.</returns>
        public bool MoveToNextItem(bool animate = true)
        {
            IDataSourceItem item = this.GetNextDataSourceItem();
            if (item == null)
            {
                return false;
            }

            if (animate && this.manipulationBehavior != null && this.IsLoaded)
            {
                this.manipulationBehavior.MoveToNextItem();
            }
            else
            {
                this.selectedDataSourceItem = item;
                this.ChangePropertySilently(SelectedItemProperty, this.selectedDataSourceItem.Value);
                if (this.itemsPanel != null)
                {
                    this.itemsPanel.InvalidateUI();
                }
            }

            return true;
        }

        /// <summary>
        /// Moves back to the previous item in the sequence.
        /// </summary>
        /// <param name="animate">True to perform a transition, false otherwise.</param>
        /// <returns>True if previous item exists, false otherwise.</returns>
        public bool MoveToPreviousItem(bool animate = true)
        {
            IDataSourceItem item = this.GetPreviousDataSourceItem();
            if (item == null)
            {
                return false;
            }

            if (this.manipulationBehavior != null && this.IsLoaded)
            {
                this.manipulationBehavior.MoveToPreviousItem();
            }
            else
            {
                this.selectedDataSourceItem = item;
                this.ChangePropertySilently(SelectedItemProperty, this.selectedDataSourceItem.Value);
                this.itemsPanel.InvalidateUI();
            }

            return true;
        }

        /// <summary>
        /// Starts a slide-show using the specified interval to move to the next slide.
        /// </summary>
        /// <returns></returns>
        public bool StartSlideShow()
        {
            return this.StartSlideShow(DefaultSlideShowInterval);
        }

        /// <summary>
        /// Starts a slide-show using the specified interval to move to the next slide.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool StartSlideShow(TimeSpan interval)
        {
            if (!this.IsProperlyTemplated || !this.IsLoaded)
            {
                return false;
            }

            if (this.slideShowTimer.IsEnabled)
            {
                this.slideShowTimer.Stop();
            }

            this.slideShowTimer.Interval = interval;
            this.slideShowTimer.Start();

            return true;
        }

        /// <summary>
        /// Stops currently running slide-show (if any).
        /// </summary>
        public void StopSlideShow()
        {
            this.slideShowTimer.Stop();
        }

        internal override DataTemplate GetItemTemplate(UIElement container, object dataItem)
        {
            if (this.realizationModeCache == SlideViewItemRealizationMode.Default)
            {
                return base.GetItemTemplate(container, dataItem);
            }

            if (this.selectedDataSourceItem != null && object.Equals(this.selectedDataSourceItem.Value, dataItem))
            {
                return base.GetItemTemplate(container, dataItem);
            }

            return this.itemPreviewTemplateCache;
        }

        internal IDataSourceItem GetNextDataSourceItem()
        {
            return this.GetNextDataSourceItem(this.selectedDataSourceItem);
        }

        internal IDataSourceItem GetNextDataSourceItem(IDataSourceItem pivotItem)
        {
            if (pivotItem == null)
            {
                return null;
            }

            if (!this.IsProperlyTemplated || this.ListSource.Count == 0)
            {
                return null;
            }

            IDataSourceItem nextItem = this.ListSource.GetItemAfter(pivotItem);
            if (nextItem == null && this.IsLoopingEnabled)
            {
                nextItem = this.ListSource.GetFirstItem();
            }

            return nextItem;
        }

        internal IDataSourceItem GetPreviousDataSourceItem()
        {
            if (!this.IsProperlyTemplated || this.ListSource.Count == 0)
            {
                return null;
            }

            Debug.Assert(this.selectedDataSourceItem != null, "Must have selected item at this point");
            IDataSourceItem previousItem = this.ListSource.GetItemBefore(this.selectedDataSourceItem);
            if (previousItem == null && this.IsLoopingEnabled)
            {
                previousItem = this.ListSource.GetLastItem();
            }

            return previousItem;
        }

        internal IDataSourceItem GetPreviousDataSourceItem(IDataSourceItem pivotItem)
        {
            if (pivotItem == null)
            {
                return null;
            }

            if (!this.IsProperlyTemplated || this.ListSource.Count == 0)
            {
                return null;
            }

            IDataSourceItem previousItem = this.ListSource.GetItemBefore(pivotItem);
            if (previousItem == null && this.IsLoopingEnabled)
            {
                previousItem = this.ListSource.GetLastItem();
            }

            return previousItem;
        }

        internal void ChangeSelectedItem(IDataSourceItem newItem, bool updateUI)
        {
            if (newItem == null)
            {
                throw new ArgumentNullException("newItem");
            }

            this.selectedDataSourceItem = newItem;
            this.ChangePropertySilently(SelectedItemProperty, newItem.Value);

            if (updateUI && this.itemsPanel != null)
            {
                this.itemsPanel.InvalidateUI();
            }
        }

        /// <summary>
        /// Creates a <see cref="SlideViewItem"/> instance used to visual the data items.
        /// </summary>
        /// <returns></returns>
        protected internal virtual SlideViewItem CreateContainer()
        {
            return new SlideViewItem();
        }

        /// <summary>
        /// Creates the core <see cref="SlideViewManipulationBehavior"/> depending on the current <see cref="TransitionMode"/> value.
        /// </summary>
        /// <returns></returns>
        protected virtual SlideViewManipulationBehavior CreateManipulationBehavior()
        {
            switch (this.TransitionMode)
            {
                case SlideViewTransitionMode.Flip:
                    return new FlipManipulationBehavior();
                default:
                    return new SlideManipulationBehavior();
            }
        }

        /// <summary>
        /// Invalidates the visual representation of the control.
        /// </summary>
        protected override void InvalidateUI()
        {
            base.InvalidateUI();

            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.ResetCache();
            }

            if (this.itemsPanel != null)
            {
                this.itemsPanel.InvalidateUI();
            }
        }

        /// <summary>
        /// Occurs when a System.Windows.FrameworkElement has been constructed and added to the object tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);

            this.behavior.Start();

            if (this.IsProperlyTemplated)
            {
                this.ResetManipulationBehavior();
            }
        }

        /// <summary>
        /// Occurs when this object is no longer connected to the main object tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            base.OnUnloaded(sender, e);

            this.behavior.Stop();
            this.slideShowTimer.Stop();
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
            this.EnsureSelectedItem();

            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// Occurs when the underlying source collection has changed (valid when the collection implements INotifyCollectionChanged).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.IsProperlyTemplated)
            {
                this.UpdateSelectedItem();
                if (this.selectedDataSourceItem == null)
                {
                    this.EnsureSelectedItem();
                }
            }
        }

        /// <summary>
        /// Occurs when the <see cref="P:ItemsSource"/> property has changed.
        /// </summary>
        /// <param name="oldSource"></param>
        protected override void OnItemsSourceChanged(IEnumerable oldSource)
        {
            base.OnItemsSourceChanged(oldSource);

            this.selectedDataSourceItem = null;
            this.ChangePropertySilently(SelectedItemProperty, null);
        }

        private static void OnItemPreviewTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.itemPreviewTemplateCache = e.NewValue as DataTemplate;
            slideView.InvalidateUI();
        }

        private static void OnClipMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.clipMarginCache = (Thickness)e.NewValue;

            if (!slideView.IsProperlyTemplated)
            {
                return;
            }

            slideView.UpdateClip(slideView.ActualWidth, slideView.ActualHeight);
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            if (!slideView.IsProperlyTemplated)
            {
                return;
            }

            if (!slideView.changePropertySilently)
            {
                if (e.NewValue == null)
                {
                    throw new ArgumentNullException("SelectedItem");
                }
                slideView.selectedDataSourceItem = slideView.ListSource.FindItem(e.NewValue);
                slideView.itemsPanel.InvalidateUI();
            }

            slideView.NotifySelectionChanged(e);
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.orientationCache = (Orientation)e.NewValue;

            if (slideView.itemsPanel != null)
            {
                slideView.itemsPanel.InvalidateMeasure();
                if (slideView.manipulationBehavior != null)
                {
                    slideView.manipulationBehavior.ResetCache();
                }
            }
        }

        private static void OnRealizationModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.realizationModeCache = (SlideViewItemRealizationMode)e.NewValue;
            if (slideView.itemsPanel != null)
            {
                slideView.itemsPanel.Reset();
            }
        }

        private static void OnAdjacentItemsPreviewModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.adjacentItemsPreviewModeCache = (SlideViewAdjacentItemsPreviewMode)e.NewValue;

            if (slideView.itemsPanel != null)
            {
                slideView.itemsPanel.Reset();
            }
        }

        private static void OnTransitionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView view = d as SlideView;
            view.ResetManipulationBehavior();
        }

        private static void OnIsLoopingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SlideView slideView = d as SlideView;
            slideView.InvalidateUI();
        }

        private void EnsureSelectedItem()
        {
            if (this.ListSource.Count > 0)
            {
                if (this.selectedDataSourceItem == null)
                {
                    this.selectedDataSourceItem = this.ListSource.GetFirstItem();
                    this.ChangePropertySilently(SelectedItemProperty, this.selectedDataSourceItem.Value);
                }
            }
            else
            {
                this.selectedDataSourceItem = null;
                this.ChangePropertySilently(SelectedItemProperty, null);
            }
        }

        private void OnGesture(GestureBehavior sender, Gesture gesture)
        {
            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.HandleGesture(gesture);
            }
        }

        private void OnGestureStarted()
        {
            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.HandleGestureStarted();
            }
        }

        private void OnGestureCompleted(Gesture gesture)
        {
            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.HandleGestureCompleted(gesture);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateClip(e.NewSize.Width, e.NewSize.Height);
            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.ResetCache();
            }
        }

        private void UpdateClip(double width, double height)
        {
            this.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, width, height) };
        }

        private void OnFlick(FlickGesture gesture)
        {
            if (this.ListSource.Count == 0)
            {
                return;
            }

            Debug.Assert(this.selectedDataSourceItem != null, "Must have selected item at this point");

            IDataSourceItem item = null;
            if (gesture.Velocity.X < 0)
            {
                item = this.ListSource.GetItemAfter(this.selectedDataSourceItem);
                if (item == null)
                {
                    item = this.ListSource.GetFirstItem();
                }
            }
            else if (gesture.Velocity.X > 0)
            {
                item = this.ListSource.GetItemBefore(this.selectedDataSourceItem);
                if (item == null)
                {
                    item = this.ListSource.GetLastItem();
                }
            }

            if (item != null)
            {
                this.selectedDataSourceItem = item;
                this.ChangePropertySilently(SelectedItemProperty, this.selectedDataSourceItem.Value);
            }
        }

        private void ResetManipulationBehavior()
        {
            if (!this.IsLoaded)
            {
                return;
            }

            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.Detach();
            }

            this.manipulationBehavior = this.CreateManipulationBehavior();
            if (this.manipulationBehavior != null)
            {
                this.manipulationBehavior.Attach(this);
                this.manipulationBehavior.OnLoaded();
            }
        }

        private void OnSlideShowTimerTick(object sender, object e)
        {
            if (!this.MoveToNextItem())
            {
                this.slideShowTimer.Stop();
            }
        }

        private void NotifySelectionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.currentIndexChangedDelegate != null)
            {
                this.currentIndexChangedDelegate(this, EventArgs.Empty);
            }

            SelectionChangedEventHandler selChanged = this.SelectionChanged;
            if (selChanged != null)
            {
                selChanged(this, new SelectionChangedEventArgs(new object[] { e.OldValue }, new object[] { e.NewValue }));
            }
        }

        private void UpdateSelectedItem()
        {
            this.selectedDataSourceItem = null;

            object selectedItem = this.GetValue(SelectedItemProperty);
            if (selectedItem != null)
            {
                this.selectedDataSourceItem = this.ListSource.FindItem(selectedItem);
            }
            if (this.selectedDataSourceItem == null && this.ListSource.Count > 0)
            {
                this.selectedDataSourceItem = this.ListSource.GetFirstItem();
                this.ChangeSelectedItem(this.selectedDataSourceItem, false);
            }
        }
    }
}
