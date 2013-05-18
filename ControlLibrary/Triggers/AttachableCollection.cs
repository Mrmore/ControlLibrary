namespace ControlLibrary.Triggers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using Windows.ApplicationModel;
    using Windows.UI.Xaml;

    /// <summary>
    /// Implements a collection of attachable objects that can be attached to a <see cref="FrameworkElement"/>.
    /// </summary>
    /// <remarks>
    /// The inner list is lazily initialized to allow for these collections to be constructed relatively cheaply
    /// even if they are never actually populated.
    /// </remarks>
    /// <typeparam name="T">
    /// The type of the elements stored in the collection. Must inherit from <see cref="FrameworkElement"/> and implement
    /// <see cref="IAssociatableElement"/>.
    /// </typeparam>
    public class AttachableCollection<T> : FrameworkElement, IList<T>, INotifyCollectionChanged, IAssociatableElement
        where T : FrameworkElement, IAssociatableElement
    {
        #region Fields

        /// <summary>
        /// The lazily initialized collection of items.
        /// </summary>
        private readonly Lazy<ObservableCollection<T>> items;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachableCollection{T}"/> class.
        /// </summary>
        public AttachableCollection()
        {
            this.items = new Lazy<ObservableCollection<T>>(this.InitializeItemsCollection);
        }

        #region Public Events

        /// <summary>
        /// Notifies changes in the collection
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public FrameworkElement AssociatedObject { get; private set; }

        /// <inheritdoc />
        public int Count
        {
            get
            {
                return this.Items.Count;
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the items associated to this instance.
        /// </summary>
        private ObservableCollection<T> Items
        {
            get
            {
                return this.items.Value;
            }
        }

        #endregion

        #region Public Indexers

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                return this.Items[index];
            }

            set
            {
                T oldItem = this.Items[index];
                if (oldItem != null)
                {
                    this.ItemRemoved(oldItem);
                    oldItem.Detach();
                }

                this.Items[index] = value;

                if (value != null)
                {
                    this.ItemAdded(value);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public void Add(T item)
        {
            this.ItemAdded(item);
            this.Items.Add(item);
        }

        /// <inheritdoc />
        public void Attach(FrameworkElement obj)
        {
            if (obj != this.AssociatedObject)
            {
                if (this.AssociatedObject != null)
                {
                    throw new InvalidOperationException("Collection is already attached to another object.");
                }

                if (!DesignMode.DesignModeEnabled)
                {
                    this.AssociatedObject = obj;
                }

                this.OnAttached();
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            foreach (T item in this.Items)
            {
                this.ItemRemoved(item);
            }

            this.Items.Clear();
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return this.Items.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.Items.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public void Detach()
        {
            if (this.AssociatedObject == null)
            {
                throw new InvalidOperationException("Collection is not attached to an object.");
            }

            this.OnDetached();
            this.AssociatedObject = null;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return this.Items.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            this.ItemAdded(item);
            this.Items.Insert(index, item);
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            this.ItemRemoved(item);
            return this.Items.Remove(item);
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            var item = this.Items[index];
            this.ItemRemoved(item);
            this.Items.RemoveAt(index);
        }

        #endregion

        #region Explicit Interface Methods

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called whenever an item is added into the collection. This will pass down the currently 
        /// associated object if one is currently set on this collection.
        /// </summary>
        /// <param name="item">
        /// The added item.
        /// </param>
        protected virtual void ItemAdded(T item)
        {
            if (this.AssociatedObject != null)
            {
                item.Attach(this.AssociatedObject);
            }
        }

        /// <summary>
        /// Called whenever an item is removed from the collection. This will detach the currently
        /// associated object from the item if one is currently set.
        /// </summary>
        /// <param name="item">
        /// The removed item.
        /// </param>
        protected virtual void ItemRemoved(T item)
        {
            if (this.AssociatedObject != null)
            {
                item.Detach();
            }
        }

        /// <summary>
        /// Called when the collection is attached to an object. This will automatically
        /// attach the associated object to any contained items.
        /// </summary>
        protected virtual void OnAttached()
        {
            // Attach each of the contained items
            foreach (var item in this)
            {
                item.Attach(this.AssociatedObject);
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyCollectionChanged.CollectionChanged"/> event on this instance.
        /// </summary>
        /// <param name="args">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> containing the event arguments.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        /// <summary>
        /// Called when the collection is detached from an object. This automatically detaches
        /// any contained items as well.
        /// </summary>
        protected virtual void OnDetached()
        {
            // Detach each of the contained items
            foreach (T item in this)
            {
                item.Detach();
            }
        }

        /// <summary>
        /// Handles the <see cref="INotifyCollectionChanged.CollectionChanged"/> event of the inner list.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="NotifyCollectionChangedEventArgs"/> containing the event arguments.
        /// </param>
        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        /// <summary>
        /// Lazily initialized the items collection.
        /// </summary>
        /// <returns>
        /// The created collection.
        /// </returns>
        private ObservableCollection<T> InitializeItemsCollection()
        {
            var collection = new ObservableCollection<T>();
            collection.CollectionChanged += this.ItemsCollectionChanged;
            return collection;
        }

        #endregion
    }
}