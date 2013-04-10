using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class BlogDetailEntity : PropertyChangedBase
    {
        /// <summary>
        /// 日志id
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Id);
            }
        }

        /// <summary>
        /// 日志所有者id 
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.User_id);
            }
        }

        /// <summary>
        /// 所有者姓名。 
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.User_name);
            }
        }

        /// <summary>
        /// 所有者头像。 
        /// </summary>
        [DataMember]
        //public string head_url { get; set; }

        private string head_url;
        public string Head_url
        {
            get
            {
                return head_url;
            }
            set
            {
                head_url = value;
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Head_url);
            }
        }

        /// <summary>
        /// 日志标题。 
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Title);
            }
        }

        /// <summary>
        /// 发表时间。
        /// </summary>
        [DataMember]
        //public string time { get; set; }

        private string time;
        public string Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Time);
            }
        }

        /// <summary>
        /// 日志内容 
        /// </summary>
        [DataMember]
        //public string content { get; set; }

        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Content);
            }
        }

        /// <summary>
        /// 查看次数。
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.View_count);
            }
        }

        /// <summary>
        /// 评论次数。
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Comment_count);
            }
        }

        /// <summary>
        /// 权限范围，有4个int值: 99(所有人),4(密码保护) ,1(好友), -1(仅自己可见)。 
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
                this.NotifyPropertyChanged(blogDetailEntity => blogDetailEntity.Visible);
            }
        }
    }

}
