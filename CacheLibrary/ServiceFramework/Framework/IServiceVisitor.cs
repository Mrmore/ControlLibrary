using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Framework
{
    public interface IServiceVisitor
    {
        Task<object> Visit(object service, params object[] args);
    }
}
