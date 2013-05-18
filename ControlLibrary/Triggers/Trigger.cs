namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// The base class for all triggers.
    /// </summary>
    [ContentProperty(Name = "TriggerActions")]
    public abstract class Trigger : AssociatableElement
    {
        #region Static Fields

        /// <summary>
        /// Defines the TriggerAction dependency property, of type <see cref="AttachableCollection{TriggerAction}"/>.
        /// </summary>
        public static readonly DependencyProperty TriggerActionsProperty = DependencyProperty.Register(
             "TriggerActions",
             typeof(AttachableCollection<TriggerAction>),
             typeof(Trigger),
             new PropertyMetadata(DependencyProperty.UnsetValue, AssociatedPropertyChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger"/> class. 
        /// </summary>
        protected Trigger()
        {
            this.TriggerActions = new AttachableCollection<TriggerAction>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the collection of <see cref="TriggerAction"/>s to invoke when the trigger is fired.
        /// </summary>
        public AttachableCollection<TriggerAction> TriggerActions
        {
            get
            {
                return (AttachableCollection<TriggerAction>)this.GetValue(TriggerActionsProperty);
            }

            set
            {
                this.SetValue(TriggerActionsProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <inheritdoc />
        public override void Attach(FrameworkElement obj)
        {
            base.Attach(obj);

            this.TriggerActions.Attach(obj);
        }

        /// <inheritdoc />
        public override void Detach()
        {
            base.Detach();

            this.TriggerActions.Detach();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the trigger is fired.
        /// </summary>
        /// <param name="eventData">
        /// The optional data associated to the event that caused the
        /// trigger to be fired.
        /// </param>
        protected virtual void OnTriggered(object eventData = null)
        {
            var actions = this.TriggerActions;
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.Invoke(eventData);
                }
            }
        }

        #endregion
    }
}