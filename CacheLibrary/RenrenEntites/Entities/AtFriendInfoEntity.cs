using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AtFriendInfoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [DataMember]
        //public int user_id { get; set; }

        private int user_id;
        public int User_id
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.User_id);
            }
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [DataMember]
        //public string user_name { get; set; }

        private string user_name;
        public string User_name
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.User_name);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [DataMember]
        private string head_url;
        public string Head_url
        {
            get
            {
                return head_url;
            }
            set
            {
                head_url = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Head_url);
            }
        }

        /// <summary>
        /// 用户所属网络列表
        /// </summary>
        [DataMember]
        private string network = string.Empty;
        public string Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Network);
            }
        }
    }
}
