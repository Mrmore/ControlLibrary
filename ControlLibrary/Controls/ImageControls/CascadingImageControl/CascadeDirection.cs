using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    public enum CascadeDirection
    {
        TopLeft = 0,
        //Top,
        TopRight = 1,
        //Right,
        BottomRight = 2,
        //Bottom,
        BottomLeft = 3,
        //Left,
        Random = 4,
        Shuffle = 5,
        TopCenter = 6,
        BottomCenter = 7,
        LeftCenter = 8,
        RightCenter = 9
    }
}
