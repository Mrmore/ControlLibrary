using System;
using System.Net;
using System.Runtime.Serialization;
using Windows.ApplicationModel.Resources;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class VisitorUserEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        private int user_id;
        public int User_id
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.User_id);
            }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [DataMember]
        private string user_name;
        public string User_name
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.User_name);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMember]
        private string user_head;
        public string User_head
        {
            get
            {
                return user_head;
            }
            set
            {
                user_head = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.User_head);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMember]
        private string large_head;
        public string Large_Head
        {
            get
            {
                large_head = this.getLargeHeaderUrl(user_id, 400);
                return large_head;
            }
        }

        /// <summary>
        /// 来访时间
        /// </summary>
        [DataMember]
        private long time;
        public long Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.Time);
            }
        }

        [DataMember]
        private string timestr;
        public string TimeStr
        {
            get
            {
                string dateResult = string.Empty;
                long tempTime = System.Convert.ToInt64(time);
                tempTime *= 10000;

                TimeSpan ts = new TimeSpan(tempTime);

                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.Add(ts);
                dt = dt.AddHours(8);

                TimeSpan timeSpan = DateTime.Now - dt;
                if (dt.Date != DateTime.Now.Date)
                {
                    dateResult = dt.ToString("MM月dd日");
                }
                else
                {
                    dateResult = dt.ToString("HH:mm");
                }
                return dateResult;
            }
            set
            {
                timestr = value;
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.TimeStr);
            }
        }

        /// <summary>
        /// ？
        /// </summary>
        [DataMember]
        private int is_online;
        public int Is_online
        {
            get
            {
                return is_online;
            }
            set
            {
                is_online = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.Is_online);
            }
        }

        /// <summary>
        /// ？
        /// </summary>
        [DataMember]
        private int gender;
        public int Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                this.NotifyPropertyChanged(visitorUserEntity => visitorUserEntity.Gender);
            }
        }

        /// <summary>
        /// 用户头像列表
        /// </summary>
        [DataMember]
        private UserUrlsEntity user_urls;
        public UserUrlsEntity UserHeadUrls
        {
            get
            {
                return user_urls;
            }
            set
            {
                user_urls = value;
                this.NotifyPropertyChanged(entity => entity.UserHeadUrls);
            }
        }

        //取大头像
        public string getLargeHeaderUrl(int uid, int width)
        {
            Random r = new Random();
            int index = r.Next(5000);
            return "http://ic.m.renren.com/gn?op=resize&w=" + width.ToString() + "&p=" + uid.ToString() + "-L&a=" + index.ToString();
        }
    }
}
