using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRtHttpHelper.Data
{
    public class DownLoadBytes
    {
        private long totalBytesToReceive = 0;
        private long bytesReceived = 0;
        /// <summary>
        /// 总共要接收的字节数
        /// </summary>
        public long TotalBytesToReceive
        {
            get
            {
                return totalBytesToReceive;
            }
            set
            {
                totalBytesToReceive = value;
            }
        }

        /// <summary>
        /// 已接收的字节数
        /// </summary>
        public long BytesReceived
        {
            get
            {
                return bytesReceived;
            }
            set
            {
                bytesReceived = value;
            }
        }
    }
}
