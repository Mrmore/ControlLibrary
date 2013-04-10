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
        static public async Task<RenrenAyncRespArgs<FriendListEntity>> SearchFriends(string sessionKey, string secretKey, string keyWord, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.FriendsSearch));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("name", keyWord));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("hasSharedFriendsCount", "1"));
            parameters.Add(new RequestParameterEntity("hasIsFriend", "1"));
            parameters.Add(new RequestParameterEntity("hasNetwork", "1"));

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("pageSize", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<FriendListEntity, RenrenAyncRespArgs<FriendListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<PageListEntity>> SearchPages(string sessionKey, string secretKey, string keyWord, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PagesSearch));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("keyword", keyWord));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<PageListEntity, RenrenAyncRespArgs<PageListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
