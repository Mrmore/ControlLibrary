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
using System.Diagnostics;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;
using DataLayerWrapper.Http;

namespace RenrenCoreWrapper.Apis
{
    public partial class Renren3GApiWrapper
    {
        /// <summary>
        /// 批量调用API的工具方法，减少网络的交互 batch.run的method_feed中 
        /// 如包含需要传入用户uid的api, 则之前调用login command 或者统一传入session_key, 
        /// method_feed中的项不要传入session_key 
        /// method_feed中每个字符串中＝号后的字符串需要urlencode，
        /// POST数据的时候，还需要对method_feed的值再urlencode一次。 
        /// 
        /// Usage:
        /// BatchRunBinder binder1 = new BatchRunBinder() { Method = Method.GetUserInfo, RespType = typeof(UserInfoEntity) };
        /// registryBinder1.AddPair("type", "8191");
        ///
        /// BatchRunBinder binder2 = new BatchRunBinder() { Method = Method.GetStatus, RespType = typeof(StatusEntity) };
        /// registryBinder2.AddPair("uid", LoginViewModel.Instance.Model.Uid.ToString());
        ///
        /// IList<BatchRunBinder> args = new List<BatchRunBinder>();
        /// args.Add(binder1);
        /// args.Add(binder2);
        ///
        /// var result = App.RenRenService.BatchRun(sessionKey, secretKey, args).Result;
        /// or
        /// var result = App.RenRenService.BatchRun(sessionKey, secretKey, binder1, binder2).Result;
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="binders"></param>
        /// <returns></returns>
        public async Task<RenRenBatchRunResponseArg> BatchRun(string sessionKey, string secretKey, params BatchRunBinder[] binders)
        {
            IList<BatchRunBinder> binders_ = new List<BatchRunBinder>();
            foreach (var item in binders)
            {
                binders_.Add(item);
            }

            return await BatchRun(sessionKey, secretKey, binders_);
        }

        public async Task<RenRenBatchRunResponseArg> BatchRun(string sessionKey, string secretKey, ICollection<BatchRunBinder> binders)
        {
            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.BatchRun));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));

            if (binders.Count > 0)
            {
                string method_feed = string.Empty;
                foreach (var binder in binders)
                {
                    // build the method_feed
                    string tmp = "method=" + binder.Method;
                    List<RequestParameterEntity> baseParameters = new List<RequestParameterEntity>();
                    baseParameters.Add(new RequestParameterEntity("method", binder.Method));
                    foreach (var key in binder.Pairs.Keys)
                    {
                        tmp += ("&" + key + "=" + Uri.EscapeDataString(binder.Pairs[key]));
                        baseParameters.Add(new RequestParameterEntity(key, binder.Pairs[key]));
                    }

                    string sig = ApiHelper.GenerateSig(baseParameters, secretKey);
                    tmp += ("&sig=" + sig);
                    method_feed += "{\"" + tmp + "\"}" + ",";
                }

                method_feed.TrimEnd(',');
                method_feed = "[" + method_feed + "]";

                Debug.WriteLine(method_feed);
                parameters.Add(new RequestParameterEntity("method_feed", method_feed));
            }

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentBatchRunReponseHandler(parameters, ConstantValue.PostMethod, null, binders);
            return result;
        }

        #region Response handler
        static private async Task<RenRenBatchRunResponseArg> agentBatchRunReponseHandler(ICollection<RequestParameterEntity> args, string method, Uri target, ICollection<BatchRunBinder> binders)
        {
            ErrorEntity error = null;
            string response = string.Empty;
            RenRenBatchRunResponseArg result = null;
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
                    response = await agent.DownloadString(ConstantValue.SpecificRequestUri);
                }

                Debug.WriteLine(response);
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
                    result = new RenRenBatchRunResponseArg(error);
                }
                else
                {
                    result = new RenRenBatchRunResponseArg(response, binders);
                }
            }
            catch (Exception ex)
            {
                result = new RenRenBatchRunResponseArg(ex);
            }

            return result;
        }
        #endregion
    }
}
