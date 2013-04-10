using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DataLayerWrapper.Http;
using System.Runtime.Serialization.Json;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Core;
using System.Reflection;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;
using RenrenCoreWrapper.Apis;

namespace RenrenCoreWrapper.Apis
{
    public partial class Renren3GApiWrapper
    {
        /// <summary>
        /// 获取登录用户新鲜事
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedList(string sessionKey, string userSecretKey)
        {
            return await GetFeedList(sessionKey, userSecretKey, -1, -1, -1);
        }
        /// <summary>
        /// 获取指定用户新鲜事
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedList(string sessionKey, string userSecretKey, int userId)
        {
            return await GetFeedList(sessionKey, userSecretKey, -1, -1, userId);
        }
        /// <summary>
        /// 获取新鲜事（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedList(string sessionKey, string userSecretKey, int page, int pageSize, int userId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFeed));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("type", "102,103,104,110,502,601,701,709,107,2003,2004,2005,2006,2008,2009,2012,2013"));
            parameters.Add(new RequestParameterEntity("has_at_id", "1"));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc2.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedListByTypes(string sessionKey, string userSecretKey, int page, int pageSize, int userId, params long[] types)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFeed));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("has_at_id", "1"));

            if (types.Length > 0)
            {
                string type = string.Empty;
                foreach (var item in types)
                {
                    type += item + ",";
                }
                type.TrimEnd(',');
                parameters.Add(new RequestParameterEntity("type", type));
            }

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc2.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedListByTypes(string sessionKey, string userSecretKey, int page, int pageSize, int userId, ICollection<int> types)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFeed));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("has_at_id", "1"));

            if (types.Count> 0)
            {
                string type = string.Empty;
                foreach (var item in types)
                {
                    type += item + ",";
                }
                type.TrimEnd(',');
                parameters.Add(new RequestParameterEntity("type", type));
            }

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc2.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 据新鲜事id列表获取相应的新鲜事列表 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="ids">新鲜事id </param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<FeedListEntity>> GetFeedByIds(string sessionKey, string secretKey, params long[] ids)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFeedByIds));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (ids.Length > 0)
            {
                string fids = string.Empty;
                foreach (var item in ids)
                {
                    fids += item + ",";
                }
                fids.TrimEnd(',');
                parameters.Add(new RequestParameterEntity("fids", fids));
            }


            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<FeedListEntity, RenrenAyncRespArgs<FeedListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
