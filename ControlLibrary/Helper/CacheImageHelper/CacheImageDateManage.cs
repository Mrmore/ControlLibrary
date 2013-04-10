using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Framework;

namespace ControlLibrary.Helper
{
    public class CacheImageDateManage
    {
        private static DateTime CacheTypeToDate(ServiceType cacheImageDateType)
        {
            switch (cacheImageDateType)
            {
                case ServiceType.CacheFeedImage:
                    {
                        return DateTime.Now.AddDays(3);
                    }
                case ServiceType.CacheHead:
                    {
                        return DateTime.Now.AddDays(3);
                    }
                case ServiceType.CachePhoto:
                    {
                        return DateTime.Now.AddDays(7);
                    }
                case ServiceType.CacheMiscellaneousImage:
                    {
                        return DateTime.Now.AddDays(1);
                    }
                default:
                    {
                        return DateTime.Now.AddDays(1);
                    }
            }
        }

        public static DateTime CacheTypeToDate(string cacheImageDateType)
        {
            switch (cacheImageDateType)
            {
                case "CacheFeedImage":
                    {
                        return DateTime.Now.AddDays(3);
                    }
                case "CacheHead":
                    {
                        return DateTime.Now.AddDays(3);
                    }
                case "CachePhoto":
                    {
                        return DateTime.Now.AddDays(7);
                    }
                case "CacheMiscellaneousImage":
                    {
                        return DateTime.Now.AddDays(1);
                    }
                default:
                    {
                        return DateTime.Now.AddDays(1);
                    }
            }
        }

        public static string CacheTypeToString(CacheImageDateType cacheImageDateType)
        {
            switch (cacheImageDateType)
            {
                case CacheImageDateType.CacheFeedImage:
                    {
                        return "CacheFeedImage";
                    }
                case CacheImageDateType.CacheHead:
                    {
                        return "CacheHead";
                    }
                case CacheImageDateType.CachePhoto:
                    {
                        return "CachePhoto";
                    }
                case CacheImageDateType.CacheMiscellaneousImage:
                    {
                        return "CacheMiscellaneousImage";
                    }
                default:
                    {
                        return "CacheMiscellaneousImage";
                    }
            }
        }
    }
}
