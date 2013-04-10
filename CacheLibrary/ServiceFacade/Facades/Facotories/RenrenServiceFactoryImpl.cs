using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Framework;
using RenrenCoreWrapper.RRServices.Builder;

namespace RenrenCoreWrapper.Facades.Facotories
{
    class RenrenServiceFactoryImpl : IRenrenServiceFactory
    {
        private IDictionary<ServiceType, object> _serviceFlyweighter = new Dictionary<ServiceType, object>();
        private IService _loginService = null;
        private RenrenServiceBuilder builder = new RenrenServiceBuilder();

        public IService CreateByServiceType(ServiceType type, LoginUserInfo login)
        {
            if (_serviceFlyweighter.ContainsKey(type))
            {
                return _serviceFlyweighter[type] as IService;
            }
            else
            {
                if (builder.Supports(type))
                {
                    Type serviceType = builder.GetType(type);
                    var serivce = Activator.CreateInstance(serviceType);
                    (serivce as IService).Init(login);
                    _serviceFlyweighter.Add(type, serivce);

                    return serivce as IService;
                }
                else
                {
                    throw new Exception("This kind of Service does not support!");
                }
            }
        }

        public IService CreateLoginService()
        {
            if (_loginService == null)
            {
                _loginService = new LoginService();
                _loginService.Init(-1, -1);
            }

            return _loginService;
        }
    }
}
