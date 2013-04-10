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
    class StatusService : RenrenAbstractService<long, StatusEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static StatusService Instance
        {
            get { return _instance; }
        }
        private static readonly StatusService _instance = new StatusService();

        /// <summary>
        /// Request and update the status content by status id
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <param name="uid">user id</param>
        /// <param name="statusId">status id</param>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<StatusEntity>> RequestStatusByUid(int uid, long statusId)
        {
            RenrenAyncRespArgs<StatusEntity> resp = await RequestById(statusId, uid);
            return resp;
        }

        /// <summary>
        /// Request and update the self status content
        /// </summary>
        /// <returns>the async result</returns>
        protected override Task<RenrenAyncRespArgs<StatusEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Request and update the status content
        /// </summary>
        /// <returns>the async result</returns>
        protected async override Task<RenrenAyncRespArgs<StatusEntity>> DoRequestById(long id, params object[] args)
        {
            if (args.Length < 1) throw new ArgumentException();
            int uid = (int)args[0];
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;

            RenrenAyncRespArgs<StatusEntity> resp = await Renren3GApiWrapper.GetStatus(seesionKey, secrectKey, uid, id);
            return resp;
        }

        /// <summary>
        /// Reset overall models
        /// </summary>
        protected override void DoReset()
        {
        }

        protected override void DoResetById(long id)
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private StatusService() 
        { }
    }
}
