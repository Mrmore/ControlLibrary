namespace ControlLibrary.Triggers
{
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// An action that is invoked in response to a <see cref="Trigger"/> firing.
    /// </summary>
    [ContentProperty(Name = "Conditions")]
    public abstract class TriggerAction : AssociatableElement
    {
        #region Static Fields

        /// <summary>
        /// The Conditions dependency property of type <see cref="AttachableCollection{Condition}"/>.
        /// </summary>
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(
            "Conditions",
            typeof(AttachableCollection<Condition>),
            typeof(TriggerAction),
            new PropertyMetadata(DependencyProperty.UnsetValue, AssociatableElement.AssociatedPropertyChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAction"/> class. 
        /// </summary>
        protected TriggerAction()
        {
            this.Conditions = new AttachableCollection<Condition>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the collection of <see cref="Condition"/>s associated to this instance.
        /// </summary>
        public AttachableCollection<Condition> Conditions
        {
            get
            {
                return (AttachableCollection<Condition>)this.GetValue(ConditionsProperty);
            }

            set
            {
                this.SetValue(ConditionsProperty, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests all the conditions associated to the action and if they all pass then invokes the action.
        /// </summary>
        /// <param name="eventData">
        /// The event data associated to the event that triggered the action.
        /// </param>
        public void Invoke(object eventData)
        {
            var conditions = this.Conditions;
            if (conditions == null || conditions.All(c => c.EvaluateCondition()))
            {
                this.DoInvoke(eventData);
            }
        }

        /// <inheritdoc />
        public override void Attach(FrameworkElement obj)
        {
            base.Attach(obj);

            this.Conditions.Attach(obj);
        }

        /// <inheritdoc />
        public override void Detach()
        {
            base.Detach();

            this.Conditions.Detach();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="eventData">
        /// The event data associated to the event that triggered the action.
        /// </param>
        protected abstract void DoInvoke(object eventData);
        #endregion
    }
}