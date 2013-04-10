using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class RequestUserEntity : PropertyChangedBase
    {
        /// <summary>
        /// 申请人id 
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
                this.NotifyPropertyChanged(requestUserEntity => requestUserEntity.User_id);
            }
        }

        /// <summary>
        /// 申请人名称
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
                this.NotifyPropertyChanged(requestUserEntity => requestUserEntity.User_name);
            }
        }

        /// <summary>
        /// 申请人头像url
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
                this.NotifyPropertyChanged(requestUserEntity => requestUserEntity.Head_url);
            }
        }

        /// <summary>
        /// 申请说明
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
                this.NotifyPropertyChanged(requestUserEntity => requestUserEntity.Content);
            }
        }

        /// <summary>
        /// 共同好友数
        /// </summary>
        [DataMember]
        //public int share_count { get; set; }

        private int share_count;
        public int Share_count
        {
            get
            {
                return share_count;
            }
            set
            {
                share_count = value;
                this.NotifyPropertyChanged(requestUserEntity => requestUserEntity.Share_count);
            }
        }
    }
}
