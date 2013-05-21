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
        TopRight,
        //Right,
        BottomRight,
        //Bottom,
        BottomLeft,
        //Left,
        Random,
        Shuffle,
        TopCenter,
        BottomCenter,
        LeftCenter,
        RightCenter
    }
}
