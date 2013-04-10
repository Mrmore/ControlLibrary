using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class CommentEntity : PropertyChangedBase
    {
        /// <summary>
        /// 评论id
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.Id);
            }
        }

        /// <summary>
        /// 评论者id
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.User_id);
            }
        }

        /// <summary>
        /// 评论者名字
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.User_name);
            }
        }

        /// <summary>
        /// 评论者头像
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.Head_url);
            }
        }

        /// <summary>
        /// 评论内容
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.Content);
            }
        }

        /// <summary>
        /// 评论时间
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(commentEntity => commentEntity.Time);
            }
        }

        /// <summary>
        /// 是否是悄悄话(1：是，0：不是)
        /// </summary>
        [DataMember]
        //public string whisper { get; set; }

        private string whisper;
        public string Whisper
        {
            get
            {
                return whisper;
            }
            set
            {
                whisper = value;
                this.NotifyPropertyChanged(commentEntity => commentEntity.Whisper);
            }
        }
    }

}
