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
        /// 获取指定用户留言板
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<GossipListEntity>> GetGossips(string sessionKey, string userSecretKey, int userId)
        {
            return await GetGossips(sessionKey, userSecretKey, userId, -1, -1);
        }
        /// <summary>
        /// 获取指定用户留言板（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        static public async Task<RenrenAyncRespArgs<GossipListEntity>> GetGossips(string sessionKey, string userSecretKey, int userId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetGossips));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<GossipListEntity, RenrenAyncRespArgs<GossipListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 给指定好友留言
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostGossip(string sessionKey, string userSecretKey, int userId, string content)
        {
            return await PostGossip(sessionKey, userSecretKey, userId, content, -1, -1);
        }
        /// <summary>
        /// 给指定好友留言[悄悄话]
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostGossipByWhisper(string sessionKey, string userSecretKey, int userId, string content, int isWhisper)
        {
            return await PostGossip(sessionKey, userSecretKey, userId, content, -1, isWhisper);
        }
        /// <summary>
        /// 在指定留言板回复指定好友留言
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="reUserId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostGossip(string sessionKey, string userSecretKey, int userId, string content, int reUserId)
        {
            return await PostGossip(sessionKey, userSecretKey, userId, content, reUserId, -1);
        }
        /// <summary>
        /// 在指定留言板回复指定好友留言
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="reUserId"></param>
        /// <param name="isWhisper">输入值为1表示悄悄话</param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostGossip(string sessionKey, string userSecretKey, int userId, string content, int reUserId, int isWhisper)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostGossip));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("userId", userId.ToString()));
            parameters.Add(new RequestParameterEntity("content", content));
            if (reUserId != -1)
                parameters.Add(new RequestParameterEntity("reUserId", reUserId.ToString()));
            if (isWhisper != -1)
                parameters.Add(new RequestParameterEntity("isWhisper", isWhisper.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
