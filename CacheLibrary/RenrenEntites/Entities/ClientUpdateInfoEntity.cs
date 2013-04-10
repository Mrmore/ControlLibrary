using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class ClientUpdateInfoEntity : PropertyChangedBase
    {
        [DataMember]
        //public string info { get; set; }

        private string info;
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                this.NotifyPropertyChanged(clientUpdateInfoEntity => clientUpdateInfoEntity.Info);
            }
        }

        [DataMember]
        //public string pubdate { get; set; }

        private string pubdate;
        public string Pubdate
        {
            get
            {
                return pubdate;
            }
            set
            {
                pubdate = value;
                this.NotifyPropertyChanged(clientUpdateInfoEntity => clientUpdateInfoEntity.Pubdate);
            }
        }

        [DataMember]
        //public string type { get; set; }

        private string type;
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                this.NotifyPropertyChanged(clientUpdateInfoEntity => clientUpdateInfoEntity.Type);
            }
        }

        [DataMember]
        //public string url { get; set; }

        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                this.NotifyPropertyChanged(clientUpdateInfoEntity => clientUpdateInfoEntity.Url);
            }
        }
    }
}
