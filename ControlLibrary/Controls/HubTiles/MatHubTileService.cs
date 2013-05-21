using System.Windows;
using Windows.UI.Xaml;

namespace ControlLibrary
{
    /// <summary>
    /// RadHubTileService provids the ability to group hub tiles with a group tag
    /// and to freeze or unfreeze groups of hub tiles.
    /// </summary>
    public static class MatHubTileService
    {
        /// <summary>
        /// Identifies the GroupTag attached property.
        /// </summary>
        public static readonly DependencyProperty GroupTagProperty =
            DependencyProperty.RegisterAttached("GroupTag", typeof(string), typeof(MatHubTileService), new PropertyMetadata(null));

        private static WeakReferenceList<HubTileBase> tiles = new WeakReferenceList<HubTileBase>();

        internal static WeakReferenceList<HubTileBase> Tiles
        {
            get
            {
                return tiles;
            }
        }

        /// <summary>
        /// Gets the group tag of the provided hub tile.
        /// </summary>
        /// <param name="hubTile">The hub tile to get the group tag from.</param>
        /// <returns>Returns the group tag of the provided hub tile.</returns>
        public static string GetGroupTag(DependencyObject hubTile)
        {
            return (string)hubTile.GetValue(MatHubTileService.GroupTagProperty);
        }

        /// <summary>
        /// Sets the group tag of the specified hub tile to the specified value.
        /// </summary>
        /// <param name="hubTile">The hub tile to set the group tag to.</param>
        /// <param name="value">The tile's new group tag.</param>
        public static void SetGroupTag(DependencyObject hubTile, string value)
        {
            hubTile.SetValue(MatHubTileService.GroupTagProperty, value);
        }

        /// <summary>
        /// Freezes a group of hub tiles.
        /// </summary>
        /// <param name="groupTag">The groupTag which will be used when searching for hub tiles to freeze.</param>
        public static void FreezeGroup(string groupTag)
        {
            tiles.Apply<HubTileBase>((tile) => tile.IsFrozen = true, (tile) => GetGroupTag(tile) == groupTag);
        }

        /// <summary>
        /// Unfreezes a group of hub tiles.
        /// </summary>
        /// <param name="groupTag">The groupTag which will be used when searching for hub tiles to unfreeze.</param>
        public static void UnfreezeGroup(string groupTag)
        {
            tiles.Apply<HubTileBase>((tile) => tile.IsFrozen = false, (tile) => GetGroupTag(tile) == groupTag);
        }
    }
}
