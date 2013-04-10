using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class VisitorsEntity : PropertyChangedBase
    {
        /// <summary>
        /// 最近访问者数量
        /// </summary>
        [DataMember]
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                this.NotifyPropertyChanged(visitorsEntity => visitorsEntity.Count);
            }
        }

        /// <summary>
        /// 表示当前分页每页的数量
        /// </summary>
        [DataMember]
        private int page_size;
        public int Page_size
        {
            get
            {
                return page_size;
            }
            set
            {
                page_size = value;
                this.NotifyPropertyChanged(visitorsEntity => visitorsEntity.Page_size);
            }
        }

        /// <summary>
        /// 所有最近访问者信息
        /// </summary>
        [DataMember]
        private ObservableCollection<VisitorUserEntity> visitor_list = new ObservableCollection<VisitorUserEntity>();
        public ObservableCollection<VisitorUserEntity> Visitor_list
        {
            get
            {
                return visitor_list;
            }
            set
            {
                visitor_list = value;
                this.NotifyPropertyChanged(visitorsEntity => visitorsEntity.Visitor_list);
            }
        }
    }
}
