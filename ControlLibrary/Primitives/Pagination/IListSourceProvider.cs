using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    internal interface IListSourceProvider
    {
        MatListSource ListSource
        {
            get;
        }
    }
}
