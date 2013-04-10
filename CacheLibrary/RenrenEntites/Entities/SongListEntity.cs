using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;
using System.Collections.ObjectModel;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class SongListEntity : PropertyChangedBase
    {
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
