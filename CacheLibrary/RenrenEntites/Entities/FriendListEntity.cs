using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FriendListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户的全部好友数量
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
                this.NotifyPropertyChanged(friendListEntity => friendListEntity.Count);
            }
        }

        [DataMember]
        private int total;
        public int Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
                this.NotifyPropertyChanged(friendListEntity => friendListEntity.Total);
            }
        }

        /// <summary>
        /// 表示搜索匹配结果的具体内容
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
                this.NotifyPropertyChanged(friendListEntity => friendListEntity.Friend_list);
            }
        }

        [DataMember]
        private ObservableCollection<FriendEntity> friends = new ObservableCollection<FriendEntity>();
        public ObservableCollection<FriendEntity> Friends
        {
            get
            {
                return friends;
            }
            set
            {
                friends = value;
                this.NotifyPropertyChanged(friendListEntity => friendListEntity.Friends);
            }
        }
    }
}
