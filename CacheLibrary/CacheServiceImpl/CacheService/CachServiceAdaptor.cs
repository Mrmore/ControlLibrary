using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Framework;
using RenrenCoreWrapper.Framework.CacheService;
using Windows.UI.Xaml.Media.Imaging;

namespace RenrenCoreWrapper.CacheService
{
    public class CachServiceAdaptor<T> : ICacheSerivce<T>, IService
    {
        /// <summary>
        /// The target implementation
        /// </summary>
        private  IService _target = null;

        /// <summary>
        /// Construct: using the implementtation type
        /// </summary>
        /// <param name="type"></param>
        public CachServiceAdaptor(CacheServiceType type)
        {
            switch (type)
            {
                case CacheServiceType.IMAGE:
                    _target = new ImageCacheServiceImpl();
                    break;
                case CacheServiceType.SERVICE:
                    _target = new ServiceCacheServiceImpl<T>();
                    break;
                default:
                    throw new ArgumentException("Implementation type error!");
            }
        }


        public bool IsValid(ICacheChip<T> chip)
        {
            return (this._target as ICacheSerivce<T>).IsValid(chip);
        }

        public Task<bool> Add(ICacheChip<T> chip, params object[] contents)
        {
            return (this._target as ICacheSerivce<T>).Add(chip, contents);
        }

        public Task<bool> Remove(ICacheChip<T> chip)
        {
            return (this._target as ICacheSerivce<T>).Remove(chip);
        }

        public Task<bool> Replace(ICacheChip<T> chip, params object[] contents)
        {
            return (this._target as ICacheSerivce<T>).Replace(chip, contents);
        }

        public Task<T> Pick(ICacheChip<T> chip)
        {
            return (this._target as ICacheSerivce<T>).Pick(chip);
        }

        public Task<Windows.Storage.IStorageFile> PickFile(ICacheChip<T> chip)
        {
            return (this._target as ICacheSerivce<T>).PickFile(chip);
        }

        public ICacheChip<T> CreateCacheChip(string hashKey, DateTime? expTime = null, IProgress<int> progress = null)
        {
            return (this._target as ICacheSerivce<T>).CreateCacheChip(hashKey, expTime, progress);
        }

        public ServiceCatalogue S_Catalogue
        {
            get { return (this._target as IService).S_Catalogue; }
        }

        public string S_UID
        {
            get { return (this._target as IService).S_UID; }
        }

        public ServiceType S_Type
        {
            get { return (this._target as IService).S_Type; }
        }

        public Task<object> Accecpt(IServiceVisitor visitor, params object[] args)
        {
            return (this._target as IService).Accecpt(visitor, args);
        }

        public Task<object> Invoke(ServiceRole role, params object[] args)
        {
            return (this._target as IService).Invoke(role, args);
        }

        public Task<bool> Init(params object[] args)
        {
            return (this._target as IService).Init(args);
        }

        public Task Reset()
        {
            return (this._target as IService).Reset();
        }
    }
}
