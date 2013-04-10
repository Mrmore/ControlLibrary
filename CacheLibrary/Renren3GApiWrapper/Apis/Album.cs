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
        /// 获取相册 
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="aid"></param>
        /// <param name="exclude_list"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<AlbumListEntity>> GetAlbumList(string sessionKey, string userSecretKey, int userId, long aid, int exclude_list, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetAlbums));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("exclude_list", exclude_list.ToString()));
            parameters.Add(new RequestParameterEntity("aid ", aid.ToString()));

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<AlbumListEntity, RenrenAyncRespArgs<AlbumListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定用户相册列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<AlbumListEntity>> GetAlbumList(string sessionKey, string userSecretKey, int userId)
        {
            return await GetAlbumList(sessionKey, userSecretKey, userId, -1, -1);
        }

        /// <summary>
        /// 获取指定用户相册列表 可扩展
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        static public async Task<RenrenAyncRespArgs<AlbumListEntity>> GetAlbumList(string sessionKey, string userSecretKey, int userId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetAlbums));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("all_album", "1"));

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<AlbumListEntity, RenrenAyncRespArgs<AlbumListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc2.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<AlbumListEntity, RenrenAyncRespArgs<AlbumListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定用户相册列表 可扩展
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        static public async Task<RenrenAyncRespArgs<AlbumListEntity>> GetAlbumList(string sessionKey, string userSecretKey, int userId,int without_head, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetAlbums));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("all_album", "1"));

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (without_head != -1)
                parameters.Add(new RequestParameterEntity("without_head", without_head.ToString()));
            

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            //var result = await agentReponseHandler<AlbumListEntity, RenrenAyncRespArgs<AlbumListEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://mc2.test.renren.com/api", UriKind.Absolute));
            var result = await agentReponseHandler<AlbumListEntity, RenrenAyncRespArgs<AlbumListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 发表相册评论
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="albumId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostAlbumComment(string sessionKey, string userSecretKey, int onwerId, long albumId, string content)
        {
            return await PostAlbumComment(sessionKey, userSecretKey, onwerId, albumId, content, -1, -1);
        }
        /// <summary>
        /// 发表相册评论（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="albumId"></param>
        /// <param name="content"></param>
        /// <param name="replayUserId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostAlbumComment(string sessionKey, string userSecretKey, int onwerId, long albumId, string content, int replayUserId, int isWhisper)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostPhotoComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uid", onwerId.ToString()));
            parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            parameters.Add(new RequestParameterEntity("content", content));
            if (replayUserId != -1)
                parameters.Add(new RequestParameterEntity("rid", replayUserId.ToString()));
            if (isWhisper != -1)
                parameters.Add(new RequestParameterEntity("whisper", isWhisper.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 新建相册
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="name"></param>
        /// <param name="visible"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<CreateNewAlbumEntity>> CreateNewAlbum(string sessionKey, string secretKey, string name, string password = null, int visible = 99)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("method", Method.CreateAlbum));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("name", name));

            if (visible != 99)
            {
                parameters.Add(new RequestParameterEntity("visible", visible.ToString()));
            }

            if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
            {
                parameters.Add(new RequestParameterEntity("password", password));
            }

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CreateNewAlbumEntity, RenrenAyncRespArgs<CreateNewAlbumEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
