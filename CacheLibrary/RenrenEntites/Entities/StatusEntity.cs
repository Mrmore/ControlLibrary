using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class StatusEntity : PropertyChangedBase
    {
        /// <summary>
        /// 状态id
        /// </summary>
        [DataMember]
        //public long id { get; set; }


        private long id;
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.NotifyPropertyChanged(statusEntity => statusEntity.Id);
            }
        }

        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        //public int user_id { get; set; }

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
                this.NotifyPropertyChanged(statusEntity => statusEntity.User_id);
            }
        }

        /// <summary>
        /// 状态时间
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
                {
                    long tempTime = (long)value;
                    tempTime *= 10000;

                    TimeSpan ts = new TimeSpan(tempTime);

                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.Add(ts);
                    dt = dt.AddHours(8);

                    string date = dt.ToString("MM-dd  HH:mm");
                    timestr = date;
                }
                this.NotifyPropertyChanged(statusEntity => statusEntity.Time);
            }
        }

        /// <summary>
        /// 状态时间 string 表示
        /// </summary>
        [DataMember]
        private string timestr;
        public string TimeStr
        {
            get
            {
                long tempTime = (long)time;
                tempTime *= 10000;

                TimeSpan ts = new TimeSpan(tempTime);

                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.Add(ts);
                dt = dt.AddHours(8);

                string date = dt.ToString("MM-dd  HH:mm");
                timestr = date;
                return timestr;
            }
            set
            {
                timestr = value;
                this.NotifyPropertyChanged(entity => entity.TimeStr);
            }
        }

        /// <summary>
        /// 状态内容
        /// </summary>
        [DataMember]
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                this.NotifyPropertyChanged(statusEntity => statusEntity.Content);
            }
        }
        
        /// <summary>
        /// 状态评论数
        /// </summary>
        [DataMember]
        private int comment_count;
        public int Comment_count
        {
            get
            {
                return comment_count;
            }
            set
            {
                comment_count = value;
                this.NotifyPropertyChanged(statusEntity => statusEntity.Comment_count);
            }
        }

        /// <summary>
        /// 用户名称
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
                this.NotifyPropertyChanged(statusEntity => statusEntity.User_name);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMember]
        private string head_url;
        public string Head_url
        {
            get
            {
                return head_url;
            }
            set
            {
                head_url = value;
                this.NotifyPropertyChanged(statusEntity => statusEntity.Head_url);
            }
        }

        /// <summary>
        /// 转发原状态
        /// </summary>
        [DataMember]
        private StatusEntity origin = new StatusEntity();
        public StatusEntity Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                this.NotifyPropertyChanged(statusEntity => statusEntity.Origin);
            }
        }

        [DataMember]
        public ForwardStatusEntity Forward_Status { get; set; }
    }
}
