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
    public static class AtomizationEffects
    {
        /// <summary>
        /// Atomization process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="v">The threshould to control the effect of atomization.</param>
        /// <returns></returns>
        public static WriteableBitmap AtomizationProcess(this WriteableBitmap src, int v)////45图像雾化
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                Random ran = new Random();
                int k = 0;
                int dx = 0;
                int dy = 0;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        k = ran.Next(v);
                        dx = (i + k) >= w ? w - 1 : (i + k);
                        dy = (j + k) >= h ? h - 1 : (j + k);
                        temp[i * 4 + j * w * 4] = (byte)tempMask[dx * 4 + dy * w * 4];
                        temp[i * 4 + 1 + j * w * 4] = (byte)tempMask[dx * 4 + 1 + dy * w * 4];
                        temp[i * 4 + 2 + j * w * 4] = (byte)tempMask[dx * 4 + 2 + dy * w * 4];
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
