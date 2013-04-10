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
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        static public async Task<RenrenAyncRespArgs<UserEntity>> LogIn(string username, string password)
        {
            password = MD5Core.GetHashString(password).ToLower();
            IDeviceInfoHelper deviceHelper = new DeviceInfoAdaptor(DeviceInfoAdaptor.ImplType.NETWORKADAPTOR);
            var deviceId = deviceHelper.GetDeviceID();

            var parameters = ApiHelper.GetBaseParameters(null).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            //parameters.Add(new RequestParameterEntity("method", Method.LogIn));
            parameters.Add(new RequestParameterEntity("uniq_id", deviceId));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("password", password));
            parameters.Add(new RequestParameterEntity("user", username));

            string sig = ApiHelper.GenerateSig(parameters, ConstantValue.SecretKey);
            //if (sig.Length > 50)
            //    sig = sig.Substring(0, 50);
            parameters.Add(new RequestParameterEntity("sig", sig));

            var result = await agentReponseHandler<UserEntity, RenrenAyncRespArgs<UserEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api/client/login", UriKind.Absolute));
            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> LogOut(string sessionKey, string secretKey)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("session_id", sessionKey));
            parameters.Add(new RequestParameterEntity("push_type", "7"));

            string sig = ApiHelper.GenerateSig(parameters, secretKey);
            //if (sig.Length > 50)
            //    sig = sig.Substring(0, 50);
            parameters.Add(new RequestParameterEntity("sig", sig));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api/client/logout", UriKind.Absolute));
            return result;
        }
    }
}
