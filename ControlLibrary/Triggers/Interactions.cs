namespace ControlLibrary.Triggers
{
    using System;

    using Windows.UI.Xaml;

    /// <summary>
    /// The entry point for the XAML - exposes the <see cref="Interactions.TriggersProperty"/> attached property.
    /// </summary>
    public static class Interactions
    {
        #region Static Fields

        /// <summary>
        /// Defines the Triggers attached property, of type <see cref="TriggerCollection"/>.
        /// </summary>
        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached(
            "Triggers", 
            typeof(TriggerCollection), 
            typeof(Interactions), 
            new PropertyMetadata(DependencyProperty.UnsetValue, OnCommandsChanged));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the value of the <see cref="TriggersProperty"/> attached property.
        /// </summary>
        /// <param name="dependencyObject">
        /// The dependency object for which to get the value.
        /// </param>
        /// <returns>
        /// The <see cref="TriggerCollection"/> associated to the given object.
        /// </returns>
        // ReSharper disable ReturnTypeCanBeEnumerable.Global
        public static TriggerCollection GetTriggers(DependencyObject dependencyObject)
        // ReSharper restore ReturnTypeCanBeEnumerable.Global
        {
            if (dependencyObject != null)
        {
            var collection = (TriggerCollection)dependencyObject.GetValue(TriggersProperty);

            if (collection == null)
            {
                collection = new TriggerCollection();
                dependencyObject.SetValue(TriggersProperty, collection);
            }

            return collection;
        }

            return null;
        }

        /// <summary>
        /// Sets the value of the Triggers attached property
        /// </summary>
        /// <param name="dependencyObject">
        /// dependency object to set the value
        /// </param>
        /// <param name="value">
        /// Instance of the command collection
        /// </param>
        public static void SetTriggers(DependencyObject dependencyObject, TriggerCollection value)
        {
            dependencyObject.SetValue(TriggersProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the changing of the Commands property.
        /// </summary>
        /// <param name="dependencyObject">
        /// The event source.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> containing the event arguments.
        /// </param>
        private static void OnCommandsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var obj = dependencyObject as FrameworkElement; 
            if (obj == null)
            {
                throw new InvalidOperationException("Triggers can only be attached to types that derive from FrameworkElement");
            }

            var oldValue = e.OldValue as TriggerCollection;
            var newValue = e.NewValue as TriggerCollection;

            if (oldValue != newValue)
            {
                if (oldValue != null)
                {
                    oldValue.Detach();
                }

                if (newValue != null)
                {
                    newValue.Attach(obj);
                }
            }
        }

        #endregion
    }
}