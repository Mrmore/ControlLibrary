using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class ShareListEntity : PropertyChangedBase
    {
        [DataMember]
        //public int count { get; set; }

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
                this.NotifyPropertyChanged(shareListEntity => shareListEntity.Count);
            }
        }

        [DataMember]
        //public List<ShareEntity> item_list { get; set; }

        private ObservableCollection<ShareEntity> item_list = new ObservableCollection<ShareEntity>();
        public ObservableCollection<ShareEntity> Item_list
        {
            get
            {
                return item_list;
            }
            set
            {
                item_list = value;
                this.NotifyPropertyChanged(shareListEntity => shareListEntity.Item_list);
            }
        }
    }
}
