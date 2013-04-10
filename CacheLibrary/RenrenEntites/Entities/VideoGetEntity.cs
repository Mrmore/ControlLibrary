using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class VideoGetEntity : PropertyChangedBase
    {
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
                this.NotifyPropertyChanged(entity => entity.Time);
            }
        }

        [DataMember]
        private long play_number;
        public long PlayNumber
        {
            get
            {
                return play_number;
            }
            set
            {
                play_number = value;
                this.NotifyPropertyChanged(entity => entity.PlayNumber);
            }
        }

        [DataMember]
        private double rating;
        public double Rating
        {
            get
            {
                return rating;
            }
            set
            {
                rating = value;
                this.NotifyPropertyChanged(entity => entity.Rating);
            }
        }

        [DataMember]
        private string src_fluency;
        public string SrcFluency
        {
            get
            {
                return src_fluency;
            }
            set
            {
                src_fluency = value;
                this.NotifyPropertyChanged(entity => entity.SrcFluency);
            }
        }

        [DataMember]
        private string tepe_fluency;
        public string TapFluency
        {
            get
            {
                return tepe_fluency;
            }
            set
            {
                tepe_fluency = value;
                this.NotifyPropertyChanged(entity => entity.TapFluency);
            }
        }

        [DataMember]
        private string src_clear;
        public string SrcClear
        {
            get
            {
                return src_clear;
            }
            set
            {
                src_clear = value;
                this.NotifyPropertyChanged(entity => entity.SrcClear);
            }
        }

        [DataMember]
        private int type_clear;
        public int TypeClear
        {
            get
            {
                return type_clear;
            }
            set
            {
                type_clear = value;
                this.NotifyPropertyChanged(entity => entity.TypeClear);
            }
        }

        [DataMember]
        private string introduction;
        public string Introduction
        {
            get
            {
                return introduction;
            }
            set
            {
                introduction = value;
                this.NotifyPropertyChanged(entity => entity.Introduction);
            }
        }

        //[DataMember]
        //private string pictures;
        //public string Pictures
        //{
        //    get
        //    {
        //        return pictures;
        //    }
        //    set
        //    {
        //        pictures = value;
        //        this.NotifyPropertyChanged(entity => entity.Pictures);
        //    }
        //}
    }
}
