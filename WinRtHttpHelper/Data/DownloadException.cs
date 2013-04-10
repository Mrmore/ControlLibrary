using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRtHttpHelper.Data
{
    public class DownloadTimeOutException : Exception
    {
        public DownloadTimeOutException()
        {

        }

        public override string Message
        {
            get
            {
                return "下载超时";
            }
        }
    }

    public class DownloadErrException : Exception
    {
        public DownloadErrException()
        {

        }

        public override string Message
        {
            get
            {
                return "下载错误";
            }
        }
    }

    public class DownloadInvalidResourceException : Exception
    {
        public DownloadInvalidResourceException()
        {
        }

        public override string Message
        {
            get
            {
                return "无效的资源";
            }
        }
    }

    public class DownloadResourceErrException : Exception
    {
        public DownloadResourceErrException()
        {
        }

        public override string Message
        {
            get
            {
                return "资源以出错";
            }
        }
    }

    public class DownloadTaskStreamErrException : Exception
    {
        public DownloadTaskStreamErrException()
        {
        }

        public override string Message
        {
            get
            {
                return "异步取消导致网络下载到内存的流错误";
            }
        }
    }

    public class DownloadTaskCancelErrException : Exception
    {
        public DownloadTaskCancelErrException()
        {
        }

        public override string Message
        {
            get
            {
                return "异步取消导致错误";
            }
        }
    }
}
