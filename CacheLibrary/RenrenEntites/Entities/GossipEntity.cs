using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class GossipEntity : PropertyChangedBase
    {
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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Id);
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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.User_id);
            }
        }

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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.User_name);
            }
        }

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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.head_url);
            }
        }

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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Content);
            }
        }

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
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Time);
            }
        }

        [DataMember]
        private string timestr;
        public string TimeStr
        {
            get
            {
                long tempTime = (long)time;
                tempTime *= 10000;

                TimeSpan ts = new TimeSpan(tempTime);

                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.Add(ts);
                dt = dt.AddHours(8);

                string date = dt.ToString("yyyy-MM-dd HH:mm");
                timestr = date;
                return timestr;
            }
            set
            {
                timestr = value;
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.TimeStr);
            }
        }

        [DataMember]
        private int whisper;
        public int Whisper
        {
            get
            {
                return whisper;
            }
            set
            {
                whisper = value;
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Whisper);
            }
        }

        [DataMember]
        private int source;
        public int Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Source);
            }
        }

        [DataMember]
        private string large_url;
        public string Large_url
        {
            get
            {
                return large_url;
            }
            set
            {
                large_url = value;
                this.NotifyPropertyChanged(gossipEntity => gossipEntity.Large_url);
            }
        }
    }
}
