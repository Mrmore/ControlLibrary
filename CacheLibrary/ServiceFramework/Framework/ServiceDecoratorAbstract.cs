using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.AsyncArgs;

namespace RenrenCoreWrapper.Framework
{
    public abstract class ServiceDecoratorAbstract : IServiceDecorator, IService
    {
        class SVBinder
        {
            public IService Service { get; set; }
            public IServiceVisitor Visitor { get; set; }
        }

        private SVBinder _preCondition = null;
        private SVBinder _postCondition = null;
        //private IList<SVBinder> _decorators = new List<SVBinder>();

        #region IServiceDecorator
        public void AddPreconditionSerivce(IService service, IServiceVisitor relevantVisitor)
        {
            if (null == service) throw new NullReferenceException();
            this._preCondition = new SVBinder() { Service = service, Visitor = relevantVisitor };
        }

        public void AddPostconditionService(IService service, IServiceVisitor relevantVisitor)
        {
            if (null == service) throw new NullReferenceException();
            this._postCondition = new SVBinder() { Service = service, Visitor = relevantVisitor };
        }

        public void AddServiceItem(IService service, IServiceVisitor relevantVisitor)
        {
            throw new NotImplementedException();
        }
        #endregion

        public ServiceCatalogue S_Catalogue
        {
            get
            {
                return Catalogue();
            }
        }

        public string S_UID
        {
            get
            {
                return UniqueID();
            }
        }

        public ServiceType S_Type
        {
            get
            {
                return Type();
            }
        }

        public Task<object> Accecpt(IServiceVisitor visitor, params object[] args)
        {
            return DoAccecpt(visitor, args);
        }

        public async Task<object> Invoke(ServiceRole role, params object[] args)
        {
            object preResult = null;
            object postResult = null;
            object midResult = null;

            // TODO: 这里需要加入 契约式的返回参数
            // 用于级联业务，例如先登陆后请求，该处的登陆应该具有：如果未登陆提示登陆的功能。
            // 也就是说需要对RenrenAsyncRespArgs进行重构，使其支持契约式的传递。
            // 需要增加对前置条件和后置角色时的处理，目前暂时不支持该功能
            // try the precondition
            if (this._preCondition != null)
            {
                preResult = await this._preCondition.Service.Invoke(ServiceRole.PreCondition, args);
                if ((preResult as IRenrenAsyncRespArgs).Status == RespStatus.Successed)
                {
                    preResult = await this.Accecpt(this._preCondition.Visitor, preResult);
                }
                else
                {
                    return preResult;
                }
            }

            if (this._preCondition != null && (preResult as IRenrenAsyncRespArgs).Status == RespStatus.Successed)
            {
                midResult = await DoInvoke(args);
            }
            else 
            {
                midResult = await DoInvoke(args);
            }

            if (this._postCondition != null) 
            {
                postResult = await this._postCondition.Service.Invoke(ServiceRole.PostCondition, args);
                if ((postResult as IRenrenAsyncRespArgs).Status == RespStatus.Successed)
                {
                    postResult = await this.Accecpt(this._postCondition.Visitor, postResult);
                }
                else
                {
                    return postResult;
                }
            }
            else {
                postResult = midResult;
            }

            return postResult;
        }

        public Task<bool> Init(params object[] args)
        {
            return DoInit(args);
        }

        public Task Reset()
        {
            return DoReset();
        }

        protected abstract ServiceCatalogue Catalogue();
        protected abstract string UniqueID();
        protected abstract ServiceType Type();
        protected abstract Task<object> DoInvoke(params object[] args);
        protected abstract Task<object> DoAccecpt(IServiceVisitor visitor, params object[] args);
        protected abstract Task<bool> DoInit(params object[] args);
        protected abstract Task DoReset();
    }
}
