using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AcceptFriendEntity : PropertyChangedBase
    {
        /// <summary>
        /// 为“1”表示成功
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(acceptFriendEntity => acceptFriendEntity.Result);
            }
        }
    }
}
