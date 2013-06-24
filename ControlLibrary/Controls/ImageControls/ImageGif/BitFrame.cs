using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary
{
    /// <summary>
    /// 每一帧的信息对象
    /// </summary>
    public class BitFrame
    {
        /// <summary>
        /// 每帧的动画时间
        /// </summary>
        public TimeSpan Delay
        { get; set; }

        /// <summary>
        /// 每帧的数据流
        /// </summary>
        public InMemoryRandomAccessStream MemoryStream
        { get; set; }

        /// <summary>
        /// 每帧的高度
        /// </summary>
        public double Height
        { get; set; }

        /// <summary>
        /// 每帧的宽度
        /// </summary>
        public double Width
        { get; set; }

        /// <summary>
        /// 每帧距顶部的距离
        /// </summary>
        public double Top
        { get; set; }

        /// <summary>
        /// 每帧距左面的距离
        /// </summary>
        public double Left
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UserInputFlag
        { get; set; }

        /// <summary>
        /// 共享图层(如果是2为不共享为False,其余不为2的为共享为True)
        /// </summary>
        public bool Disposal
        { get; set; }

        public WriteableBitmap wb
        { get; set; }
    }
}
