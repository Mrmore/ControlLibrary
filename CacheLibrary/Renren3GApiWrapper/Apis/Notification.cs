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
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> AddNotification(string sessionKey, string secretKey, string channelUrl)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.AddWin8Token));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("token", channelUrl));
            parameters.Add(new RequestParameterEntity("pusher", "7"));
            //if (!string.IsNullOrEmpty(sub_uid))
            //{
            //    parameters.Add(new RequestParameterEntity("sub_user_id", sub_uid));
            //}

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        //public static string PULL_BASE_URL = "http://win8hd.renren.com/feed";
        //public static string PULL_BASE_URL = "http://mc1.test.renren.com/api/w/feed/get";
        //public static string PULL_BASE_URL = "http://mc2.test.renren.com/api/w/feed/get";
        public static string PULL_BASE_URL = "http://api.m.renren.com/api/w/feed/get";
        static public string GenerateAppTilePolledUrl(string sessionKey, string secretKey, int uid, int suid, string tstp, bool cmpt, string tmplt, string type)
        {
            int compute = cmpt ? 1 : 0;
            var apikey = ConstantValue.ApiKey;

            //IDeviceInfoHelper deviceHelper = new DeviceInfoAdaptor(DeviceInfoAdaptor.ImplType.NETWORKADAPTOR);
            //var duid = deviceHelper.GetDeviceID();

            //List<RequestParameterEntity> parameters = new List<RequestParameterEntity>();
            //parameters.Add(new RequestParameterEntity("duid", duid));
            //parameters.Add(new RequestParameterEntity("sessionkey", sessionKey));
            //parameters.Add(new RequestParameterEntity("uid", uid.ToString()));
            //parameters.Add(new RequestParameterEntity("suid", suid.ToString()));
            //parameters.Add(new RequestParameterEntity("tstp", tstp));
            //parameters.Add(new RequestParameterEntity("apikey", apikey));
            //parameters.Add(new RequestParameterEntity("cmpt", compute.ToString()));
            //parameters.Add(new RequestParameterEntity("tmplt", tmplt));
            //parameters.Add(new RequestParameterEntity("type", type));

            //string sig = ApiHelper.GenerateSig(parameters, secretKey);
            //string PolledUrlFormat = PULL_BASE_URL + "?duid={0}&sessionkey={1}&uid={2}&suid={3}&tstp={4}&apikey={5}&cmpt={6}&tmplt={7}&type={8}&sig={9}";
            //string url = string.Format(PolledUrlFormat, duid, sessionKey, uid, suid, tstp, apikey, compute, tmplt, type, sig);

            IDeviceInfoHelper deviceHelper = new DeviceInfoAdaptor(DeviceInfoAdaptor.ImplType.NETWORKADAPTOR);
            var clientInfo = deviceHelper.GetClientInfo().Result;
            var callid = ApiHelper.GenerateTime();

            List<RequestParameterEntity> parameters = new List<RequestParameterEntity>();
            parameters.Add(new RequestParameterEntity("api_key", apikey));
            parameters.Add(new RequestParameterEntity("call_id", callid));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("client_info", clientInfo));
            parameters.Add(new RequestParameterEntity("format", "txt"));

            if (suid != -1)
            {
                parameters.Add(new RequestParameterEntity("suid", suid.ToString()));
            }

            //parameters.Add(new RequestParameterEntity("tstp", tstp));
            parameters.Add(new RequestParameterEntity("cmpt", compute.ToString()));
            parameters.Add(new RequestParameterEntity("tmplt", tmplt));
            parameters.Add(new RequestParameterEntity("type", type));

            string sig = ApiHelper.GenerateSig(parameters, secretKey);
            string PolledUrlForma = string.Empty;
            string url = string.Empty;
            if (suid != -1)
            {
                string PolledUrlFormat = PULL_BASE_URL + "?api_key={0}&call_id={1}&session_key={2}&v={3}&client_info={4}&suid={5}&cmpt={6}&tmplt={7}&type={8}&sig={9}";
                url = string.Format(PolledUrlFormat, apikey, callid, sessionKey, "1.0", clientInfo, suid, compute, tmplt, type, sig);
            }
            else
            {
                string PolledUrlFormat = PULL_BASE_URL + "?api_key={0}&call_id={1}&session_key={2}&v={3}&client_info={4}&cmpt={5}&tmplt={6}&type={7}&sig={8}";
                url = string.Format(PolledUrlFormat, apikey, callid, sessionKey, "1.0", clientInfo, compute, tmplt, type, sig);
            }
            url += "&format=txt";

            // Local test url
            //url = "http://localhost:8080/apptile.xml";

            return url;
        }
    }
}
