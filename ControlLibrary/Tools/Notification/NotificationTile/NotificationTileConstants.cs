using Windows.ApplicationModel.Background;

namespace ControlLibrary.Tools
{
    public class NotificationTileConstants
    {
        #region Background Tasks Constants
        /// <summary>
        /// This name MUST match EXACTLY to the assembly namespace and class of the task library that handles it
        /// otherwise the <see cref="BackgroundExecutionManager"/> will be unable to execute it and you'll
        /// probably spend hours trying to debug why your live tile is not updating. Just saying.
        /// </summary>
        public const string TaskLibraryName = "Tasks.Library.";
        /// <summary>
        /// Application Settings Container name
        /// </summary>
        public const string TaskSettingsContainer = "Task Settings";

        /// <summary>
        /// Tile Updates
        /// </summary>
        public const string TileUpdateTaskName = "NotificationTileUpdateTask";
        public const string TileUpdateUserTaskName = TileUpdateTaskName + "UserPresent";
        /// <summary>
        /// The Task entry point in the library.
        /// </summary>
        public const string TileUpdateTaskEntry = TaskLibraryName + TileUpdateTaskName;

        #endregion
    }
}