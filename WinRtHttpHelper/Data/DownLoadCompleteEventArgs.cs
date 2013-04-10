using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace WinRtHttpHelper.Data
{
    public class DownLoadCompleteEventArgs : EventArgs
    {
        private IRandomAccessStream downLoadStream;

        public DownLoadCompleteEventArgs(IRandomAccessStream downLoadStream)
        {
            this.downLoadStream = downLoadStream;
        }

        public IRandomAccessStream DownLoadStream
        {
            get
            {
                return this.downLoadStream;
            }
        }
    }
}
