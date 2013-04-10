using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PoiListEntity : PropertyChangedBase
    {
        [DataMember]
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
                this.NotifyPropertyChanged(entity => entity.Count);
            }
        }

        [DataMember]
        private PlaceInfoEntity info;
        public PlaceInfoEntity PoiInfo
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                this.NotifyPropertyChanged(entity => entity.PoiInfo);
            }
        }
    }
}
