using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpCacheTestDemo
{
    public class DownLoadEntity : PropertyChangedBase
    {
        private string uri;
        public string Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
                this.NotifyPropertyChanged(downLoadEntity => downLoadEntity.Uri);
            }
        }

        private long receiveBytes;
        public long ReceiveBytes
        {
            get
            {
                return receiveBytes;
            }
            set
            {
                receiveBytes = value;
                this.NotifyPropertyChanged(downLoadEntity => downLoadEntity.ReceiveBytes);
            }
        }

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
                this.NotifyPropertyChanged(downLoadEntity => downLoadEntity.Name);
            }
        }
    }
}
