using ControlLibrary.GifSynthesis;
using ControlLibrary.Tools.WriteableBitmapExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Effects
{
    public static class BlurEffect
    {
        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int[,] GaussianBlur)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmap writeableBitmapBlur = null;
            if (GaussianBlur == null)
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(cloneWriteableBitmap, WriteableBitmapExtensions.KernelGaussianBlur5x5);
            }
            else
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(cloneWriteableBitmap, GaussianBlur);
            }
            writeableBitmapBlur.Invalidate();
            return writeableBitmapBlur;
        }

        /*
        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int kernelWidth, int kernelHeight)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.GaussianBlur(cloneWriteableBitmap, kernelWidth, kernelHeight);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
        }
        */

        /// <summary>
        /// 高斯滤波
        /// </summary>
        /// <param name="writeableBitmapOld"></param>
        /// <param name="radius">0 to 100</param>
        /// <param name="sigma">0 to 30</param>
        /// <returns></returns>
        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int radius, double sigma)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            var writeableBitmap = WriteableBitmapConvolutionExtensions.GaussFilter(cloneWriteableBitmap, radius, sigma);
            writeableBitmap.Invalidate();
            return writeableBitmap;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.GaussianBlur(cloneWriteableBitmap);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int range)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.BoxBlur(cloneWriteableBitmap, range);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
        }
    }
}
