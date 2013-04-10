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
        static public async Task<RenrenAyncRespArgs<AtFriendsEntity>> GetAtFriends(string sessionKey, string secretKey, string keyWord, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.AtGetFriends));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("key_word", keyWord));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<AtFriendsEntity, RenrenAyncRespArgs<AtFriendsEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取频繁@的好友列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<AtFriendsEntity>> GetFrequentAtFriends(string sessionKey, string secretKey, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.FrequentAtFriends));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));

            if (pageSize == -1)
            {
                pageSize = 6;
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            }
            else
            {
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            }
                
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));
            var result = await agentReponseHandler<AtFriendsEntity, RenrenAyncRespArgs<AtFriendsEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
