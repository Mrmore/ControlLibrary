using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenrenCoreWrapper.Framework
{
    /// <summary>
    /// 列举所有业务分类
    /// 该分类对应Renren3GApiWrapper的接口分类
    /// </summary>
    public enum ServiceCatalogue
    {
        Unkown
    }

    public enum ServiceType
    {
        LoginServiceType,
        AlbumListServiceType,
        NewsFeedServiceType,
        Unkown,

        /// <summary>
        /// 照片7
        /// </summary>
        CachePhoto,

        /// <summary>
        /// 新鲜事图片3
        /// </summary>
        CacheFeedImage,

        /// <summary>
        /// 头像3
        /// </summary>
        CacheHead,

        /// <summary>
        /// 登录用户名头像 永久
        /// </summary>
        CacheLoginHead,

        /// <summary>
        /// 杂项图片
        /// </summary>
        CacheMiscellaneousImage
    }

    public enum ServiceRole
    {
        PreCondition,
        PostCondition,
        SelfInvoke
    }

    /// <summary>
    /// Service Type to refer to
    /// </summary>
    public enum CacheServiceType { IMAGE, SERVICE }

    /// <summary>
    /// 业务的公共基类
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 返回该业务的分类属性
        /// </summary>
        ServiceCatalogue S_Catalogue { get; }

        /// <summary>
        /// 返回该业务实体的唯一ID
        /// </summary>
        /// <returns></returns>
        string S_UID { get; }

        /// <summary>
        /// 返回该业务类型
        /// </summary>
        ServiceType S_Type { get; }

        // Extention interface reserved
        /// <summary>
        /// Visitor用于接口扩展，例如注入widget,联表查询。
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> Accecpt(IServiceVisitor visitor, params object[] args);

        /// <summary>
        /// invoke该业务的功能
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<object> Invoke(ServiceRole role, params object[] args);

        /// <summary>
        /// 所有业务都需要初始化的过程
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task<bool>Init(params object[] args);

        /// <summary>
        /// Overall reset interface
        /// </summary>
        /// <returns></returns>
        Task Reset();
    }
}
