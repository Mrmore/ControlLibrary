using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class LoginUserInfo : PropertyChangedBase
    {
        [DataMember]
        private string username;
        public string UserName
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                this.NotifyPropertyChanged(userInfo => userInfo.UserName);
            }
        }

        [DataMember]
        private string password;
        public string PassWord
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                this.NotifyPropertyChanged(userInfo => userInfo.PassWord);
            }
        }

        [DataMember]
        private UserEntity loginInfo;
        public UserEntity LoginInfo
        {
            get
            {
                return loginInfo;
            }
            set
            {
                loginInfo = value;
                this.NotifyPropertyChanged(entity => entity.LoginInfo);
            }
        }
    }
}
