using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Framework;

namespace RenrenCoreWrapper.Facades.Facotories
{
    public interface ICacheServiceFactory
    {
        IService CreateImageCacheByServiceType(ServiceType type);
        IService CreateServiceCacheByServiceType<T>(ServiceType type);
        IService CreateImageCacheByServiceType(string type);
        IService CreateServiceCacheByServiceType<T>(string type);
    }
}
