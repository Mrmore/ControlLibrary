using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Apis;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.CacheService;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Framework;
using RenrenCoreWrapper.Framework.CacheService;
using RenrenCoreWrapper.Framework.RenrenService;
using RenrenCoreWrapper.Helper;

namespace RenrenCoreWrapper.RRServices
{
    public class AlbumListService : RRAbstractService<int, AlbumListEntity>
    {
        #region Data section
        CachServiceAdaptor<AlbumListEntity> _cacheImpl = new CachServiceAdaptor<AlbumListEntity>(CacheServiceType.SERVICE);
        const int ExpirationTimeDay = 10;
        #endregion

        #region Convient functions
        public async Task<RenrenAyncRespArgs<AlbumListEntity>> RequestAlbumListByUid(int uid = -1, int page = -1, int pageSize = -1, bool needOffline = false, bool forceRequest = false)
        {
            this.NeedOfflineData = needOffline;
            this.ForceDataRequest = forceRequest;

            int uidAdaptor = uid;
            if (uid == -1)
                uidAdaptor = this.LoginInfo.LoginInfo.Uid;

            RenrenAyncRespArgs<AlbumListEntity> resp = (RenrenAyncRespArgs<AlbumListEntity>)(await this.Invoke(ServiceRole.SelfInvoke, uid, page, pageSize));
            return resp;
        }
        #endregion

        #region Comes from RRAbstractService
        protected override Task<RenrenAyncRespArgs<AlbumListEntity>> DoRequest(params object[] args)
        {
            int uid = (int)args[0];
            int page = (int)args[1];
            int pageSize = (int)args[2];

            // TODO: download the data and return through 3g api
            var resp = Renren3GApiWrapper.GetAlbumList(this.LoginInfo.LoginInfo.Session_key, this.LoginInfo.LoginInfo.Secret_key, uid, page, pageSize);
            return resp;
        }

        private string generateHashKey(int uid, int page, int pageSize)
        {
            string sample = typeof(AlbumListService).ToString() + "_" + uid + page + pageSize;
            string key = ApiHelper.ComputeMD5(sample);
            return key;
        }

        protected override async Task<RenrenAyncRespArgs<AlbumListEntity>> DoOfflineDataRequest(params object[] args)
        {
            int uid = (int)args[0];
            int page = (int)args[1];
            int pageSize = (int)args[2];

            // TODO:
            // 创建相应的缓存片段
            // 查看缓存是否有效
            // 进行缓存逻辑
            var hashKey = generateHashKey(uid, page, pageSize);
            DateTime expTime = DateTime.Now.AddDays(ExpirationTimeDay);
            ICacheChip<AlbumListEntity> chip = _cacheImpl.CreateCacheChip(hashKey, expTime);
            var valid = _cacheImpl.IsValid(chip);
            if (valid && !this.ForceDataRequest)
            {
                AlbumListEntity content = await _cacheImpl.Pick(chip);
                return new RenrenAyncRespArgs<AlbumListEntity>(content);
            }
            else
            {
                var resp = await Renren3GApiWrapper.GetAlbumList(this.LoginInfo.LoginInfo.Session_key, this.LoginInfo.LoginInfo.Secret_key, uid, page, pageSize);
                if (resp.Result != null)
                {
                    await _cacheImpl.Add(chip, resp.Result);
                }
                return resp;
            }
        }

        protected override Task<bool> DoServiceInit(params object[] args)
        {
            return this._cacheImpl.Init(typeof(AlbumListService).Name);
        }

        protected override async Task DoServiceReset()
        {
            await _cacheImpl.Reset();
        }

        protected override void DoResetById(int id)
        {
            return;
        }

        protected override void DoResetByIdnPage(int id, int page)
        {
            return;
        }

        protected override ServiceCatalogue GetCatalogue()
        {
            throw new NotImplementedException();
        }

        protected override string GetUniqueID()
        {
            throw new NotImplementedException();
        }

        protected override ServiceType GetType()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
