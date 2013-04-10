using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.Framework.RenrenService
{
    /// <summary>
    /// 人人业务Visitor的定义
    /// </summary>
    /// <typeparam name="IdType"></typeparam>
    /// <typeparam name="EntityType"></typeparam>
    abstract class RRServiceVisistor<IdType, EntityType> : IServiceVisitor where EntityType : PropertyChangedBase, INotifyPropertyChanged, new()
    {
        protected abstract Task<object> DoVisit(RRAbstractService<IdType, EntityType> service, params object[] args);

        public async Task<object> Visit(object service, params object[] args)
        {
            return await DoVisit((service as RRAbstractService<IdType, EntityType>), args);
        }
    }
}
