using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Framework.RenrenService
{
    public abstract class RRAbstractService<IdType, EntityType> : ServiceDecoratorAbstract, IRenrenService where EntityType : PropertyChangedBase, INotifyPropertyChanged, new()
    {
        #region Data member
        protected RRServiceFlyweight<IdType, EntityType> _flyweighter = new RRServiceFlyweight<IdType, EntityType>();

        public LoginUserInfo LoginInfo { get; set; }
        #endregion

        #region Contractor
        public RRAbstractService(LoginUserInfo loginInfo)
        {
            this.LoginInfo = loginInfo;
        }

        /// <summary>
        /// 允许其默认构造并通过基类的Init传递sessionkey & secrectKey
        /// </summary>
        public RRAbstractService()
        { 
        }
        #endregion

        #region Comes from IRenrenService
        public bool NeedOfflineData
        {
            get;
            protected set;
        }

        public bool ForceDataRequest
        {
            get;
            protected set;
        }

        public object Model
        {
            get;
            protected set;
        }

        public object this[object id, int page = -1]
        {
            get
            {
                return _flyweighter.Entity((IdType)id, page);
            }
            private set
            {
                _flyweighter.Add((IdType)id, (EntityType)value, page);
            }
        }

        // must: first parameter is id, and second is page
        protected async Task<object> Request(params object[] args)
        {
            //if (args.Length < 2) throw new ArgumentException(); // pls always input the the id and page

            RenrenAyncRespArgs<EntityType> resp = default(RenrenAyncRespArgs<EntityType>);
            // 处理需要缓存的情况
            if (this.NeedOfflineData)
            {
                // TODO:
                // 查看是否支持缓存
                // 查看是否存在缓存
                // 查看是否已经失效
                resp = await DoOfflineDataRequest(args);

                if (resp.LocalError == null && resp.RemoteError == null && resp.Result != null)
                {
                    this.Model = resp.Result;
                }
            }
            // 处理不需要缓存的情况
            else
            {
                object id = args[0];
                int page = (int)args[1];
                if (this[id, page] != null && this.ForceDataRequest == false)
                {
                    resp = new RenrenAyncRespArgs<EntityType>((EntityType)this[id, page]);
                    this.Model = resp.Result;
                }
                else
                {
                    // TODO:
                    // we need to reflesh login token here through LoginTokenRefleshVisitor
                    // by which we can check the login status and reflesh the sessionkey and secrectkey
                    // also we can use it to login using different login methods, e.g. username nd password, or OAuth2.0
                    // if (LoginInfo == null)
                    //      var check = this.Accecpt(PasswordLoginTokenRefleshVisitor);
                    //      var check = this.Accecpt(OAuth20LoginTokenRefleshvVisitor);
                    resp = await DoRequest(args);

                    if (resp.LocalError == null && resp.RemoteError == null && resp.Result != null)
                    {
                        this.Model = resp.Result;
                        string key = string.Empty;
                        if (args.Length >= 2)
                            key = RRServiceFlyweight<IdType, EntityType>.GenerateKey((IdType)args[0], (int)page);
                        else
                            key = RRServiceFlyweight<IdType, EntityType>.GenerateDefaultKey();

                        this._flyweighter.Add(key, (EntityType)Model);
                    }
                }
            }

            return resp;
        }

        protected override async Task DoReset()
        {
            _flyweighter.Reset();
            Model = null;
            await DoServiceReset();
        }

        public void ResetById(object id)
        {
            _flyweighter.ResetById((IdType)id);
            DoResetById((IdType)id);
        }

        public void ResetByIdnPage(object id, int page = -1)
        {
            _flyweighter.ResetByIdnPage((IdType)id, page);
            DoResetByIdnPage((IdType)id, page);
        }
        #endregion

        #region Comes from ServiceDecoratorAbstract
        protected override ServiceCatalogue Catalogue()
        {
            return GetCatalogue();
        }

        protected override string UniqueID()
        {
            return GetUniqueID();
        }

        protected override ServiceType Type()
        {
            return GetType();
        }

        protected override Task<object> DoInvoke(params object[] args)
        {
            return Request(args);
        }

        public void AttachLoginInfo(LoginUserInfo info)
        {
            if (info == null) throw new ArgumentNullException();
            this.LoginInfo = info;
        }

        public void DettachLoginInfo()
        {
            this.LoginInfo = null;
        }

        protected override Task<bool> DoInit(params object[] args)
        {
            // TODO:
            // 获取该业务的缓存信息（是否缓存，缓存失效期限等），以及
            // 一些基本的通用初始化过程。
            if (args.Length > 0 && args[0] is LoginUserInfo)
            {
                this.LoginInfo = (LoginUserInfo)args[0];
            }
            return DoServiceInit(args);
        }

        protected override Task<object> DoAccecpt(IServiceVisitor visitor, params object[] args)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Functions delay to implement
        protected abstract Task<RenrenAyncRespArgs<EntityType>> DoRequest(params object[] args);
        // 延迟离线的数据获取的到具体业务，原因有2
        // 1, 只有具体业务才能更准确的知道cache key的组成
        // 2, 只有具体业务才能知道离线数据的存储方式。
        protected abstract Task<RenrenAyncRespArgs<EntityType>>DoOfflineDataRequest(params object[] args);
        protected abstract void DoResetById(IdType id);
        protected abstract void DoResetByIdnPage(IdType id, int page);
        protected abstract Task<bool> DoServiceInit(params object[] args);
        protected abstract Task DoServiceReset();

        protected abstract ServiceCatalogue GetCatalogue();
        protected abstract string GetUniqueID();
        protected abstract ServiceType GetType();
        #endregion
    }
}
