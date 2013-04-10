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
        /// 获取当前用户好友列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<FriendListEntity>> GetFriendList(string sessionKey, string userSecretKey)
        {
            return await GetFriendList(sessionKey, userSecretKey, -1, -1);
        }
        /// <summary>
        /// 获取指定用户好友列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        static public async Task<RenrenAyncRespArgs<FriendListEntity>> GetFriendList(string sessionKey, string userSecretKey, int userId, int page, int pageSize = -1)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFriends));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("hasNetwork", "1"));
            parameters.Add(new RequestParameterEntity("hasGroup", "1"));
            parameters.Add(new RequestParameterEntity("hasGender", "1"));
            parameters.Add(new RequestParameterEntity("hasMainHeadUrl", "1"));
            parameters.Add(new RequestParameterEntity("hasLargeHeadUrl", "1"));

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("userId", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("pageSize", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<FriendListEntity, RenrenAyncRespArgs<FriendListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc1.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<FriendListEntity, RenrenAyncRespArgs<FriendListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取与指定好友的公共好友
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<CommonFriendsEntity>> GetCommonFriends(string sessionKey, string userSecretKey, int userId)
        {
            return await GetCommonFriends(sessionKey, userSecretKey, userId, -1, -1, 1, 1);
        }
        /// <summary>
        /// 获取与指定好友的公共好友（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="hasHeadImg"></param>
        /// <param name="hasNetwork"></param>
        static public async Task<RenrenAyncRespArgs<CommonFriendsEntity>> GetCommonFriends(string sessionKey, string userSecretKey, int userId, int page, int pageSize, int hasHeadImg, int hasNetwork)
        {
            //NOTE：文档错误
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetSharedFriends));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("userId", userId.ToString()));

            parameters.Add(new RequestParameterEntity("hasOnline", "1"));
            parameters.Add(new RequestParameterEntity("hasGroup", "1"));


            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("pageSize", pageSize.ToString()));
            if (hasHeadImg != -1)
                parameters.Add(new RequestParameterEntity("hasHeadImg", hasHeadImg.ToString()));
            if (hasNetwork != -1)
                parameters.Add(new RequestParameterEntity("hasNetwork", hasNetwork.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonFriendsEntity, RenrenAyncRespArgs<CommonFriendsEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 接受好友申请
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<AcceptFriendEntity>> AcceptFriendRequest(string sessionKey, string userSecretKey, int userId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.AcceptFriendRequest));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<AcceptFriendEntity, RenrenAyncRespArgs<AcceptFriendEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 忽略好友申请
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<DenyFriendEntity>> DenyFriendRequest(string sessionKey, string userSecretKey, int userId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.DenyFriendRequest));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<DenyFriendEntity, RenrenAyncRespArgs<DenyFriendEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取在线好友列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<FriendListEntity>> GetOnlineFriendList(string sessionKey, string userSecretKey)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetOnlineFriendList));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("isOnline", "1"));
            parameters.Add(new RequestParameterEntity("hasNetwork", "1"));
            parameters.Add(new RequestParameterEntity("hasGroup", "1"));
            parameters.Add(new RequestParameterEntity("hasGender", "1"));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<FriendListEntity, RenrenAyncRespArgs<FriendListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
        /// <summary>
        /// 判断是否为好友
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userIdOne"></param>
        /// <param name="userIdTwo"></param>
        static public async Task<RenrenAyncRespArgs<IsFriendResultEntity>> CheckIsFriend(string sessionKey, string userSecretKey, int userIdOne, int userIdTwo)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.CheckAreFriends));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("user_id_list_1", userIdOne.ToString()));
            parameters.Add(new RequestParameterEntity("user_id_list_2", userIdTwo.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<IsFriendResultEntity, RenrenAyncRespArgs<IsFriendResultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 好友申请
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> RequestFriend(string sessionKey, string userSecretKey, int userId, string content)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.RequestFriend));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (content != null)
                parameters.Add(new RequestParameterEntity("content", content));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        
    }
}
