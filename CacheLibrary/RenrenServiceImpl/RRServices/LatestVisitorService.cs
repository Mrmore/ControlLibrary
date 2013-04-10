using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.Apis;
using RenrenCore.AsyncArgs;
using RenrenCore.Entities;

namespace RenrenCore.RRServices
{
    class LatestVisitorService : RenrenAbstractService<int, VisitorsEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static LatestVisitorService Instance
        {
            get { return _instance; }
        }
        private static readonly LatestVisitorService _instance = new LatestVisitorService();

        /// <summary>
        /// Request and update the self visitors list
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<VisitorsEntity>> RequestMyVisitorList(int page = -1, int pageSize = -1)
        {
            RenrenAyncRespArgs<VisitorsEntity> resp = await Request(page, pageSize);
            return resp;
        }

        /// <summary>
        /// Request and update the visitors list by uid
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<VisitorsEntity>> RequestVisistorListByUid(int uid, int page = -1, int pageSize = -1)
        {
            RenrenAyncRespArgs<VisitorsEntity> resp = await RequestById(uid, page, pageSize);
            return resp;
        }

        protected async override Task<RenrenAyncRespArgs<VisitorsEntity>> DoRequest(params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            int page = args.Length > 0 ? (int)args[0] : -1;
            int pageSize = args.Length > 1 ? (int)args[1] : -1;
            RenrenAyncRespArgs<VisitorsEntity> resp = await Renren3GApiWrapper.GetVisitorList(seesionKey, secrectKey, page, pageSize);
            return resp;
        }

        protected async override Task<RenrenAyncRespArgs<VisitorsEntity>> DoRequestById(int id, params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            int page = args.Length > 0 ? (int)args[0] : -1;
            int pageSize = args.Length > 1 ? (int)args[1] : -1;
            RenrenAyncRespArgs<VisitorsEntity> resp = await Renren3GApiWrapper.GetVisitorList(seesionKey, secrectKey, id, page, pageSize);
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
        private LatestVisitorService() 
        { }
    }
}
