using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.CacheManagement
{
    public class CacheText
    {
        private volatile static CacheText _instance = null;
        private static readonly object lockHelper = new object();

        public static CacheText Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new CacheText();

                    }
                }
                return _instance;
            }
        }
    }
}
