namespace ControlLibrary.Triggers
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// An action that changes the current state associated to the <see cref="VisualStateManager"/>.
    /// </summary>
    public class GotoStateAction : TriggerAction
    {
        #region Static Fields

        /// <summary>
        /// The StateName dependency property of type <see cref="string"/>.
        /// </summary>
        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register(
            "StateName", typeof(string), typeof(GotoStateAction), new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the state to change to when the action is invoked.
        /// </summary>
        public string StateName
        {
            get
            {
                return (string)this.GetValue(StateNameProperty);
            }

            set
            {
                this.SetValue(StateNameProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void DoInvoke(object eventData)
        {
            if (this.AssociatedObject != null && !string.IsNullOrEmpty(this.StateName))
            {
                // Find the Control instance closest to the parent in the visual state tree
                // This allows triggers to be placed in a logical context, i.e. in a Grid on a Window.
                // Grid does not derive from Control so we can't pass it directly to the VSM.
                DependencyObject element = this.AssociatedObject;
                Control control;
                do
                {
                    control = element as Control;
                    element = VisualTreeHelper.GetParent(element);
                }
                while (control == null && element != null);

                if (control != null)
                {
                    VisualStateManager.GoToState(control, this.StateName, true);
                }
            }
        }

        #endregion
    }
}