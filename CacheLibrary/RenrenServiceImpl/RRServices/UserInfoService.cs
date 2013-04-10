using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.Apis;
using RenrenCore.AsyncArgs;
using RenrenCore.Entities;

namespace RenrenCore.RRServices
{
    class UserInfoService : RenrenAbstractService<int, UserInfoEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static UserInfoService Instance
        {
            get { return _instance; }
        }
        private static readonly UserInfoService _instance = new UserInfoService();

        /// <summary>
        /// Request and update self user info
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns>The info result</returns>
        public async Task<RenrenAyncRespArgs<UserInfoEntity>> RequestMyUserInfo()
        { 
            RenrenAyncRespArgs<UserInfoEntity> resp = await Request();
            return resp;
        }

        /// <summary>
        /// Request and update the user info by uid
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<UserInfoEntity>> RequestUserInfoByUid(int uid)
        {
            RenrenAyncRespArgs<UserInfoEntity> resp = await RequestById(uid);
            return resp;
        }

        protected async override Task<RenrenAyncRespArgs<UserInfoEntity>> DoRequest(params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            RenrenAyncRespArgs<UserInfoEntity> resp = await Renren3GApiWrapper.GetUserInfo(seesionKey, secrectKey);
            return resp;
        }

        protected async override Task<RenrenAyncRespArgs<UserInfoEntity>> DoRequestById(int id, params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            RenrenAyncRespArgs<UserInfoEntity> resp = await Renren3GApiWrapper.GetUserInfo(seesionKey, secrectKey, id);
            return resp;
        }

        protected override void DoReset()
        {
        }

        protected override void DoResetById(int id)
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private UserInfoService()
        { 
        }
    }
}
