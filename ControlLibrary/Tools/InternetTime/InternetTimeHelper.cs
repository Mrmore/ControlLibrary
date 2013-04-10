using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    public class InternetTimeHelper
    {
        private volatile static InternetTimeHelper _instance = null;
        private static readonly object lockHelper = new object();

        private DateTime InterdateTime;
        private TimeSpan timeSpan;

        public static InternetTimeHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new InternetTimeHelper();

                    }
                }
                return _instance;
            }
        }

        //初始化方法
        private InternetTimeHelper()
        {
            
        }

        public DateTime GetInterTime()
        {
            if (InterdateTime.Year == 1)
            {
                HttpClient getHc = null;

                if (getHc == null)
                {
                    getHc = new HttpClient();
                    //getHc.Timeout = new TimeSpan(TimeSpan.TicksPerMillisecond * 500);
                    getHc.Timeout = TimeSpan.FromMilliseconds(500);
                }

                Task task = Task.Run(async () =>
                {
                    try
                    {
                        HttpResponseMessage result = await getHc.GetAsync(new Uri("http://renren.com"));
                        DateTimeOffset? time = result.Headers.Date;
                        InterdateTime = time.Value.LocalDateTime;
                    }
                    catch
                    {
                        InterdateTime = DateTime.Now;
                    }
                });
                task.Wait();
                timeSpan = DateTime.Now - InterdateTime;
            }
            else
            {
                InterdateTime = DateTime.Now - timeSpan;
            }
            return InterdateTime;
        }
    }
}
