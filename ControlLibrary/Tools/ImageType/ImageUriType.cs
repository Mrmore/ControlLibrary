using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    public class ImageUriType
    {
        public static ImageType GetPictureType(String picName)
        {
            picName = picName.Trim(' ');
            int length = picName.Length;
            if (picName.Length > 4)
            {
                string typeStr = picName.Substring(length - 4).ToLower();
                if (typeStr == ".gif")
                    return ImageType.GIF;
                else if (typeStr == ".png")
                    return ImageType.PNG;
                else if (typeStr == ".jpg")
                    return ImageType.JPG;
                else if (typeStr == ".bmp")
                    return ImageType.BMP;
                else
                    return ImageType.None;
            }
            return ImageType.None;
        }
    }
}
