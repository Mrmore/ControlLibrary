using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class GetHomeEntity : PropertyChangedBase
    {
        [DataMember]
        private string uuid;
        public string UUID
        {
            get
            {
                return uuid;
            }
            set
            {
                uuid = value;
                this.NotifyPropertyChanged(entity => entity.UUID);
            }
        }

        [DataMember]
        private uint currentRadio;
        public uint CurrentRadio
        {
            get
            {
                return currentRadio;
            }
            set
            {
                currentRadio = value;
                this.NotifyPropertyChanged(entity => entity.CurrentRadio);
            }
        }

        [DataMember]
        private RadioUserEntity user;
        public RadioUserEntity User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                this.NotifyPropertyChanged(entity => entity.User);
            }
        }

        [DataMember]
        private ObservableCollection<RadioEntity> radios = new ObservableCollection<RadioEntity>();
        public ObservableCollection<RadioEntity> Radios
        {
            get
            {
                return radios;
            }
            set
            {
                radios = value;
                this.NotifyPropertyChanged(entity => entity.Radios);
            }
        }

        [DataMember]
        private ObservableCollection<SongEntity> songs = new ObservableCollection<SongEntity>();
        public ObservableCollection<SongEntity> Songs
        {
            get
            {
                return songs;
            }
            set
            {
                songs = value;
                this.NotifyPropertyChanged(entity => entity.Songs);
            }
        }
    }
}
