using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Framework;

namespace RenrenCoreWrapper.Facades.Facotories
{
    public interface IRenrenServiceFactory
    {
        IService CreateByServiceType(ServiceType type, LoginUserInfo login);
        IService CreateLoginService();
    }
}
