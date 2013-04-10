using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Apis;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Framework;
using RenrenCoreWrapper.Framework.RenrenService;

namespace RenrenCore.RRServices.Login
{
    public class LoginService : RRAbstractService<int, LoginUserInfo>
    {
        private UserInfoManager _infoMgr = new UserInfoManager();
        public UserInfoManager UserInfoMgr
        {
            get { return _infoMgr; }
        }

        private bool _hasLogined = false;
        public bool HasLogined
        {
            get { return _hasLogined; }
        }
        
        public async Task<RenrenAyncRespArgs<LoginUserInfo>> Login(string userName, string password)
        {
            this.NeedOfflineData = false;
            this.ForceDataRequest = true;

            RenrenAyncRespArgs<LoginUserInfo> resp = (RenrenAyncRespArgs<LoginUserInfo>)(await this.Invoke(ServiceRole.SelfInvoke, -1, -1, userName, password));

            if (resp.Result != null) _hasLogined = true;
            return resp;
        }

        public async Task<bool> Logoff()
        {
            return false;
        }

        protected async override Task<RenrenAyncRespArgs<LoginUserInfo>> DoRequest(params object[] args)
        {
            string userName = (string)args[2];
            string password = (string)args[3];

            int uid = (int)args[0];
            int page = (int)args[1];

            var resp = await Renren3GApiWrapper.LogIn(userName, password);

            RenrenAyncRespArgs<LoginUserInfo> repAdaptor = null;
            if (resp.Result != null)
            {
                LoginUserInfo info = new LoginUserInfo() { UserName = userName, PassWord = password, LoginInfo = resp.Result };
                repAdaptor = new RenrenAyncRespArgs<LoginUserInfo>(info) { HandOverParams = resp.HandOverParams, Status = resp.Status };
            }
            else if (resp.LocalError != null)
            {
                repAdaptor = new RenrenAyncRespArgs<LoginUserInfo>(resp.LocalError);
            }
            else if (resp.RemoteError != null)
            {
                repAdaptor = new RenrenAyncRespArgs<LoginUserInfo>(resp.RemoteError);
            }

            return repAdaptor;
        }

        protected override Task<RenrenAyncRespArgs<LoginUserInfo>> DoOfflineDataRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override void DoResetById(int id)
        {
            throw new NotImplementedException();
        }

        protected override void DoResetByIdnPage(int id, int page)
        {
            throw new NotImplementedException();
        }

        protected async override Task<bool> DoServiceInit(params object[] args)
        {
            _infoMgr.Init();
            return true;
        }

        protected async override Task DoServiceReset()
        {
            _hasLogined = false;
            _infoMgr.Reset();
        }

        protected override ServiceCatalogue GetCatalogue()
        {
            throw new NotImplementedException();
        }

        protected override string GetUniqueID()
        {
            throw new NotImplementedException();
        }

        protected override ServiceType GetType()
        {
            throw new NotImplementedException();
        }
    }
}
