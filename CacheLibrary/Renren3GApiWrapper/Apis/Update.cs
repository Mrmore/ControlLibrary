using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DataLayerWrapper.Http;
using System.Runtime.Serialization.Json;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using System.Threading.Tasks;
using Windows.UI.Core;
using System.Reflection;
using Windows.ApplicationModel;
using RenrenCoreWrapper.AsyncArgs;
using RenrenCoreWrapper.Helper;
using RenrenCoreWrapper.Entities;
using RenrenCoreWrapper.Constants;

namespace RenrenCoreWrapper.Apis
{
    public partial class Renren3GApiWrapper
    {
        static public async Task<RenrenAyncRespArgs<UpdateInfoEntity>> GetUpdateInfo(string sessionKey, string secretKey)
        {
            IDeviceInfoHelper deviceHelper = new DeviceInfoAdaptor(DeviceInfoAdaptor.ImplType.NETWORKADAPTOR);
            var deviceId = deviceHelper.GetDeviceID();
            var deviceName = await deviceHelper.GetDeviceName();

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            string version_ = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);
            string pubdate_ = ConstantValue.PublishDate;
            //version_ = "0.0.1";
            //pubdate_ = "20120213";

            var parameters = ApiHelper.GetBaseParameters(sessionKey).Result;
            parameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            parameters.Add(new RequestParameterEntity("method", Method.GetUpdateInfo));
            parameters.Add(new RequestParameterEntity("call_id", ApiHelper.GenerateTime()));
            parameters.Add(new RequestParameterEntity("v", "1.0"));
            parameters.Add(new RequestParameterEntity("session_key", sessionKey));
            parameters.Add(new RequestParameterEntity("name", ConstantValue.AppName));
            parameters.Add(new RequestParameterEntity("version", version_));
            parameters.Add(new RequestParameterEntity("channelId", ConstantValue.ChannelId));
            parameters.Add(new RequestParameterEntity("ua", deviceName));
            parameters.Add(new RequestParameterEntity("os", ConstantValue.OS));
            parameters.Add(new RequestParameterEntity("pubdate", pubdate_));
            parameters.Add(new RequestParameterEntity("up", ConstantValue.UpdateType));
            parameters.Add(new RequestParameterEntity("property", "13"));
            parameters.Add(new RequestParameterEntity("subproperty", "0"));

            parameters.Add(new RequestParameterEntity("sig", ApiHelper.GenerateSig(parameters, secretKey)));

            var result = await agentReponseHandler<UpdateInfoEntity, RenrenAyncRespArgs<UpdateInfoEntity>>(parameters, ConstantValue.PostMethod, new Uri("http://api.m.renren.com/api/"));
            return result;
        }
    }
}
