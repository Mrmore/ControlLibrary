using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class AlbumListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 相册列表信息
        /// </summary>
        [DataMember]
        //public List<AlbumEntity> album_list { get; set; }

        private ObservableCollection<AlbumEntity> album_list = new ObservableCollection<AlbumEntity>();
        public ObservableCollection<AlbumEntity> Album_list
        {
            get
            {
                return album_list;
            }
            set
            {
                album_list = value;
                this.NotifyPropertyChanged(albumListEntity => albumListEntity.Album_list);
            }
        }

        /// <summary>
        /// 相册数量 
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
                this.NotifyPropertyChanged(albumListEntity => albumListEntity.Count);
            }
        }
    }
}
