using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class UploadPhotoEntity : PropertyChangedBase
    {
        [DataMember]
        //public long photo_id { get; set; }

        private long photo_id;
        public long Photo_id
        {
            get
            {
                return photo_id;
            }
            set
            {
                photo_id = value;
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Photo_id);
            }
        }

        [DataMember]
        //public long album_id { get; set; }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Album_id);
            }
        }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.User_id);
            }
        }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Img_head);
            }
        }

        [DataMember]
        //public string img_main { get; set; }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Img_main);
            }
        }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Img_large);
            }
        }

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
                this.NotifyPropertyChanged(uploadPhotoEntity => uploadPhotoEntity.Caption);
            }
        }
    }
}
