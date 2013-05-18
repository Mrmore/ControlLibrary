namespace ControlLibrary.Triggers
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// A <see cref="FrameworkElement"/> that can be associated to a <see cref="FrameworkElement"/>
    /// at runtime.
    /// </summary>
    public abstract class AssociatableElement : FrameworkElement, IAssociatableElement
    {
        #region Public Properties

        /// <inheritdoc />
        public FrameworkElement AssociatedObject { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public virtual void Attach(FrameworkElement obj)
        {
            if (this.AssociatedObject != obj)
            {
                if (this.AssociatedObject != null)
                {
                    throw new InvalidOperationException("Element is already attached to another object.");
                }

                this.AssociatedObject = obj;

                if (obj != null)
                {
                    obj.Loaded += this.OnAssociatedObjectLoaded;
                }
            }
        }

        /// <inheritdoc />
        public virtual void Detach()
        {
            if (this.AssociatedObject == null)
            {
                throw new InvalidOperationException("Element is not attached to an object.");
            }

            // Just in case the event was never fired, unhook from it now
            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
            }

            // Unbind the data context associated to this instance
            this.ClearValue(FrameworkElement.DataContextProperty);

            this.AssociatedObject = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the property value changing on a dependency property of type <see cref="IAssociatableElement"/>
        /// that is associated to an object that also implements <see cref="IAssociatableElement"/>.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> the property changed on.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> containing the event arguments.
        /// </param>
        internal static void AssociatedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((IAssociatableElement)e.OldValue).Detach();
            }

            var trigger = d as IAssociatableElement;
            if (trigger != null && e.NewValue != null && trigger.AssociatedObject != null)
            {
                ((IAssociatableElement)e.NewValue).Attach(trigger.AssociatedObject);
            }
        }

        /// <summary>
        /// Called when the associated object is loaded. This is responsible for
        /// the magic that allows the binding of the <see cref="FrameworkElement.DataContext"/> from
        /// the associated object to this <see cref="AssociatableElement"/> instance.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event - this will be a <see cref="FrameworkElement"/>.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event arguments.
        /// </param>
        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            var elem = (FrameworkElement)sender;
            elem.Loaded -= this.OnAssociatedObjectLoaded;

            var binding = new Binding
                {
                   Source = elem, Path = new PropertyPath("DataContext"), Mode = BindingMode.OneWay 
                };

            this.SetBinding(FrameworkElement.DataContextProperty, binding);
        }

        #endregion
    }
}