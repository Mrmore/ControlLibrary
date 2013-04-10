using System;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class UserUrlsEntity : PropertyChangedBase
    {
        /// <summary>
        /// 50*50
        /// </summary>
        [DataMember]
        private string tiny_url;
        public string TinyUrl
        {
            get
            {
                return tiny_url;
            }
            set
            {
                tiny_url = value;
                this.NotifyPropertyChanged(entity => entity.TinyUrl);
            }
        }

        /// <summary>
        /// 100
        /// </summary>
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
                this.NotifyPropertyChanged(entity => entity.HeadUrl);
            }
        }

        /// <summary>
        /// 200
        /// </summary>
        [DataMember]
        private string main_url;
        public string MainUrl
        {
            get
            {
                return main_url;
            }
            set
            {
                main_url = value;
                this.NotifyPropertyChanged(entity => entity.MainUrl);
            }
        }

        /// <summary>
        /// 400*400
        /// </summary>
        [DataMember]
        private string flex_url;
        public string FlexUrl
        {
            get
            {
                return flex_url;
            }
            set
            {
                flex_url = value;
                this.NotifyPropertyChanged(entity => entity.FlexUrl);
            }
        }

        /// <summary>
        /// 720
        /// </summary>
        [DataMember]
        private string large_url;
        public string LargeUrl
        {
            get
            {
                return large_url;
            }
            set
            {
                large_url = value;
                this.NotifyPropertyChanged(entity => entity.LargeUrl);
            }
        }
    }
}
