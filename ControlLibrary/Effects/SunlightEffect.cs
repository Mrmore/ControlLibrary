using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using ControlLibrary.GifSynthesis;

namespace ControlLibrary.Effects
{
    public static class SunlightEffect
    {
        /// <summary>
        /// Sun light process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="A">X location of light source.</param>
        /// <param name="B">Y location of light source.</param>
        /// <param name="thresould">Light intensity value.</param>
        /// <returns></returns>
        public static WriteableBitmap SunlightProcess(this WriteableBitmap src, int X, int Y, float thresould)////41图像光照函数 
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                double b = 0, g = 0, r = 0;

                if (X >= w || Y >= h || X < 0 || Y < 0)
                {
                    X = w / 2;
                    Y = h / 2;
                }

                Point Cen = new Point(X, Y);
                int R = Math.Min(X, Y);
                float curR = 0;
                float pixelValue = 0;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        b = tempMask[i * 4 + j * w * 4];
                        g = tempMask[i * 4 + 1 + j * w * 4];
                        r = tempMask[i * 4 + 2 + j * w * 4];
                        curR = (float)Math.Sqrt(Math.Pow((i - Cen.X), 2) + Math.Pow((j - Cen.Y), 2));

                        if (curR < R)
                        {
                            pixelValue = thresould * (1.0f - curR / R);
                            b = b + pixelValue;
                            g = g + pixelValue;
                            r = r + pixelValue;
                            temp[i * 4 + j * w * 4] = (byte)(b > 0 ? (b < 255 ? b : 255) : 0);
                            temp[i * 4 + 1 + j * w * 4] = (byte)(g > 0 ? (g < 255 ? g : 255) : 0);
                            temp[i * 4 + 2 + j * w * 4] = (byte)(r > 0 ? (r < 255 ? r : 255) : 0);
                            b = 0; g = 0; r = 0;
                        }
                    }
                }

                Stream sTemp = srcImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(temp, 0, w * 4 * h);
                return srcImage;
            }
            else
            {
                return null;
            }
        }
    }
}
