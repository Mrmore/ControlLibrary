using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class V56GetVideoEntity : PropertyChangedBase
    {
        [DataMember]
        private long vid;
        public long Vid
        {
            get
            {
                return vid;
            }
            set
            {
                vid = value;
                this.NotifyPropertyChanged(entity => entity.Vid);
            }
        }

        [DataMember]
        private string textid;
        public string TextId
        {
            get
            {
                return textid;
            }
            set
            {
                textid = value;
                this.NotifyPropertyChanged(entity => entity.TextId);
            }
        }

        [DataMember]
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                this.NotifyPropertyChanged(entity => entity.Key);
            }
        }

        [DataMember]
        private string subject;
        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
                this.NotifyPropertyChanged(entity => entity.Subject);
            }
        }

        [DataMember]
        private string img;
        public string Img
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                this.NotifyPropertyChanged(entity => entity.Img);
            }
        }

        [DataMember]
        private long duration;
        public long Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                this.NotifyPropertyChanged(entity => entity.Duration);
            }
        }

        [DataMember]
        private long hd;
        public long HD
        {
            get
            {
                return hd;
            }
            set
            {
                hd = value;
                this.NotifyPropertyChanged(entity => entity.HD);
            }
        }

        [DataMember]
        private long cid2;
        public long Cid2
        {
            get
            {
                return cid2;
            }
            set
            {
                cid2 = value;
                this.NotifyPropertyChanged(entity => entity.Cid2);
            }
        }

        [DataMember]
        private ObservableCollection<V56VideoRFileEntity> rfiles;
        public ObservableCollection<V56VideoRFileEntity> RFiles
        {
            get
            {
                return rfiles;
            }
            set
            {
                rfiles = value;
                this.NotifyPropertyChanged(entity => entity.RFiles);
            }
        }
    }
}
