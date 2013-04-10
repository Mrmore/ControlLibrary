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
        /// 获取登录用户好友申请列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<RequestFriendsListEntity>> GetRequestFriendList(string sessionKey, string userSecretKey)
        {
            return await GetRequestFriendList(sessionKey, userSecretKey, -1, -1, -1, -1);
        }

        /// <summary>
        /// 获取登录用户好友申请列表（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="excludeList"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="delNew"></param>
        static public async Task<RenrenAyncRespArgs<RequestFriendsListEntity>> GetRequestFriendList(string sessionKey, string userSecretKey, int excludeList, int page, int pageSize, int delNew)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetFriendsRequest));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            if (excludeList != -1)
                parameters.Add(new RequestParameterEntity("exclude_list", excludeList.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));
            if (excludeList != -1)
                parameters.Add(new RequestParameterEntity("exclude_list", excludeList.ToString()));
            if (delNew != -1)
                parameters.Add(new RequestParameterEntity("del_news", delNew.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<RequestFriendsListEntity, RenrenAyncRespArgs<RequestFriendsListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<NewsCountEntity>> GetNewsCount(string sessionKey, string userSecretKey)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetNewsCount));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("type", "1"));//获取留言
            parameters.Add(new RequestParameterEntity("sub_types", "16,17,18,27,28,142,170,172,173,175,196,197,100001,100002,100003,100004,100005,100006,100007,100008,100009,100010, 100011,100012,100013,100014,100015,100016,100017,100018,100019,100020"));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<NewsCountEntity, RenrenAyncRespArgs<NewsCountEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        static public async Task<RenrenAyncRespArgs<NewsListEntity>> GetNewsList(string sessionKey, string userSecretKey)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetNewsList));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("type", "1"));//获取留言
            parameters.Add(new RequestParameterEntity("page_size", "99"));//获取留言
            parameters.Add(new RequestParameterEntity("sub_types", "16,17,18,27,28,142,170,172,173,175,196,197,100001,100002,100003,100004,100005,100006,100007,100008,100009,100010, 100011,100012,100013,100014,100015,100016,100017,100018,100019,100020"));
            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<NewsListEntity, RenrenAyncRespArgs<NewsListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 将消息置为已读
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="newsId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> MakeNewsToReadState(string sessionKey, string userSecretKey, long newsId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.ReadNewsById));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("news_id", newsId.ToString()));//获取留言

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取用户的最新消息推送列表 
        /// http://wiki.mobile.renren.com/index.php/News.push
        /// subTypes的参数请参考上面的链接
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="type">需要的消息种类（0x1：留言回复，0x2：站内信，0x4：好友请求，所需的种类编号按位取或，如：3表示同时取留言回复和站内信，7表示三个全取）</param>
        /// <param name="subTypes">请求消息的类型集合（当有本字段时type字段无效），指定获取消息的具体类型，typeId用逗号分隔。</param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<NewsListEntity>> GetPushNews(string sessionKey, string secretKey, int type, params long[] subTypes)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPushNews));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("type", type.ToString()));

            if (subTypes.Length > 0)
            {
                string sub_types = string.Empty;
                foreach (var item in subTypes)
                {
                    sub_types += item + ",";
                }
                sub_types.TrimEnd(',');
                parameters.Add(new RequestParameterEntity("sub_types", sub_types));
            }

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<NewsListEntity, RenrenAyncRespArgs<NewsListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}
