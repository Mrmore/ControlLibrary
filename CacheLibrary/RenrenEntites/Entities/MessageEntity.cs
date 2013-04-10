using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    /// <summary>
    /// 站内信实体
    /// </summary>
    //[DataContract]
    public class MessageEntity : PropertyChangedBase
    {
        /// <summary>
        /// 表示站内信的id
        /// </summary>
        //[DataMember]
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
                this.NotifyPropertyChanged(messageEntity => messageEntity.Id);
            }
        }

        /// <summary>
        /// 表示站内信发送者的id
        /// </summary>
        //[DataMember]
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
                this.NotifyPropertyChanged(messageEntity => messageEntity.User_id);
            }
        }

        /// <summary>
        /// 表示站内信发送者的姓名
        /// </summary>
        //[DataMember]
        //public string user_name { get; set; }

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
                this.NotifyPropertyChanged(messageEntity => messageEntity.User_name);
            }
        }

        /// <summary>
        /// 表示站内信发送者的头像
        /// </summary>
        //[DataMember]
        //public string head_url { get; set; }

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
                this.NotifyPropertyChanged(messageEntity => messageEntity.Head_url);
            }
        }

        /// <summary>
        /// 表示站内信的接收时间（long）
        /// </summary>
        //[DataMember]
        //public long time { get; set; }

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
                this.NotifyPropertyChanged(messageEntity => messageEntity.Time);
            }
        }

        /// <summary>
        /// 表示站内信的标题
        /// </summary>
        //[DataMember]
        //public string title { get; set; }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                this.NotifyPropertyChanged(messageEntity => messageEntity.Title);
            }
        }

        /// <summary>
        /// 表示站内信的发送者发送的最新内容
        /// </summary>
        //[DataMember]
        //public string content { get; set; }

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
                this.NotifyPropertyChanged(messageEntity => messageEntity.Content);
            }
        }

        /// <summary>
        /// 表示站内信是否已读 0,未读
        /// </summary>
        //[DataMember]
        //public int is_read { get; set; }

        private int is_read;
        public int Is_read
        {
            get
            {
                return is_read;
            }
            set
            {
                is_read = value;
                this.NotifyPropertyChanged(messageEntity => messageEntity.Is_read);
            }
        }

        /// <summary>
        /// 表示站内信是否为外网邮箱发布 1,是 
        /// </summary>
        //[DataMember]
        //public int is_out_message { get; set; }

        private int is_out_message;
        public int Is_out_message
        {
            get
            {
                return is_out_message;
            }
            set
            {
                is_out_message = value;
                this.NotifyPropertyChanged(messageEntity => messageEntity.Is_out_message);
            }
        }
    }
}
