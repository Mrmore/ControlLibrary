using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AttachmentEntity : PropertyChangedBase
    {
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
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Url);
            }
        }

        [DataMember]
        //public string main_url { get; set; }

        private string main_url;
        public string Main_url
        {
            get
            {
                return main_url;
            }
            set
            {
                main_url = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Main_url);
            }
        }

        [DataMember]
        //public string main_url { get; set; }

        private string large_url;
        public string Large_url
        {
            get
            {
                return large_url;
            }
            set
            {
                large_url = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Large_url);
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
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Type);
            }
        }

        [DataMember]
        //public string src { get; set; }

        private string src;
        public string Src
        {
            get
            {
                return src;
            }
            set
            {
                src = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Src);
            }
        }

        [DataMember]
        //public long media_id { get; set; }

        private long media_id;
        public long Media_id
        {
            get
            {
                return media_id;
            }
            set
            {
                media_id = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Media_id);
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
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Owner_id);
            }
        }

        [DataMember]
        //public string digest { get; set; }

        private string digest;
        public string Digest
        {
            get
            {
                return digest;
            }
            set
            {
                digest = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Digest);
            }
        }

        [DataMember]
        //public int comment_count { get; set; }

        private int comment_count;
        public int Comment_count
        {
            get
            {
                return comment_count;
            }
            set
            {
                comment_count = value;
                this.NotifyPropertyChanged(attachmentEntity => attachmentEntity.Comment_count);
            }
        }
    }
}
