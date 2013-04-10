using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class StatusListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 状态数
        /// </summary>
        [DataMember]
        //public int count { get; set; }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                this.NotifyPropertyChanged(statusListEntity => statusListEntity.Count);
            }
        }

        /// <summary>
        /// 所有状态信息
        /// </summary>
        [DataMember]
        //public List<StatusEntity> status_list { get; set; }

        private ObservableCollection<StatusEntity> status_list = new ObservableCollection<StatusEntity>();
        public ObservableCollection<StatusEntity> Status_list
        {
            get
            {
                return status_list;
            }
            set
            {
                status_list = value;
                this.NotifyPropertyChanged(statusListEntity => statusListEntity.Status_list);
            }
        }
    }
}
