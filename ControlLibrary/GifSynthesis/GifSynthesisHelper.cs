using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.Core;

namespace ControlLibrary.GifSynthesis
{
    public class GifSynthesisHelper
    {
        /// <summary>
        /// 合成GIF传入合成Gif的静态图片地址的集合，delayMilliseconds为每帧的时间默认是500毫秒(默认是以毫秒为单位),
        /// animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次),
        /// 如果成功返回true,失败则返回false.[非后台线程方法，会阻碍前台UI]
        /// </summary>
        /// <param name="pathList">静态图片地址的集合</param>
        /// <param name="delayMilliseconds">delayMilliseconds为每帧的时间默认是500毫秒(默认是以毫秒为单位)</param>
        /// <param name="animatedRepeat">animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次)</param>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果成功返回true,失败则返回false.</returns>
        public async static Task<bool> SynthesisGif(List<Uri> pathList, int delayMilliseconds = 500, int animatedRepeat = 0, string fileName = "test.gif")
        {
            try
            {
                if (pathList != null)
                {
                    AnimatedGifEncoder e = new AnimatedGifEncoder();
                    await e.CreateImageFolder();
                    //图片转换时间
                    e.SetDelay(delayMilliseconds);
                    //1表示只动一次，0：表示循环，n：表示循环n次
                    e.SetRepeat(0);
                    //e.SetTransparent(Colors.Transparent);
                    e.SetTransparent(Color.FromArgb(0, 0, 0, 0));
                    //共享图层(如果是2为不共享,其余不为2的为共享)
                    //e.SetDispose(2);
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        //WriteableBitmap aa = await (new WriteableBitmap(1, 1).FromContent(pathList.ElementAt(i)));

                        var rass = RandomAccessStreamReference.CreateFromUri(pathList.ElementAt(i));
                        var streamRandom = await rass.OpenReadAsync();

                        /*
                        //为了能判断文件头做了一个流拷贝，保存了一份字节数组
                        Stream tempStream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
                        MemoryStream ms = new MemoryStream();
                        await tempStream.CopyToAsync(ms);
                        byte[] bytes = ms.ToArray();
                        tempStream = new MemoryStream(bytes);
                        var randomAccessStream = new InMemoryRandomAccessStream();
                        var outputStream = randomAccessStream.GetOutputStreamAt(0);
                        await RandomAccessStream.CopyAsync(tempStream.AsInputStream(), outputStream);

                        InMemoryRandomAccessStream memoryStream = new InMemoryRandomAccessStream();
                        DataWriter datawriter = new DataWriter(memoryStream.GetOutputStreamAt(0));
                        datawriter.WriteBytes(bytes);
                        await datawriter.StoreAsync();
                        //
                        */

                        ////System.IO 为扩展命名空间
                        //var stream = streamRandom.GetInputStreamAt(0).AsStreamForRead();
                        //WriteableBitmap aa = await (new WriteableBitmap(1, 1).FromStream(stream));
                        /*var streamRandomCopy = streamRandom.CloneStream();*/

                        //var dispatcher = Window.Current.Dispatcher;
                        //var dispatcher = Window.Current.CoreWindow.Dispatcher;
                        //var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
                        //var dispatcher = TaskScheduler.FromCurrentSynchronizationContext();
                        //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        //    CoreDispatcherPriority.Normal, async () =>
                        //        {

                        //        });

                        /*WriteableBitmap aa = new WriteableBitmap(1, 1);
                        await aa.SetSourceAsync(streamRandom);
                        await e.AddFrame(aa, streamRandomCopy);*/

                        await e.AddFrame(streamRandom);
                    }
                    return (await e.Finish(fileName));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 合成GIF传入合成Gif的静态图片信息的集合对象,包括每帧的地址和每帧的播放间隔,
        /// 需要指定要用图片信息集合对象里面的Uri还是Stream，默认是Uri
        /// animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次),
        /// 如果成功返回true,失败则返回false.[非后台线程方法，会阻碍前台UI]
        /// </summary>
        /// <param name="pathList">合成GIF传入合成Gif的静态图片信息的集合对象,包括每帧的地址和每帧的播放间隔</param>
        /// <param name="gifDataSource">指定要用图片信息集合对象里面的Uri还是Stream，默认是Uri</param>
        /// <param name="animatedRepeat">animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次)</param>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果成功返回true,失败则返回false.</returns>
        public async static Task<bool> SynthesisGif(List<GifInformation> pathList, GifDataSource gifDataSource = GifDataSource.UriDataSource, int animatedRepeat = 0, string fileName = "test.gif")
        {
            try
            {
                if (pathList != null)
                {
                    IRandomAccessStream streamRandom = null;
                    AnimatedGifEncoder e = new AnimatedGifEncoder();
                    await e.CreateImageFolder();
                    //1表示只动一次，0：表示循环，n：表示循环n次
                    e.SetRepeat(animatedRepeat);
                    //e.SetTransparent(Colors.Transparent);
                    e.SetTransparent(Color.FromArgb(0, 0, 0, 0));
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        //图片转换时间
                        e.SetDelay(pathList[i].DelayMilliseconds);
                        if (gifDataSource == GifDataSource.UriDataSource)
                        {
                            //WriteableBitmap aa = await (new WriteableBitmap(1, 1).FromContent(pathList[i].Uri));
                            var rass = RandomAccessStreamReference.CreateFromUri(pathList[i].Uri);
                            streamRandom = await rass.OpenReadAsync();
                            ////System.IO 为扩展命名空间
                            //WriteableBitmap aa = await (new WriteableBitmap(1, 1).FromStream(streamRandom.GetInputStreamAt(0).AsStreamForRead()));
                            /*var streamRandomCopy = streamRandom.CloneStream();

                            WriteableBitmap aa = new WriteableBitmap(1, 1);
                            await aa.SetSourceAsync(streamRandom);
                            await e.AddFrame(aa, streamRandomCopy);*/
                        }
                        else
                        {
                            streamRandom = pathList[i].IRandomAccessStream;
                        }
                        await e.AddFrame(streamRandom);
                    }
                    return (await e.Finish(fileName));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 合成GIF传入合成Gif的静态图片数据流对象
        /// animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次),
        /// 如果成功返回true,失败则返回false.[非后台线程方法，会阻碍前台UI]
        /// </summary>
        /// <param name="pathList">合成GIF传入合成Gif的静态图片信息的集合对象,包括每帧的地址和每帧的播放间隔</param>
        /// <param name="animatedRepeat">animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次)</param>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果成功返回true,失败则返回false.</returns>
        public async static Task<bool> SynthesisGif(List<IRandomAccessStream> pathList, int delayMilliseconds = 500, int animatedRepeat = 0, string fileName = "test.gif")
        {
            try
            {
                if (pathList != null)
                {
                    AnimatedGifEncoder e = new AnimatedGifEncoder();
                    await e.CreateImageFolder();
                    //图片转换时间
                    e.SetDelay(delayMilliseconds);
                    //1表示只动一次，0：表示循环，n：表示循环n次
                    e.SetRepeat(0);
                    //e.SetTransparent(Colors.Transparent);
                    e.SetTransparent(Color.FromArgb(0, 0, 0, 0));
                    //共享图层(如果是2为不共享,其余不为2的为共享)
                    //e.SetDispose(2);
                    for (int i = 0; i < pathList.Count; i++)
                    {
                        await e.AddFrame(pathList.ElementAt(i));
                    }
                    return (await e.Finish(fileName));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 合成GIF传入合成Gif的静态图片地址的集合，delayMilliseconds为每帧的时间默认是500毫秒(默认是以毫秒为单位),
        /// animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次),
        /// 如果保存成功返回true,保存失败则返回false.[后台线程方法，不会阻碍前台UI]
        /// </summary>
        /// <param name="pathList">静态图片地址的集合</param>
        /// <param name="delayMilliseconds">delayMilliseconds为每帧的时间默认是500毫秒(默认是以毫秒为单位)</param>
        /// <param name="animatedRepeat">animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次)</param>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果保存成功返回true,保存失败则返回false.</returns>
        public async static Task<bool> SaveSynthesisGif(List<Uri> pathList, int delayMilliseconds = 500, int animatedRepeat = 0, string fileName = "test.gif")
        {
            bool ok = false;
            await Task.Run(async () =>
            {
                if ((await SynthesisGif(pathList, delayMilliseconds, animatedRepeat, fileName)))
                {
                    //进行保存
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal, async () =>
                        {
                            try
                            {
                                StorageFolder storageFolder = await ControlLibrary.CacheManagement.CacheManagement.Instance.GetImageFolder();
                                StorageFile storageFile = await storageFolder.GetFileAsync(fileName);
                                if (storageFile != null)
                                {
                                    FileSavePicker savePicker = new FileSavePicker();
                                    savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                                    savePicker.FileTypeChoices.Add("图片类型", new List<string>() { ".gif" });

                                    savePicker.SuggestedFileName = storageFile.Name;
                                    StorageFile file = await savePicker.PickSaveFileAsync();
                                    if (file != null)
                                    {
                                        CachedFileManager.DeferUpdates(file);
                                        await storageFile.CopyAndReplaceAsync(file);//, file.Name, NameCollisionOption.GenerateUniqueName);
                                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                                        if (status == FileUpdateStatus.Complete)
                                        {
                                            ok = true;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                ok = false;
                            }
                        });
                }
            });
            return ok;
        }

        /// <summary>
        /// 直接保存文件，传入之前合成写入的文件名称.
        /// 如果保存成功返回true,保存失败则返回false.[前台UI线程方法]
        /// </summary>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果保存成功返回true,保存失败则返回false.</returns>
        public async static Task<bool> SaveSynthesisGif(string fileName = "test.gif")
        {
            bool ok = false;
            //进行保存
            try
            {
                StorageFolder storageFolder = await ControlLibrary.CacheManagement.CacheManagement.Instance.GetImageFolder();
                StorageFile storageFile = await storageFolder.GetFileAsync(fileName);
                if (storageFile != null)
                {
                    FileSavePicker savePicker = new FileSavePicker();
                    savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    savePicker.FileTypeChoices.Add("图片类型", new List<string>() { ".gif" });

                    savePicker.SuggestedFileName = storageFile.Name;
                    StorageFile file = await savePicker.PickSaveFileAsync();
                    if (file != null)
                    {
                        CachedFileManager.DeferUpdates(file);
                        await storageFile.CopyAndReplaceAsync(file);//, file.Name, NameCollisionOption.GenerateUniqueName);
                        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                        if (status == FileUpdateStatus.Complete)
                        {
                            ok = true;
                        }
                    }
                }
            }
            catch
            {
                ok = false;
            }
            return ok;
        }

        /// <summary>
        /// 合成GIF传入合成Gif的静态图片信息的集合对象,包括每帧的地址和每帧的播放间隔,
        /// animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次),
        /// 如果保存成功返回true,保存失败则返回false.[后台线程方法，不会阻碍前台UI]
        /// </summary>
        /// <param name="pathList">合成GIF传入合成Gif的静态图片信息的集合对象,包括每帧的地址和每帧的播放间隔</param>
        /// <param name="gifDataSource">指定要用图片信息集合对象里面的Uri还是Stream，默认是Uri</param>
        /// <param name="animatedRepeat">animatedRepeat为重复次数默认是循环(1表示只动一次，0：表示循环，n：表示循环n次)</param>
        /// <param name="fileName">临时文件名，默认为"test.gif"</param>
        /// <returns>如果保存成功返回true,保存失败则返回false.</returns>
        public async static Task<bool> SaveSynthesisGif(List<GifInformation> pathList, GifDataSource gifDataSource = GifDataSource.UriDataSource, int animatedRepeat = 0, string fileName = "test.gif")
        {
            bool ok = false;
            await Task.Run(async () =>
            {
                if ((await SynthesisGif(pathList, gifDataSource, animatedRepeat, fileName)))
                {
                    //进行保存
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                         CoreDispatcherPriority.Normal, async () =>
                         {
                             try
                             {
                                 StorageFolder storageFolder = await ControlLibrary.CacheManagement.CacheManagement.Instance.GetImageFolder();
                                 StorageFile storageFile = await storageFolder.GetFileAsync(fileName);
                                 if (storageFile != null)
                                 {
                                     FileSavePicker savePicker = new FileSavePicker();
                                     savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                                     savePicker.FileTypeChoices.Add("图片类型", new List<string>() { ".gif" });

                                     savePicker.SuggestedFileName = storageFile.Name;
                                     StorageFile file = await savePicker.PickSaveFileAsync();
                                     if (file != null)
                                     {
                                         CachedFileManager.DeferUpdates(file);
                                         await storageFile.CopyAndReplaceAsync(file);//, file.Name, NameCollisionOption.GenerateUniqueName);
                                         FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                                         if (status == FileUpdateStatus.Complete)
                                         {
                                             ok = true;
                                         }
                                     }
                                 }
                             }
                             catch
                             {
                                 ok = false;
                             }
                         });
                }
            });
            return ok;
        }
    }
}
