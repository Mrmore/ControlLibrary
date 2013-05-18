namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// A <see cref="Trigger"/> that responds to an event on the associated <see cref="FrameworkElement"/>.
    /// </summary>
    public class EventTrigger : EventHookingTrigger
    {
        #region Static Fields

        /// <summary>
        /// The EventName dependency property of type <see cref="string"/>.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            "EventName", typeof(string), typeof(EventTrigger), new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the event that the trigger should respond to.
        /// </summary>
        public string EventName
        {
            get
            {
                return (string)this.GetValue(EventNameProperty);
            }

            set
            {
                // TODO - change the hooked event if this value is being changed from something other than unset.
                this.SetValue(EventNameProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override void Attach(FrameworkElement obj)
        {
            base.Attach(obj);

            if (!string.IsNullOrEmpty(this.EventName))
            {
                var eventHooker = new EventHooker(this);
                eventHooker.HookEvent(obj, this.EventName);
            }
        }

        /// <inheritdoc />
        public override void Detach()
        {
            if (this.AssociatedObject != null)
            {
                var eventHooker = new EventHooker(this);
                eventHooker.UnhookEvent(this.AssociatedObject, this.EventName);
            }
        }

        #endregion
    }
}