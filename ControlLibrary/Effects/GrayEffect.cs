using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using ControlLibrary.GifSynthesis;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Effects
{
    public static class GrayEffect
    {
        /// <summary>
        /// Gray process.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <returns></returns>
        public static WriteableBitmap GrayProcess(this WriteableBitmap src) ////1 灰度化处理
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap grayImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                for (int i = 0; i < temp.Length; i += 4)
                {
                    byte tempByte = (byte)((int)(temp[i] * 0.114 + temp[i + 1] * 0.587 + temp[i + 2] * 0.299));
                    temp[i] = tempByte;
                    temp[i + 1] = tempByte;
                    temp[i + 2] = tempByte;
                }
                Stream sTemp = grayImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(temp, 0, w * 4 * h);
                return grayImage;
            }
            else
            {
                return null;
            }
        }
    }
}
