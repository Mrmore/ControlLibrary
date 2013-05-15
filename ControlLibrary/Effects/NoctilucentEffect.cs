using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using ControlLibrary.GifSynthesis;

namespace ControlLibrary.Effects
{
    public static class NoctilucentEffect
    {
        /// <summary>
        /// Noctilucent process.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <returns></returns>
        public static WriteableBitmap NoctilucentProcess(this WriteableBitmap src) ////1夜光处理
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
                    byte tempByte = (byte)(temp[i + 2]);
                    temp[i + 2] = (byte)(tempByte / 2);
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
