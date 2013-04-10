using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class NewsListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 所有消息
        /// </summary>
        [DataMember]
        //public List<NewsEntity> news_list { get; set; }

        private ObservableCollection<NewsEntity> news_list = new ObservableCollection<NewsEntity>();
        public ObservableCollection<NewsEntity> News_list
        {
            get
            {
                return news_list;
            }
            set
            {
                news_list = value;
                this.NotifyPropertyChanged(newsListEntity => newsListEntity.News_list);
            }
        }
    }
}
