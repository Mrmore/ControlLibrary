using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PageInfoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 公共主页id
        /// </summary>
        [DataMember]
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
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Id);
            }
        }

        /// <summary>
        /// 公共主页名
        /// </summary>
        [DataMember]
        private string page_name;
        public string Page_name
        {
            get
            {
                return page_name;
            }
            set
            {
                page_name = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Page_name);
            }
        }

        /// <summary>
        /// 公共主页头像
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
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Head_url);
            }
        }

        /// <summary>
        /// 当前用户是否是次公共主页的粉丝
        /// </summary>
        [DataMember]
        private string is_fan;
        public string Is_fan
        {
            get
            {
                return is_fan;
            }
            set
            {
                is_fan = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Is_fan);
            }
        }

        /// <summary>
        /// 公共主页是否通过验证
        /// </summary>
        [DataMember]
        private string is_checked;
        public string Is_checked
        {
            get
            {
                return is_checked;
            }
            set
            {
                is_checked = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Is_checked);
            }
        }

        /// <summary>
        /// 公共主页状态
        /// </summary>
        [DataMember]
        PageStatusEntity status;
        public PageStatusEntity Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Status);
            }
        }


        /// <summary>
        /// 公共主页描述
        /// </summary>
        [DataMember]
        private string desc;
        public string Desc
        {
            get
            {
                return desc;
            }
            set
            {
                desc = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Desc);
            }
        }

        /// <summary>
        /// 公共主页相册个数
        /// </summary>
        [DataMember]
        private int albums_count;
        public int Albums_count
        {
            get
            {
                return albums_count;
            }
            set
            {
                albums_count = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Albums_count);
            }
        }

        /// <summary>
        /// 公共主页日志数
        /// </summary>
        [DataMember]
        private int blogs_count;
        public int Blogs_count
        {
            get
            {
                return blogs_count;
            }
            set
            {
                blogs_count = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Blogs_count);
            }
        }

        /// <summary>
        /// 公共主页粉丝数
        /// </summary>
        [DataMember]
        private int fans_count;
        public int Fans_count
        {
            get
            {
                return fans_count;
            }
            set
            {
                fans_count = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Fans_count);
            }
        }

        /// <summary>
        /// 公共主页基本信息
        /// </summary>
        [DataMember]
        private ObservableCollection<PageBaseInfoEntity> base_info = new ObservableCollection<PageBaseInfoEntity>();
        public ObservableCollection<PageBaseInfoEntity> Base_info
        {
            get
            {
                return base_info;
            }
            set
            {
                base_info = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Base_info);
            }
        }

        /// <summary>
        /// 公共主页详细信息
        /// </summary>
        [DataMember]
        private ObservableCollection<PageBaseInfoEntity> detail_info = new ObservableCollection<PageBaseInfoEntity>();
        public ObservableCollection<PageBaseInfoEntity> Detail_info
        {
            get
            {
                return detail_info;
            }
            set
            {
                detail_info = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Detail_info);
            }
        }

        /// <summary>
        /// 公共主页联系信息
        /// </summary>
        [DataMember]
        private ObservableCollection<PageBaseInfoEntity> contact_info = new ObservableCollection<PageBaseInfoEntity>();
        public ObservableCollection<PageBaseInfoEntity> Contact_info
        {
            get
            {
                return contact_info;
            }
            set
            {
                contact_info = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.Contact_info);
            }
        }


        /// <summary>
        /// 大头像
        /// </summary>
        private string lagerHeaderUrl;
        public string LagerHeaderUrl
        {
            get
            {
                return lagerHeaderUrl;
            }
            set
            {
                lagerHeaderUrl = value;
                this.NotifyPropertyChanged(pageInfoEntity => pageInfoEntity.LagerHeaderUrl);
            }
        }

        public FriendEntity toFriend()
        {
            FriendEntity friend = new FriendEntity();
            friend.User_id = this.id;
            friend.User_name = this.page_name;
            friend.Network = this.desc;
            friend.Head_url = this.head_url;

            return friend;
        }
    }

    /// <summary>
    /// 公共主页状态信息
    /// </summary>
    [DataContract]
    public class PageStatusEntity : PropertyChangedBase
    {
        /// <summary>
        /// 状态id
        /// </summary>
        [DataMember]
        private long status_id;
        public long Status_id
        {
            get
            {
                return status_id;
            }
            set
            {
                status_id = value;
                this.NotifyPropertyChanged(pageStatusEntity => pageStatusEntity.Status_id);
            }
        }

        /// <summary>
        /// 状态时间
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
                this.NotifyPropertyChanged(pageStatusEntity => pageStatusEntity.Time);
            }
        }

        /// <summary>
        /// 状态内容
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
                this.NotifyPropertyChanged(pageStatusEntity => pageStatusEntity.Content);
            }
        }
    }

    /// <summary>
    /// 公共主页状态信息
    /// </summary>
    [DataContract]
    public class PageBaseInfoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [DataMember]
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
                this.NotifyPropertyChanged(pageBaseInfoEntity => pageBaseInfoEntity.Key);
            }
        }

        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        private string value;
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                this.NotifyPropertyChanged(pageBaseInfoEntity => pageBaseInfoEntity.Value);
            }
        }
    }
}
