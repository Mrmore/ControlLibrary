namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The various <see cref="Storyboard"/> actions that can be invoked by the <see cref="ControlStoryboardAction"/> class.
    /// </summary>
    public enum StoryboardAction
    {
        /// <summary>
        /// Start (or re-start) the <see cref="Storyboard"/>.
        /// </summary>
        Start,

        /// <summary>
        /// Pause the <see cref="Storyboard"/>.
        /// </summary>
        Pause,

        /// <summary>
        /// Stop the <see cref="Storyboard"/>.
        /// </summary>
        Stop
    }
}