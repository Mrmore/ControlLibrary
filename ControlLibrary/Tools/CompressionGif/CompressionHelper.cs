using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI;

namespace ControlLibrary.Tools.CompressionGif
{
    public class CompressionHelper
    {
        public static async Task<IRandomAccessStream> CompressionGif(IRandomAccessStream iRandomAccessStream, int maxWidth, int maxHeight, double imgQualityFactor = 0.8)
        {
            OpenFrame openFrame = new OpenFrame();
            List<BitFrame> originalBitFrameList = await openFrame.CreateGifBitFrame(iRandomAccessStream);
            CompressionImage ci = new CompressionImage();
            List<BitFrame> compressionBitFrameList = await ci.ImageGifCompression(originalBitFrameList, maxWidth, maxHeight);

            if (compressionBitFrameList != null)
            {
                SynthesisGifEncoder synthesisGifEncoder = new SynthesisGifEncoder();
                synthesisGifEncoder.CreateGifImage();
                //1表示只动一次，0：表示循环，n：表示循环n次
                synthesisGifEncoder.SetRepeat(0);
                synthesisGifEncoder.SetTransparent(Color.FromArgb(0, 0, 0, 0));
                for (int i = 0; i < compressionBitFrameList.Count; i++)
                {
                    //图片转换时间
                    synthesisGifEncoder.SetDelay(compressionBitFrameList[i].Delay.Milliseconds);
                    synthesisGifEncoder.SetLeft(Convert.ToInt32(compressionBitFrameList[i].Left));
                    synthesisGifEncoder.SetTop(Convert.ToInt32(compressionBitFrameList[i].Top));
                    synthesisGifEncoder.SetSize(Convert.ToInt32(compressionBitFrameList[i].Width), Convert.ToInt32(compressionBitFrameList[i].Height));
                    var dispose = 0;
                    if (compressionBitFrameList[i].Disposal)
                    {
                        dispose = 0;
                    }
                    else
                    {
                        dispose = 2;
                    }
                    synthesisGifEncoder.SetDispose(dispose);
                    await synthesisGifEncoder.AddFrame(compressionBitFrameList[i].MemoryStream);
                }
                originalBitFrameList.Clear();
                compressionBitFrameList.Clear();
                return (await synthesisGifEncoder.Finish());
            }
            else
            {
                originalBitFrameList.Clear();
                compressionBitFrameList.Clear();
                return null;
            }
        }
    }
}
