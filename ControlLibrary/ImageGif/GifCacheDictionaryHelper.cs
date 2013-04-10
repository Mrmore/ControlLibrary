using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    /// <summary>
    /// 图片的帮助对象类
    /// </summary>
    public class GifCacheDictionaryHelper
    {
        private volatile static GifCacheDictionaryHelper _instance = null;
        private static readonly object lockHelper = new object();

        public static GifCacheDictionaryHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new GifCacheDictionaryHelper();

                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, GifCache> gifCacheDictionary = null;
        //初始化方法
        private GifCacheDictionaryHelper()
        {
            gifCacheDictionary = new Dictionary<string, GifCache>();
        }

        //添加的方法
        public void AddUriToBytes(string uri, byte[] bytes)
        {
            if (!gifCacheDictionary.ContainsKey(uri))
            {
                gifCacheDictionary.Add(uri, new GifCache() { Bytes = bytes, WebUri = uri });
            }
        }

        //查询的方法
        public GifCache SelectUriToBytes(string uri)
        {
            if (gifCacheDictionary.ContainsKey(uri))
            {
                return gifCacheDictionary[uri];
            }
            else
            {
                return null;
            }
        }

        //删除所有Uri的Key
        public void DelectUriToBytesAll()
        {
            if (gifCacheDictionary.Keys.Count > 0)
            {
                gifCacheDictionary.Clear();
            }
        }

        //删除指定Uri的Key
        public void DelectUriToBytes(string uri)
        {
            if (gifCacheDictionary.ContainsKey(uri))
            {
                gifCacheDictionary.Remove(uri);
            }
        }

        //同步数据字典
        public void SynchronousDictionary(Dictionary<string, GifCache> GifCacheDictionary)
        {
            if (GifCacheDictionary.Count > 0)
            {
                this.gifCacheDictionary = GifCacheDictionary;
            }
        }

        //返回当前的数据字典
        public Dictionary<string, GifCache> GetgifCacheDictionary()
        {
            if (gifCacheDictionary.Count > 0)
            {
                return gifCacheDictionary;
            }
            return null;
        }
    }
}
