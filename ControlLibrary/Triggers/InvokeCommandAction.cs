namespace ControlLibrary.Triggers
{
    using System.Windows.Input;

    using Windows.UI.Xaml;

    /// <summary>
    /// An action capable of invoking an <see cref="ICommand"/> in response to a trigger.
    /// </summary>
    public sealed class InvokeCommandAction : TriggerAction
    {
        #region Static Fields

        /// <summary>
        /// Defines the CommandParameter dependency property, of type <see cref="object"/>.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(InvokeCommandAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Defines the Command dependency property, of type <see cref="ICommand"/>.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(InvokeCommandAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Defines the Command dependency property, of type <see cref="PassEventArgsToCommand"/>.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsToCommandProperty = DependencyProperty.Register(
            "PassEventArgsToCommand",
            typeof(bool),
            typeof(InvokeCommandAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the command to be invoked.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command parameter to pass to the <see cref="ICommand"/> upon invocation.
        /// </summary>
        /// <remarks>
        /// This takes precedence over the <see cref="PassEventArgsToCommand"/> property - if <see cref="CommandParameter"/>
        /// is specified, then <see cref="PassEventArgsToCommand"/> is ignored.
        /// </remarks>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the event arguments associated to the raised
        /// event should be passed to the command.
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get
            {
                return (bool)this.GetValue(PassEventArgsToCommandProperty);
            }

            set
            {
                this.SetValue(PassEventArgsToCommandProperty, value);
            }
        }
        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void DoInvoke(object eventData)
        {
            var command = this.Command;
            if (command != null && command.CanExecute(this.CommandParameter))
            {
                command.Execute(this.CommandParameter == null && this.PassEventArgsToCommand ? eventData : this.CommandParameter);
            }
        }

        #endregion
    }
}