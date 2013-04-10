using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PhotoCommentEntity : PropertyChangedBase
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
                this.NotifyPropertyChanged(photoCommentEntity => photoCommentEntity.Count);
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
                this.NotifyPropertyChanged(photoCommentEntity => photoCommentEntity.Comment_list);
            }
        }
    }
}
