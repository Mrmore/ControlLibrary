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
    class ServiceCacheServiceImpl<T> : ICacheSerivce<T>, IService
    {
        public bool IsValid(ICacheChip<T> chip)
        {
            if (!_cacheIndexs.ContainsKey(chip.HashKey)) return false;
            else if (_cacheIndexs.ContainsKey(chip.HashKey) && DateTime.Now > _cacheIndexs[chip.HashKey].ExpirationTime)
            {
                chip.Reset();
                _cacheIndexs.Remove(chip.HashKey);
                return false;
            }
            else return true;
        }

        public async Task<bool> Add(ICacheChip<T> chip, params object[] contents)
        {
            T content = (T)contents[0];
            if (_cacheIndexs.ContainsKey(chip.HashKey))
            {
                _cacheIndexs[chip.HashKey] = chip;
            }
            else
            {
                _cacheIndexs.Add(chip.HashKey, chip);
                await chip.SaveCacheData(content);
            }
            return true;
        }

        public ICacheChip<T> CreateCacheChip(string hashKey, DateTime? expTime = null, IProgress<int> progress = null)
        {
            if (this._cacheIndexs.ContainsKey(hashKey))
            {
                return _cacheIndexs[hashKey];
            }
            else
            {
                ICacheChip<T> instance = new ServiceCacheChip<T>(this._cacheIndexs, hashKey, this._cacheFolder, expTime);
                this._cacheIndexs.Add(hashKey, instance);
                return instance;
            }
        }

        public async Task<bool> Remove(ICacheChip<T> chip)
        {
            if (this._cacheIndexs.ContainsKey(chip.HashKey))
            {
                await chip.Reset();
                _cacheIndexs.Remove(chip.HashKey);
            }
            return true;
        }

        public async Task<bool> Replace(ICacheChip<T> chip, params object[] contents)
        {
            if (contents.Length < 2) throw new ArgumentException();

            if (this._cacheIndexs.ContainsKey(chip.HashKey))
            {
                _cacheIndexs.Remove(chip.HashKey);
            }

            ICacheChip<T> newchip = contents[0] as ICacheChip<T>;
            T content = (T)contents[0];
            _cacheIndexs.Add(chip.HashKey, newchip);

            await newchip.SaveCacheData(content);
            return true;
        }

        public async Task<T> Pick(ICacheChip<T> chip)
        {
            return await chip.PickCacheData();
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
        IDictionary<string, ICacheChip<T>> _cacheIndexs = new Dictionary<string, ICacheChip<T>>();
        public async Task<bool> Init(params object[] args)
        {
            // Ensure it just can be inited only once
            lock (this)
            {
                if (true == isInited) return false;
                else isInited = true;
            }

            {
                if (args.Length < 1) throw new ArgumentException();

                _uniqueName = (string)args[0];
                _folderName = "ServiceCacheServiceImpl" + "_" + _uniqueName;
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
                        _cacheFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(_folderName, CreationCollisionOption.OpenIfExists);
                    }
                }

                if (_cacheFolder == null) return false;

                var fileList = await _cacheFolder.GetFilesAsync();

                foreach (var item in fileList)
                {
                    var compName = item.Name.Split('.');
                    var hashKey = compName[0];
                    DateTime time = new DateTime(Convert.ToInt64(compName[1]));

                    if (!_cacheIndexs.ContainsKey(hashKey))
                    {
                        var chip = this.CreateCacheChip(hashKey, time);
                        _cacheIndexs.Add(hashKey, chip);
                    }
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
            catch (Exception) { }
        }


        public Task<IStorageFile> PickFile(ICacheChip<T> chip)
        {
            return chip.PickCacheFile();
        }
    }
}
