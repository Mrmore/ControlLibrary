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
    /// id means the video url
    /// </summary>
    class VideoService : RenrenAbstractService<string, VideoGetEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static VideoService Instance
        {
            get { return _instance; }
        }
        private static readonly VideoService _instance = new VideoService();

        public async Task<RenrenAyncRespArgs<VideoGetEntity>> RequestVideoByUrl(string url)
        {
            RenrenAyncRespArgs<VideoGetEntity> resp = await RequestById(url);
            return resp;
        }

        protected override Task<RenrenAyncRespArgs<VideoGetEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected async override Task<RenrenAyncRespArgs<VideoGetEntity>> DoRequestById(string url, params object[] args)
        {
            string videoUrl = url;
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;
            RenrenAyncRespArgs<VideoGetEntity> resp = await Renren3GApiWrapper.VideoGet(seesionKey, secrectKey, videoUrl);
            return resp;
        }

        /// <summary>
        /// Reset overall models
        /// </summary>
        protected override void DoReset()
        {
        }

        protected override void DoResetById(string url)
        {
        }

        /// <summary>
        /// private construct to protected against the outside create
        /// </summary>
        private VideoService() 
        { }
    }
}
