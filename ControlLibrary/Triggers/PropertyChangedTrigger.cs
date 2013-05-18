namespace ControlLibrary.Triggers
{
    /// <summary>
    /// A <see cref="Trigger"/> that is fired whenever a property value changes.
    /// </summary>
    public class PropertyChangedTrigger : BoundTrigger
    {
        #region Methods

        /// <inheritdoc />
        protected override void OnBindingValueChanged(object oldValue, object newValue)
        {
            // Really simple - we don't care what the property was changed to, so just fire away.
            this.OnTriggered();
        }

        #endregion
    }
}