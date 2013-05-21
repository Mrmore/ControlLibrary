using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.SettingsManagement
{
    /// <summary>
    /// Represents the information for a <see cref="SettingsCommandInfo{T}"/>.
    /// </summary>
    interface ISettingsCommandInfo
    {
        #region properties

        #region HeaderText
        /// <summary>
        /// Gets the header of the settings command to display in the Settings charm.
        /// </summary>
        /// <value>The header of the settings command.</value>
        string HeaderText
        {
            get;
        }
        #endregion

        #region Instance
        /// <summary>
        /// Gets the instantiated <see cref="UserControl"/> instance which represents
        /// the content of the settings flyout.
        /// </summary>
        UserControl Instance
        {
            get;
        }
        #endregion

        #region Width
        /// <summary>
        /// Gets the width of the settings flyout.
        /// </summary>
        /// <value>The width of the settings flyout.</value>
        SettingsFlyout.SettingsFlyoutWidth Width
        {
            get;
        }
        #endregion

        #region HeaderTextBrush
        SolidColorBrush HeaderTextBrush
        {
            get;
        }
        #endregion

        #endregion
    }
}
