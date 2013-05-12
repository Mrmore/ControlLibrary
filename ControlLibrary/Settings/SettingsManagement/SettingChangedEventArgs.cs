namespace ControlLibrary.SettingsManagement
{
    /// <summary>
    /// Provides data for the <see cref="INotifySettingChanged.SettingChanged"/>
    /// event.</summary>
    public sealed class SettingChangedEventArgs : System.EventArgs
    {
        #region fields
        private readonly string _settingName;
        #endregion

        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Callisto.Controls.SettingsManagement.SettingChangedEventArgs"/>
        /// class.</summary>
        /// <param name="settingName">The name of the setting that changed.</param>
        public SettingChangedEventArgs(string settingName)
        {
            _settingName = settingName;
        }
        #endregion

        #region properties

        #region SettingName
        /// <summary>
        /// Gets the name of the setting that changed.
        /// </summary>
        /// <returns>The name of the setting that changed.</returns>
        public string SettingName
        {
            get
            {
                return _settingName;
            }
        }
        #endregion

        #endregion
    }
}
