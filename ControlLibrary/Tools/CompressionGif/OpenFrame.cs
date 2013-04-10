using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace ControlLibrary.Tools.CompressionGif
{
    public class OpenFrame
    {
        /// <summary>
        /// 拆解所有的Gif帧
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<List<BitFrame>> CreateGifBitFrame(IRandomAccessStream fileStream)
        {
            var bitFrame = new List<BitFrame>();
            var decoderId = BitmapDecoder.GifDecoderId;
            var bitDecoder = await BitmapDecoder.CreateAsync(decoderId, fileStream);
            for (int i = 0; i < bitDecoder.FrameCount; i++)
            {
                var frame = await bitDecoder.GetFrameAsync(Convert.ToUInt32(i));

                var frameProperties = await frame.BitmapProperties.GetPropertiesAsync(new List<string>());
                var imageDescriptionProperties = await (frameProperties["/imgdesc"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Top", "/Left", "/Width", "/Height" });
                double top = System.Convert.ToDouble(imageDescriptionProperties["/Top"].Value.ToString());
                double left = System.Convert.ToDouble(imageDescriptionProperties["/Left"].Value.ToString());
                double width = System.Convert.ToDouble(imageDescriptionProperties["/Width"].Value.ToString());
                double height = System.Convert.ToDouble(imageDescriptionProperties["/Height"].Value.ToString());
                var gifControlExtensionProperties = await (frameProperties["/grctlext"].Value as BitmapPropertiesView).GetPropertiesAsync(new List<string>() { "/Delay", "/UserInputFlag", "/Disposal" });
                var delayTemp = gifControlExtensionProperties["/Delay"].Value.ToString();
                if (string.IsNullOrEmpty(delayTemp) || delayTemp == "0")
                {
                    delayTemp = "10";
                }
                TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(delayTemp) / 100);
                //TimeSpan delay = TimeSpan.FromSeconds(Double.Parse(gifControlExtensionProperties["/Delay"].Value.ToString()) / 100); // delay is in 1/100s of a second
                bool userInputFlag = bool.Parse(gifControlExtensionProperties["/UserInputFlag"].Value.ToString());
                //共享图层(如果是2为不共享为False,其余不为2的为共享为True)
                var disposalSource = gifControlExtensionProperties["/Disposal"].Value.ToString();
                bool disposal = true;
                if (disposalSource == "2")
                {
                    disposal = false;
                }
                else
                {
                    disposal = true;
                }

                var decoderInformation = bitDecoder.DecoderInformation;
                var codecId = decoderInformation.CodecId;

                var pixels = await frame.GetPixelDataAsync();

                //var encoderId = BitmapEncoder.JpegEncoderId;
                var encoderId = BitmapEncoder.PngEncoderId;
                InMemoryRandomAccessStream tmemoryStream = new InMemoryRandomAccessStream();
                var encoder = await BitmapEncoder.CreateAsync(encoderId, tmemoryStream);
                var bytes = pixels.DetachPixelData();
                encoder.SetPixelData(
                    frame.BitmapPixelFormat,
                    BitmapAlphaMode.Premultiplied,
                    //frame.BitmapAlphaMode,
                    frame.PixelWidth,
                    frame.PixelHeight,
                    frame.DpiX,
                    frame.DpiY,
                    bytes);
                await encoder.FlushAsync();
                tmemoryStream.Seek(0);

                bitFrame.Add(new BitFrame() { Delay = delay, MemoryStream = tmemoryStream, Height = height, Width = width, Top = top, Left = left, UserInputFlag = userInputFlag, Disposal = disposal });//, wb = wb });
            }
            return bitFrame;
        }
    }
}
