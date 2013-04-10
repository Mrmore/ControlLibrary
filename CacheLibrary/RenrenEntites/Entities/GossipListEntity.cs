using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class GossipListEntity : PropertyChangedBase
    {
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
                this.NotifyPropertyChanged(gossipListEntity => gossipListEntity.Count);
            }
        }

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
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Page_size);
            }
        }

        [DataMember]
        private ObservableCollection<GossipEntity> gossip_list = new ObservableCollection<GossipEntity>();
        public ObservableCollection<GossipEntity> Gossip_list
        {
            get
            {
                return gossip_list;
            }
            set
            {
                gossip_list = value;
                this.NotifyPropertyChanged(gossipListEntity => gossipListEntity.Gossip_list);
            }
        }
    }
}
