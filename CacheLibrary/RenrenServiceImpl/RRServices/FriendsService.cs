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
    class FriendsService : RenrenAbstractService<int, FriendListEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static FriendsService Instance
        {
            get { return _instance; }
        }
        private static readonly FriendsService _instance = new FriendsService();

        /// <summary>
        /// Request and update the self friend list
        /// The wrapped convient method provided for outside call
        /// </summary>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<FriendListEntity>> RequestMyFriendList()
        {
            RenrenAyncRespArgs<FriendListEntity> resp = await Request();
            return resp;
        }

        /// <summary>
        /// Request and update the friend list by uid
        /// The wrapped convient method provided for outside call
        /// \note: if the uid is null means requesting self friends list
        /// </summary>
        /// <param name="uid">user id</param>
        /// <param name="page">page count</param>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<FriendListEntity>> RequestFriendListByUid(int uid, int page = -1, int pageSize = -1)
        {
            RenrenAyncRespArgs<FriendListEntity> resp = await RequestById(uid, page, pageSize);
            return resp;
        }

        /// <summary>
        /// Request and update the self friends list
        /// </summary>
        /// <returns>the async result</returns>
        protected async override Task<RenrenAyncRespArgs<FriendListEntity>> DoRequest(params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            RenrenAyncRespArgs<FriendListEntity> resp = await Renren3GApiWrapper.GetFriendList(seesionKey, secrectKey);
            return resp;
        }

        /// <summary>
        /// Request and update the friends list by uid
        /// </summary>
        /// <returns>the async result</returns>
        protected async override Task<RenrenAyncRespArgs<FriendListEntity>> DoRequestById(int id, params object[] args)
        {
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            int page = args.Length > 0 ? (int)args[0] : -1;
            int pageSize = args.Length > 1 ? (int)args[1] : -1;

            RenrenAyncRespArgs<FriendListEntity> resp = await Renren3GApiWrapper.GetFriendList(seesionKey, secrectKey, id, page, pageSize);
            return resp;
        }

        /// <summary>
        /// Reset overall models
        /// </summary>
        protected override void DoReset()
        {
        }

        protected override void DoResetById(int id)
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private FriendsService() 
        { }
    }
}
