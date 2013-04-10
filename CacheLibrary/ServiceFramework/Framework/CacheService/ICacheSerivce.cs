using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RenrenCoreWrapper.Framework.CacheService
{
    public interface ICacheSerivce<T>
    {
        /// <summary>
        /// 验证该缓存片段是否仍然有效
        /// 1，是否存在，
        /// 2，仍在有效期内
        /// </summary>
        /// <param name="chip"></param>
        /// <returns></returns>
        bool IsValid(ICacheChip<T> chip);

        /// <summary>
        /// 添加缓存片段
        /// </summary>
        /// <param name="chip"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        Task<bool> Add(ICacheChip<T> chip, params object[] contents);

        /// <summary>
        /// 移除缓存片段
        /// </summary>
        /// <param name="chip"></param>
        /// <returns></returns>
        Task<bool> Remove(ICacheChip<T> chip);
        
        /// <summary>
        /// 替换缓存片段
        /// </summary>
        /// <param name="chip"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        Task<bool> Replace(ICacheChip<T> chip, params object[] contents);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="chip"></param>
        /// <returns></returns>
        Task<T> Pick(ICacheChip<T> chip);

        /// <summary>
        /// 获取缓存数据所在的文件
        /// </summary>
        /// <param name="chip"></param>
        /// <returns></returns>
        Task<IStorageFile> PickFile(ICacheChip<T> chip);

        /// <summary>
        /// 创建缓存片段
        /// </summary>
        /// <param name="hashKey"></param>
        /// <param name="expTime"></param>
        /// <returns></returns>
        ICacheChip<T> CreateCacheChip(string hashKey, DateTime? expTime = null, IProgress<int> progress = null);
    }
}
