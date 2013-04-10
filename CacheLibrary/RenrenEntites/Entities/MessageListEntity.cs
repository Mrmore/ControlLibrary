using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class MessageListEntity : PropertyChangedBase
    {
        /// <summary>
        /// 表示用户站内信未读取的数目
        /// </summary>
        [DataMember]
        //public int unread_count { get; set; }

        private int unread_count;
        public int Unread_count
        {
            get
            {
                return unread_count;
            }
            set
            {
                unread_count = value;
                this.NotifyPropertyChanged(messageListEntity => messageListEntity.Unread_count);
            }
        }

        /// <summary>
        /// 表示用户站内信的总数
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
                this.NotifyPropertyChanged(messageListEntity => messageListEntity.Count);
            }
        }

        /// <summary>
        /// 表示当前的每个分页的数量
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
                this.NotifyPropertyChanged(messageListEntity => messageListEntity.Page_size);
            }
        }

        /// <summary>
        /// 表示站内信的数据
        /// </summary>
        [DataMember]
        //public List<MessageEntity> message_list { get; set; }

        private ObservableCollection<MessageEntity> message_list = new ObservableCollection<MessageEntity>();
        public ObservableCollection<MessageEntity> Message_list
        {
            get
            {
                return message_list;
            }
            set
            {
                message_list = value;
                this.NotifyPropertyChanged(blogListEntity => blogListEntity.Message_list);
            }
        }

    }
}
