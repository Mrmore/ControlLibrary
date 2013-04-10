using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FriendEntity : PropertyChangedBase
    {
        /// <summary>
        /// 共同好友的id
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
                this.NotifyPropertyChanged(friendEntity => friendEntity.User_id);
            }
        }

        /// <summary>
        /// 共同好友名称
        /// </summary>
        [DataMember]
        //public string user_name { get; set; }

        private string user_name = string.Empty;
        public string User_name
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.User_name);
            }
        }

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
                this.NotifyPropertyChanged(friendEntity => friendEntity.Head_url);
            }
        }

        [DataMember]
        private string large_head_url;
        public string LargeHeadUrl
        {
            get
            {
                return large_head_url;
            }
            set
            {
                large_head_url = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.LargeHeadUrl);
            }
        }

        [DataMember]
        private string main_head_url;
        public string MainHeadUrl
        {
            get
            {
                return main_head_url;
            }
            set
            {
                main_head_url = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.MainHeadUrl);
            }
        }

        //[DataMember]
        //private string large_header;
        //public string Large_Header
        //{
        //    get
        //    {
        //        return Helper.ApiHelper.GetLargeHeaderUrl(user_id, 200);
        //    }
        //    set
        //    {
        //        large_header = value;
        //        this.NotifyPropertyChanged(friendEntity => friendEntity.Large_Header);
        //    }
        //}

        /// <summary>
        /// 是否在线
        /// </summary>
        [DataMember]
        //public int is_online { get; set; }

        private int is_online;
        public int Is_online
        {
            get
            {
                return is_online;
            }
            set
            {
                is_online = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.Is_online);
            }
        }

        /// <summary>
        /// 所在网络
        /// </summary>
        [DataMember]
        //public string network { get; set; }

        private string network;
        public string Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.Network);
            }
        }

        /// <summary>
        /// 所在分组
        /// </summary>
        [DataMember]
        //public string group { get; set; }

        private string group;
        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.Group);
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        //public string gender { get; set; }

        private string gender;
        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.Gender);
            }
        }
    }
}
