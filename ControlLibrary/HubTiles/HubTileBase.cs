using System;
using System.ComponentModel;
using System.Windows;
using ControlLibrary.Exceptions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary
{
    /// <summary>
    /// Base class for all Telerik WP7 hub tiles.
    /// </summary>
    public abstract class HubTileBase : MatControl
    {
        /// <summary>
        /// Identifies the Title dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(object), typeof(HubTileBase), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(HubTileBase), new PropertyMetadata(HorizontalAlignment.Center));

        public static readonly DependencyProperty TitleVerticalAlignmentProperty =
            DependencyProperty.Register("TitleVerticalAlignment", typeof(VerticalAlignment), typeof(HubTileBase), new PropertyMetadata(VerticalAlignment.Bottom));

        public static readonly DependencyProperty TitleMarginProperty =
           DependencyProperty.Register("TitleMargin", typeof(Thickness), typeof(HubTileBase), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Identifies the UpdateInterval dependency property.
        /// </summary>
        public static readonly DependencyProperty UpdateIntervalProperty =
            DependencyProperty.Register("UpdateInterval", typeof(TimeSpan), typeof(HubTileBase), new PropertyMetadata(TimeSpan.FromSeconds(3), OnUpdateIntervalChanged));

        /// <summary>
        /// Identifies the IsFrozen dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFrozenProperty =
           DependencyProperty.Register("IsFrozen", typeof(bool), typeof(HubTileBase), new PropertyMetadata(false, OnIsFrozenChanged));

        /// <summary>
        /// Identifies the BackContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackContentProperty =
            DependencyProperty.Register("BackContent", typeof(object), typeof(HubTileBase), new PropertyMetadata(null, OnBackContentChanged));

        /// <summary>
        /// Identifies the BackContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty BackContentTemplateProperty =
            DependencyProperty.Register("BackContentTemplate", typeof(DataTemplate), typeof(HubTileBase), new PropertyMetadata(null));

        private DispatcherTimer updateTimer = new DispatcherTimer();
        private VisualStateEnumerator stateEnumerator;
        private UIElement layoutRoot;

        /// <summary>
        /// Initializes a new instance of the HubTileBase class.
        /// </summary>
        public HubTileBase()
        {
            this.DefaultStyleKey = typeof(HubTileBase);
            this.updateTimer.Tick += this.OnUpdateTimerTick;
            this.updateTimer.Interval = this.UpdateInterval;
            MatHubTileService.Tiles.Add(this);

            this.stateEnumerator = new VisualStateEnumerator(this.GetVisualStateNames());
            this.SizeChanged += this.OnSizeChanged;
        }

        /// <summary>
        /// Asdf.
        /// </summary>
        ~HubTileBase()
        {
            int index = MatHubTileService.Tiles.IndexOf(this);
            if (index != -1)
            {
                MatHubTileService.Tiles.RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        public object Title
        {
            get
            {
                return this.GetValue(HubTileBase.TitleProperty);
            }

            set
            {
                this.SetValue(HubTileBase.TitleProperty, value);
            }
        }

        public HorizontalAlignment TitleHorizontalAlignment
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(HubTileBase.TitleHorizontalAlignmentProperty);
            }

            set
            {
                this.SetValue(HubTileBase.TitleHorizontalAlignmentProperty, value);
            }
        }

        public VerticalAlignment TitleVerticalAlignment
        {
            get
            {
                return (VerticalAlignment)this.GetValue(HubTileBase.TitleVerticalAlignmentProperty);
            }

            set
            {
                this.SetValue(HubTileBase.TitleVerticalAlignmentProperty, value);
            }
        }

        public Thickness TitleMargin
        {
            get
            {
                return (Thickness)this.GetValue(HubTileBase.TitleMarginProperty);
            }

            set
            {
                this.SetValue(HubTileBase.TitleMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the IsFrozen property. Freezing a hub tile means that it will cease to
        /// periodically update itself. For example when it is offscreen..
        /// </summary>
        public bool IsFrozen
        {
            get
            {
                return (bool)this.GetValue(HubTileBase.IsFrozenProperty);
            }

            set
            {
                this.SetValue(HubTileBase.IsFrozenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the UpdateInterval. This interval determines how often the tile will
        /// update its visual states when it is not frozen.
        /// </summary>
        public TimeSpan UpdateInterval
        {
            get
            {
                return (TimeSpan)this.GetValue(HubTileBase.UpdateIntervalProperty);
            }

            set
            {
                this.SetValue(HubTileBase.UpdateIntervalProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the back content of the tile. When the back content is set,
        /// the tile flips with a swivel animation to its back side and periodically
        /// flips between its back and front states.
        /// </summary>
        public object BackContent
        {
            get
            {
                return this.GetValue(HubTileBase.BackContentProperty);
            }

            set
            {
                this.SetValue(HubTileBase.BackContentProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that is used to visualize the BackContent
        /// property.
        /// </summary>
        public DataTemplate BackContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(HubTileBase.BackContentTemplateProperty);
            }

            set
            {
                this.SetValue(HubTileBase.BackContentTemplateProperty, value);
            }
        }

        internal VisualStateEnumerator StatesEnumerator
        {
            get
            {
                return this.stateEnumerator;
            }
        }

        /// <summary>
        /// Gets a value that determines whether a rectangle clip is set on the LayoutRoot.
        /// </summary>
        protected virtual bool ShouldClip
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the LayoutRoot of the control template.
        /// </summary>
        protected UIElement LayoutRoot
        {
            get
            {
                return this.layoutRoot;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.GetTemplateChild("PART_BackContent") as ContentPresenter == null)
            {
                throw new MissingTemplatePartException(typeof(ContentPresenter), "PART_BackContent");
            }

            this.layoutRoot = this.GetTemplateChild("PART_LayoutRoot") as UIElement;
            if (this.layoutRoot == null)
            {
                throw new MissingTemplatePartException(typeof(UIElement), "PART_LayoutRoot");
            }

            if (this.IsLoaded)
            {
                this.OnFullyInitialized();
            }
        }

        /// <summary>
        /// Gets the visual states that the StatesEnumerator will enumerate.
        /// </summary>
        /// <returns>Returns the visual states that the StatesEnumerator will enumerate.</returns>
        protected virtual string[] GetVisualStateNames()
        {
            return new string[] { "NotFlipped", "Flipped" };
        }

        /// <summary>
        /// A virtual callback that is invoked when the IsFrozen property changes.
        /// </summary>
        /// <param name="newValue">The new IsFrozen value.</param>
        /// <param name="oldValue">The old IsFrozen value.</param>
        protected virtual void OnIsFrozenChanged(bool newValue, bool oldValue)
        {
            if (!this.IsLoaded)
            {
                return;
            }

            if (newValue)
            {
                this.StopTimer();
            }
            else
            {
                this.StartTimer();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            base.OnLoaded(sender, e);

            if (this.IsTemplateApplied)
            {
                this.StatesEnumerator.Reset();
                this.OnFullyInitialized();
            }
        }

        /// <summary>
        /// A virtual callback that is called when the tile is loaded and its template is applied.
        /// </summary>
        protected virtual void OnFullyInitialized()
        {
            if (!this.IsFrozen)
            {
                this.StartTimer();
            }

            this.StatesEnumerator.MoveNext();
            VisualStateManager.GoToState(this, this.StatesEnumerator.Current, false);
        }

        /// <summary>
        /// A virtual callback that is called periodically when the tile is no frozen. It can be used to
        /// update the tile visual states or other necessary operations.
        /// </summary>
        protected virtual void Update()
        {
            this.StatesEnumerator.MoveNext();
            VisualStateManager.GoToState(this, this.StatesEnumerator.Current, true);
        }

        /// <summary>
        /// A virtual callback that is invoked when the UpdateInterval property changes.
        /// </summary>
        /// <param name="newInterval">The new UpdateInterval value.</param>
        /// <param name="oldInterval">The old UpdateInterval value.</param>
        protected virtual void OnUpdateIntervalChanged(TimeSpan newInterval, TimeSpan oldInterval)
        {
            this.updateTimer.Interval = newInterval;
        }

        /// <summary>
        /// A virtual method that starts the timer.
        /// </summary>
        protected virtual void StartTimer()
        {
            if (this.IsFrozen)
            {
                return;
            }

            this.updateTimer.Start();
        }

        /// <summary>
        /// A virtual method that stops the timer.
        /// </summary>
        protected void StopTimer()
        {
            this.updateTimer.Stop();
        }

        /// <summary>
        /// A virtual method that resets the timer.
        /// </summary>
        protected void ResetTimer()
        {
            this.StopTimer();
            this.StartTimer();
        }

        /// <summary>
        /// A virtual callback that is called when the BackContent property changes.
        /// </summary>
        /// <param name="newBackContent">The new BackContent value.</param>
        /// <param name="oldBackContent">The old BackContent value.</param>
        protected virtual void OnBackContentChanged(object newBackContent, object oldBackContent)
        {
            if (newBackContent == null)
            {
                this.OnBackStateDeactivated();
            }
            else
            {
                this.OnBackStateActivated();
            }
        }

        /// <summary>
        /// This callback is invoked when BackContent is set to a non-null value.
        /// </summary>
        protected virtual void OnBackStateActivated()
        {
            this.ResetTimer();
            this.StatesEnumerator.MoveNext();
            VisualStateManager.GoToState(this, this.StatesEnumerator.Current, true);
        }

        /// <summary>
        /// This callback is invoked when BackContent is set to a null value.
        /// </summary>
        protected virtual void OnBackStateDeactivated()
        {
            this.ResetTimer();
            this.StatesEnumerator.MoveTo("NotFlipped");
            VisualStateManager.GoToState(this, this.StatesEnumerator.Current, false);
        }

        private static void OnIsFrozenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            HubTileBase hubTile = (HubTileBase)sender;
            hubTile.OnIsFrozenChanged((bool)args.NewValue, (bool)args.OldValue);
        }

        private static void OnUpdateIntervalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            HubTileBase hubTile = sender as HubTileBase;
            hubTile.OnUpdateIntervalChanged((TimeSpan)args.NewValue, (TimeSpan)args.OldValue);
        }

        private static void OnBackContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            HubTileBase tile = sender as HubTileBase;
            tile.OnBackContentChanged(args.NewValue, args.OldValue);
        }

        private void OnUpdateTimerTick(object sender, object e)
        {
            this.Update();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.ShouldClip || !this.IsTemplateApplied)
            {
                return;
            }

            this.layoutRoot.Clip = new RectangleGeometry() { Rect = new Rect(new Point(), e.NewSize) };
        }
    }
}
