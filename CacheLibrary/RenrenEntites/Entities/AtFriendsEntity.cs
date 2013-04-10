using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AtFriendsEntity : PropertyChangedBase
    {
        [DataMember]
        private ObservableCollection<AtFriendInfoEntity> at_list = new ObservableCollection<AtFriendInfoEntity>();
        public ObservableCollection<AtFriendInfoEntity> AtList
        {
            get
            {
                return at_list;
            }
            set
            {
                at_list = value;
                this.NotifyPropertyChanged(entity => entity.AtList);
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
                this.NotifyPropertyChanged(entity => entity.Total);
            }
        }
    }
}
