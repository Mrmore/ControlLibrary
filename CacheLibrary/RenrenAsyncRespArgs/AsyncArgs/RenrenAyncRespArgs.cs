using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Models;

namespace RenrenCoreWrapper.AsyncArgs
{
    /// <summary>
    /// The wrapper for async response arguments
    /// It is in charge of containing four things:
    /// 1, the overall response entity data if this request is passed
    /// 2, the local exception if it happens in local
    /// 3, the remote error if it return by server
    /// 4, the overal translated error message
    /// </summary>
    /// <typeparam name="EntityType">the entity type</typeparam>
    public class RenrenAyncRespArgs<EntityType> : IRenrenAsyncRespArgs where EntityType : PropertyChangedBase, INotifyPropertyChanged, new()
    {
        #region Properties

        /// <summary>
        /// The overall response entity data
        /// </summary>
        public EntityType Result { get; private set; }

        /// <summary>
        /// The local exception recodeing
        /// </summary>
        public Exception LocalError { get; private set; }

        /// <summary>
        /// The remote error recording
        /// </summary>
        public ErrorEntity RemoteError { get; private set; }

        /// <summary>
        /// The overall translated error message
        /// </summary>
        public string ErrMesg { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Construct using entity data
        /// </summary>
        /// <param name="result">entity data</param>
        public RenrenAyncRespArgs(EntityType result)
        {
            Result = result;
            ErrMesg = string.Empty;
            this.Status = RespStatus.Successed;
        }

        /// <summary>
        /// Construct using local exception
        /// </summary>
        /// <param name="ex">local exception</param>
        public RenrenAyncRespArgs(Exception ex)
        {
            LocalError = ex;

            // try to use the localized generic error string
            try
            {
                ErrMesg = new ResourceLoader().GetString("GenericErrorMessage");
            }
            catch (Exception)
            {
                ErrMesg = ex.Message;
            }
            this.Status = RespStatus.Failed;
        }

        /// <summary>
        /// Construct using remote error which should be translated
        /// </summary>
        /// <param name="error">remote error</param>
        public RenrenAyncRespArgs(ErrorEntity error)
        {
            RemoteError = error;
            ErrMesg = error.Error_msg;
            this.Status = RespStatus.Failed;
        }

        #endregion

        #region Comes from IRenrenAsyncRespArgs
        public RespStatus Status { get; set; }
        public object HandOverParams { get; set; }
        #endregion
    }

    public class RenRenBatchRunResponseArg : IRenrenAsyncRespArgs
    {
        private IDictionary<BatchRunBinder, object> _result = new Dictionary<BatchRunBinder, object>();
        #region Properties

        public IDictionary<BatchRunBinder, object> Result { get; private set; }

        /// <summary>
        /// The local exception recodeing
        /// </summary>
        public Exception LocalError { get; private set; }

        /// <summary>
        /// The remote error recording
        /// </summary>
        public ErrorEntity RemoteError { get; private set; }

        /// <summary>
        /// The overall translated error message
        /// </summary>
        public string ErrMesg { get; private set; }

        #endregion
        string _regexPattern = @"({0}): (\{.*?\})";
        #region Constructors

        /// <summary>
        /// Construct using entity data
        /// </summary>
        /// <param name="result">entity data</param>
        public RenRenBatchRunResponseArg(string result, ICollection<BatchRunBinder> args)
        {
            //Result = result;
            this._result.Clear();

            foreach (var item in args)
            {
                Regex regex = new Regex(string.Format(this._regexPattern, item.Method), RegexOptions.IgnoreCase);
                var m = regex.Match(result);
                if (m.Success)
                {
                    string data = m.Groups[2].Value;
                    object obj = JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(data)), item.RespType);
                    this._result.Add(item, obj);
                }
            }

            this.Status = RespStatus.Successed;
            ErrMesg = string.Empty;
        }

        /// <summary>
        /// Construct using local exception
        /// </summary>
        /// <param name="ex">local exception</param>
        public RenRenBatchRunResponseArg(Exception ex)
        {
            LocalError = ex;

            // try to use the localized generic error string
            try
            {
                ErrMesg = new ResourceLoader().GetString("GenericErrorMessage");
            }
            catch (Exception)
            {
                ErrMesg = ex.Message;
            }

            this.Status = RespStatus.Failed;
        }

        /// <summary>
        /// Construct using remote error
        /// </summary>
        /// <param name="error">remote error</param>
        public RenRenBatchRunResponseArg(ErrorEntity error)
        {
            RemoteError = error;
            ErrMesg = error.Error_msg;
            this.Status = RespStatus.Failed;
        }

        #endregion

        #region Comes from IRenrenAsyncRespArgs
        public RespStatus Status { get; set; }
        public object HandOverParams { get; set; }
        #endregion
    }
}
