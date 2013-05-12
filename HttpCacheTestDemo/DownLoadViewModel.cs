using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using WinRtHttpHelper;

namespace HttpCacheTestDemo
{
    public class DownLoadViewModel
    {
        public ObservableCollection<DownLoadEntity> DownLoadList { set; get; }
        private BackgroundDownloaderHelper DownLoadHelp;
        private int Count = 0;

        public DownLoadViewModel()
        {
            DownLoadList = new ObservableCollection<DownLoadEntity>();
            DownLoadHelp = BackgroundDownloaderHelper.Instance;
            DownLoadHelp.DownloadComplete += DownLoadHelp_DownloadComplete;
            DownLoadHelp.DownloadFail += DownLoadHelp_DownloadFail;
            DownLoadHelp.DownloadCancel += DownLoadHelp_DownloadCancel;
            DownLoadHelp.process += DownLoadHelp_process;
            Ini();
            
        }

        void DownLoadHelp_process(List<Tuple<string, ulong, ulong>> sender)
        {
            if (sender != null && DownLoadList!= null)
            for (int i = 0; i < sender.Count; i++)
            {
                for (int j = 0; j < DownLoadList.Count; j++)
                {
                    if (sender[i].Item1 == DownLoadList[j].Uri)
                    {
                        DownLoadList[j].ReceiveBytes = (long)sender[i].Item2;
                    }
                }
            }
        }

        void DownLoadHelp_DownloadCancel(string sender)
        {
            
        }

        void DownLoadHelp_DownloadFail(string sender)
        {
            
        }

        void DownLoadHelp_DownloadComplete(string sender)
        {
            foreach (var item in DownLoadList)
            {
                if (item.Uri == sender)
                {
                    DownLoadList.Remove(item);
                }
            }
        }

        public void AddDown(Uri uri,StorageFile file)
        {
            Count++;
            DownLoadEntity entity = new DownLoadEntity()
            {
                 Name = Count.ToString(),
                 Uri = uri.ToString(),
                 ReceiveBytes = 0
            };
            DownLoadList.Add(entity);
            DownLoadHelp.AddDownLoader(uri,file);
        }

        private async void Ini()
        {
           
           IReadOnlyList<DownloadOperation>  items = await DownLoadHelp.GetCurrentDownLoader();
           foreach (var item in items)
           {
               Count++;
               if (DownLoadList != null)
               {
                   DownLoadEntity entity = new DownLoadEntity()
                   {
                       Name = Count.ToString(),
                       Uri = item.RequestedUri.ToString(),
                       ReceiveBytes = (long)item.Progress.BytesReceived
                   };
                   DownLoadList.Add(entity);
               }
           }
        }
    }
}
