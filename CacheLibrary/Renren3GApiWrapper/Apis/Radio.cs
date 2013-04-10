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
        static public async Task<RenrenAyncRespArgs<GetHomeEntity>> RadioGetHome(string sessionKey, string secretKey, string uuid, string quality, string resolution)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.RadioGetHome));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("quality", quality));
            parameters.Add(new RequestParameterEntity("resolution", resolution));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            if (!string.IsNullOrEmpty(sessionKey))
                parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (string.IsNullOrEmpty(secretKey))
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, ConstantValue.SecretKey)));
            else
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<GetHomeEntity, RenrenAyncRespArgs<GetHomeEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<SongListEntity>> GetRadio(string sessionKey, string secretKey, string uuid, uint radioId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetRadio));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("radioId", radioId.ToString()));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            if (!string.IsNullOrEmpty(sessionKey))
                parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (string.IsNullOrEmpty(secretKey))
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, ConstantValue.SecretKey)));
            else
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<SongListEntity, RenrenAyncRespArgs<SongListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<SongEntity>> GetNextSong(string sessionKey, string secretKey, string uuid, uint radioId, uint songId, uint duration)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetNextSong));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("radioId", radioId.ToString()));
            parameters.Add(new RequestParameterEntity("songId", songId.ToString()));
            parameters.Add(new RequestParameterEntity("duration", duration.ToString()));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            if (!string.IsNullOrEmpty(sessionKey))
                parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (string.IsNullOrEmpty(secretKey))
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, ConstantValue.SecretKey)));
            else
                parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<SongEntity, RenrenAyncRespArgs<SongEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> RadioAddFavorite(string sessionKey, string secretKey, string uuid, uint songId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.RadioAddFavorite));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("songId", songId.ToString()));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> RadioRemoveFavorite(string sessionKey, string secretKey, string uuid, uint songId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.RadioRemoveFavorite));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("songId", songId.ToString()));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> RadioShareSong(string sessionKey, string secretKey, string uuid, uint songId, string comment)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.RadioShareSong));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uuid", uuid));
            parameters.Add(new RequestParameterEntity("songId", songId.ToString()));
            parameters.Add(new RequestParameterEntity("comment", comment));
            parameters.Add(new RequestParameterEntity("version", "1.0.1"));

            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
