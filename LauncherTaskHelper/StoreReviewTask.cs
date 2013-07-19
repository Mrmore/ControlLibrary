using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.System;

namespace LauncherTaskHelper
{
    /// <summary>
    /// 程序评论Task
    /// </summary>
    public sealed class StoreReviewTask : IStoreReviewTask
	{
		public string Application
		{
			get;
			set;
		}

		public StoreReviewTask()
		{
            this.Application = Package.Current.Id.FamilyName;
		}

		public IAsyncOperation<bool> Show()
		{
			return Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", new object[]
			{
				this.Application
			})));
		}
	}
}
