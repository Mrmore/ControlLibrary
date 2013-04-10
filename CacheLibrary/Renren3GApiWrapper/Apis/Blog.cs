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
        /// 获取指定用户的日志列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<BlogListEntity>> GetBlogList(string sessionKey, string userSecretKey, int userId)
        {
            return await GetBlogList(sessionKey, userSecretKey, userId, -1, -1);
        }

        /// <summary>
        /// 获取指定用户的日志列表（支持扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        static public async Task<RenrenAyncRespArgs<BlogListEntity>> GetBlogList(string sessionKey, string userSecretKey, int userId, int pageNumber, int pageSize)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetUserBlogs));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (pageNumber != -1)
                parameters.Add(new RequestParameterEntity("page", pageNumber.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<BlogListEntity, RenrenAyncRespArgs<BlogListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定日志信息
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="blogId"></param>
        static public async Task<RenrenAyncRespArgs<BlogEntity>> GetBlog(string sessionKey, string userSecretKey, int userId, long blogId)
        {
            return await GetBlog(sessionKey, userSecretKey, userId, blogId, null);
        }

        static public async Task<RenrenAyncRespArgs<BlogEntity>> GetBlog(string sessionKey, string userSecretKey, int userId, long blogId, string password)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetBlog));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("id", blogId.ToString()));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));
            parameters.Add(new RequestParameterEntity("need_html", "1"));   //设置获取日志信息为HTML格式
            parameters.Add(new RequestParameterEntity("only_desc", "0"));  // 设置不是只要摘要
            parameters.Add(new RequestParameterEntity("content_in_page", "0")); // 设置不需要分页
            if (!string.IsNullOrEmpty(password))
                parameters.Add(new RequestParameterEntity("password", password));


            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<BlogEntity, RenrenAyncRespArgs<BlogEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<BlogCommentEntity>> GetBlogComment(string sessionKey, string userSecretKey, int userId, long blogId)
        {
            return await GetBlogComment(sessionKey, userSecretKey, userId, blogId, -1);
        }

        /// <summary>
        /// 获取指定日志评论信息
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="blogId"></param>
        static public async Task<RenrenAyncRespArgs<BlogCommentEntity>> GetBlogComment(string sessionKey, string userSecretKey, int userId, long blogId, int page)
        {
            return await GetBlogComment(sessionKey, userSecretKey, userId, blogId, page, -1, null);
        }

        static public async Task<RenrenAyncRespArgs<BlogCommentEntity>> GetBlogComment(string sessionKey, string userSecretKey, int userId, long blogId, int page, int pageSize, string password)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetBlogComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("id", blogId.ToString()));
            parameters.Add(new RequestParameterEntity("user_id", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (!string.IsNullOrEmpty(password))
                parameters.Add(new RequestParameterEntity("password", password));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<BlogCommentEntity, RenrenAyncRespArgs<BlogCommentEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 发布新日志
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<PostBlogEntity>> PostBlog(string sessionKey, string userSecretKey, string title, string content)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostBlog));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("title", title));
            parameters.Add(new RequestParameterEntity("content", content));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PostBlogEntity, RenrenAyncRespArgs<PostBlogEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 评论日志
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="blogId"></param>
        /// <param name="content"></param>
        /// <param name="ownerUserId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostBlogComment(string sessionKey, string userSecretKey, long blogId, string content, int ownerUserId)
        {
            return await PostBlogComment(sessionKey, userSecretKey, blogId, content, ownerUserId, -1, -1);
        }

        /// <summary>
        /// 评论日志（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="blogId"></param>
        /// <param name="content"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="replayId"></param>
        /// <param name="replayType"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostBlogComment(string sessionKey, string userSecretKey, long blogId, string content, int ownerUserId, int replayId, int replayType)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostBlogComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("id", blogId.ToString()));
            parameters.Add(new RequestParameterEntity("content", content));
            parameters.Add(new RequestParameterEntity("user_id", ownerUserId.ToString()));
            if (replayId != -1)
                parameters.Add(new RequestParameterEntity("rid", replayId.ToString()));
            if (replayType != -1)
                parameters.Add(new RequestParameterEntity("type", replayType.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
