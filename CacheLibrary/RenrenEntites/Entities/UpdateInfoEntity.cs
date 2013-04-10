using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class UpdateInfoEntity : PropertyChangedBase
    {
        /// <summary>
        /// 0：没有更新；1：有强制更新；2：有可选更新；3：有提示评价/活动
        /// </summary>
        public enum UpdateType
        { UpdateNA = 0, MandatoryUpdate = 1, OptionalUpdate = 2, PromptInfo = 3 }

        [DataMember]
        private UpdateConfigEntity configInfo;
        public UpdateConfigEntity ConfigInfo
        {
            get
            {
                return configInfo;
            }
            set
            {
                configInfo = value;
                this.NotifyPropertyChanged(entity => entity.ConfigInfo);
            }
        }

        [DataMember]
        private string info;
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                this.NotifyPropertyChanged(entity => entity.Info);
            }
        }

        [DataMember]
        private int pubdate;
        public int PubishDate
        {
            get
            {
                return pubdate;
            }
            set
            {
                pubdate = value;
                this.NotifyPropertyChanged(entity => entity.PubishDate);
            }
        }

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
                this.NotifyPropertyChanged(entity => entity.Type);
            }
        }

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
                this.NotifyPropertyChanged(entity => entity.Url);
            }
        }

        [DataMember]
        private string lastTag;
        public string LastTag 
        {
            get
            {
                return lastTag;
            }
            set
            {
                lastTag = value;
                this.NotifyPropertyChanged(entity => entity.LastTag);
            }
        }

        [DataMember]
        private string version;
        public string Version 
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
                this.NotifyPropertyChanged(entity => entity.Version);
            }
        }

        [DataMember]
        private string introUrl;
        public string IntroUrl 
        {
            get
            {
                return introUrl;
            }
            set
            {
                introUrl = value;
                this.NotifyPropertyChanged(entity => entity.IntroUrl);
            }
        }
    }
}