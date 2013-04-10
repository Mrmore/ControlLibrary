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

namespace RenrenCoreWrapper.Apis
{
    public partial class Renren3GApiWrapper
    {
        /// <summary>
        /// 获取登录用户当前状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<StatusEntity>> GetStatus(string sessionKey, string userSecretKey)
        {
            return await GetStatus(sessionKey, userSecretKey, -1, -1);
        }
        /// <summary>
        /// 获取指定用户最近状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<StatusEntity>> GetStatus(string sessionKey, string userSecretKey, int userId)
        {
            return await GetStatus(sessionKey, userSecretKey, userId, -1);
        }
        /// <summary>
        /// 获取指定用户指定状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="statusId"></param>
        static public async Task<RenrenAyncRespArgs<StatusEntity>> GetStatus(string sessionKey, string userSecretKey, int userId, long statusId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetStatus));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (statusId != -1)
                parameters.Add(new RequestParameterEntity("id", statusId.ToString()));
            string sig = ApiHelper.GenerateSig(parameters, userSecretKey);
            parameters.Add(new RequestParameterEntity("sig", sig));

            var result = await agentReponseHandler<StatusEntity, RenrenAyncRespArgs<StatusEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取登录用户信息列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<StatusListEntity>> GetStatusList(string sessionKey, string userSecretKey)
        {
            return await GetStatusList(sessionKey, userSecretKey, -1, -1, -1);
        }
        /// <summary>
        /// 获取指定用户信息列表（支持扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        static public async Task<RenrenAyncRespArgs<StatusListEntity>> GetStatusList(string sessionKey, string userSecretKey, int userId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetStatusList));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<StatusListEntity, RenrenAyncRespArgs<StatusListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定状态的评论列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="statusId"></param>
        /// <param name="ownerId"></param>
        static public async Task<RenrenAyncRespArgs<StatusCommentsEntity>> GetStatusComments(string sessionKey, string userSecretKey, long statusId, int ownerId)
        {
            return await GetStatusComments(sessionKey, userSecretKey, statusId, ownerId, -1, -1, -1, -1);
        }

        static public async Task<RenrenAyncRespArgs<StatusCommentsEntity>> GetStatusComments(string sessionKey, string userSecretKey, long statusId, int ownerId, int page)
        {
            return await GetStatusComments(sessionKey, userSecretKey, statusId, ownerId, page, -1, -1, -1);
        }
        /// <summary>
        /// 获取指定状态的评论列表（支持扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="statusId"></param>
        /// <param name="ownerId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="withStatusDetail"></param>
        /// <param name="needSort"></param>
        static public async Task<RenrenAyncRespArgs<StatusCommentsEntity>> GetStatusComments(string sessionKey, string userSecretKey, long statusId, int ownerId, int page, int pageSize, int withStatusDetail, int needSort)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetStatusComments));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("id", statusId.ToString()));
            parameters.Add(new RequestParameterEntity("owner_id", ownerId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (withStatusDetail != -1)
                parameters.Add(new RequestParameterEntity("with_status", pageSize.ToString()));
            if (needSort != -1)
                parameters.Add(new RequestParameterEntity("sort", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<StatusCommentsEntity, RenrenAyncRespArgs<StatusCommentsEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 发布新状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="status"></param>
        static public async Task<RenrenAyncRespArgs<SetStatusEntity>> SetStatus(string sessionKey, string userSecretKey, string status, string place_data = null)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.SetStatus));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("status", status));

            if (!string.IsNullOrEmpty(place_data))
            {
                parameters.Add(new RequestParameterEntity("place_data", place_data));
            }

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<SetStatusEntity, RenrenAyncRespArgs<SetStatusEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 转发状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="status"></param>
        /// <param name="statusId"></param>
        /// <param name="ownerId"></param>
        static public async Task<RenrenAyncRespArgs<ForwardStatusEntity>> ForwardStatus(string sessionKey, string userSecretKey, string status, long statusId, int ownerId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.ForwardStatus));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("status", status));
            parameters.Add(new RequestParameterEntity("forward_doing_id", statusId.ToString()));
            parameters.Add(new RequestParameterEntity("forward_owner_id", ownerId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<ForwardStatusEntity, RenrenAyncRespArgs<ForwardStatusEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 评论状态
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="content"></param>
        /// <param name="statusId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostStatusComment(string sessionKey, string userSecretKey, int onwerId, string content, long statusId)
        {
            return await PostStatusComment(sessionKey, userSecretKey, onwerId, content, statusId, -1);
        }
        /// <summary>
        /// 评论状态（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="content"></param>
        /// <param name="statusId"></param>
        /// <param name="replayUserId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostStatusComment(string sessionKey, string userSecretKey, int onwerId, string content, long statusId, int replayUserId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostStatusComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("owner_id", onwerId.ToString()));
            parameters.Add(new RequestParameterEntity("content", content));
            parameters.Add(new RequestParameterEntity("status_id", statusId.ToString()));
            if (replayUserId != -1)
                parameters.Add(new RequestParameterEntity("rid", replayUserId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
