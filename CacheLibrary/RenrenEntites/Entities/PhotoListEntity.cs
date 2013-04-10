using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PhotoListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 相册包含照片总数
        /// </summary>
        [DataMember]
        //public int count { get; set; }

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
                this.NotifyPropertyChanged(photoListEntity => photoListEntity.Count);
            }
        }

        /// <summary>
        /// 相册包含的所有相片
        /// </summary>
        [DataMember]
        //public List<PhotoEntity> photo_list { get; set; }

        private ObservableCollection<PhotoEntity> photo_list = new ObservableCollection<PhotoEntity>();
        public ObservableCollection<PhotoEntity> Photo_list
        {
            get
            {
                return photo_list;
            }
            set
            {
                photo_list = value;
                this.NotifyPropertyChanged(photoListEntity => photoListEntity.Photo_list);
            }
        }

        /// <summary>
        /// 相册名
        /// </summary>
        [DataMember]
        //public string album_name { get; set; }

        private string album_name;
        public string Album_name
        {
            get
            {
                return album_name;
            }
            set
            {
                album_name = value;
                this.NotifyPropertyChanged(photoListEntity => photoListEntity.Album_name);
            }
        }
    }
}
