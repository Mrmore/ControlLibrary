using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary.Tools
{
    public static class ImageHelper
    {
        public static async Task<WriteableBitmap> GetImage(string source)
        {
            WriteableBitmap image = new WriteableBitmap(250, 250);
            try
            {
                if (source.Contains(";"))
                {
                    var speakers = source.Split(';');
                    int count = 0;
                    foreach (string speaker in speakers)
                    {
                        var rass1 = RandomAccessStreamReference.CreateFromUri(new System.Uri(speaker, UriKind.Absolute));
                        IRandomAccessStream stream1 = await rass1.OpenReadAsync();

                        //We initialize the bitmap with height and width, but the actual size will be reset after the FromStream method!
                        WriteableBitmap speakerImage = new WriteableBitmap(150, 150);
                        speakerImage = await speakerImage.FromStream(stream1);

                        int xPosition = 0;
                        int yPosition = 0;

                        if (count == 1 || count == 3)
                            yPosition = yPosition + 125;

                        if (count == 2 || count == 3)
                            xPosition = xPosition + 125;

                        //We use Blit to do image resizing and image merging
                        image.Blit(new Rect() { Height = 125, Width = 125, X = xPosition, Y = yPosition }, speakerImage, new Rect() { Height = speakerImage.PixelHeight, Width = speakerImage.PixelWidth, X = 0, Y = 0 }, WriteableBitmapExtensions.BlendMode.Additive);

                        count++;
                    }
                }
                else
                {
                    var rass1 = RandomAccessStreamReference.CreateFromUri(new System.Uri(source, UriKind.Absolute));
                    IRandomAccessStream stream1 = await rass1.OpenReadAsync();

                    //We initialize the bitmap with height and width, but the actual size will be reset after the FromStream method!
                    image = await image.FromStream(stream1);
                }

                return image;
            }
            catch (Exception exception)
            {
                return image;
            }
        }
    }
}
