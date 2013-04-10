using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    public class FlickrHelp
    {
        /// <summary>
        /// Represents the method that will handle the Slide events
        /// </summary>
        public delegate void SlideHandler(object sender, object e);

        /// <summary>
        /// Occurs when the Slide Duration is completion
        /// </summary>
        public static SlideHandler SlideDurationCompleting;

        /// <summary>
        /// Random Number Generator to be used throughout the app
        /// </summary>
        public static Random RandomNumber = new Random();

        public static Album SelectedAlbum { get; set; }
    }

    public class AlbumImage
    {
        public Uri AlbumUri
        { get; set; }
    }

    public class Album
    {
        public List<AlbumImage> AlbumList
        { get; set; }
    }
}
