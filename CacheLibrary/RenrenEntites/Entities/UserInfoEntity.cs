using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class UserInfoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 用户id
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
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.User_id);
            }
        }

        /// <summary>
        /// 用户姓名
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
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.User_name);
            }
        }

        /// <summary>
        /// 用户头像
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
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Head_url);
            }
        }

        [DataMember]
        private string large_header;
        public string Large_Header
        {
            get
            {
                return Helper.ApiHelper.GetLargeHeaderUrl(user_id, 310);
            }
            set
            {
                large_header = value;
                this.NotifyPropertyChanged(friendEntity => friendEntity.Large_Header);
            }
        }

        /// <summary>
        /// 来访时间
        /// </summary>
        [DataMember]
        private string is_star;
        public string Is_star
        {
            get
            {
                return is_star;
            }
            set
            {
                is_star = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Is_star);
            }
        }

        /// <summary>
        /// 用户所属网络列表
        /// </summary>
        [DataMember]
        private ObservableCollection<string> network = new ObservableCollection<string>();
        public ObservableCollection<string> Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Network);
            }
        }

        /// <summary>
        /// 性别（1：男，0：女）
        /// </summary>
        [DataMember]
        private int gender;
        public int Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Gender);
            }
        }

        /// <summary>
        /// 用户生日
        /// </summary>
        [DataMember]
        private BirthdayEntity birth = new BirthdayEntity();
        public BirthdayEntity Birth
        {
            get
            {
                return birth;
            }
            set
            {
                birth = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Birth);
            }
        }

        /// <summary>
        /// 家乡 省
        /// </summary>
        [DataMember]
        private string hometown_province;
        public string Hometown_province
        {
            get
            {
                return hometown_province;
            }
            set
            {
                hometown_province = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Hometown_province);
            }
        }

        /// <summary>
        /// 家乡 城市
        /// </summary>
        [DataMember]
        private string hometown_city;
        public string Hometown_city
        {
            get
            {
                return hometown_city;
            }
            set
            {
                hometown_city = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Hometown_city);
            }
        }

        /// <summary>
        /// 用户最新的状态
        /// </summary>
        [DataMember]
        private StatusEntity status = new StatusEntity();
        public StatusEntity Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Status);
            }
        }

        /// <summary>
        /// 与登录用户是否是好友(1:是，2：不是)
        /// </summary>
        [DataMember]
        private int is_friend;
        public int Is_friend
        {
            get
            {
                return is_friend;
            }
            set
            {
                is_friend = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Is_friend);
            }
        }

        /// <summary>
        /// 访问人数
        /// </summary>
        [DataMember]
        private int visitor_count;
        public int Visitor_count
        {
            get
            {
                return visitor_count;
            }
            set
            {
                visitor_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Visitor_count);
            }
        }

        /// <summary>
        /// 日志数
        /// </summary>
        [DataMember]
        private int blog_count;
        public int Blog_count
        {
            get
            {
                return blog_count;
            }
            set
            {
                blog_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Blog_count);
            }
        }

        /// <summary>
        /// 相册数
        /// </summary>
        [DataMember]
        private int album_count;
        public int Album_count
        {
            get
            {
                return album_count;
            }
            set
            {
                album_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Album_count);
            }
        }

        /// <summary>
        /// 好友数
        /// </summary>
        [DataMember]
        private int friend_count;
        public int Friend_count
        {
            get
            {
                return friend_count;
            }
            set
            {
                friend_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Friend_count);
            }
        }

        /// <summary>
        /// 留言数
        /// </summary>
        [DataMember]
        private int gossip_count;
        public int Gossip_count
        {
            get
            {
                return gossip_count;
            }
            set
            {
                gossip_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Gossip_count);
            }
        }

        /// <summary>
        /// 与登录用户的共同好友数
        /// </summary>
        [DataMember]
        private int share_friends_count;
        public int Share_friends_count
        {
            get
            {
                return share_friends_count;
            }
            set
            {
                share_friends_count = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Share_friends_count);
            }
        }

        /// <summary>
        /// 是否有访问权限（0:有，1：没有）
        /// </summary>
        [DataMember]
        private int has_right;
        public int Has_right
        {
            get
            {
                return has_right;
            }
            set
            {
                has_right = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Has_right);
            }
        }

        /// <summary>
        /// vip等级(1-6)
        /// </summary>
        [DataMember]
        private int vip_level;
        public int Vip_level
        {
            get
            {
                return vip_level;
            }
            set
            {
                vip_level = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Vip_level);
            }
        }

        /// <summary>
        /// 用户当前vip的状态（按位表示状态（1是0不是），0：不是vip； 
        /// vip_stat&1: 是否是vip； vip_stat&2：曾经是否是vip； vip_stat&4：是否是迪士尼vip）
        /// </summary>
        [DataMember]
        private int vip_stat;
        public int Vip_stat
        {
            get
            {
                return vip_stat;
            }
            set
            {
                vip_stat = value;
                this.NotifyPropertyChanged(userInfoEntity => userInfoEntity.Vip_stat);
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

        /// <summary>
        /// 用户头像列表
        /// </summary>
        [DataMember]
        private UserUrlsEntity user_urls;
        public UserUrlsEntity UserHeadUrls
        {
            get
            {
                return user_urls;
            }
            set
            {
                user_urls = value;
                this.NotifyPropertyChanged(entity => entity.UserHeadUrls);
            }
        }
    }
}
