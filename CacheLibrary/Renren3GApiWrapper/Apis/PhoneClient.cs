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
        /// 获取客户端的升级信息
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="phoneModel">手机型号</param>
        /// <param name="phoneOS">手机操作系统</param>
        /// <param name="pubDate">当前客户端的发布日期（打包时间 格式：20090909）</param>
        public async Task<RenrenAyncRespArgs<ClientUpdateInfoEntity>> GetClientUpdateInfo(string sessionKey, string userSecretKey, string phoneModel, string phoneOS, int pubDate, int up)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetUpldateInfo));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("name", "0"));
            //parameters.Add(new RequestParameterEntity("property", "8"));
            parameters.Add(new RequestParameterEntity("property", "10"));
            parameters.Add(new RequestParameterEntity("subproperty", "0"));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));
            parameters.Add(new RequestParameterEntity("channelId", "9100301"));
            parameters.Add(new RequestParameterEntity("ua", phoneModel));
            parameters.Add(new RequestParameterEntity("os", phoneOS));
            parameters.Add(new RequestParameterEntity("pubdate", pubDate.ToString()));
            parameters.Add(new RequestParameterEntity("up", up.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<ClientUpdateInfoEntity, RenrenAyncRespArgs<ClientUpdateInfoEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
