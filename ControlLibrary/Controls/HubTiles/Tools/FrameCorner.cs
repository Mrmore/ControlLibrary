using System.ComponentModel;

namespace ControlLibrary
{
    /// <summary>
    /// Determines which corner of an image a CustomMosaicTile should display.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public enum FrameCorner
    {
        /// <summary>
        /// Top left corner.
        /// </summary>
        TopLeft = 0,

        /// <summary>
        /// Top right corner.
        /// </summary>
        TopRight,

        /// <summary>
        /// Bottom left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Bottom right corner.
        /// </summary>
        BottomRight
    }
}
