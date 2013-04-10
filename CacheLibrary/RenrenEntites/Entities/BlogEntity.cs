using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class BlogEntity : PropertyChangedBase
    {
        /// <summary>
        /// 日志id
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Id);
            }
        }

        /// <summary>
        /// 发表时间
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Time);
            }
        }

        /// <summary>
        /// 日志种类
        /// </summary>
        [DataMember]
        //public string cate { get; set; }

        private string cate;
        public string Cate
        {
            get
            {
                return cate;
            }
            set
            {
                cate = value;
                this.NotifyPropertyChanged(blogEntity => blogEntity.Cate);
            }
        }

        /// <summary>
        /// 日志标题
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Title);
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Content);
            }
        }

        /// <summary>
        /// 查看次数
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.View_count);
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Comment_count);
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Visible);
            }
        }

        [DataMember]
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
                this.NotifyPropertyChanged(blogEntity => blogEntity.Visible);
            }
        }

        [DataMember]
        private string user_name;
        public string UserName
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(blogEntity => blogEntity.Visible);
            }
        }

        [DataMember]
        private string head_url;
        public string HeadUrl
        {
            get
            {
                return head_url;
            }
            set
            {
                head_url = value;
                this.NotifyPropertyChanged(blogEntity => blogEntity.Visible);
            }
        }

        [DataMember]
        private string share_count;
        public string ShareCount
        {
            get
            {
                return share_count;
            }
            set
            {
                share_count = value;
                this.NotifyPropertyChanged(blogEntity => blogEntity.Visible);
            }
        }
    }

}
