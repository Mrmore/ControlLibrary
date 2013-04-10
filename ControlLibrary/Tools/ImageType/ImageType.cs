using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    /// <summary>
    /// 图像文件的类型
    /// </summary>
    public enum ImageType
    {
        None = 0,
        BMP = 0x4D42,
        JPG = 0xD8FF,
        GIF = 0x4947,
        PCX = 0x050A,
        PNG = 0x5089,
        PSD = 0x4238,
        RAS = 0xA659,
        SGI = 0xDA01,
        TIFF = 0x4949
    }
}
