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
    public class NewsFeedService : RRAbstractService<int, FeedListEntity>
    {
        #region Data section
        CachServiceAdaptor<FeedListEntity> _cacheImpl = new CachServiceAdaptor<FeedListEntity>(CacheServiceType.SERVICE);
        const int ExpirationTimeDay = 10;
        #endregion

        #region Convient functions
        public async Task<RenrenAyncRespArgs<FeedListEntity>> RequestFeedListByUid(int uid = -1, int page = -1, int pageSize = -1, bool needOffline = false, bool forceRequest = false)
        {
            this.NeedOfflineData = needOffline;
            this.ForceDataRequest = forceRequest;

            //int uidAdaptor = uid;
            //if (uid == -1)
            //    uidAdaptor = this.LoginInfo.LoginInfo.Uid;

            RenrenAyncRespArgs<FeedListEntity> resp = (RenrenAyncRespArgs<FeedListEntity>)(await this.Invoke(ServiceRole.SelfInvoke, uid, page, pageSize));
            return resp;
        }
        #endregion

        #region Comes from RRAbstractService
        protected override Task<RenrenAyncRespArgs<FeedListEntity>> DoRequest(params object[] args)
        {
            int uid = (int)args[0];
            int page = (int)args[1];
            int pageSize = (int)args[2];

            // TODO: download the data and return through 3g api
            var resp = Renren3GApiWrapper.GetFeedList(this.LoginInfo.LoginInfo.Session_key, this.LoginInfo.LoginInfo.Secret_key, page, pageSize,uid);
            return resp;
        }

        private string generateHashKey(int uid, int page, int pageSize)
        {
            string sample = typeof(NewsFeedService).ToString() + "_" + uid + page + pageSize;
            string key = ApiHelper.ComputeMD5(sample);
            return key;
        }

        protected override async Task<RenrenAyncRespArgs<FeedListEntity>> DoOfflineDataRequest(params object[] args)
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
            ICacheChip<FeedListEntity> chip = _cacheImpl.CreateCacheChip(hashKey, expTime);
            var valid = _cacheImpl.IsValid(chip);
            if (valid && !this.ForceDataRequest)
            {
                FeedListEntity content = await _cacheImpl.Pick(chip);
                return new RenrenAyncRespArgs<FeedListEntity>(content);
            }
            else
            {
                var resp = await Renren3GApiWrapper.GetFeedList(this.LoginInfo.LoginInfo.Session_key, this.LoginInfo.LoginInfo.Secret_key, page, pageSize, uid);
                if (resp.Result != null)
                {
                    await _cacheImpl.Add(chip, resp.Result);
                }
                return resp;
            }
        }

        protected override Task<bool> DoServiceInit(params object[] args)
        {
            return this._cacheImpl.Init(typeof(NewsFeedService).Name);
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
