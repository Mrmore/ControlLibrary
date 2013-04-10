using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Helper
{
    public class UserHeadHelper
    {
        //取大头像
        public static string GetLargeHeaderUrl(int uid, int width)
        {
            Random r = new Random();
            int index = r.Next(5000);
            return "http://ic.m.renren.com/gn?op=resize&w=" + width.ToString() + "&p=" + uid.ToString() + "-L&a=" + index.ToString();
        }
    }
}
