namespace ControlLibrary.Triggers
{
    using System;

    using Windows.UI.Xaml;

    /// <summary>
    /// A <see cref="Trigger"/> much like <see cref="PropertyChangedTrigger"/>, except this implementation
    /// only fires when the property is set to a specific value.
    /// </summary>
    public class PropertySetTrigger : BoundTrigger
    {
        #region Static Fields

        /// <summary>
        /// Defines the RequiredValue dependency property, of type <see cref="string"/>.
        /// </summary>
        public static readonly DependencyProperty RequiredValueProperty = DependencyProperty.Register(
            "RequiredValue", 
            typeof(string), 
            typeof(PropertySetTrigger), 
            new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expected value that must be met for the state change to occur.
        /// </summary>
        public string RequiredValue
        {
            get
            {
                return (string)this.GetValue(RequiredValueProperty);
            }

            set
            {
                this.SetValue(RequiredValueProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void OnBindingValueChanged(object oldValue, object newValue)
        {
            // Perhaps not the best way to do this - any suggestions?
            if (string.Compare(this.RequiredValue, newValue.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                this.OnTriggered();
            }
        }

        #endregion
    }
}