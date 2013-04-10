using System;
using System.Collections.Generic;
using System.Net;
using RenrenCoreWrapper.Entities;

namespace RenrenCoreWrapper.Helper
{
    public class ParameterComparer : IComparer<RequestParameterEntity>
    {

        public int Compare(RequestParameterEntity x, RequestParameterEntity y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
