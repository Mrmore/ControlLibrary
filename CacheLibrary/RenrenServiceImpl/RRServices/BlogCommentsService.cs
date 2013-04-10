using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCore.Apis;
using RenrenCore.AsyncArgs;
using RenrenCore.Entities;
using RenrenCore.Models;

namespace RenrenCore.RRServices
{
    class BlogCommentsService : RenrenAbstractService<long, BlogCommentEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static BlogCommentsService Instance
        {
            get { return _instance; }
        }
        private static readonly BlogCommentsService _instance = new BlogCommentsService();

        /// <summary>
        /// Request and update the blog comment list by uid
        /// The wrapped convient method provided for outside call
        /// \note: if the uid is null means requesting self friends list
        /// </summary>
        /// <param name="uid">user id</param>
        /// <param name="page">page count</param>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<BlogCommentEntity>> RequestBlogCommentsByUid(int uid, long blogId, int page, int pageSize, string password)
        {
            RenrenAyncRespArgs<BlogCommentEntity> resp = await RequestById(blogId, uid, page, pageSize, password);
            return resp;
        }

        /// <summary>
        /// Request and update the self  blog comment list
        /// </summary>
        /// <returns>the async result</returns>
        protected override Task<RenrenAyncRespArgs<BlogCommentEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Request and update the  blog comment list by uid
        /// </summary>
        /// <returns>the async result</returns>
        protected async override Task<RenrenAyncRespArgs<BlogCommentEntity>> DoRequestById(long id, params object[] args)
        {
            if (args.Length < 1) throw new ArgumentException();
            int uid = (int)args[0];
            int page = args.Length > 1 ? (int)args[1] : 1;
            int pageSize = args.Length > 2 ? (int)args[2] : -1;
            string password = args.Length > 3 ? (string)args[3] : string.Empty;
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;

            RenrenAyncRespArgs<BlogCommentEntity> resp = await Renren3GApiWrapper.GetBlogComment(seesionKey, secrectKey, uid, id, page, pageSize, password);
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
        private BlogCommentsService() 
        { }
    }
}
