using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ControlLibrary.GifSynthesis
{
    /// <summary>
    /// 图像文件的类型
    /// </summary>
    public enum GifDataSource
    {
        UriDataSource = 0,
        StreamDataSource = 1
    }

    public class GifInformation
    {
        /// <summary>
        /// 每帧的图片的地址
        /// </summary>
        public Uri Uri
        { get; set; }

        /// <summary>
        /// 每帧的动画时间(以毫秒为单位 )
        /// </summary>
        private int delayMilliseconds = 500;
        public int DelayMilliseconds
        {
            get
            {
                return delayMilliseconds;
            }
            set
            {
                delayMilliseconds = value;
            }
        }

        public IRandomAccessStream IRandomAccessStream
        { get; set; }
    }
}
