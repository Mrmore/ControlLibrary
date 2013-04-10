using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class CommonReultEntity : PropertyChangedBase
    {
        /// <summary>
        /// 添加公共主页返回值, 值为1表示添加成功,0表示添加失败
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
                this.NotifyPropertyChanged(commentReultEntity => commentReultEntity.Result);
            }
        }
    }
}
