using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class DenyFriendEntity : PropertyChangedBase
    {
        /// <summary>
        /// 为1表示成功
        /// </summary>
        [DataMember]
        //public string result { get; set; }

        private string result;
        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
                this.NotifyPropertyChanged(denyFriendEntity => denyFriendEntity.Result);
            }
        }
    }
}
