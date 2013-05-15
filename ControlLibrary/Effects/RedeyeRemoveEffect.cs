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
    public static class RedeyeRemoveEffect
    {
        /// <summary>
        /// Redeye remove Process.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <returns></returns>
        public static WriteableBitmap RedeyeRemoveProcess(this WriteableBitmap src)////红眼去除
        {
            if (src != null)
            {
                var cloneWriteableBitmap = WriteableBitmapExpansion.CopyWriteableBitmap(src);
                int w = cloneWriteableBitmap.PixelWidth;
                int h = cloneWriteableBitmap.PixelHeight;
                WriteableBitmap srcImage = new WriteableBitmap(w, h);
                byte[] temp = cloneWriteableBitmap.PixelBuffer.ToArray();
                byte[] tempMask = (byte[])temp.Clone();
                int r, g, b;
                int Rc, Gc, Bc;

                for (int i = 0; i < temp.Length; i += 4)
                {
                    b = tempMask[i];
                    g = tempMask[i + 1];
                    r = tempMask[i + 2];

                    if (r > (int)(g + b))//这里 只是简单的判断一下红眼像素只为说明红眼去除算法，实际上要使用一定的红眼判断算法决策
                    {
                        Rc = (int)((g + b) / 2);
                        Gc = (int)((g + Rc) / 2);
                        Bc = (int)((b + Rc) / 2);
                        temp[i] = (byte)Bc;
                        temp[i + 1] = (byte)Gc;
                        temp[i + 2] = (byte)Rc;
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
