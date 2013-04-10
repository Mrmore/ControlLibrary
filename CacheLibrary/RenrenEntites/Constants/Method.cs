using System;
using System.Net;

namespace RenrenCoreWrapper.Constants
{
    public static class Method
    {
        /// <summary>
        /// “登陆”方法名
        /// </summary>
        public static string LogIn = "client.login";
        /// <summary>
        /// “获取状态”方法名
        /// </summary>
        public static string GetStatus = "status.get";
        /// <summary>
        /// “获取状态列表”方法名
        /// </summary>
        public static string GetStatusList = "status.gets";
        /// <summary>
        /// “获取指定状态评论”方法名
        /// </summary>
        public static string GetStatusComments = "status.getComments";
        /// <summary>
        /// “指定状态发布评论”方法名
        /// </summary>
        public static string PostStatusComment = "status.addComment";
        /// <summary>
        /// “获取当前用户新鲜事内容”方法名
        /// </summary>
        public static string GetFeed = "feed.get";
        /// <summary>
        /// “获取指定用户日志列表”方法名
        /// </summary>
        public static string GetUserBlogs = "blog.gets";
        /// <summary>
        /// “获取一篇日志全部信息”方法名
        /// </summary>
        public static string GetBlog = "blog.get";
        /// <summary>
        /// “获取一篇日志评论列表”方法名
        /// </summary>
        public static string GetBlogComment = "blog.getComments";
        /// <summary>
        /// “获取好友列表”方法名
        /// </summary>
        public static string GetFriends = "friends.getFriends";
        /// <summary>
        /// “获取相册列表”方法名
        /// </summary>
        public static string GetAlbums = "photos.getAlbums";
        /// <summary>
        /// 创建相册
        /// </summary>
        public static string CreateAlbum = "photos.createAlbum";
        /// <summary>
        /// “获取照片列表”方法名
        /// </summary>
        public static string GetPhotos = "photos.get";
        /// <summary>
        /// “获取照片评论列表”方法名
        /// </summary>
        public static string GetPhotoComments = "photos.getComments";
        /// <summary>
        /// “添加照片或相册评论”方法名
        /// </summary>
        public static string PostPhotoComment = "photos.addComment";
        /// <summary>
        /// “获取个人主页所需信息”方法名
        /// </summary>
        public static string GetUserInfo = "profile.getInfo";
        /// <summary>
        /// “获取最近访问列表”方法名
        /// </summary>
        public static string GetVisitors = "user.getVisitors";
        /// <summary>
        /// “获取留言板”方法名
        /// </summary>
        public static string GetGossips = "gossip.gets";
        /// <summary>
        /// “获取站内信列表”方法名
        /// </summary>
        public static string GetMessages = "message.getList";
        /// <summary>
        /// “获取好友申请列表”方法名
        /// </summary>
        public static string GetFriendsRequest = "friends.getRequests";
        /// <summary>
        /// “获取共同好友”方法名
        /// </summary>
        public static string GetSharedFriends = "friends.getSharedFriends";
        /// <summary>
        /// “接受好友申请”方法名
        /// </summary>
        public static string AcceptFriendRequest = "friends.accept";
        /// <summary>
        /// “忽略好友申请”方法名
        /// </summary>
        public static string DenyFriendRequest = "friends.deny";
        /// <summary>
        /// “获取当前用户的消息列表”方法名
        /// </summary>
        public static string GetNewsList = "news.newsList";
        /// <summary>
        /// “获取当前用户的消息总数”方法名
        /// </summary>
        public static string GetNewsCount = "news.getCount";
        /// <summary>
        /// "获取分享"
        /// </summary>
        public static string GetTheShare = "share.get";
        /// <summary>
        /// “获取分享列表”方法名
        /// </summary>
        public static string GetShareList = "share.gets";
        /// <summary>
        /// “获取在线好友列表”方法名
        /// </summary>
        public static string GetOnlineFriendList = "friends.getOnlineFriends";
        /// <summary>
        /// “判断是否为好友”方法
        /// </summary>
        public static string CheckAreFriends = "friends.areFriends";

        /// <summary>
        /// “发布状态”方法名
        /// </summary>
        public static string SetStatus = "status.set";
        /// <summary>
        /// “转发状态”方法名
        /// </summary>
        public static string ForwardStatus = "status.forward";
        /// <summary>
        /// “针对指定日志发布评论”方法名
        /// </summary>
        public static string PostBlogComment = "blog.addComment";
        /// <summary>
        /// “发布新日志”方法名
        /// </summary>
        public static string PostBlog = "blog.add";
        /// <summary>
        /// “上传图片”方法名
        /// </summary>
        public static string PostPhoto = "photos.upload";
        /// <summary>
        /// “留言或回复留言”方法名
        /// </summary>
        public static string PostGossip = "gossip.postGossip";
        /// <summary>
        /// “好友申请”方法名
        /// </summary>
        public static string RequestFriend = "friends.request";
        /// <summary>
        /// “发布分享”方法名
        /// </summary>
        public static string PostShare = "share.publish";

        public static string PublishLink = "share.publishLink";
        /// <summary>
        /// “添加分享评论”方法名
        /// </summary>
        public static string PostShareComment = "share.addComment";
        /// <summary>
        /// “添加推送注册”方法名
        /// </summary>
        public static string AddNotification = "windowphone.addNotification";
        /// <summary>
        /// 获取客户端升级信息
        /// </summary>
        public static string GetUpldateInfo = "phoneclient.getUpdateInfo";
        /// <summary>
        /// “通过消息Id将消息置为已读“方法名
        /// </summary>
        public static string ReadNewsById = "news.readNewsById";
        /// <summary>
        /// “获取分享评论列表”方法名
        /// </summary>
        public static string GetShareComment = "share.getComments";

        /// <summary>
        /// 获取公共主页的资料
        /// </summary>
        public static string GetPageInfo = "page.getInfo";

        // bellows are radio relevant stuff
        public static string RadioGetHome = "radio.getHome";
        public static string GetRadio = "radio.getRadio";
        public static string GetNextSong = "radio.getNextSong";
        public static string RadioAddFavorite = "radio.addFavorite";
        public static string RadioRemoveFavorite = "radio.removeFavorite";
        public static string RadioShareSong = "radio.shareSong";
        public static string VideoGet = "video.get";
        public static string V56GetVideo = "v56.getVideo";
        public static string AddWin8Token = "push.addToken";
        public static string GetPoilist = "place.poiList";
        public static string AtGetFriends = "at.getFriends";
        public static string FrequentAtFriends = "at.getFreqAtFriends";
        public static string FriendsSearch = "friends.search";
        public static string PagesSearch = "page.search";
        public static string PageBecomeFan = "page.becomeFan";
        public static string GetNewUploaded = "photos.getNewUploaded";
        public static string GetUpdateInfo = "phoneclient.getUpdateInfo";
        //据新鲜事id列表获取相应的新鲜事列表 
        public static string GetFeedByIds = "feed.getByIds";
        //获取用户的最新消息推送列表
        public static string GetPushNews = "news.push";
        // 批量调用API的工具方法
        public static string BatchRun = "batch.run";
    }
}