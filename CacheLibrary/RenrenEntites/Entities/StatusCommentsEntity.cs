using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class StatusCommentsEntity : PropertyChangedBase
    {
        /// <summary>
        /// 评论总数
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
                this.NotifyPropertyChanged(statusCommentsEntity => statusCommentsEntity.Count);
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        //public int page_size { get; set; }

        private int page_size;
        public int Page_size
        {
            get
            {
                return page_size;
            }
            set
            {
                page_size = value;
                this.NotifyPropertyChanged(statusCommentsEntity => statusCommentsEntity.Page_size);
            }
        }

        /// <summary>
        /// 所有评论信息
        /// </summary>
        [DataMember]
        //public List<CommentEntity> comment_list { get; set; }

        private ObservableCollection<CommentEntity> comment_list = new ObservableCollection<CommentEntity>();
        public ObservableCollection<CommentEntity> Comment_list
        {
            get
            {
                return comment_list;
            }
            set
            {
                comment_list = value;
                this.NotifyPropertyChanged(statusCommentsEntity => statusCommentsEntity.Comment_list);
            }
        }
    }
}
