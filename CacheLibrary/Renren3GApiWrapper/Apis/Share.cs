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
        static public async Task<RenrenAyncRespArgs<ShareCommentEntity>> GetShareCommentList(string sessionKey, string userSecretKey, long shareId, int shareOwnerId)
        {
            return await GetShareCommentList(sessionKey, userSecretKey, shareId, shareOwnerId, 1, 10);
        }

        static public async Task<RenrenAyncRespArgs<ShareCommentEntity>> GetShareCommentList(string sessionKey, string userSecretKey, long shareId, int shareOwnerId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetShareComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("id", shareId.ToString()));
            parameters.Add(new RequestParameterEntity("user_id", shareOwnerId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<ShareCommentEntity, RenrenAyncRespArgs<ShareCommentEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定分享
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<ShareEntity>> GetTheShare(string sessionKey, string userSecretKey, int userId, long shareId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetTheShare));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (shareId != -1)
                parameters.Add(new RequestParameterEntity("id", shareId.ToString()));
            parameters.Add(new RequestParameterEntity("with_source", "1"));
            parameters.Add(new RequestParameterEntity("need_html", "1"));
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<ShareEntity, RenrenAyncRespArgs<ShareEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取分享列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<ShareListEntity>> GetShareList(string sessionKey, string userSecretKey)
        {
            return await GetShareList(sessionKey, userSecretKey, -1, -1, -1);
        }
        /// <summary>
        /// 获取分享列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<ShareListEntity>> GetShareList(string sessionKey, string userSecretKey, int userId)
        {
            return await GetShareList(sessionKey, userSecretKey, userId, -1, -1);
        }
        /// <summary>
        /// 获取分享列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<ShareListEntity>> GetShareList(string sessionKey, string userSecretKey, int userId, int page, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetShareList));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<ShareListEntity, RenrenAyncRespArgs<ShareListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 发布分享（针对链接）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="sourceType"></param>
        /// <param name="url"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostShare(string sessionKey, string userSecretKey, int sourceType, string url)
        {
            return await PostShare(sessionKey, userSecretKey, sourceType, -1, -1, url, null);
        }
        /// <summary>
        /// 发布分享
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="sourceType"></param>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostShare(string sessionKey, string userSecretKey, int sourceType, long id, int ownerId)
        {
            return await PostShare(sessionKey, userSecretKey, sourceType, id, ownerId, null, null);
        }
        /// <summary>
        /// 发布分享(可扩展)
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="sourceType"></param>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostShare(string sessionKey, string userSecretKey, int sourceType, long id, int ownerId, string url, string comment)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostShare));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("source_type", sourceType.ToString()));
            //parameters.Add(new RequestParameterEntity("type", "0"));//表示分享 默认为0
            if (id != -1)
                parameters.Add(new RequestParameterEntity("id", id.ToString()));
            if (ownerId != -1)
                parameters.Add(new RequestParameterEntity("uid", ownerId.ToString()));
            if (url != null)
                parameters.Add(new RequestParameterEntity("url", url));
            if (!string.IsNullOrEmpty(comment))
                parameters.Add(new RequestParameterEntity("comment", comment));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PublishLink(string sessionKey, string userSecretKey, string url, string thumbUrl, string title, string description, int fromId, string comment)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PublishLink));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("url", url));

            if (!string.IsNullOrEmpty(thumbUrl))
                parameters.Add(new RequestParameterEntity("thumb_url", thumbUrl));
            if (!string.IsNullOrEmpty(title))
                parameters.Add(new RequestParameterEntity("title", title));
            if (!string.IsNullOrEmpty(description))
                parameters.Add(new RequestParameterEntity("desc", description));
            if (fromId != -1)
                parameters.Add(new RequestParameterEntity("from", fromId.ToString()));
            if (!string.IsNullOrEmpty(comment))
                parameters.Add(new RequestParameterEntity("comment", comment));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        ///发布收藏 （可在PostShare里扩展这里为了不影响之前的逻辑先这么写会有冗余代码暂时这样吧）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="sourceType"></param>
        /// <param name="id"></param>
        /// <param name="ownerId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostCollect(string sessionKey, string userSecretKey, int sourceType, long id, int ownerId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostShare));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("source_type", sourceType.ToString()));
            parameters.Add(new RequestParameterEntity("type", "1"));

            //parameters.Add(new RequestParameterEntity("type", "0"));//表示分享 默认为0
            if (id != -1)
                parameters.Add(new RequestParameterEntity("id", id.ToString()));
            if (ownerId != -1)
                parameters.Add(new RequestParameterEntity("uid", ownerId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 评论分享
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="shareId"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostShareComment(string sessionKey, string userSecretKey, long shareId, int userId, string content)
        {
            return await PostShareComment(sessionKey, userSecretKey, shareId, userId, content, -1);
        }
        /// <summary>
        /// 评论分享 可扩展
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="shareId"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="reUserid"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostShareComment(string sessionKey, string userSecretKey, long shareId, int userId, string content, int reUserid)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostShareComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("id", shareId.ToString()));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));
            parameters.Add(new RequestParameterEntity("content", content));

            if (reUserid != -1)
                parameters.Add(new RequestParameterEntity("rid", reUserid.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
