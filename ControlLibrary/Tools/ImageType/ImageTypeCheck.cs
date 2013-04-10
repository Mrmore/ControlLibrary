using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    /// <summary>
    /// 通过文件头判断图像文件的类型
    /// </summary>
    public class ImageTypeCheck
    {
        private static SortedDictionary<int, ImageType> _imageTag = InitImageTag();

        public static readonly string ErrType = ImageType.None.ToString();

        private static SortedDictionary<int, ImageType> InitImageTag()
        {
            SortedDictionary<int, ImageType> list = new SortedDictionary<int, ImageType>();
            list.Add((int)ImageType.BMP, ImageType.BMP);
            list.Add((int)ImageType.JPG, ImageType.JPG);
            list.Add((int)ImageType.GIF, ImageType.GIF);
            list.Add((int)ImageType.PCX, ImageType.PCX);
            list.Add((int)ImageType.PNG, ImageType.PNG);
            list.Add((int)ImageType.PSD, ImageType.PSD);
            list.Add((int)ImageType.RAS, ImageType.RAS);
            list.Add((int)ImageType.SGI, ImageType.SGI);
            list.Add((int)ImageType.TIFF, ImageType.TIFF);
            return list;
        }
        /// <summary>
        /// 通过文件头判断图像文件的类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string CheckImageTypeName(string path)
        {
            return CheckImageType(path).ToString();
        }

        /// <summary>
        /// 通过文件头判断图像文件的类型
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ImageType CheckImageType(string path)
        {

            byte[] buf = new byte[2];
            try
            {
                using (StreamReader sr = null)
                {
                    //new StreamReader(path)
                    int i = sr.BaseStream.Read(buf, 0, buf.Length);
                    if (i != buf.Length)
                    {
                        return ImageType.None;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return ImageType.None;
            }
            return CheckImageType(buf);
        }

        /// <summary>
        /// 通过文件的前两个自己判断图像类型
        /// </summary>
        /// <param name="buf">至少2个字节</param>
        /// <returns></returns>
        public static ImageType CheckImageType(byte[] buf)
        {
            if (buf == null || buf.Length < 2)
            {
                return ImageType.None;
            }

            int key = (buf[1] << 8) + buf[0];
            ImageType s;
            if (_imageTag.TryGetValue(key, out s))
            {
                return s;
            }
            return ImageType.None;
        }
    }
}
