using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.CacheService
{
    // Summary:
    //     Represents the all status
    public enum CacheStatus
    {
        READY,
        PENDING,
        FAILED,
        UNKOWN
    }

    // Summary:
    //     Represents the method that will handle the RenrenCore.CacheService.ICacheChip.Completed
    //     event raised when the cache chip process complete.
    public delegate void ChipCompleteEventHandler(object sender, CacheStatus e, params object[] args);
}
