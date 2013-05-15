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
    public static class SharpenEffect
    {
        /// <summary>
        /// Wallis sharpen process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <returns></returns>
        public static WriteableBitmap WallisSharpen(this WriteableBitmap src)////37Wallis锐化函数 
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap sharpenImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                double b = 0, g = 0, r = 0, srR = 0, srG = 0, srB = 0;

                for (int j = 1; j < h - 1; j++)
                {
                    for (int i = 4; i < w * 4 - 4; i += 4)
                    {
                        srB = tempMask[i + j * w * 4];
                        srG = tempMask[i + 1 + j * w * 4];
                        srR = tempMask[i + 2 + j * w * 4];
                        b = 46 * Math.Abs(5 * Math.Log(srB + 1) - Math.Log(tempMask[i - 4 + j * w * 4] + 1) - Math.Log(tempMask[i + 4 + j * w * 4] + 1) - Math.Log(tempMask[i + (j - 1) * w * 4] + 1) - Math.Log(tempMask[i + (j + 1) * w * 4] + 1));
                        g = 46 * Math.Abs(5 * Math.Log(srG + 1) - Math.Log(tempMask[i - 4 + 1 + j * w * 4] + 1) - Math.Log(tempMask[i + 4 + 1 + j * w * 4] + 1) - Math.Log(tempMask[i + 1 + (j - 1) * w * 4] + 1) - Math.Log(tempMask[i + 1 + (j + 1) * w * 4] + 1));
                        r = 46 * Math.Abs(5 * Math.Log(srR + 1) - Math.Log(tempMask[i - 4 + 2 + j * w * 4] + 1) - Math.Log(tempMask[i + 4 + 2 + j * w * 4] + 1) - Math.Log(tempMask[i + 2 + (j - 1) * w * 4] + 1) - Math.Log(tempMask[i + 2 + (j + 1) * w * 4] + 1));
                        temp[i + j * w * 4] = (byte)(b > 0 ? (b < 255 ? b : 255) : 0);
                        temp[i + 1 + j * w * 4] = (byte)(g > 0 ? (g < 255 ? g : 255) : 0);
                        temp[i + 2 + j * w * 4] = (byte)(r > 0 ? (r < 255 ? r : 255) : 0);
                        b = 0; g = 0; r = 0; srR = 0; srG = 0; srB = 0;
                    }
                }

                Stream sTemp = sharpenImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(temp, 0, w * 4 * h);
                return sharpenImage;
            }
            else
            {
                return null;
            }
        }
    }
}
