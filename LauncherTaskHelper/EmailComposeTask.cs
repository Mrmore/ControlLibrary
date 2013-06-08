using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

namespace LauncherTaskHelper
{
    public sealed class EmailComposeTask : IEmailComposeTask
    {
        private const string URLMASK = "mailto:{0}?subject={1}&body={2}";

        public string To
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }

        public IAsyncOperation<bool> Show()
		{
			string uriString = string.Format("mailto:{0}?subject={1}&body={2}", new object[]
			{
				this.To,
				EmailComposeTask.Encode(this.Subject),
				EmailComposeTask.Encode(this.Body)
			});
			return Launcher.LaunchUriAsync(new Uri(uriString));
		}

        private static string Encode(string input)
        {
            input = input.Replace(" ", "%20").Replace("&", "&amp;").Replace("\"", "&quat;").Replace("=", "%3D").Replace(":", "%3A").Replace("@", "%40").Replace("?", "%3F");
            return input;
        }
    }
}
