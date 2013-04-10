using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataLayerWrapper.Storage;
using RenrenCoreWrapper.Framework;
using RenrenCoreWrapper.Framework.CacheService;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace RenrenCoreWrapper.CacheService
{
    /// <summary>
    /// 对于图片，没有失效日期的问题，所以不支持失效日期
    /// </summary>
    class ImageCacheServiceImpl : ICacheSerivce<BitmapImage>, IService
    {
        public bool IsValid(ICacheChip<BitmapImage> chip)
        {
            if (_cacheIndexs.ContainsKey(chip.HashKey) && (_cacheIndexs[chip.HashKey] as ImageCacheChip).ImageReady)
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> Add(ICacheChip<BitmapImage> chip, params object[] contents)
        {
            //lock (this)
            {
                string url = (string)contents[0];
                if (_cacheIndexs.ContainsKey(chip.HashKey))
                {
                    _cacheIndexs[chip.HashKey] = chip;
                }
                else
                {
                    _cacheIndexs.Add(chip.HashKey, chip);
                }

                await chip.SaveCacheData(url);
            }

            return true;
        }

        public ICacheChip<BitmapImage> CreateCacheChip(string hashKey, DateTime? expTime = null, IProgress<int> progress = null)
        {
            if (this._cacheIndexs.ContainsKey(hashKey))
            {
                return _cacheIndexs[hashKey];
            }
            else
            {
                ICacheChip<BitmapImage> instance = new ImageCacheChip(this._cacheIndexs, hashKey, this._cacheFolder, expTime, progress);
                this._cacheIndexs.Add(hashKey, instance);
                return instance;
            }
        }

        public async Task<bool> Remove(ICacheChip<BitmapImage> chip)
        {
            if (this._cacheIndexs.ContainsKey(chip.HashKey))
            {
                await chip.Reset();
                _cacheIndexs.Remove(chip.HashKey);
            }
            return true;
        }

        public async Task<bool> Replace(ICacheChip<BitmapImage> chip, params object[] contents)
        {
            if (contents.Length < 2) throw new ArgumentException();

            if (this._cacheIndexs.ContainsKey(chip.HashKey))
            {
                _cacheIndexs.Remove(chip.HashKey);
            }

            ICacheChip<BitmapImage> newchip = contents[0] as ICacheChip<BitmapImage>;
            string url = contents[1] as string;
            _cacheIndexs.Add(chip.HashKey, newchip);

            await newchip.SaveCacheData(url);
            return true;
        }

        public async Task<BitmapImage> Pick(ICacheChip<BitmapImage> chip)
        {
            BitmapImage result = null;
            {
                result = chip.PickCacheData().Result;
            }
            return result;
        }

        public ServiceCatalogue S_Catalogue
        {
            get { throw new NotImplementedException(); }
        }

        public string S_UID
        {
            get { throw new NotImplementedException(); }
        }

        public ServiceType S_Type
        {
            get { throw new NotImplementedException(); }
        }

        public Task<object> Accecpt(IServiceVisitor visitor, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Task<object> Invoke(ServiceRole role, params object[] args)
        {
            throw new NotImplementedException();
        }

        StorageFolder _cacheFolder = null;
        string _folderName = string.Empty;
        string _uniqueName = string.Empty;
        bool isInited = false;
        IDictionary<string, ICacheChip<BitmapImage>> _cacheIndexs = new Dictionary<string, ICacheChip<BitmapImage>>();
        public async Task<bool> Init(params object[] args)
        {
            if (args.Length < 1) throw new ArgumentException();

            // Ensure it just can be inited only once
            lock (this)
            {
                if (true == isInited) return false;
                else isInited = true;
            }

            _uniqueName = (string)args[0];
            _folderName = typeof(ImageCacheServiceImpl).ToString() + "_" + _uniqueName;
            if (_cacheFolder == null)
            {
                bool folderExist = false;
                try
                {
                    _cacheFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(_folderName);
                    folderExist = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    folderExist = false;
                }

                if (folderExist == false)
                {
                    try
                    {
                        _cacheFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            if (_cacheFolder == null) return false;

            var fileList = await _cacheFolder.GetFilesAsync();

            foreach (var item in fileList)
            {
                if (!_cacheIndexs.ContainsKey(item.Name))
                {
                    var chip = this.CreateCacheChip(item.Name, DateTime.Now);
                    (chip as ImageCacheChip).ImageReady = true;
                    _cacheIndexs.Add(item.Name, chip);
                }
            }

            return true;
        }

        public async Task Reset()
        {
            _cacheIndexs.Clear();
            try
            {
                await this._cacheFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                _cacheFolder = null;
                await Init(this._uniqueName);
            }
            catch (Exception)  {  }
        }


        public Task<IStorageFile> PickFile(ICacheChip<BitmapImage> chip)
        {
            return chip.PickCacheFile();
        }
    }
}
