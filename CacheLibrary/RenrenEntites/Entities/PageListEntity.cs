using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Serialization;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Entities
{
    [DataContract]
    public class PageListEntity : PropertyChangedBase
    {
        [DataMember]
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                this.NotifyPropertyChanged(entity => entity.Count);
            }
        }

        [DataMember]
        private ObservableCollection<PageInfoEntity> page_list = new ObservableCollection<PageInfoEntity>();
        public ObservableCollection<PageInfoEntity> Page_list
        {
            get
            {
                return page_list;
            }
            set
            {
                page_list = value;
                this.NotifyPropertyChanged(entity => entity.Page_list);
            }
        }
    }
}
