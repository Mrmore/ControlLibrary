using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlLibrary.Helper;
using ControlLibrary.Helper.CacheImageHelper;

namespace TestDemoApp.Helper
{
    public class CacheImageManageHelper
    {
        /// <summary>
        /// 初始化创建缓存所有的文件夹
        /// </summary>
        public static void CacheImageManageInitialization()
        {
            CacheServiceImageInitializationHelper.CacheImageInitializationHelper(CacheImageDateType.CacheHead);
            CacheServiceImageInitializationHelper.CacheImageInitializationHelper(CacheImageDateType.CacheFeedImage);
            CacheServiceImageInitializationHelper.CacheImageInitializationHelper(CacheImageDateType.CachePhoto);
            CacheServiceImageInitializationHelper.CacheImageInitializationHelper(CacheImageDateType.CacheMiscellaneousImage);
        }
    }
}
