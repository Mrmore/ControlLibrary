using ControlLibrary.Tools;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace Tasks.Library
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NotificationTileUpdateTask : IBackgroundTask
    {
        private volatile bool _cancelRequested;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //This is a critical component to use if any of your
            //tile update classes/methods etc. contain await/async calls
            var defferal = taskInstance.GetDeferral();

            taskInstance.Canceled += OnCanceled;
            if (_cancelRequested) return;

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();

            //Keep in mind that a tile queue can have only 5 items
            updater.EnableNotificationQueue(true);
            updater.Clear();

            //正式项目要把ViewModel的值带进来
            NotificationTileHelper.UpdateTileWithImages(new List<NotificationTile>());
            NotificationTileHelper.UpdateTileWithText(new NotificationTile());

            defferal.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            _cancelRequested = true;
        }
    }
}
