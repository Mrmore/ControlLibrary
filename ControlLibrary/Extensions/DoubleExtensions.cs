using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Extensions
{
    public static class DoubleExtensions
    {
        public static bool IsNaN(this double d)
        {
            return Double.IsNaN(d);
        }
    }
}
