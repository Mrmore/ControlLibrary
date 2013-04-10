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
        //todo:注意BoxType属性 默认不设置的情况下得到的是发件箱内容
        /// <summary>
        /// 获取站内信
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<MessageListEntity>> GetMessages(string sessionKey, string userSecretKey)
        {
            return await GetMessages(sessionKey, userSecretKey, -1, -1, -1, -1, -1);
        }
        /// <summary>
        /// 获取站内信（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<MessageListEntity>> GetMessages(string sessionKey, string userSecretKey, int boxType, int page, int count, int excludeList, int delNews)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetMessages));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (boxType != -1)
                parameters.Add(new RequestParameterEntity("box", boxType.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (count != -1)
                parameters.Add(new RequestParameterEntity("count", count.ToString()));
            if (excludeList != -1)
                parameters.Add(new RequestParameterEntity("exclude_list", count.ToString()));
            if (delNews != -1)
                parameters.Add(new RequestParameterEntity("del_news", count.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<MessageListEntity, RenrenAyncRespArgs<MessageListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
