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
        /// 获取登录用户账户信息
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<UserInfoEntity>> GetUserInfo(string sessionKey, string userSecretKey)
        {
            return await GetUserInfo(sessionKey, userSecretKey, -1);
        }
        /// <summary>
        /// 获取指定用户账户信息
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<UserInfoEntity>> GetUserInfo(string sessionKey, string userSecretKey, int userId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetUserInfo));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("type", "8191"));//根据Type生成的值 获取用户所有信息

            int head_url_switch = 1 + 2 + 4 + 8 + 16;
            parameters.Add(new RequestParameterEntity("head_url_switch", head_url_switch.ToString()));//Fetch the user head urls

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<UserInfoEntity, RenrenAyncRespArgs<UserInfoEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取登录用户最近来访
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<VisitorsEntity>> GetVisitorList(string sessionKey, string userSecretKey, int page, int pageSize)
        {
            return await GetVisitorList(sessionKey, userSecretKey, -1, page, pageSize);
        }
        /// <summary>
        /// 获取指定用户最近来访
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<VisitorsEntity>> GetVisitorList(string sessionKey, string userSecretKey, int userId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetVisitors));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            int head_url_switch = 1 + 2 + 4 + 8 + 16;
            parameters.Add(new RequestParameterEntity("head_url_switch", head_url_switch.ToString()));//Fetch the user head urls

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<VisitorsEntity, RenrenAyncRespArgs<VisitorsEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
