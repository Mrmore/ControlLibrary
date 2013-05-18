namespace ControlLibrary.Triggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// Describes an object that can be associated to another <see cref="FrameworkElement"/>.
    /// </summary>
    public interface IAssociatableElement
    {
        #region Public Properties

        /// <summary>
        /// Gets the instance of the <see cref="FrameworkElement"/> currently attached to this instance.
        /// </summary>
        FrameworkElement AssociatedObject { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Attaches a <see cref="FrameworkElement"/> to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="FrameworkElement"/> to attach to this instance.
        /// </param>
        void Attach(FrameworkElement obj);

        /// <summary>
        /// Detaches the currently associated <see cref="FrameworkElement"/>.
        /// </summary>
        void Detach();

        #endregion
    }
}