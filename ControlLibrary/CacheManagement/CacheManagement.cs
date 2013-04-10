using ControlLibrary.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ControlLibrary.CacheManagement
{
    /// <summary>
    /// 缓存的管理类 回头会改internal级别
    /// </summary>
    public class CacheManagement
    {
        private volatile static CacheManagement _instance = null;
        private static readonly object lockHelper = new object();

        public static CacheManagement Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                            _instance = new CacheManagement();

                    }
                }
                return _instance;
            }
        }

        //文件夹名
        private const string CacheImageFolder = "CacheImageFolder";
        private const string ImageFolderPath = "ms-appdata:///local/" + CacheImageFolder + "/";
        //过期时间 默认是5天
        private const int CacheTime = 5;
        //设置上线文件夹的大小
        private const ulong CacheSize = 200UL;
        //设置标线文件夹的大小
        private const ulong CacheStandardSize = 100UL;

        /// <summary>
        /// 创建存放到本地放图片的文件夹.返回trur为成功,false为失败.
        /// </summary>
        public async Task<bool> CreateImageFolder()
        {
            var fileName = string.Empty;
            StorageFolder imageLocalFolder = null;
            StorageFolder tempLocalFolder = ApplicationData.Current.LocalFolder;
            bool isImageLocalFolder = false;
            try
            {
                imageLocalFolder = await tempLocalFolder.CreateFolderAsync(CacheImageFolder, CreationCollisionOption.FailIfExists);
            }
            catch
            {
                isImageLocalFolder = true;
            }
            if (isImageLocalFolder)
            {
                imageLocalFolder = await tempLocalFolder.GetFolderAsync(CacheImageFolder);
            }
            //imageLocalFolder = await tempLocalFolder.CreateFolderAsync(CacheImageFolder, CreationCollisionOption.OpenIfExists);
            if (imageLocalFolder != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除本地文件夹的方法
        /// </summary>
        public async Task<bool> DelectImageLocalFolder()
        {
            await CreateImageFolder();
            try
            {
                //删除系统本地文件夹的myFolder文件夹及其子文件夹
                StorageFolder tempLocalFolder = ApplicationData.Current.LocalFolder;
                StorageFolder imageLocalFolder = await tempLocalFolder.GetFolderAsync(CacheImageFolder);
                if (imageLocalFolder != null)
                {
                    await imageLocalFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 返回文件夹的大小
        /// </summary>
        /// <returns></returns>
        public async Task<ulong> GetImageLocalFolderSize()
        {
            await CreateImageFolder();
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var basicProperties = await tempFolder.GetBasicPropertiesAsync();
            return basicProperties.Size;
        }

        /// <summary>
        /// 返回文件夹中每个文件的大小
        /// </summary>
        /// <returns></returns>
        public async Task<List<ulong>> GetImageLocalFolderFileSize()
        {
            await CreateImageFolder();
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var storageFileList = await tempFolder.GetFilesAsync();
            List<ulong> sizeList = new List<ulong>();
            foreach (var item in storageFileList)
            {
                sizeList.Add((await item.GetBasicPropertiesAsync()).Size);
            }
            return sizeList;
        }

        /// <summary>
        /// 返回文件夹的大小
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImageLocalFolderSizeString()
        {
            return SizeLongToString.Format_FileSize((await GetImageLocalFolderSize()));
        }

        /// <summary>
        /// 返回文件夹中每个文件的大小
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetImageLocalFolderFileSizeString()
        {
            await CreateImageFolder();
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var storageFileList = await tempFolder.GetFilesAsync();
            List<string> sizeList = new List<string>();
            foreach (var item in storageFileList)
            {
                sizeList.Add(SizeLongToString.Format_FileSize((await item.GetBasicPropertiesAsync()).Size));
            }
            return sizeList;
        }

        /// <summary>
        /// 返回文件夹中所有文件的大小和
        /// </summary>
        /// <returns></returns>
        public async Task<ulong> GetImageLocalFolderAllFileSize()
        {
            await CreateImageFolder();
            ulong allFileSize = 0UL;
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var storageFileList = await tempFolder.GetFilesAsync();
            foreach (var item in storageFileList)
            {
                allFileSize += (await item.GetBasicPropertiesAsync()).Size;
            }
            return allFileSize;
        }

        /// <summary>
        /// 返回文件夹中所有文件的大小和
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImageLocalFolderAllFileSizeString()
        {
            await CreateImageFolder();
            string allFileSize = string.Empty;
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var storageFileList = await tempFolder.GetFilesAsync();
            foreach (var item in storageFileList)
            {
                allFileSize += SizeLongToString.Format_FileSize((await item.GetBasicPropertiesAsync()).Size);
            }
            return allFileSize;
        }

        /// <summary>
        /// 返回缓存图片文件夹所有文件的修改时间
        /// </summary>
        /// <returns></returns>
        public async Task<List<DateTimeOffset>> GetImageLocalFolderFileTime()
        {
            await CreateImageFolder();
            var tempFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder);
            var storageFileList = await tempFolder.GetFilesAsync();
            List<DateTimeOffset> dtoList = new List<DateTimeOffset>();
            foreach (var item in storageFileList)
            {
                dtoList.Add((await item.GetBasicPropertiesAsync()).DateModified);
            }
            return dtoList;
        }

        /// <summary>
        /// 返回缓存图片文件夹修改时间
        /// </summary>
        /// <returns></returns>
        public async Task<DateTimeOffset> GetImageLocalFolderTime()
        {
            await CreateImageFolder();
            return (await (
                    await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder)).GetBasicPropertiesAsync()
                   ).DateModified;
        }

        /// <summary>
        /// 获取缓存图片的文件夹
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFolder> GetImageFolder()
        {
            await CreateImageFolder();
            return (await ApplicationData.Current.LocalFolder.GetFolderAsync(CacheImageFolder));
        }
    }
}
