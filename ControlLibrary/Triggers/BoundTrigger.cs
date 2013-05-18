namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// A trigger that is bound to some value.
    /// </summary>
    public abstract class BoundTrigger : Trigger
    {
        #region Static Fields

        /// <summary>
        /// The Binding dependency property of type <see cref="object"/>.
        /// </summary>
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding", 
            typeof(object), 
            typeof(BoundTrigger), 
            new PropertyMetadata(DependencyProperty.UnsetValue, BindingValueChanged));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the binding that the trigger watches for changes on.
        /// </summary>
        public object Binding
        {
            get
            {
                return this.GetValue(BindingProperty);
            }

            set
            {
                this.SetValue(BindingProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the value associated to the bound property changes value.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected abstract void OnBindingValueChanged(object oldValue, object newValue);

        /// <summary>
        /// Handles the event raised by the <see cref="BindingProperty"/> when its value changes.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> the property changed on.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> containing the event arguments.
        /// </param>
        private static void BindingValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var command = d as BoundTrigger;
            if (command != null)
            {
                command.OnBindingValueChanged(e.OldValue, e.NewValue);
            }
        }

        #endregion
    }
}