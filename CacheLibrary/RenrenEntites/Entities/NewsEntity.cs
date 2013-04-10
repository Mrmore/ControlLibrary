using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class NewsEntity : PropertyChangedBase
    {
        /// <summary>
        /// 合并消息的id数组
        /// </summary>
        [DataMember]
        //public List<long> id { get; set; }

        private ObservableCollection<long> id = new ObservableCollection<long>();
        public ObservableCollection<long> Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Id);
            }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [DataMember]
        //public int type { get; set; }

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
                this.NotifyPropertyChanged(newsEntity => newsEntity.Type);
            }
        }

        /// <summary>
        /// 产生消息的用户ID数组（只显示前三个不同用户的id）
        /// </summary>
        [DataMember]
        //public List<int> user_id { get; set; }

        private ObservableCollection<int> user_id = new ObservableCollection<int>();
        public ObservableCollection<int> User_id
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.User_id);
            }
        }

        /// <summary>
        /// 产生消息的用户姓名数组（只显示前三个不同用户的姓名）
        /// </summary>
        [DataMember]
        //public List<string> user_name { get; set; }

        private ObservableCollection<string> user_name = new ObservableCollection<string>();
        public ObservableCollection<string> User_name
        {
            get
            {
                return user_name;
            }
            set
            {
                user_name = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.User_name);
            }
        }

        /// <summary>
        /// 产生消息的用户头像
        /// </summary>
        [DataMember]
        //public List<string> head_url { get; set; }

        private ObservableCollection<string> head_url = new ObservableCollection<string>();
        public ObservableCollection<string> Head_url
        {
            get
            {
                return head_url;
            }
            set
            {
                head_url = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Head_url);
            }
        }

        /// <summary>
        /// 时间
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
                this.NotifyPropertyChanged(newsEntity => newsEntity.Time);
            }
        }

        /// <summary>
        /// 消息对应的源内容的ID
        /// </summary>
        [DataMember]
        //public long source_id { get; set; }

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
                this.NotifyPropertyChanged(newsEntity => newsEntity.Source_id);
            }
        }

        /// <summary>
        /// 消息对应的源内容的用户ID
        /// </summary>
        [DataMember]
        //public int owner_id { get; set; }

        private int owner_id;
        public int Owner_id
        {
            get
            {
                return owner_id;
            }
            set
            {
                owner_id = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Owner_id);
            }
        }
        /// <summary>
        /// 消息对应的源内容所有者的名称
        /// </summary>
        [DataMember]
        private string owner_name;
        public string Owner_name
        {
            get
            {
                return owner_name;
            }
            set
            {
                owner_name = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Owner_name);
            }
        }
        /// <summary>
        /// 前缀
        /// </summary>
        [DataMember]
        //public string prefix { get; set; }

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
                this.NotifyPropertyChanged(newsEntity => newsEntity.Prefix);
            }
        }

        /// <summary>
        /// 消息内容
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
                this.NotifyPropertyChanged(newsEntity => newsEntity.Title);
            }
        }

        /// <summary>
        /// 后缀
        /// </summary>
        [DataMember]
        //public string sufix { get; set; }

        private string sufix;
        public string Sufix
        {
            get
            {
                return sufix;
            }
            set
            {
                sufix = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Sufix);
            }
        }

        /// <summary>
        /// 附加信息
        /// </summary>
        [DataMember]
        //public string brief { get; set; }

        private string brief;
        public string Brief
        {
            get
            {
                return brief;
            }
            set
            {
                brief = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Brief);
            }
        }

        /// <summary>
        /// 是否为未读消息
        /// </summary>
        [DataMember]
        //public int latest { get; set; }

        private int latest;
        public int Latest
        {
            get
            {
                return latest;
            }
            set
            {
                latest = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.Latest);
            }
        }

        /// <summary>
        /// 包含人数
        /// </summary>
        [DataMember]
        //public int user_count { get; set; }

        private int user_count;
        public int User_count
        {
            get
            {
                return user_count;
            }
            set
            {
                user_count = value;
                this.NotifyPropertyChanged(newsEntity => newsEntity.User_count);
            }
        }
    }
}
