// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoryboardCompletedTrigger.cs" company="">
//   
// </copyright>
// <summary>
//   A <see cref="Trigger" /> that is fired whenever a storyboard completes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    ///     A <see cref="Trigger" /> that is fired whenever a storyboard completes.
    /// </summary>
    public class StoryboardCompletedTrigger : EventHookingTrigger
    {
        #region Static Fields

        /// <summary>
        /// The Storyboard dependency property of type <see cref="Storyboard" />.
        /// </summary>
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
            "Storyboard", 
            typeof(Storyboard), 
            typeof(StoryboardCompletedTrigger), 
            new PropertyMetadata(DependencyProperty.UnsetValue, StoryboardPropertyChanged));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the storyboard to wait for the completion event on.
        /// </summary>
        public Storyboard Storyboard
        {
            get
            {
                return (Storyboard)this.GetValue(StoryboardProperty);
            }

            set
            {
                this.SetValue(StoryboardProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the event raised by the <see cref="StoryboardProperty"/> when its value changes.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> the property changed on.
        /// </param>
        /// <param name="e">
        /// The <see cref="DependencyPropertyChangedEventArgs"/> containing the event arguments.
        /// </param>
        private static void StoryboardPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var trigger = d as StoryboardCompletedTrigger;
            if (trigger == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                new EventHooker(trigger).UnhookEvent(e.OldValue, "Completed");
            }

            if (e.NewValue != null)
            {
                new EventHooker(trigger).HookEvent(e.NewValue, "Completed");
            }
        }

        #endregion
    }
}