using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class UserEntity : PropertyChangedBase
    {
        /// <summary>
        /// 表示当前用户的session_key，有一定的有效期。
        /// </summary>
        [DataMember]
        //public string session_key { get; set; }

        private string session_key;
        public string Session_key
        {
            get
            {
                return session_key;
            }
            set
            {
                session_key = value;
                this.NotifyPropertyChanged(userEntity => userEntity.Session_key);
            }
        }

        /// <summary>
        /// 用户的票。
        /// </summary>
        [DataMember]
        //public string ticket { get; set; }

        private string ticket;
        public string Ticket
        {
            get
            {
                return ticket;
            }
            set
            {
                ticket = value;
                this.NotifyPropertyChanged(userEntity => userEntity.Ticket);
            }
        }

        /// <summary>
        /// 表示当前用户的ID。
        /// </summary>
        [DataMember]
        //public int uid { get; set; }

        private int uid;
        public int Uid
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value;
                this.NotifyPropertyChanged(userEntity => userEntity.Uid);
            }
        }

        /// <summary>
        /// 用户的密钥。
        /// </summary>
        [DataMember]
        //public string secret_key { get; set; }

        private string secret_key;
        public string Secret_key
        {
            get
            {
                return secret_key;
            }
            set
            {
                secret_key = value;
                this.NotifyPropertyChanged(userEntity => userEntity.Secret_key);
            }
        }

        /// <summary>
        /// 用户名。
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
                this.NotifyPropertyChanged(userEntity => userEntity.User_name);
            }
        }

        /// <summary>
        /// 用户的头像
        /// </summary>
        [DataMember]
        //public string head_url { get; set; }

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
                this.NotifyPropertyChanged(userEntity => userEntity.Head_url);
            }
        }

        /// <summary>
        /// ？
        /// </summary>
        [DataMember]
        //public string now { get; set; }

        private string now;
        public string Now
        {
            get
            {
                return now;
            }
            set
            {
                now = value;
                this.NotifyPropertyChanged(userEntity => userEntity.Now);
            }
        }

        /// <summary>
        /// ？
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
                this.NotifyPropertyChanged(userEntity => userEntity.Is_online);
            }
        }
    }
}
