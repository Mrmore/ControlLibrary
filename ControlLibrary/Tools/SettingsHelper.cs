using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools
{
    public class SettingsHelper
    {
        private static double slideDuration = 10.0;
        public static double SlideDuration
        {
            get 
            {
                return slideDuration;
            }
            set 
            {
                slideDuration = value;
            }
        }

        private static double holdDuration = 2.0;
        public static double HoldDuration
        {
            get 
            {
                return holdDuration; 
            }
            set
            {
                holdDuration = value;
            }
        }

        private static double forwardGestureGreaterThanAngle = 315.0;
        public static double ForwardGestureGreaterThanAngle
        {
            get
            {
                return forwardGestureGreaterThanAngle;
            }
            set
            {
                forwardGestureGreaterThanAngle = value;
            }
        }

        private static double forwardGestureLessThanAngle = 135.0;
        public static double ForwardGestureLessThanAngle
        {
            get
            {
                return forwardGestureLessThanAngle;
            }
            set
            {
                forwardGestureLessThanAngle = value;
            }
        }
    }
}
