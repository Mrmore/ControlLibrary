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
using Windows.Storage;
using Windows.Devices.Geolocation;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;

namespace RenrenCoreWrapper.Apis
{
    public partial class Renren3GApiWrapper
    {
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> UploadHead(string sessionKey, string secretKey, StorageFile file, string caption, int from)
        {
            var check = await ApiHelper.CheckUploadImageFile<RenrenAyncRespArgs<CommonReultEntity>>(file);
            if (check != null) return check;

            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("method", "photos.uploadHead"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("caption", caption));
            parameters.Add(new RequestParameterEntity("from", from.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseMpHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api", UriKind.Absolute), file);
            return result;
        }

        /// <summary>
        /// 发送图片 using multi-part
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="basedata"></param>
        static public async Task<RenrenAyncRespArgs<UploadPhotoEntity>> UploadPhotoMPart(string sessionKey, string userSecretKey, StorageFile file, long albumId, string caption, int p = 1)
        {
            var check = await ApiHelper.CheckUploadImageFile<RenrenAyncRespArgs<UploadPhotoEntity>>(file);
            if (check != null) return check;

            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("method", "photos.uploadbin"));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            if (albumId != -1)
                parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            if (caption != null)
                parameters.Add(new RequestParameterEntity("caption", caption));

            if (p != -1)
                parameters.Add(new RequestParameterEntity("p", p.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseMpHandler<UploadPhotoEntity, RenrenAyncRespArgs<UploadPhotoEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api", UriKind.Absolute), file);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<UploadPhotoEntity>> UploadPhotoMPartWithPlace(string sessionKey, string secretKey, StorageFile file, long albumId, string caption, PoiListEntity place, Geoposition pos, int p = 1)
        {
            var check = await ApiHelper.CheckUploadImageFile<RenrenAyncRespArgs<UploadPhotoEntity>>(file);
            if (check != null) return check;

            long lon = (long)(pos.Coordinate.Longitude * 1000000.0);
            long lat = (long)(pos.Coordinate.Latitude * 1000000.0);

            // get the poi list
            var info = place.PoiInfo;

            string temp = string.Format(_placeFormat, info.Longitude, info.Latitude, lon, lat, info.PoiName);
            string placeData = "{" + temp + "}";
            var response = await UploadPhotoMPartWithPlace(sessionKey, secretKey, file, albumId, caption, placeData, p);
            return response;
        }

        const string _placeFormat = "\"place_longitude\":{0},\"place_latitude\":{1},\"gps_longitude\":{2},\"gps_latitude\":{3},\"d\":1,\"locate_type\":0,\"place_name\":\"{4}\",\"privacy\":2,\"source_type\":4";
        /// <summary>
        /// 发布带LBS的照片
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="file"></param>
        /// <param name="albumId"></param>
        /// <param name="caption"></param>
        /// <param name="place_data"></param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<UploadPhotoEntity>> UploadPhotoMPartWithPlace(string sessionKey, string userSecretKey, StorageFile file, long albumId, string caption, string place_data = null, int uploadType = -1, int photoIndex = -1, int photoTotal = -1, int p = 1)
        {
            var check = await ApiHelper.CheckUploadImageFile<RenrenAyncRespArgs<UploadPhotoEntity>>(file);
            if (check != null) return check;

            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("method", "photos.uploadbin"));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));

            if (albumId != -1)
                parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            if (caption != null)
                parameters.Add(new RequestParameterEntity("caption", caption));

            if (!string.IsNullOrEmpty(place_data) && !string.IsNullOrWhiteSpace(place_data))
            {
                parameters.Add(new RequestParameterEntity("place_data", place_data));
            }

            if (uploadType != -1)
                parameters.Add(new RequestParameterEntity("upload_type", uploadType.ToString()));
            if (photoIndex != -1)
                parameters.Add(new RequestParameterEntity("photo_index", photoIndex.ToString()));
            if (photoTotal != -1)
                parameters.Add(new RequestParameterEntity("photo_total", photoTotal.ToString()));

            if (p != -1)
                parameters.Add(new RequestParameterEntity("p", p.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseMpHandler<UploadPhotoEntity, RenrenAyncRespArgs<UploadPhotoEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api", UriKind.Absolute), file);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<PhotoListEntity>> GetNewUploadedPhotos(string sessionKey, string userSecretKey, int userId = -1,  int page = -1, int pageSize = -1)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetNewUploaded));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            //parameters.Add(new RequestParameterEntity("exclude_list", exclude_list.ToString()));
            //parameters.Add(new RequestParameterEntity("aid ", aid.ToString()));

            if (userId != -1)
                parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (pageSize != -1)
                parameters.Add(new RequestParameterEntity("page_size", pageSize.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PhotoListEntity, RenrenAyncRespArgs<PhotoListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> SendFeedPhotos(string sessionKey, string secretKey, long aid, int from)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("method", "photos.sendFeed"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("aid", aid.ToString()));
            parameters.Add(new RequestParameterEntity("send_feed", "1"));

            if (from != -1)
                parameters.Add(new RequestParameterEntity("from", from.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<CommonReultEntity, RenrenAyncRespArgs<CommonReultEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        //NOTE:接口不完整
        /// <summary>
        /// 获取指定相册内照片列表
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        static public async Task<RenrenAyncRespArgs<PhotoListEntity>> GetPhotoList(string sessionKey, string userSecretKey, int userId, long albumId)
        {
            return await GetPhotoList(sessionKey, userSecretKey, userId, albumId, -1, -1);
        }

        /// <summary>
        /// 获取指定相册内照片列表[支持获取更多]
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        static public async Task<RenrenAyncRespArgs<PhotoListEntity>> GetPhotoList(string sessionKey, string userSecretKey, int userId, long albumId, int page, int page_size)
        {
            return await GetPhotoList(sessionKey, userSecretKey, userId, albumId, page, page_size, null);
        }

        static public async Task<RenrenAyncRespArgs<PhotoListEntity>> GetPhotoList(string sessionKey, string userSecretKey, int userId, long albumId, int page, int page_size, string password)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPhotos));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            if (page != -1)
                parameters.Add(new RequestParameterEntity("page", page.ToString()));
            if (page_size != -1)
                parameters.Add(new RequestParameterEntity("page_size", page_size.ToString()));
            if (!string.IsNullOrEmpty(password))
                parameters.Add(new RequestParameterEntity("password", password.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PhotoListEntity, RenrenAyncRespArgs<PhotoListEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取照片列表或者单张照片
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="photoId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public async Task<RenrenAyncRespArgs<PhotoEntity>> GetPhoto(string sessionKey, string userSecretKey, int userId, long photoId, string password)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPhotos));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            parameters.Add(new RequestParameterEntity("pid", photoId.ToString()));
            if (!string.IsNullOrEmpty(password))
                parameters.Add(new RequestParameterEntity("password", password));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PhotoEntity, RenrenAyncRespArgs<PhotoEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        /// <summary>
        /// 获取指定照片评论
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <param name="picId"></param>
        static public async Task<RenrenAyncRespArgs<PhotoCommentEntity>> GetPhotoComments(string sessionKey, string userSecretKey, int userId, long albumId, long picId)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPhotoComments));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));
            parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            parameters.Add(new RequestParameterEntity("pid", picId.ToString()));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PhotoCommentEntity, RenrenAyncRespArgs<PhotoCommentEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
        /// <summary>
        /// 获取指定照片评论[支持分页]
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="userId"></param>
        /// <param name="albumId"></param>
        /// <param name="picId"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        static public async Task<RenrenAyncRespArgs<PhotoCommentEntity>> GetPhotoComments(string sessionKey, string userSecretKey, int userId, long albumId, long picId, int page, int pagesize, string password)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetPhotoComments));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("uid", userId.ToString()));

            if (albumId != -1)
            {
                parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            }

            if (picId != -1)
            {
                parameters.Add(new RequestParameterEntity("pid", picId.ToString()));
            }

            parameters.Add(new RequestParameterEntity("page", page.ToString()));
            parameters.Add(new RequestParameterEntity("page_size", pagesize.ToString()));

            if (!string.IsNullOrEmpty(password))
            {
                parameters.Add(new RequestParameterEntity("password", password));
            }


            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<PhotoCommentEntity, RenrenAyncRespArgs<PhotoCommentEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }

        
        /// <summary>
        /// 发表照片评论
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="photoId"></param>
        /// <param name="content"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostPhotoComment(string sessionKey, string userSecretKey, int onwerId, long photoId, string content)
        {
            return await PostPhotoComment(sessionKey, userSecretKey, onwerId, photoId, content, -1, -1);
        }

        /// <summary>
        /// 发表照片评论（可扩展）
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="onwerId"></param>
        /// <param name="photoId"></param>
        /// <param name="content"></param>
        /// <param name="replayUserId"></param>
        static public async Task<RenrenAyncRespArgs<CommonReultEntity>> PostPhotoComment(string sessionKey, string userSecretKey, int onwerId, long photoId, string content, int replayUserId, int isWhisper)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostPhotoComment));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("uid", onwerId.ToString()));
            parameters.Add(new RequestParameterEntity("pid", photoId.ToString()));
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
        /// 上传图片至默认相册 未添加描述信息、相册等操作
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="basedata"></param>
        static public async Task<RenrenAyncRespArgs<UploadPhotoEntity>> UploadPhoto(string sessionKey, string userSecretKey, string basedata)
        {
            return await UploadPhoto(sessionKey, userSecretKey, basedata, -1, null);
        }

        /// <summary>
        /// 发送图片
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="userSecretKey"></param>
        /// <param name="basedata"></param>
        static public async Task<RenrenAyncRespArgs<UploadPhotoEntity>> UploadPhoto(string sessionKey, string userSecretKey, string basedata, long albumId, string caption)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("method", Method.PostPhoto));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("base_data", basedata));
            if (albumId != -1)
                parameters.Add(new RequestParameterEntity("aid", albumId.ToString()));
            if (caption != null)
                parameters.Add(new RequestParameterEntity("caption", caption));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, userSecretKey)));

            var result = await agentReponseHandler<UploadPhotoEntity, RenrenAyncRespArgs<UploadPhotoEntity>>(parameters, ConstantValue.PostMethod);
            return result;
        }
    }
}