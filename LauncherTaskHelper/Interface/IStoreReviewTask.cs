using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LauncherTaskHelper
{
    internal interface IStoreReviewTask
    {
        string Application
        {
            get;
            set;
        }

        IAsyncOperation<bool> Show();
    }
}
