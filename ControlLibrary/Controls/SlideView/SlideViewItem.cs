using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace ControlLibrary
{
    public sealed class SlideViewItem : MatContentControl
    {
        /// <summary>
        /// Identifies the <see cref="LoadAnimation"/> property.
        /// </summary>
        //public static readonly DependencyProperty LoadAnimationProperty =
        //    DependencyProperty.Register("LoadAnimation", typeof(RadAnimation), typeof(SlideViewItem), new PropertyMetadata(null));
        public static readonly DependencyProperty LoadAnimationProperty =
           DependencyProperty.Register("LoadAnimation", typeof(Timeline), typeof(SlideViewItem), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="BusyIndicatorStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty BusyIndicatorStyleProperty =
            DependencyProperty.Register("BusyIndicatorStyle", typeof(Style), typeof(SlideViewItem), new PropertyMetadata(null));

        private IDataSourceItem dataItem;
        private ContentPresenter contentPresenter;
        //private RadBusyIndicator busyIndicator;
        private ProgressRing busyIndicator;
        private SlideViewPanel owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlideViewItem"/> class.
        /// </summary>
        public SlideViewItem()
        {
            this.DefaultStyleKey = typeof(SlideViewItem);
        }

        /// <summary>
        /// Gets or sets the <see cref="RadAnimation"/> instance applied on the item when the RealizationMode property of the owning <see cref="RadSlideView"/> is set to ViewportItem and the item's content is loaded.
        /// </summary>
        //public RadAnimation LoadAnimation
        //{
        //    get
        //    {
        //        return this.GetValue(LoadAnimationProperty) as RadAnimation;
        //    }
        //    set
        //    {
        //        this.SetValue(LoadAnimationProperty, value);
        //    }
        //}
        public Timeline LoadAnimation
        {
            get
            {
                return this.GetValue(LoadAnimationProperty) as Timeline;
            }
            set
            {
                this.SetValue(LoadAnimationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Style"/> instance that describes the appearance of the built-in <see cref="RadBusyIndicator"/>.
        /// </summary>
        public Style BusyIndicatorStyle
        {
            get
            {
                return this.GetValue(BusyIndicatorStyleProperty) as Style;
            }
            set
            {
                this.SetValue(BusyIndicatorStyleProperty, value);
            }
        }

        /// <summary>
        /// Gets the current <see cref="IDataSourceItem"/> instance currently represented by this container.
        /// </summary>
        internal IDataSourceItem DataItem
        {
            get
            {
                return this.dataItem;
            }
        }

        internal ProgressRing BusyIndicator
        {
            get
            {
                return this.busyIndicator;
            }
        }

        /// <summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.contentPresenter = this.GetTemplateChild("contentPresenter") as ContentPresenter;
            this.busyIndicator = this.GetTemplateChild("busyIndicator") as ProgressRing;
        }

        internal void BeginLoad()
        {
            if (this.busyIndicator == null)
            {
                return;
            }

            this.busyIndicator.Opacity = 1;
            this.busyIndicator.IsActive = true;
        }

        internal void EndLoad()
        {
            if (this.busyIndicator == null)
            {
                return;
            }

            this.busyIndicator.IsActive = false;
            this.busyIndicator.Opacity = 0;
        }

        internal void SetOwner(SlideViewPanel owner)
        {
            this.owner = owner;
        }

        internal void Attach(IDataSourceItem item)
        {
            this.dataItem = item;

            if (item != null)
            {
                FrameworkElement contentAsUI = item.Value as FrameworkElement;
                if (contentAsUI != null)
                {
                    ContentControl parent = contentAsUI.Parent as ContentControl;
                    if (parent != null)
                    {
                        parent.Content = null;
                    }
                }

                this.Content = item.Value;
                this.DataContext = item.Value;
            }
            else
            {
                this.Content = null;
                this.DataContext = null;
            }
        }

        internal void Detach()
        {
            this.dataItem = null;
            this.EndLoad();
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        /// <param name="e">Do not use.</param>
       protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);

            if (this.owner == null || this.dataItem == null)
            {
                return;
            }

            if (object.Equals(this.dataItem.Value, this.owner.View.SelectedItem))
            {
                return;
            }

            int index = this.owner.Children.IndexOf(this);
            int pivotIndex = this.owner.Children.Count / 2;

            if (index > pivotIndex)
            {
                this.owner.View.MoveToNextItem();
            }
            else
            {
                this.owner.View.MoveToPreviousItem();
            }
        }
    }
}
