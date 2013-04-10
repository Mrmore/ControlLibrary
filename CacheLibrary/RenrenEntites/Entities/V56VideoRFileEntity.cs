using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class V56VideoRFileEntity : PropertyChangedBase
    {
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
        private long filesize;
        public long FileSize
        {
            get
            {
                return filesize;
            }
            set
            {
                filesize = value;
                this.NotifyPropertyChanged(entity => entity.FileSize);
            }
        }

        [DataMember]
        private long totaltime;
        public long TotalTime
        {
            get
            {
                return totaltime;
            }
            set
            {
                totaltime = value;
                this.NotifyPropertyChanged(entity => entity.TotalTime);
            }
        }

        [DataMember]
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
                this.NotifyPropertyChanged(entity => entity.Type);
            }
        }
    }
}
