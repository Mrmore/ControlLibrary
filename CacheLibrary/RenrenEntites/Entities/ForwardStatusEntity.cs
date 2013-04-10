using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class ForwardStatusEntity : PropertyChangedBase
    {
        [DataMember]
        //public long id { get; set; }

        private long id;
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.NotifyPropertyChanged(forwardStatusEntity => forwardStatusEntity.Id);
            }
        }

        [DataMember]
        //public string status { get; set; }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.NotifyPropertyChanged(forwardStatusEntity => forwardStatusEntity.Status);
            }
        }

        [DataMember]
        //public int owner_id { get; set; }

        private int owner_id;
        public int Owner_id
        {
            get
            {
                return owner_id;
            }
            set
            {
                owner_id = value;
                this.NotifyPropertyChanged(forwardStatusEntity => forwardStatusEntity.Owner_id);
            }
        }

        [DataMember]
        //public string owner_name { get; set; }

        private string owner_name;
        public string Owner_name
        {
            get
            {
                return owner_name;
            }
            set
            {
                owner_name = value;
                this.NotifyPropertyChanged(forwardStatusEntity => forwardStatusEntity.Owner_name);
            }
        }

        [DataMember]
        //用户名 ： 状态内容 StatusEntity  user_name : content
        private string statusContentString;
        public string StatusContentString
        {
            get
            {
                return string.Format("{0} : {1}", owner_name, status);
            }
            set
            {
                statusContentString = value;
                this.NotifyPropertyChanged(forwardStatusEntity => forwardStatusEntity.StatusContentString);
            }
        }
    }
}
