using System;

namespace ControlLibrary.SettingsManagement
{
    /// <summary>
    /// Notifies clients that a setting value has changed.
    /// </summary>
    public interface INotifySettingChanged
    {
        /// <summary>
        /// Occurs when a setting value changes.
        /// </summary>
        event EventHandler<SettingChangedEventArgs> SettingChanged;
    }

}
