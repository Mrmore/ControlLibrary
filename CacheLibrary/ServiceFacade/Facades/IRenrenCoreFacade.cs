using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.RRServices.Login;
using RenrenCoreWrapper.Facades.Facotories;
using RenrenCoreWrapper.Framework;

namespace RenrenCoreWrapper.Facades
{
    public interface IRenrenCoreFacade
    {
        IServiceAbstractFactory GetServiceAbstractFactry();
        LoginService GetLoginService();
    }
}
