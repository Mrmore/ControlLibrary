namespace ControlLibrary.Triggers
{
    using System;
    using System.Reflection;

    using Windows.UI.Xaml;

    /// <summary>
    /// An action that sets a property on an object when it is fired.
    /// </summary>
    public class SetPropertyAction : TriggerAction
    {
        #region Static Fields

        /// <summary>
        /// The PropertyName dependency property of type <see cref="string" />.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName", 
            typeof(string), 
            typeof(SetPropertyAction), 
            new PropertyMetadata(DependencyProperty.UnsetValue, TargetPropertyChanged));

        /// <summary>
        /// The Target dependency property of type <see cref="object" />.
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target", 
            typeof(object), 
            typeof(SetPropertyAction), 
            new PropertyMetadata(DependencyProperty.UnsetValue, TargetPropertyChanged));

        /// <summary>
        /// The Value dependency property of type <see cref="object" />.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(object), typeof(SetPropertyAction), new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Fields

        /// <summary>
        /// The property information for the named property on the target object type.
        /// </summary>
        private PropertyInfo propertyInfo;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the property to set the value on.
        /// </summary>
        public string PropertyName
        {
            get
            {
                return (string)this.GetValue(PropertyNameProperty);
            }

            set
            {
                this.SetValue(PropertyNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target object to set the property on.
        /// </summary>
        public object Target
        {
            get
            {
                return this.GetValue(TargetProperty);
            }

            set
            {
                this.SetValue(TargetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value to store on associated object when fired.
        /// </summary>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void DoInvoke(object eventData)
        {
            if (this.propertyInfo == null)
            {
                if (this.Target != null && !string.IsNullOrEmpty(this.PropertyName))
                {
                    var targetType = this.Target.GetType().GetTypeInfo();
                    this.propertyInfo = targetType.GetDeclaredProperty(this.PropertyName);
                }
            }

            // propertyInfo could still be null if some of the dependency properties haven't been set
            // or the target type doesn't expose the property publicly (or at all).
            if (this.propertyInfo != null && this.propertyInfo.CanWrite)
            {
                // Attempt to convert the value to the correct property type
                var value = Convert.ChangeType(this.Value, this.propertyInfo.PropertyType);

                this.propertyInfo.SetValue(this.Target, value);
            }
        }

        /// <summary>
        /// Handles the event raised by the <see cref="TargetProperty"/> and <see cref="PropertyNameProperty"/> when their values change.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> the property changed on.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> containing the event arguments.
        /// </param>
        private static void TargetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Make sure that any cached property info is reset - the next time the trigger fires 
            // it will be re-cached.
            var action = d as SetPropertyAction;
            if (action != null)
            {
                action.propertyInfo = null;
            }
        }

        #endregion
    }
}