using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class CommonFriendsEntity : PropertyChangedBase
    {
        /// <summary>
        /// 共同好友数量
        /// </summary>
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
                this.NotifyPropertyChanged(commonFriendsEntity => commonFriendsEntity.Count);
            }
        }

        /// <summary>
        /// 共同好友内容
        /// </summary>
        [DataMember]
        //public List<FriendEntity> friend_list { get; set; }

        private ObservableCollection<FriendEntity> friend_list = new ObservableCollection<FriendEntity>();
        public ObservableCollection<FriendEntity> Friend_list
        {
            get
            {
                return friend_list;
            }
            set
            {
                friend_list = value;
                this.NotifyPropertyChanged(commonFriendsEntity => commonFriendsEntity.Friend_list);
            }
        }
    }
}
