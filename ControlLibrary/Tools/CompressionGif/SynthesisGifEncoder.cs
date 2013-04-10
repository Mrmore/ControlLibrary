using ControlLibrary.GifSynthesis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools.CompressionGif
{
    /// <summary>
    /// 缺少GIF图片每帧的Left,Top 和共享图层等信息，
    /// 这里只是一个简版合成GIF的合成类
    /// </summary>
    public class SynthesisGifEncoder
    {
        protected int width; // image size
        protected int height;
        protected Color transparent = Color.FromArgb(255, 0, 0, 0); // transparent color if given
        protected int transIndex; // transparent index in color table
        protected int repeat = -1; // no repeat
        protected int delay = 0; // frame delay (hundredths)
        protected bool started = false; // ready to output frames
        protected MemoryStream fs;

        //补齐3个重要信息Left,Top 和共享图层
        protected int top = 0;
        protected int left = 0;
        //共享图层(如果是2为不共享,其余不为2的为共享)
        //disposal 可能就是 dispose
        protected int disposal = 2;

        protected byte[] pixels; // BGR byte array from frame
        protected byte[] indexedPixels; // converted frame indexed to palette
        protected int colorDepth; // number of bit planes
        protected byte[] colorTab; // RGB palette
        protected bool[] usedEntry = new bool[256]; // active palette entries
        protected int palSize = 7; // color table size (bits-1)
        protected int dispose = -1; // disposal code (-1 = use default)
        protected bool closeStream = false; // close stream when finished
        protected bool firstFrame = true;
        protected bool sizeSet = false; // if false, get size from first frame
        protected int sample = 10; // default sample interval for quantizer
        protected IRandomAccessStream imageIAStream; // current frame

        /**
         * Sets the delay time between each frame, or changes it
         * for subsequent frames (applies to last frame added).
         *
         * @param ms int delay time in milliseconds
         */
        public void SetDelay(int ms)
        {
            delay = (int)Math.Round(ms / 10.0f);
        }

        /**
         * Sets the GIF frame disposal code for the last added frame
         * and any subsequent frames.  Default is 0 if no transparent
         * color has been set, otherwise 2.
         * @param code int disposal code.
         */
        public void SetDispose(int code)
        {
            if (code >= 0)
            {
                dispose = code;
            }
        }

        /**
         * Sets the number of times the set of GIF frames
         * should be played.  Default is 1; 0 means play
         * indefinitely.  Must be invoked before the first
         * image is added.
         *
         * @param iter int number of iterations.
         * @return
         */
        public void SetRepeat(int iter)
        {
            if (iter >= 0)
            {
                repeat = iter;
            }
        }

        /**
         * Sets the transparent color for the last added frame
         * and any subsequent frames.
         * Since all colors are subject to modification
         * in the quantization process, the color in the final
         * palette for each frame closest to the given color
         * becomes the transparent color for that frame.
         * May be set to null to indicate no transparent color.
         *
         * @param c Color to be treated as transparent on display.
         */
        public void SetTransparent(Color c)
        {
            transparent = c;
        }

        public void SetLeft(int left = 0)
        {
            this.left = left;
        }

        public void SetTop(int top = 0)
        {
            this.top = top;
        }

        /**
         * Adds next GIF frame.  The frame is not written immediately, but is
         * actually deferred until the next frame is received so that timing
         * data can be inserted.  Invoking <code>finish()</code> flushes all
         * frames.  If <code>setSize</code> was not invoked, the size of the
         * first image is used for all subsequent frames.
         *
         * @param im BufferedImage containing frame to write.
         * @return true if successful.
         */

        public async Task<bool> AddFrame(IRandomAccessStream iRandomAccessStream)
        {
            if ((iRandomAccessStream == null) || !started || iRandomAccessStream.Size <= 0)
            {
                return false;
            }
            bool ok = true;
            try
            {
                if (!sizeSet)
                {
                    // use first frame's size
                    /*Point pointWH = await WriteableBitmapExpansion.GetPixelWidthAndHeight(iRandomAccessStream);
                    SetSize((int)pointWH.X, (int)pointWH.Y);*/
                }
                imageIAStream = iRandomAccessStream;
                GetImagePixels(iRandomAccessStream); // convert to correct format if necessary
                AnalyzePixels(); // build color table & map pixels
                if (firstFrame)
                {
                    WriteLSD(); // logical screen descriptior
                    WritePalette(); // 444
                    if (repeat >= 0)
                    {
                        // use NS app extension to indicate reps
                        WriteNetscapeExt();
                    }
                }
                WriteGraphicCtrlExt(); // write graphic control extension
                WriteImageDesc(); // image descriptor
                if (!firstFrame)
                {
                    WritePalette(); // local color table
                }
                WritePixels(); // encode and write pixel data
                firstFrame = false;
            }
            catch (IOException e)
            {
                ok = false;
            }

            return ok;
        }

        /**
         * Flushes any pending data and closes output file.
         * If writing to an OutputStream, the stream is not
         * closed.
         */
        public async Task<InMemoryRandomAccessStream> Finish()
        {
            InMemoryRandomAccessStream memoryStream = null;
            if (!started) return null;
            started = false;
            try
            {
                fs.WriteByte(0x3b); // gif trailer
                fs.Flush();
                byte[] bytes = (fs as MemoryStream).ToArray();
                memoryStream = new InMemoryRandomAccessStream();
                DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
                datawriter.WriteBytes(bytes);
                await datawriter.StoreAsync();
            }
            catch (IOException e)
            {
                return null;
            }

            // reset for subsequent use
            transIndex = 0;
            fs = null;
            pixels = null;
            indexedPixels = null;
            colorTab = null;
            closeStream = false;
            firstFrame = true;

            return memoryStream;
        }

        /**
         * Sets frame rate in frames per second.  Equivalent to
         * <code>setDelay(1000/fps)</code>.
         *
         * @param fps float frame rate (frames per second)
         */
        public void SetFrameRate(float fps)
        {
            if (fps != 0f)
            {
                delay = (int)Math.Round(100f / fps);
            }
        }

        /**
         * Sets quality of color quantization (conversion of images
         * to the maximum 256 colors allowed by the GIF specification).
         * Lower values (minimum = 1) produce better colors, but slow
         * processing significantly.  10 is the default, and produces
         * good color mapping at reasonable speeds.  Values greater
         * than 20 do not yield significant improvements in speed.
         *
         * @param quality int greater than 0.
         * @return
         */
        public void SetQuality(int quality)
        {
            if (quality < 1) quality = 1;
            sample = quality;
        }

        /**
         * Sets the GIF frame size.  The default size is the
         * size of the first frame added if this method is
         * not invoked.
         *
         * @param w int frame width.
         * @param h int frame width.
         */
        public void SetSize(int w, int h)
        {
            if (started && !firstFrame) return;
            width = w;
            height = h;
            if (width < 1) width = 320;
            if (height < 1) height = 240;
            sizeSet = true;
        }

        /**
         * Initiates GIF file creation on the given stream.  The stream
         * is not closed automatically.
         *
         * @param os OutputStream on which GIF images are written.
         * @return false if initial write failed.
         */
        /// <summary>
        /// 创建合成GIF临时流
        /// </summary>
        public bool CreateGifImage()
        {
            bool ok = true;
            closeStream = false;
            try
            {
                fs = new MemoryStream();
                WriteString("GIF89a"); // header GIF89a GIF87a
            }
            catch (IOException e)
            {
                if (e != null)
                {
                    Debug.WriteLine(e.Message);
                }
                ok = false;
            }
            return started = ok;
        }

        /**
         * Analyzes image colors and creates color map.
         */
        protected void AnalyzePixels()
        {
            int len = pixels.Length;
            int nPix = len / 3;
            indexedPixels = new byte[nPix];
            NeuQuant nq = new NeuQuant(pixels, len, sample);
            // initialize quantizer
            colorTab = nq.Process(); // create reduced palette
            //for (int i = 0; i < colorTab.Length; i++)
            //{
            //    if (colorTab[i] == 0)
            //    {
            //        colorTab[i] = 255;
            //    }
            //}

            // convert map from BGR to RGB
            //for (int i = 0; i < colorTab.Length; i += 3)
            //{
            //    byte temp = colorTab[i];
            //    colorTab[i] = colorTab[i + 2];
            //    colorTab[i + 2] = temp;
            //    usedEntry[i / 3] = false;
            //}

            for (int i = 0; i < colorTab.Length; i += 3)
            {
                usedEntry[i / 3] = false;
            }

            // map image pixels to new palette
            int k = 0;
            for (int i = 0; i < nPix; i++)
            {
                int index =
                    nq.Map(pixels[k++] & 0xff,
                    pixels[k++] & 0xff,
                    pixels[k++] & 0xff);
                usedEntry[index] = true;
                indexedPixels[i] = (byte)index;
            }
            pixels = null;
            colorDepth = 8;
            palSize = 7;
            // get closest match to transparent color if specified
            if (transparent != Color.FromArgb(255, 0, 0, 0))
            {
                transIndex = FindClosest(transparent);//255
            }
        }

        /**
         * Returns index of palette color closest to c
         *
         */
        protected int FindClosest(Color c)
        {
            if (colorTab == null) return -1;
            int r = c.R;
            int g = c.G;
            int b = c.B;
            int minpos = 0;
            int dmin = 256 * 256 * 256;
            int len = colorTab.Length;
            for (int i = 0; i < len; )
            {
                int dr = r - (colorTab[i++] & 0xff);
                int dg = g - (colorTab[i++] & 0xff);
                int db = b - (colorTab[i] & 0xff);
                int d = dr * dr + dg * dg + db * db;
                int index = i / 3;
                if (usedEntry[index] && (d < dmin))
                {
                    dmin = d;
                    minpos = index;
                }
                i++;
            }
            return minpos;
        }

        /**
         * Extracts image pixels into byte array "pixels"
         */
        protected void GetImagePixels(IRandomAccessStream iRandomAccessStream)
        {
            /*Point pointWH = await WriteableBitmapExpansion.GetPixelWidthAndHeight(iRandomAccessStream);
            int w = (int)pointWH.X;
            int h = (int)pointWH.Y;*/
            int count = 0;
            byte[] tempByte = null;

            /*
            //进行统一尺寸的格式压缩
            if ((w != width)
                || (h != height)
                )
            {
                var bytes = await WriteableBitmapExpansion.ResizeBytes(iRandomAccessStream, width, height, BitmapInterpolationMode.Cubic);
                pixels = new Byte[3 * width * height];
                pointWH = new Point(width, height);
                tempByte = bytes;
            }
            else
            {
                pointWH = await WriteableBitmapExpansion.GetPixelWidthAndHeight(imageIAStream);
                pixels = new Byte[3 * (int)pointWH.X * (int)pointWH.Y];
                tempByte = await WriteableBitmapExpansion.WriteableBitmapToBytes(imageIAStream);
            }
            */

            pixels = new Byte[3 * (int)width * (int)height];
            /*tempByte = await WriteableBitmapExpansion.WriteableBitmapToBytes(imageIAStream);*/
            Stream inputStream = WindowsRuntimeStreamExtensions.AsStreamForRead(imageIAStream.GetInputStreamAt(0));
            tempByte = inputStream.ConvertStreamTobyte();

            for (int th = 0; th < height; th++)
            {
                for (int tw = 0; tw < width; tw++)
                {
                    Color color = WriteableBitmapExpansion.GetPixel(tempByte, Convert.ToInt32(width), tw, th);
                    pixels[count] = color.R;
                    count++;
                    pixels[count] = color.G;
                    count++;
                    pixels[count] = color.B;
                    count++;
                }
            }
        }

        /**
         * Writes Graphic Control Extension
         */
        protected void WriteGraphicCtrlExt()
        {
            fs.WriteByte(0x21); // extension introducer
            fs.WriteByte(0xf9); // GCE label
            fs.WriteByte(4); // data block size
            int transp, disp;
            if (transparent == Color.FromArgb(255, 0, 0, 0))
            {
                transp = 0;
                disp = 0; // dispose = no action
            }
            else
            {
                transp = 1;
                disp = 2; // force clear if using transparent color
            }
            if (dispose >= 0)
            {
                disp = dispose & 7; // user override
            }
            disp <<= 2;

            // packed fields
            fs.WriteByte(Convert.ToByte(0 | // 1:3 reserved
                disp | // 4:6 disposal
                0 | // 7   user input - 0 = none
                transp)); // 8   transparency flag

            WriteShort(delay); // delay x 1/100 sec
            fs.WriteByte(Convert.ToByte(transIndex)); // transparent color index //颜色
            fs.WriteByte(0); // block terminator
        }

        /**
         * Writes Image Descriptor
         */
        protected void WriteImageDesc()
        {
            fs.WriteByte(Convert.ToByte(0x2c)); // image separator
            WriteShort(top); // image position x,y = 0,0
            WriteShort(left);
            WriteShort(width); // image size
            WriteShort(height);

            // packed fields
            if (firstFrame)
            {
                // no LCT  - GCT is used for first (or only) frame
                fs.WriteByte(0);
            }
            else
            {
                // specify normal LCT
                fs.WriteByte(Convert.ToByte(0x80 | // 1 local color table  1=yes 128
                    0 | // 2 interlace - 0=no
                    0 | // 3 sorted - 0=no
                    0 | // 4-5 reserved
                    palSize)); // 6-8 size of color table
            }
        }

        /**
         * Writes Logical Screen Descriptor
         */
        protected void WriteLSD()
        {
            // logical screen size
            WriteShort(width);
            WriteShort(height);
            // packed fields
            fs.WriteByte(Convert.ToByte(0x80 | // 1   : global color table flag = 1 (gct used)
                0x70 | // 2-4 : color resolution = 7
                0x00 | // 5   : gct sort flag = 0
                palSize)); // 6-8 : gct size

            fs.WriteByte(0); // background color index
            fs.WriteByte(0); // pixel aspect ratio - assume 1:1
        }

        /**
         * Writes Netscape application extension to define
         * repeat count.
         */
        protected void WriteNetscapeExt()
        {
            fs.WriteByte(0x21); // extension introducer
            fs.WriteByte(0xff); // app extension label
            fs.WriteByte(11); // block size
            WriteString("NETSCAPE" + "2.0"); // app id + auth code
            fs.WriteByte(3); // sub-block size
            fs.WriteByte(1); // loop sub-block id
            WriteShort(repeat); // loop count (extra iterations, 0=repeat forever)
            fs.WriteByte(0); // block terminator
        }

        /**
         * Writes color table
         */
        protected void WritePalette()
        {
            fs.Write(colorTab, 0, colorTab.Length);
        }

        /**
         * Encodes and writes pixel data
         */
        protected void WritePixels()
        {
            LZWEncoder encoder =
                new LZWEncoder(width, height, indexedPixels, colorDepth);
            encoder.Encode(fs);
        }

        /**
         *    Write 16-bit value to output stream, LSB first
         */
        protected void WriteShort(int value)
        {
            fs.WriteByte(Convert.ToByte(value & 0xff));
            fs.WriteByte(Convert.ToByte((value >> 8) & 0xff));
        }

        /**
         * Writes string to output stream
         */
        protected void WriteString(String s)
        {
            char[] chars = s.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                fs.WriteByte((byte)chars[i]);
            }
        }
    }
}
