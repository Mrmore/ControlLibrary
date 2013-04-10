using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using Windows.ApplicationModel.Resources;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class FeedEntity : PropertyChangedBase
    {
        /// <summary>
        /// 表示新鲜事的id
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Id);
            }
        }

        /// <summary>
        /// 表示新鲜事内容主体的id，例如日志id、相册id和分享id等等
        /// </summary>
        [DataMember]
        private long source_id;
        public long Source_id
        {
            get
            {
                return source_id;
            }
            set
            {
                source_id = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Source_id);
            }
        }

        /// <summary>
        /// 表示新鲜事类型
        /// </summary>
        [DataMember]
        private int type;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Type);
            }
        }

        /// <summary>
        /// 表示新鲜事更新时间
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Time);
            }
        }

        /// <summary>
        /// 可选的格式化时间
        /// </summary>
        [DataMember]
        private string str_time;
        public string Str_time
        {
            get
            {
                return str_time;
            }
            set
            {
                str_time = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Str_time);
            }
        }

        /// <summary>
        /// 表示新鲜事用户的id
        /// </summary>
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.User_id);
            }
        }

        /// <summary>
        /// 表示新鲜事用户的姓名
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.User_name);
            }
        }

        /// <summary>
        /// 表示新鲜事用户的头像
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Head_url);
            }
        }

        /// <summary>
        /// 表示新鲜事内容的前缀
        /// </summary>
        [DataMember]
        private string prefix;
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Prefix);
            }
        }

        /// <summary>
        /// 表示新鲜事用户自定义输入内容，状态
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Content);
            }
        }

        /// <summary>
        ///表示新鲜事的主题内容 
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Title);
            }
        }

        /// <summary>
        /// 表示新鲜事主题的相关链接
        /// </summary>
        [DataMember]
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Url);
            }
        }

        /// <summary>
        /// 表示新鲜事来源type
        /// </summary>
        [DataMember]
        private int origin_type;
        public int Origin_type
        {
            get
            {
                return origin_type;
            }
            set
            {
                origin_type = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Origin_type);
            }
        }

        /// <summary>
        /// 表示新鲜事来源描述
        /// </summary>
        [DataMember]
        private string origin_title;
        public string Origin_title
        {
            get
            {
                return origin_title;
            }
            set
            {
                origin_title = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Origin_title);
            }
        }

        /// <summary>
        /// 表示新鲜事来源图标地址
        /// </summary>
        [DataMember]
        private string origin_img;
        public string Origin_img
        {
            get
            {
                return origin_img;
            }
            set
            {
                origin_img = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Origin_img);
            }
        }

        /// <summary>
        /// 表示新鲜事来源pageid
        /// </summary>
        [DataMember]
        private int origin_page_id;
        public int Origin_page_id
        {
            get
            {
                return origin_page_id;
            }
            set
            {
                origin_page_id = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Origin_page_id);
            }
        }

        /// <summary>
        /// 表示新鲜事来源指定url
        /// </summary>
        [DataMember]
        private string origin_url;
        public string Origin_url
        {
            get
            {
                return origin_url;
            }
            set
            {
                origin_url = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Origin_url);
            }
        }

        /// <summary>
        /// 表示新鲜事的具体内容 如日志内容
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Description);
            }
        }

        /// <summary>
        /// 表示评论的数量
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Comment_count);
            }
        }

        /// <summary>
        /// 表示新鲜事中包含的评论内容，目前返回最新和最早的评论
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(feedEntity => feedEntity.Comment_list);
            }
        }

        /// <summary>
        /// 附件列表
        /// </summary>
        [DataMember]
        private ObservableCollection<AttachmentEntity> attachement_list = new ObservableCollection<AttachmentEntity>();
        public ObservableCollection<AttachmentEntity> Attachement_list
        {
            get
            {
                return attachement_list;
            }
            set
            {
                attachement_list = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Attachement_list);
            }
        }

        /// <summary>
        /// 转发状态源内容
        /// </summary>
        [DataMember]
        private ForwardStatusEntity status_forward = new ForwardStatusEntity();
        public ForwardStatusEntity Status_forward
        {
            get
            {
                return status_forward;
            }
            set
            {
                status_forward = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Status_forward);
            }
        }

        /// <summary>
        /// 分享的源信息
        /// </summary>
        [DataMember]
        private ShareEntity share = new ShareEntity();
        public ShareEntity Share
        {
            get
            {
                return share;
            }
            set
            {
                share = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Share);
            }
        }

        [DataMember]
        private FeedPlaceEntity place = new FeedPlaceEntity();
        public FeedPlaceEntity Place
        {
            get
            {
                return place;
            }
            set
            {
                place = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Place);
            }
        }

        [DataMember]
        private FeedLikeEntity like = new FeedLikeEntity();
        public FeedLikeEntity Like
        {
            get
            {
                return like;
            }
            set
            {
                like = value;
                this.NotifyPropertyChanged(feedEntity => feedEntity.Like);
            }
        }
    }
}
