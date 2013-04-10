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
    public class SongEntity : PropertyChangedBase
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
        private uint artistId;
        public uint ArtistId
        {
            get
            {
                return artistId;
            }
            set
            {
                artistId = value;
                this.NotifyPropertyChanged(entity => entity.ArtistId);
            }
        }

        [DataMember]
        private string artistName;
        public string ArtistName
        {
            get
            {
                return artistName;
            }
            set
            {
                artistName = value;
                this.NotifyPropertyChanged(entity => entity.ArtistName);
            }
        }

        [DataMember]
        private string src;
        public string Source
        {
            get
            {
                return src;
            }
            set
            {
                src = value;
                this.NotifyPropertyChanged(entity => entity.Source);
            }
        }

        [DataMember]
        private uint duration;
        public uint Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                this.NotifyPropertyChanged(entity => entity.Duration);
            }
        }

        [DataMember]
        private string lyric;
        public string Lyric
        {
            get
            {
                return lyric;
            }
            set
            {
                lyric = value;
                this.NotifyPropertyChanged(entity => entity.Lyric);
            }
        }

        [DataMember]
        private string albumImg;
        public string AlbumImg
        {
            get
            {
                return albumImg;
            }
            set
            {
                albumImg = value;
                this.NotifyPropertyChanged(entity => entity.AlbumImg);
            }
        }

        [DataMember]
        private string albumInfo;
        public string AlbumInfo
        {
            get
            {
                return albumInfo;
            }
            set
            {
                albumInfo = value;
                this.NotifyPropertyChanged(entity => entity.AlbumInfo);
            }
        }

        [DataMember]
        private string fav;
        public string Favorited
        {
            get
            {
                return fav;
            }
            set
            {
                fav = value;
                this.NotifyPropertyChanged(entity => entity.Favorited);
            }
        }

        [DataMember]
        private string albumName;
        public string AlbumName
        {
            get
            {
                return albumName;
            }
            set
            {
                albumName = value;
                this.NotifyPropertyChanged(entity => entity.AlbumName);
            }
        }

    }
}
