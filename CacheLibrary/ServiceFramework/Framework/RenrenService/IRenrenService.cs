using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Framework.RenrenService
{
    public interface IRenrenService
    {
        /// <summary>
        /// The latest one to be able to compatiable with before version
        /// </summary>
        object Model { get; }

        /// <summary>
        /// Flag, getter and setter the offline data switcher
        /// Note: you just need set this switcher using relevant parameters in interface
        /// you should not change it directly using this property
        /// </summary>
        bool NeedOfflineData { get; }

        /// <summary>
        /// Flag, getter nd setter the force to request data from internet
        /// Note: you just need set this switcher using relevant parameters in interface
        /// you should not change it directly using this property
        /// </summary>
        bool ForceDataRequest { get; }

        /// <summary>
        /// The Data model of view by id and page
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        object this[object id, int page = -1]
        {
            get;
        }

        void ResetById(object id);

        void ResetByIdnPage(object id, int page = -1);
    }
}
