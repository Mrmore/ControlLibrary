using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PhotoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 照片id
        /// </summary>
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
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Id);
            }
        }

        /// <summary>
        /// 相册id
        /// </summary>
        [DataMember]
        private long album_id;
        public long Album_id
        {
            get
            {
                return album_id;
            }
            set
            {
                album_id = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Album_id);
            }
        }

        [DataMember]
        private string album_name = string.Empty;
        public string AlbumName
        {
            get
            {
                return album_name;
            }
            set
            {
                album_name = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.AlbumName);
            }
        }

        /// <summary>
        /// 所有者id
        /// </summary>
        [DataMember]
        //public int user_id { get; set; }

        private int user_id;
        public int User_id
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.User_id);
            }
        }

        /// <summary>
        /// 照片小图片
        /// </summary>
        [DataMember]
        //public string img_head { get; set; }

        private string img_head;
        public string Img_head
        {
            get
            {
                return img_head;
            }
            set
            {
                img_head = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Img_head);
            }
        }

        /// <summary>
        /// 照片大图片
        /// </summary>
        [DataMember]
        //public string img_large { get; set; }

        private string img_large;
        public string Img_large
        {
            get
            {
                return img_large;
            }
            set
            {
                img_large = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Img_large);
            }
        }

        /// <summary>
        /// 照片中等图
        /// </summary>
        [DataMember]
        private string img_main;
        public string Img_main
        {
            get
            {
                return img_main;
            }
            set
            {
                img_main = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Img_main);
            }
        }

        /// <summary>
        /// 照片描述
        /// </summary>
        [DataMember]
        //public string caption { get; set; }

        private string caption;
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Caption);
            }
        }

        /// <summary>
        /// 照片时间
        /// </summary>
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
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Time);
            }
        }

        /// <summary>
        /// 浏览次数
        /// </summary>
        [DataMember]
        //public int view_count { get; set; }

        private int view_count;
        public int View_count
        {
            get
            {
                return view_count;
            }
            set
            {
                view_count = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.View_count);
            }
        }

        /// <summary>
        /// 大图宽度
        /// </summary>
        [DataMember]
        private int img_large_width;
        public int Img_large_width
        {
            get
            {
                return img_large_width;
            }
            set
            {
                img_large_width = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Img_large_width);
            }
        }

        /// <summary>
        /// 大图高度
        /// </summary>
        [DataMember]
        private int img_large_height;
        public int Img_large_height
        {
            get
            {
                return img_large_height;
            }
            set
            {
                img_large_height = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Img_large_height);
            }
        }

        /// <summary>
        /// 评论次数
        /// </summary>
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
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.Comment_count);
            }
        }

        /// <summary>
        /// 所有者名称
        /// </summary>
        [DataMember]
        //public string user_name { get; set; }

        private string user_name;
        public string User_name
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(PhotoEntity => PhotoEntity.User_name);
            }
        }

        private string user_head = String.Empty;
        public string User_Head
        {
            get
            {
                return user_head;
            }
            set
            {
                user_head = value;
                this.NotifyPropertyChanged(entity => entity.User_Head);
            }
        }
    }
}
