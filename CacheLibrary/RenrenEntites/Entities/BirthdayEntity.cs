using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class BirthdayEntity : PropertyChangedBase
    {
        /// <summary>
        ///生日年（返回x0后） 
        /// </summary>
        [DataMember]
        //public string year { get; set; }

        private string year;
        public string Year
        {
            get
            {
                return year;
            }
            set
            {
                year = value;
                this.NotifyPropertyChanged(birthdayEntity => birthdayEntity.Year);
            }
        }

        /// <summary>
        /// 生日月
        /// </summary>
        [DataMember]
        //public int month { get; set; }

        private int month;
        public int Month
        {
            get
            {
                return month;
            }
            set
            {
                month = value;
                this.NotifyPropertyChanged(birthdayEntity => birthdayEntity.Month);
            }
        }

        /// <summary>
        /// 生日天
        /// </summary>
        [DataMember]
        //public int day { get; set; }

        private int day;
        public int Day
        {
            get
            {
                return day;
            }
            set
            {
                day = value;
                this.NotifyPropertyChanged(birthdayEntity => birthdayEntity.Day);
            }
        } 
    }
}
