using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Entities;

namespace RenrenCoreWrapper.Facades.Facotories
{
    class ServiceAbstractFactoryImpl : IServiceAbstractFactory
    {
        IRenrenServiceFactory _renren = null;
        ICacheServiceFactory _cache = null;

        public ServiceAbstractFactoryImpl()
        {
            _renren = new RenrenServiceFactoryImpl();
            _cache = new CacheServiceFactoryImpl();
        }

        public ICacheServiceFactory CreateCacheServiceFactry()
        {
            return _cache;
        }

        public IRenrenServiceFactory CreateRRServiceFactry(LoginUserInfo login = null)
        {
            return _renren;
        }
    }
}
