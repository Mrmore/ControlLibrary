using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class LikeInfoEntity : PropertyChangedBase
    {
        [DataMember]
        private int self_count;
        public int SelfCount
        {
            get
            {
                return self_count;
            }
            set
            {
                self_count = value;
                this.NotifyPropertyChanged(entity => entity.SelfCount);
            }
        }

        [DataMember]
        private int friend_count;
        public int FriendCount
        {
            get
            {
                return friend_count;
            }
            set
            {
                friend_count = value;
                this.NotifyPropertyChanged(entity => entity.FriendCount);
            }
        }

        [DataMember]
        private int total_count;
        public int TotalCount
        {
            get
            {
                return total_count;
            }
            set
            {
                total_count = value;
                this.NotifyPropertyChanged(entity => entity.TotalCount);
            }
        }

        [DataMember]
        private int with_friend_list;
        public int WithFriendList
        {
            get
            {
                return with_friend_list;
            }
            set
            {
                with_friend_list = value;
                this.NotifyPropertyChanged(entity => entity.WithFriendList);
            }
        }

        [DataMember]
        private int show_strangers;
        public int ShowStrangers
        {
            get
            {
                return show_strangers;
            }
            set
            {
                show_strangers = value;
                this.NotifyPropertyChanged(entity => entity.ShowStrangers);
            }
        }

        [DataMember]
        private int detail_limit;
        public int DetailLimit
        {
            get
            {
                return detail_limit;
            }
            set
            {
                detail_limit = value;
                this.NotifyPropertyChanged(entity => entity.DetailLimit);
            }
        }

        [DataMember]
        private ObservableCollection<LikeUserDetailEntity> user_details = new ObservableCollection<LikeUserDetailEntity>();
        public ObservableCollection<LikeUserDetailEntity> UserDetails 
        {
            get
            {
                return user_details;
            }
            set
            {
                user_details = value;
                this.NotifyPropertyChanged(entity => entity.UserDetails);
            }
        }
    }
}
