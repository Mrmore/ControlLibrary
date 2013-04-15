using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    public enum CascadeSequence
    {
        /// <summary>
        /// The tile cascade animations end at the same time
        /// </summary>
        EndTogether,
        /// <summary>
        /// The tile cascade animations are equal duration
        /// </summary>
        EqualDuration
    }
}
