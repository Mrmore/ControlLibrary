using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AlbumEntity : PropertyChangedBase
    {
        /// <summary>
        /// 相册id
        /// </summary>
        [DataMember]
        //public int id { get; set; }

        private int id;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Id);
            }
        }


        /// <summary>
        /// 相册封面？
        /// </summary>
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
                this.NotifyPropertyChanged(albumEntity => albumEntity.Img);
            }
        }

        [DataMember]
        private string large_img;
        public string LargeImage
        {
            get
            {
                return large_img;
            }
            set
            {
                large_img = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.LargeImage);
            }
        }

        [DataMember]
        private string main_img;
        public string Main_img
        {
            get
            {
                return main_img;
            }
            set
            {
                main_img = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Main_img);
            }
        }

        /// <summary>
        /// 相册标题
        /// </summary>
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
                this.NotifyPropertyChanged(albumEntity => albumEntity.Title);
            }
        }

        /// <summary>
        /// 相册创建时间
        /// </summary>
        [DataMember]
        //public long create_time { get; set; }

        private long create_time;
        public long Create_time
        {
            get
            {
                return create_time;
            }
            set
            {
                create_time = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Create_time);
            }
        }

        /// <summary>
        /// 相册最后更新时间
        /// </summary>
        [DataMember]
        //public long upload_time { get; set; }

        private long upload_time;
        public long Upload_time
        {
            get
            {
                return upload_time;
            }
            set
            {
                upload_time = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Upload_time);
            }
        }

        /// <summary>
        /// 相册描述
        /// </summary>
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
                this.NotifyPropertyChanged(albumEntity => albumEntity.Description);
            }
        }

        /// <summary>
        /// 位置信息
        /// </summary>
        [DataMember]
        //public string location { get; set; }

        private string location;
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Location);
            }
        }

        /// <summary>
        /// 相册容量
        /// </summary>
        [DataMember]
        //public int size { get; set; }

        private int size;
        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Size);
            }
        }

        /// <summary>
        /// 是否可见？
        /// </summary>
        [DataMember]
        //public int visible { get; set; }

        private int visible;
        public int Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Visible);
            }
        }

        /// <summary>
        /// 评论数量
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
                this.NotifyPropertyChanged(albumEntity => albumEntity.Comment_count);
            }
        }

        /// <summary>
        /// 相册所属人id
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
                this.NotifyPropertyChanged(albumEntity => albumEntity.User_id);
            }
        }

        /// <summary>
        /// 相册是否有密码
        /// </summary>
        [DataMember]
        //public int has_password { get; set; }

        private int has_password;
        public int Has_password
        {
            get
            {
                return has_password;
            }
            set
            {
                has_password = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Has_password);
            }
        }

        /// <summary>
        /// 相册类型
        /// </summary>
        [DataMember]
        //public int album_type { get; set; }

        private int album_type;
        public int Album_type
        {
            get
            {
                return album_type;
            }
            set
            {
                album_type = value;
                this.NotifyPropertyChanged(albumEntity => albumEntity.Album_type);
            }
        }
    }
}
