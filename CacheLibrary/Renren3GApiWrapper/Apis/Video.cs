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
        static public async Task<RenrenAyncRespArgs<VideoGetEntity>> VideoGet(string sessionKey, string secretKey, string videoUrl)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.VideoGet));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("video_url", videoUrl));
            parameters.Add(new RequestParameterEntity("platform", "9"));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<VideoGetEntity, RenrenAyncRespArgs<VideoGetEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<V56GetVideoEntity>> V56GetVideo(string sessionKey, string secretKey, string flvid)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.V56GetVideo));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("flvid", flvid));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<V56GetVideoEntity, RenrenAyncRespArgs<V56GetVideoEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
