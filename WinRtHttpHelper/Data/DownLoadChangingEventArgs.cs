using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRtHttpHelper.Data
{
    //http://msdn.microsoft.com/zh-cn/library/system.eventargs(v=vs.80).aspx
    public class DownLoadChangingEventArgs : EventArgs
    {
        private int progress;
        private DownLoadBytes downLoadBytes;
        private byte[] bytes;

        public DownLoadChangingEventArgs(DownLoadBytes downLoadBytes, int progress, byte[] bytes)
        {
            this.downLoadBytes = downLoadBytes;
            this.progress = progress;
            this.bytes = bytes;
        }

        /// <summary>
        /// 下载的接受字节信息
        /// </summary>
        public DownLoadBytes DownLoadBytes
        {
            get
            {
                return this.downLoadBytes;
            }
        }

        /// <summary>
        /// 下载的进度
        /// </summary>
        public int Progress
        {
            get
            {
                //我会在外面做好
                //Math.Round(downLoadBytes.BytesReceived * 100.0 / downLoadBytes.TotalBytesToReceive, MidpointRounding.ToEven);
                return this.progress;
            }
        }

        /// <summary>
        /// 下载的进度实时的字节数组
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                return this.bytes;
            }
        }
    }
}
