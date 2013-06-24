using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary
{
    [DataContract]
    public class GifCache
    {
        //[DataMember]
        private byte[] bytes = null;
        public byte[] Bytes
        {
            get
            {
                return bytes;
            }
            set
            {
                bytes = value;
            }
        }

        [DataMember]
        private string webUri = string.Empty;
        public string WebUri
        {
            get
            {
                return webUri;
            }
            set
            {
                webUri = value;
            }
        }

        [DataMember]
        private string localUri = string.Empty;
        public string LocalUri
        {
            get
            {
                return localUri;
            }
            set
            {
                localUri = value;
            }
        }

        private BitmapImage bitmapImage = null;
        public BitmapImage BitmapImage
        {
            get
            {
                return bitmapImage;
            }
            set
            {
                bitmapImage = value;
            }
        }

        [DataMember]
        public string Base64String
        {
            get { return Convert.ToBase64String(this.Bytes); }
            set
            {
                Bytes = Convert.FromBase64String(value);
                //SetBitmap(Bytes);
            }
        }

        //转换BitmapImage 变换成 Byte 字节数组
        private void BitmapImageToByte(BitmapImage bi)
        {
            if (bi != null)
            {
                WriteableBitmap writeableBitmap = new WriteableBitmap(bi.PixelWidth, bi.PixelHeight);
            }
        }

        /*
        //https://github.com/waynebaby/Table-Game-Sidekick/tree/master/Code
        //https://github.com/waynebaby/Table-Game-Sidekick/blob/master/Code/TableGameSidekick_Metro/TableGameSidekick_Metro/DataEntity/ImageData.cs
        private async void SetBitmap(byte[] bytes)
        {
            using (var im = await CreateStreamAsync(bytes))
            {
                var bm = new BitmapImage();
                bm.SetSource(im);
                this.BitmapImage = bm;
            };
        }

        private static async Task<InMemoryRandomAccessStream> CreateStreamAsync(byte[] bs)
        {
            var im = new InMemoryRandomAccessStream();
            var dr = new DataWriter(im);
            dr.WriteBytes(bs);
            await dr.StoreAsync();
            im.Seek(0);
            return im;
        }

        public async Task<InMemoryRandomAccessStream> GetStreamAsync()
        {
            return await CreateStreamAsync(Bytes);
        }
        */
    }
}
