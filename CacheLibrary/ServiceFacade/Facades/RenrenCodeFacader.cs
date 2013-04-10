using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Facades.Facotories;

namespace RenrenCoreWrapper.Facades
{
    public class RenrenCodeFacader : IRenrenCoreFacade
    {
        static IServiceAbstractFactory _services = null;
        static LoginService _loginService = null;
        public LoginService GetLoginService()
        {
            if (_loginService == null)
            {
                var af = GetServiceAbstractFactry();
                var serviceFactory = af.CreateRRServiceFactry();
                _loginService = serviceFactory.CreateLoginService() as LoginService;
            }

            return _loginService;
        }

        public IServiceAbstractFactory GetServiceAbstractFactry()
        {
            if (_services == null)
            {
                _services = new ServiceAbstractFactoryImpl();
            }
            return _services;
        }
    }
}
