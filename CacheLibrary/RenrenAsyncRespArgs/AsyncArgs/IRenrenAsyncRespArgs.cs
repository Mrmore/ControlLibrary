using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.AsyncArgs
{
    // TODO:
    // Add the necessary comments here
    public enum RespStatus
    { Successed, Failed, Pendding, Unkown }

    public interface IRenrenAsyncRespArgs
    {
        RespStatus Status { get; set; }
        object HandOverParams { get; set; }
    }
}
