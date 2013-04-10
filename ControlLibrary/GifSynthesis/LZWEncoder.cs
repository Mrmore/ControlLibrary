using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace ControlLibrary.GifSynthesis
{
    /// <summary>
    /// 缺少GIF图片每帧的Left,Top 和共享图层等信息，
    /// 这里只是一个简版合成GIF的合成类
    /// </summary>
    public class LZWEncoder
    {

        private static readonly int EOF = -1;

        private int imgW, imgH;
        private byte[] pixAry;
        private int initCodeSize;
        private int remaining;
        private int curPixel;

        // GIFCOMPR.C       - GIF Image compression routines
        //
        // Lempel-Ziv compression based on 'compress'.  GIF modifications by
        // David Rowley (mgardi@watdcsu.waterloo.edu)

        // General DEFINEs

        static readonly int BITS = 12;

        static readonly int HSIZE = 5003; // 80% occupancy

        // GIF Image compression - modified 'compress'
        //
        // Based on: compress.c - File compression ala IEEE Computer, June 1984.
        //
        // By Authors:  Spencer W. Thomas      (decvax!harpo!utah-cs!utah-gr!thomas)
        //              Jim McKie              (decvax!mcvax!jim)
        //              Steve Davies           (decvax!vax135!petsd!peora!srd)
        //              Ken Turkowski          (decvax!decwrl!turtlevax!ken)
        //              James A. Woods         (decvax!ihnp4!ames!jaw)
        //              Joe Orost              (decvax!vax135!petsd!joe)

        int n_bits; // number of bits/code
        int maxbits = BITS; // user settable max # bits/code
        int maxcode; // maximum code, given n_bits
        int maxmaxcode = 1 << BITS; // should NEVER generate this code

        int[] htab = new int[HSIZE]; //这个是放hash的筒子,在这里面可以很快的找到1个key
        int[] codetab = new int[HSIZE];

        int hsize = HSIZE; // for dynamic table sizing

        int free_ent = 0; // first unused entry

        // block compression parameters -- after all codes are used up,
        // and compression rate changes, start over.
        bool clear_flg = false;

        // Algorithm:  use open addressing double hashing (no chaining) on the
        // prefix code / next character combination.  We do a variant of Knuth's
        // algorithm D (vol. 3, sec. 6.4) along with G. Knott's relatively-prime
        // secondary probe.  Here, the modular division first probe is gives way
        // to a faster exclusive-or manipulation.  Also do block compression with
        // an adaptive reset, whereby the code table is cleared when the compression
        // ratio decreases, but after the table fills.  The variable-length output
        // codes are re-sized at this point, and a special CLEAR code is generated
        // for the decompressor.  Late addition:  construct the table according to
        // file size for noticeable speed improvement on small files.  Please direct
        // questions about this implementation to ames!jaw.

        int g_init_bits;

        int ClearCode;
        int EOFCode;

        // output
        //
        // Output the given code.
        // Inputs:
        //      code:   A n_bits-bit integer.  If == -1, then EOF.  This assumes
        //              that n_bits =< wordsize - 1.
        // Outputs:
        //      Outputs code to the file.
        // Assumptions:
        //      Chars are 8 bits long.
        // Algorithm:
        //      Maintain a BITS character long buffer (so that 8 codes will
        // fit in it exactly).  Use the VAX insv instruction to insert each
        // code in turn.  When the buffer fills up empty it and start over.

        int cur_accum = 0;
        int cur_bits = 0;

        int[] masks =
		{
			0x0000,
			0x0001,
			0x0003,
			0x0007,
			0x000F,
			0x001F,
			0x003F,
			0x007F,
			0x00FF,
			0x01FF,
			0x03FF,
			0x07FF,
			0x0FFF,
			0x1FFF,
			0x3FFF,
			0x7FFF,
			0xFFFF };

        // Number of characters so far in this 'packet'
        int a_count;

        // Define the storage for the packet accumulator
        byte[] accum = new byte[256];

        //----------------------------------------------------------------------------
        public LZWEncoder(int width, int height, byte[] pixels, int color_depth)
        {
            imgW = width;
            imgH = height;
            pixAry = pixels;
            initCodeSize = Math.Max(2, color_depth);
        }

        // Add a character to the end of the current packet, and if it is 254
        // characters, flush the packet to disk.
        void Add(byte c, Stream outs)
        {
            accum[a_count++] = c;
            if (a_count >= 254)
                Flush(outs);
        }

        // Clear out the hash table

        // table clear for block compress
        void ClearTable(Stream outs)
        {
            ResetCodeTable(hsize);
            free_ent = ClearCode + 2;
            clear_flg = true;

            Output(ClearCode, outs);
        }

        // reset code table
        // 全部初始化为-1
        void ResetCodeTable(int hsize)
        {
            for (int i = 0; i < hsize; ++i)
                htab[i] = -1;
        }

        void Compress(int init_bits, Stream outs)
        {
            int fcode;
            int i /* = 0 */;
            int c;
            int ent;
            int disp;
            int hsize_reg;
            int hshift;

            // Set up the globals:  g_init_bits - initial number of bits
            //原始数据的字长,在gif文件中，原始数据的字长可以为1(单色图),4(16色)，和8(256色)
            //开始的时候先加上1
            //但是当原始数据长度为1的时候，开始为3
            //因此原始长度1->3,4->5,8->9

            //?为何原始数据字长为1的时候，开始长度为3呢？?
            //如果+1=2，只能表示四种状态，加上clearcode和endcode就用完了。所以必须扩展到3
            g_init_bits = init_bits;

            // Set up the necessary values
            //是否需要加清除标志
            //GIF为了提高压缩率，采用的是变长的字长(VCL)。比如说原始数据是8位，那么开始先加上1位(8+1=9)
            //当标号到2^9=512的时候，超过了当前长度9所能表现的最大值，此时后面的标号就必须用10位来表示
            //以此类推，当标号到2^12的时候，因为最大为12,不能继续扩展了，需要在2^12=4096的位置上插入一个ClearCode,
            //表示从这往后，从9位重新再来了
            clear_flg = false;
            n_bits = g_init_bits;
            //获得n位数能表述的最大值(gif图像中开始一般为3,5,9，故maxcode一般为7,31,511)
            maxcode = MaxCode(n_bits);

            //表示从这里我重新开始构造字典字典了，以前的所有标记作废，
            //开始使用新的标记。这个标号集的大小多少比较合适呢？
            //据说理论上是越大压缩率越高（我个人感觉太大了也不见得就好），
            //不过处理的开销也呈指数增长
            //gif规定，clearcode的值为原始数据最大字长所能表达的数值+1;
            //比如原始数据长度为8,则clearcode=1<<(9-1)=256
            ClearCode = 1 << (init_bits - 1);
            //结束标志为clearcode+1
            EOFCode = ClearCode + 1;
            //这个是解除结束的
            free_ent = ClearCode + 2;
            //清楚数量
            a_count = 0; // clear packet
            //从图像中获得下一个像素
            ent = NextPixel();

            hshift = 0;
            for (fcode = hsize; fcode < 65536; fcode *= 2)
                ++hshift;
            //设置hash码范围
            hshift = 8 - hshift; // set hash code range bound

            hsize_reg = hsize;
            //清除固定大小的hash表，用于存储标记，这个相当于字典
            ResetCodeTable(hsize_reg); // clear hash table

            Output(ClearCode, outs);

        outer_loop: while ((c = NextPixel()) != EOF)
            {
                fcode = (c << maxbits) + ent;
                i = (c << hshift) ^ ent; // xor hashing
                //嘿嘿,小样,又来了,我认识你
                if (htab[i] == fcode)
                {
                    ent = codetab[i];
                    continue;
                }
                //这小子,新来的
                else if (htab[i] >= 0) // non-empty slot
                {
                    disp = hsize_reg - i; // secondary hash (after G. Knott)
                    if (i == 0)
                        disp = 1;
                    do
                    {
                        if ((i -= disp) < 0)
                            i += hsize_reg;

                        if (htab[i] == fcode)
                        {
                            ent = codetab[i];
                            goto outer_loop;
                        }
                    } while (htab[i] >= 0);
                }
                Output(ent, outs);
                //从这里可以看出,ent就是前缀（prefix）,而当前正在处理的字符标志就是后缀（suffix）
                ent = c;
                //判断终止结束符是否超过当前位数所能表述的范围
                if (free_ent < maxmaxcode)
                {
                    //如果没有超
                    codetab[i] = free_ent++; // code -> hashtable
                    //hash表里面建立相应索引
                    htab[i] = fcode;
                }
                else
                    //说明超过了当前所能表述的范围,清空字典,重新再来
                    ClearTable(outs);
            }
            // Put out the final code.
            Output(ent, outs);
            Output(EOFCode, outs);
        }

        //----------------------------------------------------------------------------
        public void Encode(Stream os)
        {
            os.WriteByte(Convert.ToByte(initCodeSize)); // write "initial code size" byte
            //这个图像包含多少个像素
            remaining = imgW * imgH; // reset navigation variables
            //当前处理的像素索引
            curPixel = 0;

            Compress(initCodeSize + 1, os); // compress and write the pixel data

            os.WriteByte(0); // write block terminator
        }

        // Flush the packet to disk, and reset the accumulator
        void Flush(Stream outs)
        {
            //if (a_count > 0)
            //{
            //    if (a_count > 255)
            //    {
            //        byte[] left_buffer = new byte[accum.Length - 255];
            //        accum.CopyTo(255, left_buffer.ConvertBytesToIBuffer(), 0, left_buffer.Length);
            //        outs.WriteByte(Convert.ToByte(left_buffer.Length));
            //        outs.Write(left_buffer, 0, left_buffer.Length);
            //    }
            //    else
            //    {
            //        outs.WriteByte(Convert.ToByte(a_count));
            //        outs.Write(accum, 0, a_count);
            //        //a_count = 0;
            //    }
            //    a_count = 0;
            //}

            if (a_count > 0)
            {
                outs.WriteByte(Convert.ToByte(a_count));
                outs.Write(accum, 0, a_count);
                a_count = 0;
            }
        }

        /// <summary>
        /// 获得n位数所能表达的最大数值
        /// </summary>
        /// <param name="n_bits">位数，一般情况下n_bits = 9</param>
        /// <returns>最大值,例如n_bits=8,则返回值就为2^8-1=255</returns>
        int MaxCode(int n_bits)
        {
            return (1 << n_bits) - 1;
        }

        //----------------------------------------------------------------------------
        // Return the next pixel from the image
        //----------------------------------------------------------------------------
        /// <summary>
        /// 从图像中获得下一个像素
        /// </summary>
        /// <returns></returns>
        private int NextPixel()
        {
            //还剩多少个像素没有处理
            //如果没有了,返回结束标志
            if (remaining == 0)
                return EOF;
            //否则处理下一个,并将未处理像素数目-1
            --remaining;
            //当前处理的像素
            //int temp = curPixel + 1;
            int temp = curPixel - 1;
            //如果当前处理像素在像素范围之内
            if (temp < pixAry.GetUpperBound(0))
            {
                //下一个像素
                byte pix = pixAry[curPixel++];

                return pix & 0xff;
            }
            return 0xff;
        }

        /// <summary>
        /// 输出字到输出流
        /// </summary>
        /// <param name="code">要输出的字</param>
        /// <param name="outs">输出流</param>
        void Output(int code, Stream outs)
        {
            //得到当前标志位所能表示的最大标志值
            cur_accum &= masks[cur_bits];

            if (cur_bits > 0)
                cur_accum |= (code << cur_bits);
            else
                //如果标志位为0,就将当前标号为输入流
                cur_accum = code;
            //当前能标志的最大字长度(9-10-11-12-9-10。。。。。。。)
            cur_bits += n_bits;
            //如果当前最大长度大于8
            while (cur_bits >= 8)
            {
                //向流中输出一个字节
                Add(Convert.ToByte((cur_accum & 0xff)), outs);
                //将当前标号右移8位
                cur_accum >>= 8;
                cur_bits -= 8;
            }

            // If the next entry is going to be too big for the code size,
            // then increase it, if possible.
            if (free_ent > maxcode || clear_flg)
            {
                if (clear_flg)
                {
                    maxcode = MaxCode(n_bits = g_init_bits);
                    clear_flg = false;
                }
                else
                {
                    ++n_bits;
                    if (n_bits == maxbits)
                        maxcode = maxmaxcode;
                    else
                        maxcode = MaxCode(n_bits);
                }
            }

            if (code == EOFCode)
            {
                // At EOF, write the rest of the buffer.
                while (cur_bits > 0)
                {
                    Add((byte)(cur_accum & 0xff), outs);
                    cur_accum >>= 8;
                    cur_bits -= 8;
                }

                Flush(outs);
            }
        }
    }
}