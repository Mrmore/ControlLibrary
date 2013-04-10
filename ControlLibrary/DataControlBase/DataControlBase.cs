using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControlLibrary
{
    /// <summary>
    /// Base class for data-bound controls like <see cref="SlideView"/>.
    /// </summary>
    public abstract class DataControlBase : TemplateProviderControl, IListSourceProvider
    {
        /// <summary>
        /// Identifies the ItemsSource dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataControlBase), new PropertyMetadata(null, OnItemsSourceChanged));

        private MatListSource listSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataControlBase"/> class.
        /// </summary>
        protected DataControlBase()
        {
            this.listSource = this.CreateListSource();
            this.listSource.CollectionChanged += this.OnListSourceCollectionChanged;
        }

        /// <summary>
        /// Gets or sets a collection used to generate the content of the <see cref="RadVirtualizingDataControl"/>. 
        /// </summary>
        /// <value>The object that is used to generate the content of the <see cref="RadVirtualizingDataControl"/>. The default is null.</value>
        public IEnumerable ItemsSource
        {
            get
            {
                return this.GetValue(ItemsSourceProperty) as IEnumerable;
            }
            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        MatListSource IListSourceProvider.ListSource
        {
            get
            {
                return this.listSource;
            }
        }

        internal MatListSource ListSource
        {
            get
            {
                return this.listSource;
            }
        }

        internal virtual MatListSource CreateListSource()
        {
            return new MatListSource();
        }

        /// <summary>
        /// Occurs when the underlying source collection has changed (valid when the collection implements INotifyCollectionChanged).
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when the <see cref="ItemsSource"/> property has changed.
        /// </summary>
        /// <param name="oldSource"></param>
        protected virtual void OnItemsSourceChanged(IEnumerable oldSource)
        {
        }

        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DataControlBase control = sender as DataControlBase;
            control.OnItemsSourceChanged(e.OldValue as IEnumerable);

            if (control.listSource != null)
            {
                control.listSource.SourceCollection = e.NewValue as IEnumerable;
            }
        }

        private void OnListSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnItemsChanged(e);
            this.InvalidateUI();
        }
    }
}
