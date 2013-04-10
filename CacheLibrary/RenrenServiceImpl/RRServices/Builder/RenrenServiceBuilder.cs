using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Framework;

namespace RenrenCoreWrapper.RRServices.Builder
{
    public class RenrenServiceBuilder
    {
        private IDictionary<ServiceType, Type> _typeBinder = new Dictionary<ServiceType, Type>();
        public RenrenServiceBuilder()
        {
            _typeBinder.Add(ServiceType.LoginServiceType, typeof(LoginService));
            _typeBinder.Add(ServiceType.AlbumListServiceType, typeof(AlbumListService));
            _typeBinder.Add(ServiceType.NewsFeedServiceType, typeof(NewsFeedService));
        }

        public bool Supports(ServiceType type)
        {
            return _typeBinder.ContainsKey(type);
        }

        public Type GetType(ServiceType type)
        {
            if (_typeBinder.ContainsKey(type))
            {
                return _typeBinder[type];
            }
            else
            {
                return null;
            }
        }
    }
}
