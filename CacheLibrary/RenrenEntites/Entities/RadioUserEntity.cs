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
    public class RadioUserEntity : PropertyChangedBase
    {
        [DataMember]
        private uint id;
        public uint Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.NotifyPropertyChanged(entity => entity.Id);
            }
        }

        [DataMember]
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.NotifyPropertyChanged(entity => entity.Name);
            }
        }

        [DataMember]
        private string tinyUrl;
        public string TinyUrl
        {
            get
            {
                return tinyUrl;
            }
            set
            {
                tinyUrl = value;
                this.NotifyPropertyChanged(entity => entity.TinyUrl);
            }
        }
    }
}
