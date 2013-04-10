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
    public interface IServiceAbstractFactory
    {
        ICacheServiceFactory CreateCacheServiceFactry();
        IRenrenServiceFactory CreateRRServiceFactry(LoginUserInfo login = null);
    }
}
