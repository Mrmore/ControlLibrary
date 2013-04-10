using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class IsFriendResultEntity : PropertyChangedBase
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
                this.NotifyPropertyChanged(isFriendResultEntity => isFriendResultEntity.Count);
            }
        }

        [DataMember]
        //public List<IsFriendEntity> friend_info_list { get; set; }

        private ObservableCollection<IsFriendEntity> friend_info_list = new ObservableCollection<IsFriendEntity>();
        public ObservableCollection<IsFriendEntity> Friend_info_list
        {
            get
            {
                return friend_info_list;
            }
            set
            {
                friend_info_list = value;
                this.NotifyPropertyChanged(isFriendResultEntity => isFriendResultEntity.Friend_info_list);
            }
        }
    }
}
