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
    public static class MosaicEffects
    {
        /// <summary>
        /// Mosaic process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="v">The threshould to control the result of mosaic process.</param>
        /// <returns></returns>
        public static WriteableBitmap MosaicProcess(this WriteableBitmap src, int v)////图像马赛克效果
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                int dR = 0;
                int dG = 0;
                int dB = 0;
                int dstX = 0;
                int dstY = 0;

                dR = tempMask[2];
                dG = tempMask[1];
                dB = tempMask[0];

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        dstX = i;
                        dstY = j;
                        if (j % v == 0)
                        {
                            if (i % v == 0)
                            {
                                dB = tempMask[dstX * 4 + dstY * w * 4];
                                dG = tempMask[dstX * 4 + 1 + dstY * w * 4];
                                dR = tempMask[dstX * 4 + 2 + dstY * w * 4];
                            }
                            else
                            {
                                temp[dstX * 4 + dstY * w * 4] = (byte)dB;
                                temp[dstX * 4 + 1 + dstY * w * 4] = (byte)dG;
                                temp[dstX * 4 + 2 + dstY * w * 4] = (byte)dR;
                            }
                        }
                        else
                        {
                            temp[dstX * 4 + dstY * w * 4] = temp[dstX * 4 + (dstY - 1) * w * 4];
                            temp[dstX * 4 + 1 + dstY * w * 4] = temp[dstX * 4 + 1 + (dstY - 1) * w * 4];
                            temp[dstX * 4 + 2 + dstY * w * 4] = temp[dstX * 4 + 2 + (dstY - 1) * w * 4];
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
