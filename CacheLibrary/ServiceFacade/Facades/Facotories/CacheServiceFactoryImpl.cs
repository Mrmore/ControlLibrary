using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.CacheService;
using RenrenCoreWrapper.Framework;
using Windows.UI.Xaml.Media.Imaging;

namespace RenrenCoreWrapper.Facades.Facotories
{
    class CacheServiceFactoryImpl : ICacheServiceFactory
    {
        private IDictionary<string, object> _serviceFlyweighter = new Dictionary<string, object>();
        private IDictionary<string, object> _imageFlyweighter = new Dictionary<string, object>();

        public IService CreateImageCacheByServiceType(ServiceType type)
        {
            return CreateImageCacheByServiceType(type.ToString());
        }

        public IService CreateServiceCacheByServiceType<T>(ServiceType type)
        {
            return CreateServiceCacheByServiceType<T>(type.ToString());
        }

        public IService CreateImageCacheByServiceType(string type)
        {
            if (_imageFlyweighter.ContainsKey(type))
            {
                return _imageFlyweighter[type] as IService;
            }
            else
            {
                var servise = new CachServiceAdaptor<BitmapImage>(CacheServiceType.IMAGE);
                servise.Init(type);
                _imageFlyweighter.Add(type, servise);
                return servise;
            }
        }

        public IService CreateServiceCacheByServiceType<T>(string type)
        {
            if (_serviceFlyweighter.ContainsKey(type))
            {
                return _serviceFlyweighter[type] as IService;
            }
            else
            {
                var servise = new CachServiceAdaptor<T>(CacheServiceType.SERVICE);
                servise.Init(type);
                _serviceFlyweighter.Add(type, servise);
                return servise;
            }
        }
    }
}
