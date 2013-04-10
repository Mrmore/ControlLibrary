using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class NewsCountEntity : PropertyChangedBase
    {
        /// <summary>
        /// 留言回复数
        /// </summary>
        [DataMember]
        //public int gossip_reply_count { get; set; }

        private int gossip_reply_count;
        public int Gossip_reply_count
        {
            get
            {
                return gossip_reply_count;
            }
            set
            {
                gossip_reply_count = value;
                this.NotifyPropertyChanged(newsCountEntity => newsCountEntity.Gossip_reply_count);
            }
        }

        /// <summary>
        /// 站内信数
        /// </summary>
        [DataMember]
        //public int message_count { get; set; }

        private int message_count;
        public int Message_count
        {
            get
            {
                return message_count;
            }
            set
            {
                message_count = value;
                this.NotifyPropertyChanged(newsCountEntity => newsCountEntity.Message_count);
            }
        }

        /// <summary>
        /// 好友请求数
        /// </summary>
        [DataMember]
        //public int friend_request_count { get; set; }

        private int friend_request_count;
        public int Friend_request_count
        {
            get
            {
                return friend_request_count;
            }
            set
            {
                friend_request_count = value;
                this.NotifyPropertyChanged(newsCountEntity => newsCountEntity.Friend_request_count);
            }
        }

        [DataMember]

        private int news_count;
        public int News_count
        {
            get
            {
                return news_count;
            }
            set
            {
                news_count = value;
                this.NotifyPropertyChanged(newsCountEntity => newsCountEntity.News_count);
            }
        }
    }
}
