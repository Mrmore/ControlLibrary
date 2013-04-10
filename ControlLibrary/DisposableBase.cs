using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLibrary
{
    /// <summary>
    /// Dispose模式的代码实现
    /// Dispose Pattern
    /// </summary>
    public class DisposableBase : IDisposable
    {
        /// <summary>
        /// 避免被释放多次的标记
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 无参构造
        /// </summary>
        public DisposableBase()
        {
        }

        /// <summary>
        /// 真正执行Dispose的虚方法
        /// 以便子类对自己的资源进行适当的清理
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 释放非托管资源
                    // 如引用对象， 数组等
                }

                // 释放托管资源， 如文件， Socket连接等
                disposed = true;
            }
        }

        /// <summary>
        /// 清理资源的方法
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // 通知系统不需要再调用该对象的终结器
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// 终结器
        /// 有.NetGC自动调用
        /// 具有不确定性
        /// Finalizer
        /// </summary>
        ~DisposableBase()
        {
            // 若调用者没有手动的进行
            // 资源的回收， 则有系统GC进行回收
            Dispose(false);
        }
    }

    // 如何继承该基础类
    /*public class DerivedDisposeObj : DisposableBase
    {
        private bool isSelfDisposed = false;

        protected override void Dispose(bool disposing)
        {
            if (!isSelfDisposed)
            {
                if (disposing)
                {
                    // 释放托管资源的代码
                }

                // 释放非托管资源的代码

                base.Dispose(disposing);
                // 进行标记
                isSelfDisposed = true; 
            }
        }
    }*/
}
