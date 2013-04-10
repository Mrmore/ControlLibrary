using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Core;
using System.Reflection;
using DataLayerWrapper.Http;
using System.Diagnostics;
using Windows.Storage;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;

namespace RenrenCoreWrapper.Apis
{
    // refine to 3GApiWrapper
    public partial class Renren3GApiWrapper
    {
        #region 3G api convient singleton wrapper
        private static Renren3GApiWrapper _instance = null;
        public static Renren3GApiWrapper Renren3GApi
        {
            get {
                lock (typeof(Renren3GApiWrapper))
                {
                    if (null == _instance) {
                        _instance = new Renren3GApiWrapper();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 3G api reponse handler, overall remote, local error and returned result has been handled in this section for generic post request
        static private async Task<ArgType> agentReponseHandler<EntityType, ArgType>(ICollection<RequestParameterEntity> args, string method, Uri target = null)
        {
            EntityType entity = default(EntityType);
            ErrorEntity error = null;
            ArgType result = default(ArgType);
            string response = string.Empty;
            try
            {
                var agent = new HttpWebRequestAgent();
                agent.Method = method;

                foreach (var parameter in args)
                {
                    agent.AddParameters(parameter.Name, Uri.EscapeDataString(parameter.Values));
                }

                //string response = string.Empty;
                if (null != target)
                {
                    response = await agent.DownloadString(target);
                }
                else
                {
                    response = await agent.DownloadString(ConstantValue.RequestUri);
                }

                Debug.WriteLine(response);
                entity = (EntityType)JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(response)), typeof(EntityType));

                //entity = (EntityType)JsonUtility.DeserializeObj(new MemoryStream(Encoding.Unicode.GetBytes(response)),typeof(EntityType));

                error = (ErrorEntity)JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(response)),
                                                                         typeof(ErrorEntity));

                if (error != null && error.Error_msg != null)
                {
                    // Try to translate the error mesg
                    if (RemoteErrorMsgTranslator.Instance.Ready)
                    {
                        if (RemoteErrorMsgTranslator.Instance.ErrorTable.ContainsKey(error.Error_code))
                        {
                            var errorEntity = RemoteErrorMsgTranslator.Instance.ErrorTable[error.Error_code];
                            error.Error_msg = errorEntity.Mesg;
                            error.Error_Type = errorEntity.Type;
                        }
                    }
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), error);
                }
                else if (entity != null)
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), entity);
                }
                else
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), new ArgumentException());
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;

                if (errorMsg.Contains("Encountered invalid character"))
                {
                    bool ifBreak = false;
                    while (!ifBreak)
                    {
                        var f = errorMsg.IndexOf("'") + 1;
                        var l = errorMsg.LastIndexOf("'");
                        var m = errorMsg.Substring(f, l - f);

                        int ret = DecoderJson(response.Replace(Convert.ToChar(m), '?'), typeof(EntityType));

                        if (ret < 0)
                        {
                            result = (ArgType)Activator.CreateInstance(typeof(ArgType), ex);
                            ifBreak = true;
                        }
                        else if (ret > 0)
                        {
                            entity = (EntityType)JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(response.Replace(Convert.ToChar(m), '?'))), typeof(EntityType));
                            result = (ArgType)Activator.CreateInstance(typeof(ArgType), entity);
                            ifBreak = true;
                        }

                    }
                }
                else
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), ex);
                }
            }

            return result;
        }

        static string errorMsg;

        static int DecoderJson(string msg, Type type)
        {
            try
            {
                var result = JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(msg)), type);
                return 1;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                if (errorMsg.Contains("Encountered invalid character"))
                {
                    return 0;
                }
                return -1;
            }
        }

        #endregion

        #region 3G api reponse handler, overall remote, local error and returned result has been handled in this section for multi-part post request
        static private async Task<ArgType> agentReponseMpHandler<EntityType, ArgType>(ICollection<RequestParameterEntity> args, string method, Uri uri, StorageFile file)
        {
            EntityType entity = default(EntityType);
            ErrorEntity error = null;
            ArgType result = default(ArgType);
            try
            {
                var agent = new HttpMPRequestAgent();
                agent.Method = method;

                foreach (var parameter in args)
                {
                    agent.AddParameters(parameter.Name, parameter.Values);
                }

                string response = await agent.DownloadString(uri, file);
                entity = (EntityType)JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(response)),
                                                                         typeof(EntityType));
                error = (ErrorEntity)JsonUtility.DeserializeObj(new MemoryStream(Encoding.UTF8.GetBytes(response)),
                                                                         typeof(ErrorEntity));

                if (error != null && error.Error_msg != null)
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), error);
                }
                else if (entity != null)
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), entity);
                }
                else
                {
                    result = (ArgType)Activator.CreateInstance(typeof(ArgType), new ArgumentException());
                }
            }
            catch (Exception ex)
            {
                result = (ArgType)Activator.CreateInstance(typeof(ArgType), ex);
            }

            return result;
        }

        #endregion
    }
}
