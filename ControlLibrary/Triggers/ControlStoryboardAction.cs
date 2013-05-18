namespace ControlLibrary.Triggers
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The control storyboard action.
    /// </summary>
    public class ControlStoryboardAction : TriggerAction
    {
        #region Static Fields

        /// <summary>
        /// The Action dependency property of type <see cref="StoryboardAction"/>.
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action", 
            typeof(StoryboardAction), 
            typeof(ControlStoryboardAction), 
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The Storyboard dependency property of type <see cref="Storyboard"/>.
        /// </summary>
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
            "Storyboard", 
            typeof(Storyboard), 
            typeof(ControlStoryboardAction), 
            new PropertyMetadata(DependencyProperty.UnsetValue));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the action to invoke on the <see cref="Storyboard"/>.
        /// </summary>
        public StoryboardAction Action
        {
            get
            {
                return (StoryboardAction)this.GetValue(ActionProperty);
            }

            set
            {
                this.SetValue(ActionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the associated <see cref="Storyboard"/>.
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

        /// <inheritdoc />
        protected override void DoInvoke(object eventData)
        {
            if (this.Storyboard != null)
            {
                switch (this.Action)
                {
                    case StoryboardAction.Stop:
                        this.Storyboard.Stop();
                        break;
                    case StoryboardAction.Start:
                        this.Storyboard.Begin();
                        break;
                    case StoryboardAction.Pause:
                        this.Storyboard.Pause();
                        break;
                    default:
                        throw new InvalidOperationException("Unknown action " + this.Action);
                }
            }
        }

        #endregion
    }
}