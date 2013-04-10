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
    class PhotoService : RenrenAbstractService<long, PhotoEntity>
    {
        /// <summary>
        /// Singleton property
        /// </summary>
        public static PhotoService Instance
        {
            get { return _instance; }
        }
        private static readonly PhotoService _instance = new PhotoService();

        public async Task<RenrenAyncRespArgs<PhotoEntity>> RequestPhotoByPid(int userId, long photoId, string password)
        {
            RenrenAyncRespArgs<PhotoEntity> resp = await RequestById(photoId, userId, password);
            return resp;
        }

        protected override Task<RenrenAyncRespArgs<PhotoEntity>> DoRequest(params object[] args)
        {
            throw new NotImplementedException();
        }

        protected async override Task<RenrenAyncRespArgs<PhotoEntity>> DoRequestById(long id, params object[] args)
        {
            if (args.Length < 1) throw new ArgumentException();
            int uid = (int)args[0];
            string password = args.Length > 1 ? (string)args[1] : string.Empty;
            string seesionKey = LoginService.Instance.Model.Session_key;
            string secrectKey = LoginService.Instance.Model.Secret_key;

            RenrenAyncRespArgs<PhotoEntity> resp = await Renren3GApiWrapper.GetPhoto(seesionKey, secrectKey, uid, id, password);
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
        private PhotoService() 
        { }
    }
}
