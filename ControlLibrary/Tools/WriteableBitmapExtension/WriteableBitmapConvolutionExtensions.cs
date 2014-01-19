using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools.WriteableBitmapExtension
{
    public static class WriteableBitmapConvolutionExtensions
    {
        private static byte[] _tinyGaussianBlurKernel = 
            {16,26,16,
             26,41,26,
             16,26,16};

        private static byte[] _gaussianBlurKernel = 
            { 1, 4, 7, 4,1,
              4,16,26,16,4,
              7,26,41,26,7,
              4,16,26,16,4,
              1, 4, 7, 4,1};

        public static void GaussianBlurTiny(this WriteableBitmap bmp)
        {
            bmp.Convolute(_tinyGaussianBlurKernel, 3, 3);
        }

        public static void GaussianBlur(this WriteableBitmap bmp)
        {
            bmp.Convolute(_gaussianBlurKernel, 5, 5);
        }

        public static void GaussianBlur(this WriteableBitmap bmp, int kernelWidth, int kernelHeight)
        {
            bmp.Convolute(_gaussianBlurKernel, kernelWidth, kernelHeight);
        }

        private static byte[] _1dgaussianBlurKernel = { 1, 4, 7, 4, 1, };
        private static byte[] _tiny1dGaussianBlurKernel = { 4, 7, 4 };

        public static void GaussianBlurFast(this WriteableBitmap bmp)
        {
            bmp.ConvoluteX(_tiny1dGaussianBlurKernel);
            bmp.ConvoluteY(_tiny1dGaussianBlurKernel);
        }

        public static void Convolute(this WriteableBitmap bmp, byte[] kernel, int kernelWidth, int kernelHeight)
        {
            if ((kernelWidth & 1) == 0)
            {
                Debug.WriteLine("Kernel width must be odd!");
                return;
            }
            if ((kernelHeight & 1) == 0)
            {
                Debug.WriteLine("Kernel height must be odd!");
                return;
            }
            if (kernel.Length != kernelWidth * kernelHeight)
            {
                Debug.WriteLine("Kernel size doesn't match width*height!");
                return;
            }

            int[] pixels = bmp.GetPixels();
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int index = 0;
            int halfKernelWidth = kernelWidth / 2;
            int halfKernelHeight = kernelHeight / 2;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int kernelSum = 0;
                    int r = 0;
                    int g = 0;
                    int b = 0;

                    for (int kx = -halfKernelWidth; kx <= halfKernelWidth; kx++)
                    {
                        int px = kx + x;
                        if (px < 0 || px >= w)
                        {
                            continue;
                        }

                        for (int ky = -halfKernelHeight; ky <= halfKernelHeight; ky++)
                        {
                            int py = ky + y;
                            if (py < 0 || py >= h)
                            {
                                continue;
                            }

                            int kernelIndex = (ky + halfKernelHeight) * kernelWidth + (kx + halfKernelWidth);
                            byte kernelWeight = kernel[kernelIndex];
                            kernelSum += kernelWeight;

                            int innerIndex = py * w + px;
                            int col = pixels[innerIndex];
                            r += ((byte)(col >> 16)) * kernelWeight;
                            g += ((byte)(col >> 8)) * kernelWeight;
                            b += ((byte)col) * kernelWeight;
                        }
                    }

                    byte br = (byte)(r / kernelSum);
                    byte bg = (byte)(g / kernelSum);
                    byte bb = (byte)(b / kernelSum);

                    int color =
                        (255 << 24)
                        | (br << 16)
                        | (bg << 8)
                        | (bb);

                    pixels[index++] = color;
                }
            }
        }

        public static void ConvoluteX(this WriteableBitmap bmp, byte[] kernel)
        {
            int kernelWidth = kernel.Length;
            if ((kernelWidth & 1) == 0)
            {
                Debug.WriteLine("Kernel width must be odd!");
                return;
            }

            int[] pixels = bmp.GetPixels();
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int index = 0;
            int halfKernelWidth = kernelWidth / 2;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int kernelSum = 0;
                    int r = 0;
                    int g = 0;
                    int b = 0;

                    for (int kx = -halfKernelWidth; kx <= halfKernelWidth; kx++)
                    {
                        int px = kx + x;
                        if (px < 0 || px >= w)
                        {
                            continue;
                        }

                        int kernelIndex = kx + halfKernelWidth;
                        byte kernelWeight = kernel[kernelIndex];
                        kernelSum += kernelWeight;

                        int innerIndex = y * w + px;
                        int col = pixels[innerIndex];
                        r += ((byte)(col >> 16)) * kernelWeight;
                        g += ((byte)(col >> 8)) * kernelWeight;
                        b += ((byte)col) * kernelWeight;

                    }

                    byte br = (byte)(r / kernelSum);
                    byte bg = (byte)(g / kernelSum);
                    byte bb = (byte)(b / kernelSum);

                    int color =
                        (255 << 24)
                        | (br << 16)
                        | (bg << 8)
                        | (bb);

                    pixels[index++] = color;
                }
            }
        }

        public static void ConvoluteY(this WriteableBitmap bmp, byte[] kernel)
        {
            int kernelHeight = kernel.Length;
            if ((kernelHeight & 1) == 0)
            {
                Debug.WriteLine("Kernel height must be odd!");
                return;
            }

            int[] pixels = bmp.GetPixels();
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int index = 0;
            int halfKernelHeight = kernelHeight / 2;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int kernelSum = 0;
                    int r = 0;
                    int g = 0;
                    int b = 0;

                    for (int ky = -halfKernelHeight; ky <= halfKernelHeight; ky++)
                    {
                        int py = ky + y;
                        if (py < 0 || py >= h)
                        {
                            continue;
                        }

                        int kernelIndex = (ky + halfKernelHeight);
                        byte kernelWeight = kernel[kernelIndex];
                        kernelSum += kernelWeight;

                        int innerIndex = py * w + x;
                        int col = pixels[innerIndex];
                        r += ((byte)(col >> 16)) * kernelWeight;
                        g += ((byte)(col >> 8)) * kernelWeight;
                        b += ((byte)col) * kernelWeight;
                    }

                    byte br = (byte)(r / kernelSum);
                    byte bg = (byte)(g / kernelSum);
                    byte bb = (byte)(b / kernelSum);

                    int color =
                        (255 << 24)
                        | (br << 16)
                        | (bg << 8)
                        | (bb);

                    pixels[index++] = color;
                }
            }
        }

        private static byte[] pixel;
        //高斯模糊
        public static void BoxBlur(this WriteableBitmap bmp, int range)
        {
            if ((range & 1) == 0)
            {
                Debug.WriteLine("Range must be odd!");
                return;
            }

            //pixel = WindowsRuntimeBufferExtensions.ToArray(bmp.PixelBuffer);
            bmp.BoxBlurHorizontal(range);
            bmp.BoxBlurVertical(range);
        }

        //高斯模糊
        public static void BoxBlur(this WriteableBitmap bmp, int x, int y)
        {
            if ((x & 1) == 0 || (y & 1) == 0)
            {
                Debug.WriteLine("Range must be odd!");
                return;
            }

            bmp.BoxBlurHorizontal(x);
            bmp.BoxBlurVertical(y);
        }

        //BitConverter
        public static void BoxBlurHorizontal(this WriteableBitmap bmp, int range)
        {
            int[] pixels = bmp.GetPixels();
            //int[] pixels = pixel.ToInt();
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int halfRange = range / 2;
            int index = 0;
            int[] newColors = new int[w];

            for (int y = 0; y < h; y++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                for (int x = -halfRange; x < w; x++)
                {
                    int oldPixel = x - halfRange - 1;
                    if (oldPixel >= 0)
                    {
                        int col = pixels[index + oldPixel];
                        if (col != 0)
                        {
                            r -= ((byte)(col >> 16));
                            g -= ((byte)(col >> 8));
                            b -= ((byte)col);
                        }
                        hits--;
                    }

                    int newPixel = x + halfRange;
                    if (newPixel < w)
                    {
                        int col = pixels[index + newPixel];
                        if (col != 0)
                        {
                            r += ((byte)(col >> 16));
                            g += ((byte)(col >> 8));
                            b += ((byte)col);
                        }
                        hits++;
                    }

                    if (x >= 0)
                    {
                        int color =
                            (255 << 24)
                            | ((byte)(r / hits) << 16)
                            | ((byte)(g / hits) << 8)
                            | ((byte)(b / hits));

                        newColors[x] = color;
                    }
                }

                for (int x = 0; x < w; x++)
                {
                    pixels[index + x] = newColors[x];
                }

                index += w;
            }
        }

        public static void BoxBlurVertical(this WriteableBitmap bmp, int range)
        {
            int[] pixels = bmp.GetPixels();
            //int[] pixels = pixel.ToInt();
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int halfRange = range / 2;

            int[] newColors = new int[h];
            int oldPixelOffset = -(halfRange + 1) * w;
            int newPixelOffset = (halfRange) * w;

            for (int x = 0; x < w; x++)
            {
                int hits = 0;
                int r = 0;
                int g = 0;
                int b = 0;
                int index = -halfRange * w + x;
                for (int y = -halfRange; y < h; y++)
                {
                    int oldPixel = y - halfRange - 1;
                    if (oldPixel >= 0)
                    {
                        int col = pixels[index + oldPixelOffset];
                        if (col != 0)
                        {
                            r -= ((byte)(col >> 16));
                            g -= ((byte)(col >> 8));
                            b -= ((byte)col);
                        }
                        hits--;
                    }

                    int newPixel = y + halfRange;
                    if (newPixel < h)
                    {
                        int col = pixels[index + newPixelOffset];
                        if (col != 0)
                        {
                            r += ((byte)(col >> 16));
                            g += ((byte)(col >> 8));
                            b += ((byte)col);
                        }
                        hits++;
                    }

                    if (y >= 0)
                    {
                        int color =
                            (255 << 24)
                            | ((byte)(r / hits) << 16)
                            | ((byte)(g / hits) << 8)
                            | ((byte)(b / hits));

                        newColors[y] = color;
                    }

                    index += w;
                }

                for (int y = 0; y < h; y++)
                {
                    pixels[y * w + x] = newColors[y];
                }
            }
        }

        public static int[] GetPixels(this WriteableBitmap source)
        {
            BitmapContext imgContext = new BitmapContext(source, ReadWriteMode.ReadWrite);
            return imgContext.Pixels;
        }

        public static int[] ToInt(this byte[] bytes)
        {
            return bytes.Select(x => (int)x).ToArray();
        }

        public static int[] ToDouble(this double[] doubles)
        {
            return doubles.Select(x => (int)x).ToArray();
        }

        private static double[,] GaussFuc(int r, double sigma)
        {
            int size = 2 * r + 1;
            double[,] gaussResult = new double[size, size];
            double k = 0.0;
            for (int y = -r, h = 0; y <= r; y++, h++)
            {
                for (int x = -r, w = 0; x <= r; x++, w++)
                {
                    gaussResult[w, h] = (1.0 / (2.0 * Math.PI * sigma * sigma)) * (Math.Exp(-((double)x * (double)x + (double)y * (double)y) / (2.0 * sigma * sigma)));
                    k += gaussResult[w, h];
                }
            }
            return gaussResult;
        }

        //一维高斯模糊
        private static double[] GaussKernel1D(int r, double sigma)
        {
            double[] filter = new double[2 * r + 1];
            double sum = 0.0;
            for (int i = 0; i < filter.Length; i++)
            {
                filter[i] = Math.Exp((double)(-(i - r) * (i - r)) / (2.0 * sigma * sigma));
                sum += filter[i];
            }
            for (int i = 0; i < filter.Length; i++)
            {
                filter[i] = filter[i] / sum;
            }
            return filter;
        }

        //多维高斯模糊
        private static double[] GaussKernel(int radius, double sigma)
        {
            int length = 2 * radius + 1;
            double[] kernel = new double[length];
            double sum = 0.0;
            for (int i = 0; i < length; i++)
            {
                kernel[i] = Math.Exp((double)(-(i - radius) * (i - radius)) / (2.0 * sigma * sigma));
                sum += kernel[i];
            }
            for (int i = 0; i < length; i++)
            {
                kernel[i] = kernel[i] / sum;
            }
            return kernel;
        }

        /// <summary>
        /// Gauss filter process
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="radius">The radius of gauss kernel,from 0 to 100.</param>
        /// <param name="sigma">The convince of gauss kernel, from 0 to 30.</param>
        /// <returns></returns>
        public static WriteableBitmap GaussFilter(this WriteableBitmap src, int radius, double sigma) ////高斯滤波
        {
            if (src != null)
            {
                int w = src.PixelWidth;
                int h = src.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] srcValue = src.PixelBuffer.ToArray();
                byte[] tempValue = (byte[])srcValue.Clone();
                double[] kernel = GaussKernel(radius, sigma);
                double tempB = 0.0, tempG = 0.0, tempR = 0.0;
                int rem = 0;
                int t = 0;
                int v = 0;
                double K = 0.0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        tempB = tempG = tempR = 0.0;
                        for (int k = -radius; k <= radius; k++)
                        {
                            rem = (Math.Abs(x + k) % w);
                            t = rem * 4 + y * w * 4;
                            K = kernel[k + radius];
                            tempB += srcValue[t] * K;
                            tempG += srcValue[t + 1] * K;
                            tempR += srcValue[t + 2] * K;
                        }
                        v = x * 4 + y * w * 4;
                        tempValue[v] = (byte)tempB;
                        tempValue[v + 1] = (byte)tempG;
                        tempValue[v + 2] = (byte)tempR;
                    }
                }
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        tempB = tempG = tempR = 0.0;
                        for (int k = -radius; k <= radius; k++)
                        {
                            rem = (Math.Abs(y + k) % h);
                            t = rem * w * 4 + x * 4;
                            K = kernel[k + radius];
                            tempB += tempValue[t] * K;
                            tempG += tempValue[t + 1] * K;
                            tempR += tempValue[t + 2] * K;
                        }
                        v = x * 4 + y * w * 4;
                        srcValue[v] = (byte)tempB;
                        srcValue[v + 1] = (byte)tempG;
                        srcValue[v + 2] = (byte)tempR;
                    }
                }
                Stream sTemp = srcImage.PixelBuffer.AsStream();
                sTemp.Seek(0, SeekOrigin.Begin);
                sTemp.Write(srcValue, 0, w * 4 * h);
                return srcImage;
            }
            else
            {
                return null;
            }
        }
    }
}
