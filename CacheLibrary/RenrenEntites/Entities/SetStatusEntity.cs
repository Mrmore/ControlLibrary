using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class SetStatusEntity : PropertyChangedBase
    {
        /// <summary>
        /// 修改状态成功，值为1
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
                this.NotifyPropertyChanged(setStatusEntity => setStatusEntity.Result);
            }
        }
    }
}
