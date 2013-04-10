using System;
using System.Net;

namespace RenrenCoreWrapper.Constants
{
    public class ConstantValue
    {
        public static string ApiKey = "";

        public static string SecretKey = "";

        public static Uri RequestUri = new Uri("http://api.m.renren.com/api", UriKind.Absolute);

        public static Uri SpecificRequestUri = new Uri("http://m.apis.tk/api", UriKind.Absolute);

        public static Uri TestingUri = new Uri("http://mc1.test.renren.com/api", UriKind.Absolute);

        public static Uri ShareSongRequestUri = new Uri("http://api.m.renren.com/api/radio/shareSong", UriKind.Absolute);

        public static string PostMethod = "POST";

        // Followings are used to compose the update info
        // For MS store
        public static string ChannelId = "8001201";
        // For 华硕ID：2000705 
        //public static string ChannelId = "2000705";
        // For 联想ID：2000810
        //public static string ChannelId = "2000810";

        public static string PublishDate = "20120626";
        public static string OS = "Windows 8";
        public static string AppID = "177807";
        public static string AppName = "Renren Windows 8 HD";
        // 0:手动检查更新；1:自动检查更新
        public static string UpdateType = "1";
    }
}
