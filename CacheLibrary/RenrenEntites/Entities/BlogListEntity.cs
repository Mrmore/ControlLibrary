using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class BlogListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户id 
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
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.User_id);
            }
        }

        /// <summary>
        /// 用户名称 
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
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.User_name);
            }
        }

        /// <summary>
        /// 日志个数 
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
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Count);
            }
        }

        /// <summary>
        /// 日志数据 
        /// </summary>
        [DataMember]
        //public List<BlogEntity> blog_list { get; set; }

        private ObservableCollection<BlogEntity> blog_list = new ObservableCollection<BlogEntity>();
        public ObservableCollection<BlogEntity> Blog_list
        {
            get
            {
                return blog_list;
            }
            set
            {
                blog_list = value;
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Blog_list);
            }
        }
    }
}
