using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PlaceInfoEntity : PropertyChangedBase
    {
        [DataMember]
        private long lon;
        public long Longitude
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
                this.NotifyPropertyChanged(entity => entity.Longitude);
            }
        }

        [DataMember]
        private long lat;
        public long Latitude
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
                this.NotifyPropertyChanged(entity => entity.Latitude);
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
        private string county;
        public string County
        {
            get
            {
                return county;
            }
            set
            {
                county = value;
                this.NotifyPropertyChanged(entity => entity.County);
            }
        }

        [DataMember]
        private string province;
        public string Province
        {
            get
            {
                return province;
            }
            set
            {
                province = value;
                this.NotifyPropertyChanged(entity => entity.Province);
            }
        }

        [DataMember]
        private string street_name;
        public string Street_name
        {
            get
            {
                return street_name;
            }
            set
            {
                street_name = value;
                this.NotifyPropertyChanged(entity => entity.Street_name);
            }
        }

        [DataMember]
        private string nation;
        public string Nation
        {
            get
            {
                return nation;
            }
            set
            {
                nation = value;
                this.NotifyPropertyChanged(entity => entity.Nation);
            }
        }

        [DataMember]
        private string city;
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
                this.NotifyPropertyChanged(entity => entity.City);
            }
        }

        [DataMember]
        private string poi_name;
        public string PoiName
        {
            get
            {
                return poi_name;
            }
            set
            {
                poi_name = value;
                this.NotifyPropertyChanged(entity => entity.PoiName);
            }
        }
    }
}
