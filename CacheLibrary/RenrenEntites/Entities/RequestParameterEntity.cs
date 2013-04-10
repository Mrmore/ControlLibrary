using System;
using System.Net;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    public class RequestParameterEntity : PropertyChangedBase
    {
        //public string Name { get; set; }
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
                this.NotifyPropertyChanged(requestParameterEntity => requestParameterEntity.Name);
            }
        }

        //public string Value { get; set; }
        private string values;
        public string Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
                this.NotifyPropertyChanged(requestParameterEntity => requestParameterEntity.Values);
            }
        }

        public RequestParameterEntity(string name, string value)
        {
            Name = name;
            Values = value;
        }
    }
}
