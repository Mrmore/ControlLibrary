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
    public class UpdateConfigEntity : PropertyChangedBase
    {
        [DataMember]
        private string leftKey;
        public string LeftKey
        {
            get
            {
                return leftKey;
            }
            set
            {
                leftKey = value;
                this.NotifyPropertyChanged(entity => entity.LeftKey);
            }
        }

        [DataMember]
        private string rightKey;
        public string RightKey
        {
            get
            {
                return rightKey;
            }
            set
            {
                rightKey = value;
                this.NotifyPropertyChanged(entity => entity.RightKey);
            }
        }

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
                this.NotifyPropertyChanged(entity => entity.Title);
            }
        }
    }
}