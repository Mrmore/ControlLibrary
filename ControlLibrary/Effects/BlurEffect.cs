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
    //http://stackoverflow.com/questions/19988450/windows-phone-quick-blur-effect-use-on-bitmap-image-in-background
    //http://social.msdn.microsoft.com/Forums/wpapps/en-US/8193493f-ff72-49ca-be86-0c13eaa07c62/is-there-an-easy-way-to-scale-and-blur-an-bitmapimage-for-windows-phone-app-c?forum=wpdevelop
    //http://stackoverflow.com/questions/13927845/wp8-is-there-an-easy-way-to-scale-and-blur-an-bitmapimage-for-windows-phone-app
    //http://en.wikipedia.org/wiki/Gaussian_blur
    //http://dongtingyueh.blog.163.com/blog/static/4619453201211264536361/
    public static class BlurEffect
    {
        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int[,] GaussianBlur, int kernelFactorSum, int kernelOffsetSum)
        {
            //var cloneWriteableBitmap = WriteableBitmapExpansions.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmap writeableBitmapBlur = null;
            if (GaussianBlur == null)
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(writeableBitmapOld, WriteableBitmapExtensions.KernelGaussianBlur5x5, 5, 5);
            }
            else
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(writeableBitmapOld, GaussianBlur, kernelFactorSum, kernelOffsetSum);
            }
            writeableBitmapBlur.Invalidate();
            return writeableBitmapBlur;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, KernelType kernelType = KernelType.KernelGaussianBlur5x5)
        {
            //var cloneWriteableBitmap = WriteableBitmapExpansions.CopyWriteableBitmap(writeableBitmapOld);
            var kernelValue = 5;
            kernelValue = ConverKernelValue(kernelType);
            WriteableBitmap writeableBitmapBlur = WriteableBitmapExtensions.Convolute(writeableBitmapOld, ConverKernelType(kernelType), kernelValue, kernelValue);
            writeableBitmapBlur.Invalidate();
            return writeableBitmapBlur;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld)
        {
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.GaussianBlur(cloneWriteableBitmap);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
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

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int range)
        {
            if ((range & 1) == 0)
                return null;
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.BoxBlur(cloneWriteableBitmap, range);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int x, int y)
        {
            if ((x & 1) == 0 || (y & 1) == 0)
                return null;
            var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(writeableBitmapOld);
            WriteableBitmapConvolutionExtensions.BoxBlur(cloneWriteableBitmap, x, y);
            cloneWriteableBitmap.Invalidate();
            return cloneWriteableBitmap;
        }

        private static int[,] ConverKernelType(KernelType kernelType = KernelType.KernelGaussianBlur5x5)
        {
            var converKernel = WriteableBitmapExtensions.KernelGaussianBlur5x5;
            if (kernelType == KernelType.KernelGaussianBlur5x5)
                converKernel = WriteableBitmapExtensions.KernelGaussianBlur5x5;
            else if (kernelType == KernelType.KernelGaussianBlur3x3)
                converKernel = WriteableBitmapExtensions.KernelGaussianBlur3x3;
            else
                converKernel = WriteableBitmapExtensions.KernelSharpen3x3;
            return converKernel;
        }

        private static int ConverKernelValue(KernelType kernelType = KernelType.KernelGaussianBlur5x5)
        {
            var kernelValue = 5;
            if (kernelType == KernelType.KernelGaussianBlur5x5)
                kernelValue = 5;
            else if (kernelType == KernelType.KernelGaussianBlur3x3)
                kernelValue = 3;
            else
                kernelValue = 3;
            return kernelValue;
        }
    }

    public enum KernelType
    {
        KernelGaussianBlur3x3 = 0,
        KernelGaussianBlur5x5 = 1,
        KernelSharpen3x3 = 2
    }
}
