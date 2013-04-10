using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FeedListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户的新鲜事列表
        /// </summary>
        [DataMember]
        //public List<FeedEntity> feed_list { get; set; }

        private ObservableCollection<FeedEntity> feed_list = new ObservableCollection<FeedEntity>();
        public ObservableCollection<FeedEntity> Feed_list
        {
            get
            {
                return feed_list;
            }
            set
            {
                feed_list = value;
                this.NotifyPropertyChanged(feedListEntity => feedListEntity.Feed_list);
            }
        }

        /// <summary>
        /// ？
        /// </summary>
        [DataMember]
        //public int has_more { get; set; }

        private int has_more;
        public int Has_more
        {
            get
            {
                return has_more;
            }
            set
            {
                has_more = value;
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Has_more);
            }
        }

        /// <summary>
        /// ？
        /// </summary>
        [DataMember]
        //public int is_mini_feed { get; set; }

        private int is_mini_feed;
        public int Is_mini_feed
        {
            get
            {
                return is_mini_feed;
            }
            set
            {
                is_mini_feed = value;
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Is_mini_feed);
            }
        }
    }
}
