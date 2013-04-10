using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class RequestFriendsListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 好友申请总数
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
                this.NotifyPropertyChanged(requestFriendsListEntity => requestFriendsListEntity.Count);
            }
        }

        /// <summary>
        /// 当前请求列表分页每页的数量
        /// </summary>
        [DataMember]
        //public int page_size { get; set; }

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
                this.NotifyPropertyChanged(requestFriendsListEntity => requestFriendsListEntity.Page_size);
            }
        }

        /// <summary>
        /// 好友申请内容
        /// </summary>
        [DataMember]
        //public List<RequestUserEntity> friend_apply_list { get; set; }

        private ObservableCollection<RequestUserEntity> friend_apply_list = new ObservableCollection<RequestUserEntity>();
        public ObservableCollection<RequestUserEntity> Friend_apply_list
        {
            get
            {
                return friend_apply_list;
            }
            set
            {
                friend_apply_list = value;
                this.NotifyPropertyChanged(requestFriendsListEntity => requestFriendsListEntity.Friend_apply_list);
            }
        }
    }
}
