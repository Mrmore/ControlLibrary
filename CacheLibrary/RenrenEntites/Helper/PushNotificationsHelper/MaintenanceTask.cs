using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace RenrenCoreWrapper.Helper.PushNotificationsHelper
{
    public sealed class MaintenanceTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Notifier notifier = new Notifier();

            // It's important not to block UI threads. Since this is a background task, we do need
            // to block on the channel operations completing

            ManualResetEventSlim completedEvent = new ManualResetEventSlim();
            
            notifier.RenewAllAsync(false).Completed += (IAsyncAction action, AsyncStatus status) =>
            {
                completedEvent.Set();
            };

            completedEvent.Wait();
        }
    }
}
