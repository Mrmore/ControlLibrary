using ControlLibrary.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.GifSynthesis
{
    public static class WriteableBitmapExpansion
    {
        internal const int SizeOfArgb = 4;

        public static Stream WriteableBitmapToStream(this WriteableBitmap source)
        {
            source.Invalidate();
            var inputStream = source.PixelBuffer.AsStream();
            return inputStream;
        }

        public static byte[] WriteableBitmapToBytes(this WriteableBitmap source)
        {
            var inputStream = WriteableBitmapToStream(source);
            var copiedBytes =
                new byte[4 * source.PixelWidth * source.PixelHeight];
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.Read(copiedBytes, 0, copiedBytes.Length);
            return copiedBytes;
        }

        /// <summary>
        /// WriteableBitmap克隆
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static WriteableBitmap CopyWriteableBitmap(this WriteableBitmap source)
        {
            //source.Invalidate();
            //var copiedBytes =
            //    new byte[4 * source.PixelWidth * source.PixelHeight];
            //var inputStream = source.PixelBuffer.AsStream();
            //inputStream.Seek(0, SeekOrigin.Begin);
            //inputStream.Read(copiedBytes, 0, copiedBytes.Length);
            var copiedBytes = WriteableBitmapToBytes(source);

            var target = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(copiedBytes, 0, copiedBytes.Length);
            target.Invalidate();
            return target;
        }

        /// <summary>
        /// WriteableBitmap克隆
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static WriteableBitmap CloneWriteableBitmap(this WriteableBitmap source)
        {
            BitmapContext srcContext = new BitmapContext(source, ReadWriteMode.ReadOnly);
            WriteableBitmap result = BitmapFactory.New(srcContext.Width, srcContext.Height);
            BitmapContext destContext = new BitmapContext(result);
            System.Buffer.BlockCopy(srcContext.Pixels, 0, destContext.Pixels, 0, srcContext.Length * SizeOfArgb);
            return result;
        }

        /// <summary>
        /// 获取像素
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Color GetPixel(this WriteableBitmap source, int x, int y)
        {
            BitmapContext imgContext = new BitmapContext(source);
            var c = imgContext.Pixels[y * imgContext.Width + x];
            var a = (byte)(c >> 24);

            // Prevent division by zero
            int ai = a;
            if (ai == 0)
            {
                ai = 1;
            }

            // Scale inverse alpha to use cheap integer mul bit shift
            ai = ((255 << 8) / ai);
            Color color = Color.FromArgb(a,
                                  (byte)((((c >> 16) & 0xFF) * ai) >> 8),
                                  (byte)((((c >> 8) & 0xFF) * ai) >> 8),
                                  (byte)((((c & 0xFF) * ai) >> 8)));
            return color;
        }

        public static Color GetPixel(byte[] image, int PixelWidth, int x, int y)
        {
            int start = (y * PixelWidth + x) * 4;
            Color result = new Color();
            result.B = image[start + 0];
            result.G = image[start + 1];
            result.R = image[start + 2];
            result.A = image[start + 3];
            return result;
        }

        public static async Task<WriteableBitmap> Resize(this WriteableBitmap source, IRandomAccessStream iRandomAccessStream, int width, int height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor)
        {
            Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(iRandomAccessStream.GetInputStreamAt(0));
            var copiedBytes = ConvertStreamTobyte(inputStream);
            Stream tempStream = new MemoryStream(copiedBytes);

            Guid decoderId = Guid.Empty;
            ImageType type = ImageTypeCheck.CheckImageType(copiedBytes);
            switch (type)
            {
                case ImageType.GIF:
                    {
                        break;
                    }
                case ImageType.JPG:
                    {
                        decoderId = BitmapDecoder.JpegDecoderId;
                        break;
                    }
                case ImageType.PNG:
                    {
                        decoderId = BitmapDecoder.PngDecoderId;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, randomAccessStream);
            var frame = await bitDecoder.GetFrameAsync(0);
            var pixels = await frame.GetPixelDataAsync(
            frame.BitmapPixelFormat,
            frame.BitmapAlphaMode,
            new BitmapTransform()
            {
                ScaledHeight = Convert.ToUInt32(height),
                ScaledWidth = Convert.ToUInt32(width),
                InterpolationMode = bitmapInterpolationMode,
            },
            ExifOrientationMode.RespectExifOrientation,
            ColorManagementMode.ColorManageToSRgb);

            var bytes = pixels.DetachPixelData();
            var target = new WriteableBitmap(width, height);
            var outputStreamWB = target.PixelBuffer.AsStream();
            outputStreamWB.Seek(0, SeekOrigin.Begin);
            outputStreamWB.Write(bytes, 0, bytes.Length);
            target.Invalidate();
            return target;
        }

        public static async Task<WriteableBitmap> Resize(this WriteableBitmap source, int width, int height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor)
        {
            return new WriteableBitmap(width, height);
        }

        public static WriteableBitmap Resize(this WriteableBitmap image, int width, int height)
        {
            byte[] newPixels = new byte[width * height * 4];

            var source = new byte[4 * image.PixelWidth * image.PixelHeight];
            var inputStream = image.PixelBuffer.AsStream();
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.Read(source, 0, source.Length);

            int srcWidth = image.PixelWidth;
            int srcHeight = image.PixelHeight;


            byte[] sourcePixels = source;

            var GetColor = new Func<double, double, int, byte>((x, y, offset) => sourcePixels[(int)((y * srcWidth + x) * 4 + offset)]);

            double factorX = (double)srcWidth / width;
            double factorY = (double)srcHeight / height;

            double fractionX, oneMinusX, l, r;
            double fractionY, oneMinusY, t, b;

            byte c1, c2, c3, c4, b1, b2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int dstOffset = (y * width + x) * 4;

                    l = (int)Math.Floor(x * factorX);
                    t = (int)Math.Floor(y * factorY);

                    r = l + 1;
                    b = t + 1;

                    if (r >= srcWidth)
                    {
                        r = l;
                    }

                    if (b >= srcHeight)
                    {
                        b = t;
                    }

                    fractionX = x * factorX - l;
                    fractionY = y * factorY - t;

                    oneMinusX = 1.0 - fractionX;
                    oneMinusY = 1.0 - fractionY;

                    var function = new Func<int, byte>(offset =>
                    {
                        c1 = GetColor(l, t, offset);
                        c2 = GetColor(r, t, offset);
                        c3 = GetColor(l, b, offset);
                        c4 = GetColor(r, b, offset);

                        b1 = (byte)(oneMinusX * c1 + fractionX * c2);
                        b2 = (byte)(oneMinusX * c3 + fractionX * c4);

                        return (byte)(oneMinusY * b1 + fractionY * b2);
                    });

                    newPixels[dstOffset + 0] = function(0);
                    newPixels[dstOffset + 1] = function(1);
                    newPixels[dstOffset + 2] = function(2);
                    newPixels[dstOffset + 3] = 255;
                }
            }
            WriteableBitmap target = new WriteableBitmap(width, height);
            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(newPixels, 0, newPixels.Length);
            return target;
        }

        public static byte[] ConvertStreamTobyte(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public async static Task<IRandomAccessStream> ConvertBytesToIRandomAccessStream(this byte[] bytes)
        {
            InMemoryRandomAccessStream memoryRandomAccessStream = new InMemoryRandomAccessStream();
            DataWriter datawriter = new DataWriter(memoryRandomAccessStream.GetOutputStreamAt(0));
            datawriter.WriteBytes(bytes);
            await datawriter.StoreAsync();
            return memoryRandomAccessStream;
        }

        public async static Task<IRandomAccessStream> ConvertStreamIRandomAccessStream(this Stream stream)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(stream.AsInputStream(), outputStream);
            return randomAccessStream;
        }

        public static async Task<IRandomAccessStream> ResizeStream(this IRandomAccessStream iRandomAccessStream, int width, int height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor, bool IsCompression = true)
        {
            Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(iRandomAccessStream.GetInputStreamAt(0));
            var copiedBytes = ConvertStreamTobyte(inputStream);
            Stream tempStream = new MemoryStream(copiedBytes);

            Guid decoderId = Guid.Empty;
            ImageType type = ImageTypeCheck.CheckImageType(copiedBytes);
            switch (type)
            {
                case ImageType.GIF:
                    {
                        break;
                    }
                case ImageType.JPG:
                    {
                        decoderId = BitmapDecoder.JpegDecoderId;
                        break;
                    }
                case ImageType.PNG:
                    {
                        decoderId = BitmapDecoder.PngDecoderId;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, randomAccessStream);
            var frame = await bitDecoder.GetFrameAsync(0);
            var ph = frame.PixelHeight;
            var pw = frame.OrientedPixelWidth;
            PixelDataProvider pixels = null;
            if (IsCompression)
            {
                pixels = await frame.GetPixelDataAsync(
                frame.BitmapPixelFormat,
                frame.BitmapAlphaMode,
                new BitmapTransform()
                {
                    ScaledHeight = Convert.ToUInt32(height),
                    ScaledWidth = Convert.ToUInt32(width),
                    InterpolationMode = bitmapInterpolationMode,
                },
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.ColorManageToSRgb);
            }
            else
            {
                pixels = await frame.GetPixelDataAsync();
            }

            var bytes = pixels.DetachPixelData();
            return (await ConvertBytesToIRandomAccessStream(bytes));

            //var encoderId = BitmapEncoder.PngEncoderId;
            //InMemoryRandomAccessStream tmemoryStream = new InMemoryRandomAccessStream();
            //var encoder = await BitmapEncoder.CreateAsync(encoderId, tmemoryStream);
            //var bytes = pixels.DetachPixelData();
            //encoder.SetPixelData(
            //    frame.BitmapPixelFormat,
            //    BitmapAlphaMode.Premultiplied,
            //    //frame.BitmapAlphaMode,
            //    Convert.ToUInt32(width),
            //    Convert.ToUInt32(height),
            //    Convert.ToUInt32(frame.DpiX * (ph / height)),
            //    Convert.ToUInt32(frame.DpiY * (pw / width)),
            //    bytes);
            //await encoder.FlushAsync();
            //tmemoryStream.Seek(0);
            //return tmemoryStream;
        }

        //后扩展
        public static async Task<IRandomAccessStream> ResizeStream(this IRandomAccessStream iRandomAccessStream, double width, double height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor, bool IsCompression = true)
        {
            return (await ResizeStream(iRandomAccessStream, Convert.ToInt32(width), Convert.ToInt32(height), bitmapInterpolationMode, IsCompression));
        }

        public static async Task<byte[]> ResizeBytes(this IRandomAccessStream iRandomAccessStream, int width, int height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor, bool IsCompression = true)
        {
            Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(iRandomAccessStream.GetInputStreamAt(0));
            var copiedBytes = ConvertStreamTobyte(inputStream);
            Stream tempStream = new MemoryStream(copiedBytes);

            Guid decoderId = Guid.Empty;
            ImageType type = ImageTypeCheck.CheckImageType(copiedBytes);
            switch (type)
            {
                case ImageType.GIF:
                    {
                        break;
                    }
                case ImageType.JPG:
                    {
                        decoderId = BitmapDecoder.JpegDecoderId;
                        break;
                    }
                case ImageType.PNG:
                    {
                        decoderId = BitmapDecoder.PngDecoderId;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, randomAccessStream);
            var frame = await bitDecoder.GetFrameAsync(0);
            PixelDataProvider pixels = null;
            if (IsCompression)
            {
                pixels = await frame.GetPixelDataAsync(
                frame.BitmapPixelFormat,
                frame.BitmapAlphaMode,
                new BitmapTransform()
                {
                    ScaledHeight = Convert.ToUInt32(height),
                    ScaledWidth = Convert.ToUInt32(width),
                    InterpolationMode = bitmapInterpolationMode,
                },
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.ColorManageToSRgb);
            }
            else
            {
                pixels = await frame.GetPixelDataAsync();
            }

            return pixels.DetachPixelData();
        }

        //后扩展
        public static async Task<byte[]> ResizeBytes(this IRandomAccessStream iRandomAccessStream, double width, double height,
            BitmapInterpolationMode bitmapInterpolationMode = BitmapInterpolationMode.NearestNeighbor, bool IsCompression = true)
        {
            return (await ResizeBytes(iRandomAccessStream, Convert.ToInt32(width), Convert.ToInt32(height), bitmapInterpolationMode, IsCompression));
        }

        public static async Task<Point> GetPixelWidthAndHeight(this IRandomAccessStream iRandomAccessStream)
        {
            var tempIRandomAccessStream = iRandomAccessStream.CloneStream();
            Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(tempIRandomAccessStream.GetInputStreamAt(0));
            var copiedBytes = ConvertStreamTobyte(inputStream);
            Stream tempStream = new MemoryStream(copiedBytes);

            Guid decoderId = Guid.Empty;
            ImageType type = ImageTypeCheck.CheckImageType(copiedBytes);
            switch (type)
            {
                case ImageType.GIF:
                    {
                        break;
                    }
                case ImageType.JPG:
                    {
                        decoderId = BitmapDecoder.JpegDecoderId;
                        break;
                    }
                case ImageType.PNG:
                    {
                        decoderId = BitmapDecoder.PngDecoderId;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, randomAccessStream);
            var frame = await bitDecoder.GetFrameAsync(0);
            Point point = new Point();
            point.X = frame.PixelWidth;
            point.Y = frame.PixelHeight;
            return point;
        }

        public static byte[] ConvertIRandomAccessStreamToByte(this IRandomAccessStream input)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(input.GetInputStreamAt(0));
            return ConvertStreamTobyte(stream);
        }

        public static async Task<byte[]> WriteableBitmapToBytes(this IRandomAccessStream input)
        {
            try
            {
                var point = await GetPixelWidthAndHeight(input);
                Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(input.GetInputStreamAt(0));
                var copiedBytes = ConvertStreamTobyte(inputStream);
                Stream tempStream = new MemoryStream(copiedBytes);

                Guid decoderId = Guid.Empty;
                ImageType type = ImageTypeCheck.CheckImageType(copiedBytes);
                switch (type)
                {
                    case ImageType.GIF:
                        {
                            break;
                        }
                    case ImageType.JPG:
                        {
                            decoderId = BitmapDecoder.JpegDecoderId;
                            break;
                        }
                    case ImageType.PNG:
                        {
                            decoderId = BitmapDecoder.PngDecoderId;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                var randomAccessStream = new InMemoryRandomAccessStream();
                var outputStream = randomAccessStream.GetOutputStreamAt(0);
                await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

                var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, randomAccessStream);
                var frame = await bitDecoder.GetFrameAsync(0);
                PixelDataProvider pixels = await frame.GetPixelDataAsync(
                frame.BitmapPixelFormat,
                BitmapAlphaMode.Premultiplied,
                new BitmapTransform()
                {
                    ScaledHeight = Convert.ToUInt32(point.Y),
                    ScaledWidth = Convert.ToUInt32(point.X)
                },
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.ColorManageToSRgb);
                var bytes = pixels.DetachPixelData();
                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static IBuffer ConvertBytesToIBuffer(this byte[] bytes)
        {
            return WindowsRuntimeBufferExtensions.AsBuffer(bytes, 0, bytes.Length);
        }

        public static byte[] ConvertIBufferToBytes(this IBuffer ibuffer)
        {
            return WindowsRuntimeBufferExtensions.ToArray(ibuffer, 0, Convert.ToInt32(ibuffer.Length));
        }

        //convert the bytes to WriteableBitmap
        private static WriteableBitmap BytesToImage(this byte[] src, int lw, int lh)
        {
            WriteableBitmap wbbitmap = new WriteableBitmap(lw, lh);
            Stream s = wbbitmap.PixelBuffer.AsStream();
            s.Seek(0, SeekOrigin.Begin);
            s.Write(src, 0, lw * lh * 3);
            return wbbitmap;
        }

        //convert the WriteableBitmap to bytes 24位RGB格式
        private static byte[] WriteableBitmapToBytes24(this WriteableBitmap src)
        {
            Stream temp = src.PixelBuffer.AsStream();
            byte[] tempBytes = new byte[src.PixelWidth * src.PixelHeight * 3];
            for (int i = 0; i < tempBytes.Length; i++)
            {
                temp.Seek(i, SeekOrigin.Begin);
                temp.Write(tempBytes, 0, tempBytes.Length);
            }
            temp.Dispose();
            return tempBytes;
        }

        private static int GetImageStride(int lWidth, int num)
        {
            int tlWidth = 0;
            int tnum = 0;
            int res = 0;
            if (lWidth > 0 && num > 0)
            {
                tlWidth = lWidth;
                tnum = num;
                res = tlWidth % tnum;
                while (res != 0)
                {
                    tlWidth = tnum;
                    tnum = res;
                    res = tlWidth % tnum;
                }
            }
            return (int)(3 * lWidth * num / tnum);
        }
    }
}
