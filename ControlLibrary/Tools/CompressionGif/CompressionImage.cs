using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using ControlLibrary.GifSynthesis;

namespace ControlLibrary.Tools.CompressionGif
{
    public class CompressionImage
    {
        public async Task<List<BitFrame>> ImageGifCompression(List<BitFrame> bitFrameList, int maxWidth, int maxHeight, double imgQualityFactor = 0.6)
        {
            var w = maxWidth / bitFrameList.OrderByDescending(s => s.Width).FirstOrDefault().Width;
            var h = maxHeight / bitFrameList.OrderByDescending(s => s.Height).FirstOrDefault().Height;
            var difference = Math.Min(w, h);
            //if (difference < 1)
            //{
            //    for (int i = 0; i < bitFrameList.Count; i++)
            //    {
            //        var iRandomAccessStream = await bitFrameList[i].MemoryStream.ResizeStream(bitFrameList[i].Width * difference, bitFrameList[i].Height * difference, BitmapInterpolationMode.Cubic);
            //        bitFrameList[i].MemoryStream = iRandomAccessStream as InMemoryRandomAccessStream;
            //        bitFrameList[i].Height =  Convert.ToInt32(bitFrameList[i].Height * difference);
            //        bitFrameList[i].Width =  Convert.ToInt32(bitFrameList[i].Width * difference);
            //        bitFrameList[i].Left =  Convert.ToInt32(bitFrameList[i].Left * difference);
            //        bitFrameList[i].Top =  Convert.ToInt32(bitFrameList[i].Top * difference);
            //    }
            //}

            if (difference < 1)
            {
                difference = 1;
            }
            else
            {
                difference = imgQualityFactor;
            }
            for (int i = 0; i < bitFrameList.Count; i++)
            {
                var iRandomAccessStream = await bitFrameList[i].MemoryStream.ResizeStream(bitFrameList[i].Width * difference, bitFrameList[i].Height * difference, BitmapInterpolationMode.Cubic);
                bitFrameList[i].MemoryStream = iRandomAccessStream as InMemoryRandomAccessStream;
                bitFrameList[i].Height = Convert.ToInt32(bitFrameList[i].Height * difference);
                bitFrameList[i].Width = Convert.ToInt32(bitFrameList[i].Width * difference);
                bitFrameList[i].Left = Convert.ToInt32(bitFrameList[i].Left * difference);
                bitFrameList[i].Top = Convert.ToInt32(bitFrameList[i].Top * difference);
            }
            return bitFrameList;
        }
    }
}
