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
        static public async Task<RenrenAyncRespArgs<PoiListEntity>> GetPoiList(string sessionKey, string secretKey, long lat, long lon, int radius, int d, int page, int page_size)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPoilist));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("lon_gps", lon.ToString()));
            parameters.Add(new RequestParameterEntity("lat_gps", lat.ToString()));
            parameters.Add(new RequestParameterEntity("radius", radius.ToString()));
            parameters.Add(new RequestParameterEntity("d", d.ToString()));
            parameters.Add(new RequestParameterEntity("page", page.ToString()));
            parameters.Add(new RequestParameterEntity("page_size", page_size.ToString()));


            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<PoiListEntity, RenrenAyncRespArgs<PoiListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
