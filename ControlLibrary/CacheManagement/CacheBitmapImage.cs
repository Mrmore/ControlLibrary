using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ControlLibrary.CacheManagement
{
    /// <summary>
    /// 图片的缓存类
    /// </summary>
    public class CacheBitmapImage
    {
        private volatile static CacheBitmapImage _instance = null;
        private static readonly object lockHelper = new object();

        public static CacheBitmapImage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheBitmapImage();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, GifCache> gifCacheDictionary = null;
        private string CacheBitmapImageKey = "CacheBitmapImageKey";
        private IPropertySet dataSet = ApplicationData.Current.LocalSettings.Values;

        //文件形式
        private const string FileName = "CacheFile";

        //私有构造
        private CacheBitmapImage()
        {
            if (gifCacheDictionary == null)
            {
                gifCacheDictionary = new Dictionary<string, GifCache>();
            }
            else
            {
                gifCacheDictionary.Clear();
            }
            //var createBool = CreateImageFolderAndFile().Result;
        }

        //创建文件夹
        private async Task<bool> CreateImageFolderAndFile()
        {
            return await CacheManagement.Instance.CreateImageFolder();
        }

        //基础方法
        private async Task Restore()
        {
            await CreateImageFolderAndFile();
            if (gifCacheDictionary == null)
            {
                gifCacheDictionary = new Dictionary<string, GifCache>();
            }
            else
            {
                gifCacheDictionary.Clear();
            }
            try
            {
                /*
                if (dataSet.ContainsKey(CacheBitmapImageKey))
                {
                    string cahceList = (string)dataSet[CacheBitmapImageKey];
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(cahceList)))
                    {
                        DataContractSerializer deserializer = new DataContractSerializer(typeof(Dictionary<string, GifCache>));
                        gifCacheDictionary = (Dictionary<string, GifCache>)deserializer.ReadObject(stream);
                    }
                }
                */

                StorageFile storageFile = await (await CacheManagement.Instance.GetImageFolder()).GetFileAsync(FileName);
                using (var streamFile = await storageFile.OpenAsync(FileAccessMode.Read))
                {
                    if (streamFile != null)
                    {
                        var input = streamFile.GetInputStreamAt(0);
                        DataReader dataReader = new DataReader(input);
                        uint fileLength = await dataReader.LoadAsync((uint)streamFile.Size);
                        string jsonContent = dataReader.ReadString(fileLength);
                        if (!string.IsNullOrEmpty(jsonContent))
                        {
                            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                            {
                                DataContractSerializer deserializer = new DataContractSerializer(typeof(Dictionary<string, GifCache>));
                                gifCacheDictionary = (Dictionary<string, GifCache>)deserializer.ReadObject(stream);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }
        }

        private async Task SaveData()
        {
            await CreateImageFolderAndFile();
            try
            {
                /*
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, GifCache>));
                    serializer.WriteObject(stream, gifCacheDictionary);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataSet[CacheBitmapImageKey] = reader.ReadToEnd();
                    }
                }
                */

                StorageFile storageFile = await (await CacheManagement.Instance.GetImageFolder()).CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, GifCache>));
                    serializer.WriteObject(stream, gifCacheDictionary);
                    stream.Position = 0;
                    using (var streamFile = await storageFile.OpenStreamForWriteAsync())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var output = streamFile.AsOutputStream();
                            DataWriter dataWriter = new DataWriter(output);
                            dataWriter.WriteString(reader.ReadToEnd());
                            await dataWriter.StoreAsync();
                            await output.FlushAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        //

        /// <summary>
        /// 重置删除所有
        /// </summary>
        public async void Reset()
        {
            this.gifCacheDictionary.Clear();
            //dataSet.Remove(CacheBitmapImageKey);
            await CacheManagement.Instance.DelectImageLocalFolder();
        }

        /// <summary>
        /// 返回图片的数据字典
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, GifCache>> GifCacheList()
        {
            await Restore();
            return gifCacheDictionary;
        }

        /// <summary>
        /// 同步方法 可以用GifCacheDictionaryHelper类，进行操作
        /// </summary>
        public async void LoadAndSynchronousDictionary()
        {
            await Restore();
            GifCacheDictionaryHelper.Instance.SynchronousDictionary(gifCacheDictionary);
        }

        /// <summary>
        /// 添加方法 可以用GifCacheDictionaryHelper类，进行操作
        /// </summary>
        public async void AddCacheList()
        {
            this.gifCacheDictionary = GifCacheDictionaryHelper.Instance.GetgifCacheDictionary();
            await SaveData();
        }
    }
}
