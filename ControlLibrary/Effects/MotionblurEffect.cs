using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using ControlLibrary.GifSynthesis;

namespace ControlLibrary.Effects
{
    public static class MotionblurEffect
    {
        /// <summary>
        /// Motion blur process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="k">The offset of motion, from 0 to 200.</param>
        /// <param name="direction">The direction of motion, x:1, y:2.</param>
        /// <returns></returns>
        public static WriteableBitmap MotionblurProcess(WriteableBitmap src, int k, int direction)////运动模糊处理
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                int b, g, r;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        b = g = r = 0;
                        switch (direction)
                        {
                            case 1:
                                if (x >= k)
                                {
                                    for (int i = 0; i <= k; i++)
                                    {
                                        b += (int)tempMask[(x - i) * 4 + y * w * 4];
                                        g += (int)tempMask[(x - i) * 4 + 1 + y * w * 4];
                                        r += (int)tempMask[(x - i) * 4 + 2 + y * w * 4];
                                    }
                                    temp[x * 4 + y * w * 4] = (byte)(b / (k + 1));
                                    temp[x * 4 + 1 + y * w * 4] = (byte)(g / (k + 1));
                                    temp[x * 4 + 2 + y * w * 4] = (byte)(r / (k + 1));
                                }
                                else
                                {
                                    if (x > 0)
                                    {
                                        for (int i = 0; i < x; i++)
                                        {
                                            b += (int)tempMask[(x - i) * 4 + y * w * 4];
                                            g += (int)tempMask[(x - i) * 4 + 1 + y * w * 4];
                                            r += (int)tempMask[(x - i) * 4 + 2 + y * w * 4];
                                        }
                                        temp[x * 4 + y * w * 4] = (byte)(b / (x + 1));
                                        temp[x * 4 + 1 + y * w * 4] = (byte)(g / (x + 1));
                                        temp[x * 4 + 2 + y * w * 4] = (byte)(r / (x + 1));
                                    }
                                    else
                                    {
                                        temp[x * 4 + y * w * 4] = (byte)(tempMask[x * 4 + y * w * 4] / k);
                                        temp[x * 4 + 1 + y * w * 4] = (byte)(tempMask[x * 4 + 1 + y * w * 4] / k);
                                        temp[x * 4 + 2 + y * w * 4] = (byte)(tempMask[x * 4 + 2 + y * w * 4] / k);
                                    }
                                }
                                break;
                            case 2:
                                if (y >= k)
                                {
                                    for (int i = 0; i <= k; i++)
                                    {
                                        b += (int)tempMask[x * 4 + (y - i) * w * 4];
                                        g += (int)tempMask[x * 4 + 1 + (y - i) * w * 4];
                                        r += (int)tempMask[x * 4 + 2 + (y - i) * w * 4];
                                    }
                                    temp[x * 4 + y * w * 4] = (byte)(b / (k + 1));
                                    temp[x * 4 + 1 + y * w * 4] = (byte)(g / (k + 1));
                                    temp[x * 4 + 2 + y * w * 4] = (byte)(r / (k + 1));
                                }
                                else
                                {
                                    if (y > 0)
                                    {
                                        for (int i = 0; i < y; i++)
                                        {
                                            b += (int)tempMask[x * 4 + (y - i) * w * 4];
                                            g += (int)tempMask[x * 4 + 1 + (y - i) * w * 4];
                                            r += (int)tempMask[x * 4 + 2 + (y - i) * w * 4];
                                        }
                                        temp[x * 4 + y * w * 4] = (byte)(b / (y + 1));
                                        temp[x * 4 + 1 + y * w * 4] = (byte)(g / (y + 1));
                                        temp[x * 4 + 2 + y * w * 4] = (byte)(r / (y + 1));
                                    }
                                    else
                                    {
                                        temp[x * 4 + y * w * 4] = (byte)(tempMask[x * 4 + y * w * 4] / k);
                                        temp[x * 4 + 1 + y * w * 4] = (byte)(tempMask[x * 4 + 1 + y * w * 4] / k);
                                        temp[x * 4 + 2 + y * w * 4] = (byte)(tempMask[x * 4 + 2 + y * w * 4] / k);
                                    }
                                }
                                break;
                            default:
                                break;
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
