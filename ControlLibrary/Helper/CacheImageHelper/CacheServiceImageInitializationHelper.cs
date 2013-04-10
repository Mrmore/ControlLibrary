using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Facades;

namespace ControlLibrary.Helper.CacheImageHelper
{
    public class CacheServiceImageInitializationHelper
    {
        public static void CacheImageInitializationHelper(string cacheImageDateType)
        {
            var af = new RenrenCodeFacader().GetServiceAbstractFactry();
            var cacheFactory = af.CreateCacheServiceFactry();
            var imageCache = cacheFactory.CreateImageCacheByServiceType(cacheImageDateType);
        }

        public static void CacheImageInitializationHelper(CacheImageDateType cacheImageDateType)
        {
            var af = new RenrenCodeFacader().GetServiceAbstractFactry();
            var cacheFactory = af.CreateCacheServiceFactry();
            var imageCache = cacheFactory.CreateImageCacheByServiceType(CacheImageDateManage.CacheTypeToString(cacheImageDateType));
        }
    }
}
