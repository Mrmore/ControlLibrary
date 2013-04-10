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
    class BlogService : RenrenAbstractService<long, BlogEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static BlogService Instance
        {
            get { return _instance; }
        }
        private static readonly BlogService _instance = new BlogService();

        /// <summary>
        /// Request and update the blog content by blog id
        /// The wrapped convient method provided for outside call
        /// \note: if the uid is null means requesting self friends list
        /// </summary>
        /// <param name="uid">user id</param>
        /// <param name="page">page count</param>
        /// <returns></returns>
        public async Task<RenrenAyncRespArgs<BlogEntity>> RequestBlogByUid(int uid, long blogId, string password)
        {
            RenrenAyncRespArgs<BlogEntity> resp = await RequestById(blogId, uid, password);
            return resp;
        }

        /// <summary>
        /// Request and update the self blog content
        /// </summary>
        /// <returns>the async result</returns>
        protected override Task<RenrenAyncRespArgs<BlogEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Request and update the blog content
        /// </summary>
        /// <returns>the async result</returns>
        protected async override Task<RenrenAyncRespArgs<BlogEntity>> DoRequestById(long id, params object[] args)
        {
            if (args.Length < 1) throw new ArgumentException();
            int uid = (int)args[0];
            string password = args.Length > 1 ? (string)args[1] : string.Empty; ;
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;

            RenrenAyncRespArgs<BlogEntity> resp = await Renren3GApiWrapper.GetBlog(seesionKey, secrectKey, uid, id, password);
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
        private BlogService() 
        { }
    }
}
