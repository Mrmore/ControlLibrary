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
    public class CreateNewAlbumEntity : PropertyChangedBase
    {
        [DataMember]
        private string result;
        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                if (value != result)
                {
                    result = value;
                    this.NotifyPropertyChanged("Result");
                }
            }
        }

        [DataMember]
        private int album_id;
        public int Album_Id
        {
            get
            {
                return album_id;
            }
            set
            {
                if (value != album_id)
                {
                    album_id = value;
                    this.NotifyPropertyChanged("Album_Id");
                }
            }
        }

        /// <summary>
        /// 相册的标题
        /// </summary>
        private string album_title;
        public string Album_Title
        {
            get
            {
                return album_title;
            }
            set
            {
                if (value != album_title)
                {
                    album_title = value;
                }
            }
        }
    }
}
