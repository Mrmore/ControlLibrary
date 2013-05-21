namespace ControlLibrary
{
    /// <summary>
    /// This type is used to determine the state of the item selected and the
    /// previous items.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public enum RatingSelectionMode
    {
        /// <summary>
        /// All items before the selected ones are selected.
        /// </summary>
        Continuous,

        /// <summary>
        /// Only the item selected is visually distinguished.
        /// </summary>
        Individual
    }
}