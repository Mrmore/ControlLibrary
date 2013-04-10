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
    class PageInfoService : RenrenAbstractService<int, PageInfoEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static PageInfoService Instance
        {
            get { return _instance; }
        }
        private static readonly PageInfoService _instance = new PageInfoService();


        /// <summary>
        /// Request and update the user info by uid
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<PageInfoEntity>> RequestUserInfoByUid(int uid)
        {
            RenrenAyncRespArgs<PageInfoEntity> resp = await RequestById(uid);
            return resp;
        }


        protected async override Task<RenrenAyncRespArgs<PageInfoEntity>> DoRequestById(int id, params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            RenrenAyncRespArgs<PageInfoEntity> resp = await Renren3GApiWrapper.GetPageInfo(seesionKey, secrectKey, id);
            return resp;
        }

        protected override Task<RenrenAyncRespArgs<PageInfoEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected override void DoResetById(int id)
        {
        }

        protected override void DoReset()
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private PageInfoService() 
        { }
    }
}
