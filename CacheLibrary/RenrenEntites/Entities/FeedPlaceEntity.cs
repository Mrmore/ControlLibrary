using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FeedPlaceEntity : PropertyChangedBase
    {
        [DataMember]
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
                this.NotifyPropertyChanged(entity => entity.Id);
            }
        }

        [DataMember]
        private string pid;
        public string Pid
        {
            get
            {
                return pid;
            }
            set
            {
                pid = value;
                this.NotifyPropertyChanged(entity => entity.Pid);
            }
        }

        [DataMember]
        private string pname;
        public string PlaceName
        {
            get
            {
                return pname;
            }
            set
            {
                pname = value;
                this.NotifyPropertyChanged(entity => entity.PlaceName);
            }
        }

        [DataMember]
        private string address;
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
                this.NotifyPropertyChanged(entity => entity.Address);
            }
        }

        [DataMember]
        private string comment;
        public string Comment 
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
                this.NotifyPropertyChanged(entity => entity.Comment);
            }
        }

        [DataMember]
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
                this.NotifyPropertyChanged(entity => entity.Url);
            }
        }

        [DataMember]
        private long longitude;
        public long Longitude 
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                this.NotifyPropertyChanged(entity => entity.Longitude);
            }
        }

        [DataMember]
        private long latitude;
        public long Latitude 
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                this.NotifyPropertyChanged(entity => entity.Latitude);
            }
        }
    }
}
