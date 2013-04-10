using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FeedLikeEntity : PropertyChangedBase
    {
        [DataMember]
        private string gid;
        public string Gid
        {
            get
            {
                return gid;
            }
            set
            {
                gid = value;
                this.NotifyPropertyChanged(entity => entity.Gid);
            }
        }
    }
}
