using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRtHttpHelper.Data
{
    public enum DownLoadState
    {
        Start = 0,
        Suspended = 1,
        Continue = 2,
        Stop = 3
    }
}
