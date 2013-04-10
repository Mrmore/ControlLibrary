using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class ShareEntity : PropertyChangedBase
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
                this.NotifyPropertyChanged(shareEntity => shareEntity.Id);
            }
        }

        [DataMember]
        //public int type { get; set; }

        private int type;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Type);
            }
        }

        [DataMember]
        //public long time { get; set; }

        private long time;
        public long Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Time);
            }
        }

        [DataMember]
        //public long source_id { get; set; }

        private long source_id;
        public long Source_id
        {
            get
            {
                return source_id;
            }
            set
            {
                source_id = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Source_id);
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
                this.NotifyPropertyChanged(shareEntity => shareEntity.Owner_id);
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
                this.NotifyPropertyChanged(shareEntity => shareEntity.Owner_name);
            }
        }

        [DataMember]
        //public int source_owner_id { get; set; }

        private int source_owner_id;
        public int Source_owner_id
        {
            get
            {
                return source_owner_id;
            }
            set
            {
                source_owner_id = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Source_owner_id);
            }
        }

        [DataMember]
        //public string source_owner_name { get; set; }

        private string source_owner_name;
        public string Source_owner_name
        {
            get
            {
                return source_owner_name;
            }
            set
            {
                source_owner_name = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Source_owner_name);
            }
        }

        [DataMember]
        //public string title { get; set; }

        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Title);
            }
        }

        [DataMember]
        //public string photo { get; set; }

        private string photo;
        public string Photo
        {
            get
            {
                return photo;
            }
            set
            {
                photo = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Photo);
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
                this.NotifyPropertyChanged(shareEntity => shareEntity.Url);
            }
        }

        [DataMember]
        private string video_url;
        public string VideoUrl
        {
            get
            {
                return video_url;
            }
            set
            {
                video_url = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.VideoUrl);
            }
        }

        [DataMember]
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
                this.NotifyPropertyChanged(shareEntity => shareEntity.Comment_count);
            }
        }

        [DataMember]
        private int share_count;
        public int ShareCount
        {
            get
            {
                return share_count;
            }
            set
            {
                share_count = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.ShareCount);
            }
        }

        [DataMember]
        private int view_count;
        public int ViewCount
        {
            get
            {
                return view_count;
            }
            set
            {
                view_count = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.ViewCount);
            }
        }

        [DataMember]
        //public string description { get; set; }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.Description);
            }
        }

        [DataMember]
        private int video_support;
        public int VideoSupport
        {
            get
            {
                return video_support;
            }
            set
            {
                video_support = value;
                this.NotifyPropertyChanged(shareEntity => shareEntity.VideoSupport);
            }
        }
        [DataMember]
        private BlogEntity blog_info;
        public BlogEntity Blog_info
        {
            get
            {
                return blog_info;
            }
            set
            {
                blog_info = value;
                //NotifyPropertyChanged(shareEntity => shareEntity.ShareBlog);
            }
        }
    }
}
