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
    /// <summary>
    /// id means the use id
    /// </summary>
    class GossipsService : RenrenAbstractService<int, GossipListEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static GossipsService Instance
        {
            get { return _instance; }
        }
        private static readonly GossipsService _instance = new GossipsService();

        public async Task<RenrenAyncRespArgs<GossipListEntity>> RequestGossipsByUid(int userId, int page, int pageSize)
        {
            RenrenAyncRespArgs<GossipListEntity> resp = await RequestById(userId, page, pageSize);
            return resp;
        }

        protected override Task<RenrenAyncRespArgs<GossipListEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected async override Task<RenrenAyncRespArgs<GossipListEntity>> DoRequestById(int userId, params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            int page = args.Length > 0 ? (int)args[0] : 1; // by default, page is 1
            int pageSize = args.Length > 1 ? (int)args[1] : 10; // by default, page size is 10
            RenrenAyncRespArgs<GossipListEntity> resp = await Renren3GApiWrapper.GetGossips(seesionKey, secrectKey, userId, page, pageSize);
            return resp;
        }

        /// <summary>
        /// Reset overall models
        /// </summary>
        protected override void DoReset()
        {
        }

        protected override void DoResetById(int userId)
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private GossipsService() 
        { }
    }
}
