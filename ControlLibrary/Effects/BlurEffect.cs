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
            WriteableBitmap writeableBitmapBlur = null;
            if (GaussianBlur == null)
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(writeableBitmapOld, WriteableBitmapExtensions.KernelGaussianBlur5x5);
            }
            else
            {
                writeableBitmapBlur = WriteableBitmapExtensions.Convolute(writeableBitmapOld, GaussianBlur);
            }
            return writeableBitmapBlur;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int kernelWidth, int kernelHeight)
        {
            WriteableBitmapConvolutionExtensions.GaussianBlur(writeableBitmapOld, kernelWidth, kernelHeight);
            return writeableBitmapOld;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld)
        {
            WriteableBitmapConvolutionExtensions.GaussianBlur(writeableBitmapOld);
            return writeableBitmapOld;
        }

        public static WriteableBitmap WriteableBitmapBlur(this WriteableBitmap writeableBitmapOld, int range)
        {
            WriteableBitmapConvolutionExtensions.BoxBlur(writeableBitmapOld, range);
            return writeableBitmapOld;
        }
    }
}
