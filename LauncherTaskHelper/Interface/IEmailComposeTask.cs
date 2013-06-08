using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LauncherTaskHelper
{
    internal interface IEmailComposeTask
    {
        string To
        {
            get;
            set;
        }

        string Subject
        {
            get;
            set;
        }

        string Body
        {
            get;
            set;
        }

        IAsyncOperation<bool> Show();
    }
}
