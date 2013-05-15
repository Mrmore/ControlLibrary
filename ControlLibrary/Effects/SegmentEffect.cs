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
    public static class SegmentEffect
    {
        /// <summary>
        /// P-Parameter method of image segmention.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="P">The ratio of object, from 0 to 1.</param>
        /// <returns></returns>
        public static WriteableBitmap PParameterThSegment(this WriteableBitmap src, double P) ////P参数法阈值分割
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap dstImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                //定义灰度图像信息存储变量
                int[] srcData = new int[w * h];
                //定义背景和目标像素个数变量
                int C1 = 0, C2 = 0;
                //定义阈值变量
                int Th = 0;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        srcData[i + j * w] = (int)((double)tempMask[i * 4 + j * w * 4] * 0.114 + (double)tempMask[i * 4 + 1 + j * w * 4] * 0.587 + (double)tempMask[i * 4 + 2 + j * w * 4] * 0.299);
                    }
                }

                for (int T = 0; T <= 255; T++)
                {
                    for (int i = 0; i < srcData.Length; i++)
                    {
                        if (srcData[i] > T)
                        {
                            C1++;
                        }
                        else
                        {
                            C2++;
                        }
                    }
                    double t = Math.Abs((double)((double)C1 / ((double)C1 + (double)C2)) - P);
                    if (t < 0.01)
                    {
                        Th = T;
                        break;
                    }
                    C1 = 0;
                    C2 = 0;
                }
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        temp[i * 4 + j * w * 4] = temp[i * 4 + 1 + j * w * 4] = temp[i * 4 + 2 + j * w * 4] = (byte)(srcData[i + j * w] < Th ? 0 : 255);
                    }
                }
                Stream sTemp = dstImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(temp, 0, w * 4 * h);
                return dstImage;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Entropy max method of image segmention.
        /// </summary>
        /// <param name="src">The source iamge.</param>
        /// <returns></returns>
        public static WriteableBitmap EntropymaxThSegment(this WriteableBitmap src) ////一维熵最大法阈值分割
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap dstImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                //定义灰度图像信息存储变量
                int[] srcData = new int[w * h];
                //定义阈值变量
                int Th = 0;
                //定义直方图存储变量
                int[] histogram = new int[256];
                //定义熵值变量 
                double Ht = 0.0;
                double Hl = 0.0;
                double sigma = 0.0;
                //定义灰度最值变量
                int max = 0;
                int min = 255;

                //定义临时变量
                double t = 0.0, pt = 0.0, tempMax = 0.0;
                int tempV = 0;

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        tempV = (int)((double)tempMask[i * 4 + j * w * 4] * 0.114 + (double)tempMask[i * 4 + 1 + j * w * 4] * 0.587 + (double)tempMask[i * 4 + 2 + j * w * 4] * 0.299);
                        srcData[i + j * w] = tempV;
                        histogram[tempV]++;

                        if (tempV > max)
                        {
                            max = tempV;
                        }

                        if (tempV < min)
                        {
                            min = tempV;
                        }
                    }
                }

                for (int i = min; i < max; i++)
                {
                    t = (double)((double)histogram[i] / (double)(w * h));
                    if (t > 0.00000001)
                    {
                        Hl += -t * Math.Log10(t);
                    }
                    else
                        continue;
                }
                for (int i = min; i < max; i++)
                {
                    t = (double)((double)histogram[i] / (double)(w * h));
                    pt += t;
                    if (t > 0.00000001)
                    {
                        Ht += -t * Math.Log10(t);
                        sigma = Math.Log10(pt * (1 - pt)) * Ht / pt + (Hl - Ht) / (1 - pt);
                        if (sigma > tempMax)
                        {
                            tempMax = (int)sigma;
                            Th = i;
                        }
                    }
                    else
                        continue;
                }
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {

                        temp[i * 4 + j * w * 4] = temp[i * 4 + 1 + j * w * 4] = temp[i * 4 + 2 + j * w * 4] = (byte)(srcData[i + j * w] < Th ? 0 : 255);
                    }
                }
                Stream sTemp = dstImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(temp, 0, w * 4 * h);
                return dstImage;
            }
            else
            {
                return null;
            }
        }
    }
}
