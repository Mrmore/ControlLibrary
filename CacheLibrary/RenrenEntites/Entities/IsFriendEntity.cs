using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class IsFriendEntity : PropertyChangedBase
    {
        [DataMember]
        //public int user_id_1 { get; set; }

        private int user_id_1;
        public int User_id_1
        {
            get
            {
                return user_id_1;
            }
            set
            {
                user_id_1 = value;
                this.NotifyPropertyChanged(isFriendEntity => isFriendEntity.User_id_1);
            }
        }

        [DataMember]
        //public int user_id_2 { get; set; }

        private int user_id_2;
        public int User_id_2
        {
            get
            {
                return user_id_2;
            }
            set
            {
                user_id_2 = value;
                this.NotifyPropertyChanged(isFriendEntity => isFriendEntity.User_id_2);
            }
        }

        [DataMember]
        //public int are_friends { get; set; }

        private int are_friends;
        public int Are_friends
        {
            get
            {
                return are_friends;
            }
            set
            {
                are_friends = value;
                this.NotifyPropertyChanged(isFriendEntity => isFriendEntity.Are_friends);
            }
        }
    }
}
