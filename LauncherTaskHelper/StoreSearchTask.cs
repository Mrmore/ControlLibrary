using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

namespace LauncherTaskHelper
{
    /// <summary>
    /// 程序搜索Task
    /// </summary>
    public sealed class StoreSearchTask : IStoreSearchTask
	{
		public string SearchQuery
		{
			get;
			set;
		}

		public IAsyncOperation<bool> Show()
		{
			return Launcher.LaunchUriAsync(new Uri(string.Format("ms-windows-store:Search?query={0}", new object[]
			{
				this.SearchQuery
			})));
		}
	}
}
