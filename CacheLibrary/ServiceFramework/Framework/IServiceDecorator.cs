using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Framework
{
    /// <summary>
    /// 用于绑定其他业务的功能，或者添加前置或者后置功能。
    /// 例如：用于绑定cache，识别是否需要cache服务
    /// </summary>
    interface IServiceDecorator
    {
        /// <summary>
        /// 添加前置条件业务
        /// </summary>
        /// <param name="service"></param>
        void AddPreconditionSerivce(IService service, IServiceVisitor relevantVisitor);

        /// <summary>
        /// 添加后置条件业务
        /// </summary>
        /// <param name="service"></param>
        void AddPostconditionService(IService service, IServiceVisitor relevantVisitor);

        /// <summary>
        /// 添加其他业务 : 预留接口
        /// </summary>
        /// <param name="service"></param>
        void AddServiceItem(IService service, IServiceVisitor relevantVisitor);
    }
}
