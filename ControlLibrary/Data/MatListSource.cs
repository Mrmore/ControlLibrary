using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlLibrary.CommonControl;
using Windows.UI.Xaml.Data;

namespace ControlLibrary
{
    /// <summary>
    /// Implements a simple list-based data source that provides currency management and implements the <see cref="System.Collections.Specialized.INotifyCollectionChanged"/> interface.
    /// </summary>
    internal partial class MatListSource :
        DisposableObject,
        IEnumerable,
        ICollection,
        INotifyCollectionChanged,
        ICurrencyManager,
        IWeakEventListener
    {
        public const string CollectionChangedEventName = "CollectionChanged";
        public const string PropertyChangedEventName = "PropertyChanged";
        public const string CurrentChangedEventName = "CurrentChanged";

        internal static readonly object UnsetObject = new object();

        private const uint SettingSourceStateKey = DisposableObjectStateKey << 1;
        private const uint RefreshingStateKey = SettingSourceStateKey << 1;
        private const uint UpdatingCurrentStateKey = RefreshingStateKey << 1;

        private IEnumerable sourceCollection;
        private ICollectionView sourceCollectionAsCollectionView;
        private List<IDataSourceItem> items;

        private WeakEventHandler<NotifyCollectionChangedEventArgs> collectionChangedHandler;
        private WeakEventHandler<EventArgs> currentChangedHandler;

        private IDataSourceItemFactory itemFactory;

        private NotifyCollectionChangedEventHandler collectionChangedEvent;
        private EventHandler<ItemPropertyChangedEventArgs> itemPropertyChangedChangedEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RadListSource"/> class.
        /// </summary>
        public MatListSource()
        {
            this.itemFactory = new DataSourceItemFactory();
            this.currencyMode = CurrencyManagementMode.LocalAndExternal;

            this.items = new List<IDataSourceItem>();
        }

        ~MatListSource()
        {
            if (this.collectionChangedHandler != null)
            {
                this.collectionChangedHandler.Unsubscribe();
            }

            if (this.currentChangedHandler != null)
            {
                this.currentChangedHandler.Unsubscribe();
            }

            this.UnhookPropertyChanged();
        }

        /// <summary>
        /// Raised when an internal change in the collection has occurred.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                this.collectionChangedEvent += value;
            }
            remove
            {
                this.collectionChangedEvent -= value;
            }
        }

        /// <summary>
        /// Raised when a change in a single item property has occurred.
        /// </summary>
        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged
        {
            add
            {
                this.itemPropertyChangedChangedEvent += value;
            }
            remove
            {
                this.itemPropertyChangedChangedEvent -= value;
            }
        }

        /// <summary>
        /// Determines whether the source is in a process of refreshing its items.
        /// </summary>
        public bool IsRefreshing
        {
            get
            {
                return this[RefreshingStateKey];
            }
        }

        /// <summary>
        /// Determines whether raw data initialization is running.
        /// </summary>
        public bool IsSettingSource
        {
            get
            {
                return this[SettingSourceStateKey];
            }
        }

        /// <summary>
        /// Gets or sets the raw data associated with this data source.
        /// </summary>
        public IEnumerable SourceCollection
        {
            get
            {
                return this.sourceCollection;
            }
            set
            {
                if (this.sourceCollection == value)
                {
                    return;
                }

                this.SetSource(value);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDataSourceItemFactory"/> instance used to create <see cref="IDataSourceItem"/> instances, stored in this data source.
        /// </summary>
        public IDataSourceItemFactory ItemFactory
        {
            get
            {
                return this.itemFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.itemFactory = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets the count of the collection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.items.Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the list with all data source items.
        /// </summary>
        protected List<IDataSourceItem> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets the IEnumerable instance to be used when building inner view models (<see cref="IDataSourceItem"/> instances).
        /// </summary>
        protected virtual IEnumerable DataItemsSource
        {
            get
            {
                return this.sourceCollection;
            }
        }

        /// <summary>
        /// Gets the source collection as ICollectionView instance.
        /// </summary>
        protected ICollectionView OriginalCollectionView
        {
            get
            {
                return this.sourceCollectionAsCollectionView;
            }
        }

        void IWeakEventListener.ReceiveEvent(object sender, EventArgs e)
        {
            if (e is NotifyCollectionChangedEventArgs)
            {
                this.OnSourceCollectionChanged(sender, e as NotifyCollectionChangedEventArgs);
            }
            else if (e is PropertyChangedEventArgs)
            {
                this.OnItemPropertyChanged(sender, e as PropertyChangedEventArgs);
            }
            else
            {
                this.OnSourceCollectionCurrentChanged(sender, e);
            }
        }

        /// <summary>
        /// Re-evaluates the current state of the source and re-builds all the data items.
        /// </summary>
        public void Refresh()
        {
            if (!this.IsSuspended && !this[RefreshingStateKey])
            {
                this[RefreshingStateKey] = true;
                this.RefreshOverride();
                this[RefreshingStateKey] = false;
            }
        }

        /// <summary>
        /// Retrieves the IDataSourceItem that wraps the specified data object.
        /// </summary>
        /// <param name="data">The raw data object to search for.</param>
        /// <returns></returns>
        public IDataSourceItem FindItem(object data)
        {
            IDataSourceItem currentItem = this.GetFirstItem();
            while (currentItem != null)
            {
                if (object.Equals(currentItem.Value, data))
                {
                    return currentItem;
                }

                currentItem = currentItem.Next;
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="IEnumerator"/> instance used to iterate through currently available data items.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        internal object RequestDataForItem(IDataSourceItem item)
        {
            IList source = this.sourceCollection as IList;

            if (source == null)
            {
                return null;
            }

            // item might have already been removed from the collection, while its index is still not updated
            // so, check for index bounds
            if (item.Index < source.Count)
            {
                return source[item.Index];
            }

            return MatListSource.UnsetObject;
        }

        internal virtual void BuildDataItems()
        {
            // create IDataSourceItem instance for each raw data object
            IDataSourceItem previous = null;
            int index = 0;

            IEnumerable itemsSource = this.DataItemsSource;
            IDataSourceItem newCurrentItem = null;

            if (itemsSource != null)
            {
                foreach (object dataItem in itemsSource)
                {
                    IDataSourceItem dataSourceItem = this.CreateNewOrGetCurrent(dataItem, ref newCurrentItem);

                    // update the view model with index and prev/next references
                    dataSourceItem.Index = index;
                    if (previous != null)
                    {
                        dataSourceItem.Previous = previous;
                        previous.Next = dataSourceItem;
                    }

                    this.items.Add(dataSourceItem);

                    previous = dataSourceItem;

                    if (this.currencyMode != CurrencyManagementMode.None)
                    {
                        if (this.sourceCollectionAsCollectionView != null &&
                            object.Equals(this.sourceCollectionAsCollectionView.CurrentItem, dataItem))
                        {
                            newCurrentItem = dataSourceItem;
                        }
                    }

                    index++;
                }
            }

            this.UpdateCurrentItem(newCurrentItem);
        }

        /// <summary>
        /// Performs the core refresh logic. Allows inheritors to specify some additional logic or to completely override the existing one.
        /// </summary>
        protected virtual void RefreshOverride()
        {
            this.previousCurrentItem = this.currentItem;
            this.currentItem = null;

            this.UnhookPropertyChanged();

            // core data items creation
            this.items.Clear();
            this.BuildDataItems();

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            // check whether current item actually changed
            if (this.previousCurrentItem != this.currentItem)
            {
                this.OnCurrentItemChanged(EventArgs.Empty);
            }
            this.previousCurrentItem = null;
        }

        /// <summary>
        /// Allows inheritors to perform some additional logic upon attaching to raw data.
        /// </summary>
        protected virtual void AttachDataOverride()
        {
        }

        /// <summary>
        /// Allows inheritors to perform some additional logic upon detaching to raw data.
        /// </summary>
        protected virtual void DetachDataOverride()
        {
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.collectionChangedEvent != null)
            {
                this.collectionChangedEvent(this, e);
            }
        }

        /// <summary>
        /// Allows inheritors to perform some additional logic upon change in a single item's property.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void ItemPropertyChangedOverride(ItemPropertyChangedEventArgs args)
        {
        }

        /// <summary>
        /// Updates the current instance after a change in the raw data.
        /// Allows inheritors to provide additional logic upon the change.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void SourceCollectionChangedOverride(NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    this.Refresh();
                    break;
                case NotifyCollectionChangedAction.Add:
                    this.AddItems(e);

                    // raise CollectionChanged with proper arguments
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, this.items[e.NewStartingIndex], e.NewStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Remove:

                    // get the corresponding RemovedItems before they are actually updated
                    IDataSourceItem removedItem = this.items[e.OldStartingIndex];

                    this.RemoveItems(e);

                    // raise the CollectionChanged event
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, e.OldStartingIndex));

                    this.CheckCurrentRemoved(removedItem);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    this.ReplaceItems(e);

                    // raise the CollectionChanged event
                    IDataSourceItem replacedItem = this.items[e.NewStartingIndex];
                    this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, replacedItem, e.OldItems, e.NewStartingIndex));

                    this.CheckCurrentRemoved(replacedItem);
                    break;
            }
        }

        /// <summary>
        /// Adds new items to the source after a change of type Add has occurred in the original collection.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void AddItems(NotifyCollectionChangedEventArgs e)
        {
            int insertIndex = e.NewStartingIndex;
            IDataSourceItem previous = null;
            if (insertIndex > 0)
            {
                previous = this.items[insertIndex - 1];
            }

            for (int i = 0; i < e.NewItems.Count; i++)
            {
                IDataSourceItem dataItem = this.itemFactory.CreateItem(this, e.NewItems[i]);
                dataItem.Index = insertIndex;
                dataItem.Previous = previous;
                if (previous != null)
                {
                    previous.Next = dataItem;
                }

                this.items.Insert(insertIndex++, dataItem);

                previous = dataItem;
            }

            // update items' indexes
            this.ShiftIndexes(e.NewStartingIndex + e.NewItems.Count, e.NewItems.Count);

            // update previous/next references
            if (insertIndex < this.items.Count)
            {
                IDataSourceItem next = this.items[insertIndex];
                next.Previous = previous;
                if (previous != null)
                {
                    previous.Next = next;
                }
            }
        }

        /// <summary>
        /// Updates data items after a change of type Replace has occurred in the original collection. 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void ReplaceItems(NotifyCollectionChangedEventArgs e)
        {
            // replace old values with the new ones
            int startIndex = e.NewStartingIndex;
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                IDataSourceItem dataItem = this.items[startIndex++];

                // remove propertychanged subscription for old value
                (dataItem as DataSourceItem).UnhookPropertyChanged();

                // change value and hook propertychanged notification
                dataItem.ChangeValue(e.NewItems[i]);
                (dataItem as DataSourceItem).HookPropertyChanged();
            }
        }

        /// <summary>
        /// Removes data items from the source after a change of type Remove has occurred in the original collection.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void RemoveItems(NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < e.OldItems.Count; i++)
            {
                IDataSourceItem dataItem = this.items[e.OldStartingIndex];

                (dataItem as DataSourceItem).UnhookPropertyChanged();
                dataItem.Next = null;
                dataItem.Previous = null;

                this.items.RemoveAt(e.OldStartingIndex);
            }

            // update items' indexes
            this.ShiftIndexes(e.OldStartingIndex, -e.OldItems.Count);

            // update previous/next references
            IDataSourceItem previous = null;
            if (e.OldStartingIndex > 0)
            {
                previous = this.items[e.OldStartingIndex - 1];
            }
            IDataSourceItem next = null;
            if (e.OldStartingIndex < this.items.Count)
            {
                next = this.items[e.OldStartingIndex];
            }

            if (next != null)
            {
                next.Previous = previous;
            }
            if (previous != null)
            {
                previous.Next = next;
            }
        }

        /// <summary>
        /// Gets all the events, exposed by this instance. Used to clean-up event subscriptions upon disposal.
        /// </summary>
        /// <param name="events"></param>
        protected override void CollectEvents(List<Delegate> events)
        {
            base.CollectEvents(events);

            events.Add(this.collectionChangedEvent);
            events.Add(this.itemPropertyChangedChangedEvent);
            events.Add(this.currentItemChangingEvent);
            events.Add(this.currentItemChangedEvent);
        }

        /// <summary>
        /// Notifies that this instance is no longer suspended.
        /// Allows inheritors to provide their own update logic.
        /// </summary>
        /// <param name="update">True if an Update is requested, false otherwise.</param>
        protected override void OnResumed(bool update)
        {
            base.OnResumed(update);

            if (update)
            {
                this.Refresh();
            }
        }

        private void UnhookPropertyChanged()
        {
            if (this.items.Count == 0)
            {
                return;
            }

            DataSourceItem firstItem = this.items[0] as DataSourceItem;
            while (firstItem != null)
            {
                firstItem.UnhookPropertyChanged();
                firstItem = firstItem.Next as DataSourceItem;
            }
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.IsSuspended)
            {
                return;
            }

            ItemPropertyChangedEventArgs args = new ItemPropertyChangedEventArgs(sender, e.PropertyName);
            this.ItemPropertyChangedOverride(args);

            // raise the event
            if (this.itemPropertyChangedChangedEvent != null)
            {
                this.itemPropertyChangedChangedEvent(this, args);
            }
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.itemFactory.OnOwningListSourceCollectionChanged(e);

            if (!this.IsSuspended)
            {
                this.SourceCollectionChangedOverride(e);
            }
        }

        private void AttachData(IEnumerable data)
        {
            if (this.sourceCollection != null)
            {
                this.DetachData(this.sourceCollection);
            }

            this.sourceCollection = data;

            if (this.sourceCollection != null)
            {
                INotifyCollectionChanged collectionChanged = data as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    this.collectionChangedHandler = new WeakEventHandler<NotifyCollectionChangedEventArgs>(collectionChanged, this, CollectionChangedEventName);
                }

                this.sourceCollectionAsCollectionView = data as ICollectionView;
                if (this.sourceCollectionAsCollectionView != null)
                {
                    this.currentChangedHandler = new WeakEventHandler<EventArgs>(this.sourceCollectionAsCollectionView, this, CurrentChangedEventName);
                }
            }

            this.AttachDataOverride();
            this.Refresh();
        }

        private void DetachData(IEnumerable data)
        {
            if (this.collectionChangedHandler != null)
            {
                this.collectionChangedHandler.Unsubscribe();
                this.collectionChangedHandler = null;
            }

            if (this.currentChangedHandler != null)
            {
                this.currentChangedHandler.Unsubscribe();
                this.currentChangedHandler = null;
            }

            this.DetachDataOverride();
        }

        private void SetSource(IEnumerable value)
        {
            this[SettingSourceStateKey] = true;

            // reset current position
            this.currentItem = null;

            // rebuild data
            this.AttachData(value);

            this[SettingSourceStateKey] = false;
        }

        private void ShiftIndexes(int start, int offset)
        {
            // shift indexes
            int count = this.items.Count;
            for (int i = start; i < count; i++)
            {
                this.items[i].Index += offset;
            }
        }
    }
}
