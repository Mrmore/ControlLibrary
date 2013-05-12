using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlLibrary.SettingsManagement
{
    /// <summary>
    /// Represents the information for a <see cref="SettingsCommandInfo{T}"/>.
    /// </summary>
    /// <typeparam name="T">A <see cref="UserControl"/> which represents the content for the settings flyout.</typeparam>
    class SettingsCommandInfo<T> : ISettingsCommandInfo where T : UserControl, new()
    {
        #region constructors
        public SettingsCommandInfo(string headerText, SettingsFlyout.SettingsFlyoutWidth width, SolidColorBrush headerTextBrush)
        {
            HeaderText = headerText;
            Width = width;
            HeaderTextBrush = headerTextBrush;
        }
        #endregion

        #region properties

        #region HeaderText
        /// <summary>
        /// Gets the header of the settings command to display in the Settings charm.
        /// </summary>
        /// <value>The header of the settings command.</value>
        public string HeaderText
        {
            get;
            private set;
        }
        #endregion

        #region Instance
        /// <summary>
        /// Gets the instantiated <see cref="UserControl"/> instance which represents
        /// the content of the settings flyout.
        /// </summary>
        public UserControl Instance
        {
            get
            {
                return new T();
            }
        }
        #endregion

        #region Width
        /// <summary>
        /// Gets the width of the settings flyout.
        /// </summary>
        /// <value>The width of the settings flyout.</value>
        public SettingsFlyout.SettingsFlyoutWidth Width
        {
            get;
            private set;
        }
        #endregion

        #region HeaderTextBrush
        public SolidColorBrush HeaderTextBrush
        {
            get;
            private set;
        }
        #endregion

        #endregion
    }
}
