using System;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace ControlLibrary.Tools
{
    /// <summary>
    /// Register the Tile update task library with the <see cref="BackgroundExecutionManager"/>
    /// </summary>
    public sealed class NotificationTileUpdateTaskRegistration
    {
        public static BackgroundTaskCompletedEventHandler TileUpdateCompleted;
        public static BackgroundTaskProgressEventHandler TileUpdateInProgress;

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Background tasks are restored from suspension/termination based on their <see cref="Guid"/>
        /// </remarks>
        /// <param name="container">Container to store the task state keys</param>
        public async void CreateTileUpdateTasks(ApplicationDataContainer container)
        {
#if !SIMULATOR
            //If your running/debugging the app over the simulator it will die here
            //So set the compiler directive to bypass this if needed
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.Denied) return;
#endif
            if (!container.Values.ContainsKey(NotificationTileConstants.TileUpdateTaskName))
            {
                container.Values.Add(NotificationTileConstants.TileUpdateTaskName, CreateBackgroundTileUpdateTask());
            }

            if (!container.Values.ContainsKey(NotificationTileConstants.TileUpdateUserTaskName))
            {
                container.Values.Add(NotificationTileConstants.TileUpdateUserTaskName, CreateUserPresentTileUpdateTask());
                return;
            }

            //Re-connection of the event handlers for the tasks
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if ((Guid)container.Values[NotificationTileConstants.TileUpdateTaskName] == task.Key)
                {
                    task.Value.Completed += OnCompleted;
                    task.Value.Progress += OnProgress;
                    continue;
                }

                if ((Guid)container.Values[NotificationTileConstants.TileUpdateUserTaskName] == task.Key)
                {
                    task.Value.Completed += OnCompleted;
                    task.Value.Progress += OnProgress;
                    continue;
                }

                task.Value.Unregister(true);
            }
        }

        /// <summary>
        /// Create a background task for the <see cref="SystemTriggerType.UserPresent"/>
        /// </summary>
        /// <returns>Task ID</returns>
        private Guid CreateUserPresentTileUpdateTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == NotificationTileConstants.TileUpdateUserTaskName)
                {
                    task.Value.Completed -= OnCompleted;
                    task.Value.Progress -= OnProgress;
                    task.Value.Unregister(true);
                }
            }

            var builder = new BackgroundTaskBuilder
            {
                Name = NotificationTileConstants.TileUpdateUserTaskName,
                TaskEntryPoint = NotificationTileConstants.TileUpdateTaskEntry
            };

            builder.SetTrigger(new SystemTrigger(SystemTriggerType.UserPresent, false));

            var tileupdater = builder.Register();
            tileupdater.Completed += OnCompleted;
            tileupdater.Progress += OnProgress;
            return tileupdater.TaskId;
        }

        /// <summary>
        /// Create a background task for the <see cref="TimeTrigger"/>
        /// </summary>
        /// <returns>Task ID</returns>
        private Guid CreateBackgroundTileUpdateTask()
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == NotificationTileConstants.TileUpdateTaskName)
                {
                    task.Value.Completed -= OnCompleted;
                    task.Value.Progress -= OnProgress;
                    task.Value.Unregister(true);
                }
            }

            var builder = new BackgroundTaskBuilder
            {
                Name = NotificationTileConstants.TileUpdateTaskName,
                TaskEntryPoint = NotificationTileConstants.TileUpdateTaskEntry
            };

            builder.SetTrigger(new TimeTrigger(15, false));

            var tileupdater = builder.Register();
            tileupdater.Completed += OnCompleted;
            tileupdater.Progress += OnProgress;
            return tileupdater.TaskId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnCompleted(IBackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs e)
        {
            var handler = TileUpdateCompleted;
            if (null != handler)
                handler(sender as BackgroundTaskRegistration, e);
        }

        private static void OnProgress(IBackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs e)
        {
            var handler = TileUpdateInProgress;
            if (null != handler)
                handler(sender as BackgroundTaskRegistration, e);
        }
    }
}