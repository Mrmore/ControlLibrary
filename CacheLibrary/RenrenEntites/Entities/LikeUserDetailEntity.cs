using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class LikeUserDetailEntity : PropertyChangedBase
    {
        [DataMember]
        private int uid;
        public int Uid
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value;
                this.NotifyPropertyChanged(entity => entity.Uid);
            }
        }

        [DataMember]
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.NotifyPropertyChanged(entity => entity.Name);
            }
        }

        [DataMember]
        private string img;
        public string Image 
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                this.NotifyPropertyChanged(entity => entity.Image);
            }
        }
    }
}
