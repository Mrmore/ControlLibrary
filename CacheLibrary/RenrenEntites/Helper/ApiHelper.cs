using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Constants;
using RenrenCoreWrapper.Entities;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Imaging;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace RenrenCoreWrapper.Helper
{
    public class ApiHelper
    {
        public static List<RequestParameterEntity> GetBaseParameters(string apiMethod, string sessionKey)
        {
            List<RequestParameterEntity> baseParameters = new List<RequestParameterEntity>();
            baseParameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            baseParameters.Add(new RequestParameterEntity("v", "1.0"));
            baseParameters.Add(new RequestParameterEntity("call_id", GenerateTime()));
            baseParameters.Add(new RequestParameterEntity("method", apiMethod));
            baseParameters.Add(new RequestParameterEntity("gz", "compression"));
            if (!String.IsNullOrEmpty(sessionKey))
            {
                baseParameters.Add(new RequestParameterEntity("session_key", sessionKey));
            }
            return baseParameters;
        }

        public async static Task<List<RequestParameterEntity>> GetBaseParameters(string sessionKey)
        {
            List<RequestParameterEntity> baseParameters = new List<RequestParameterEntity>();
            //baseParameters.Add(new RequestParameterEntity("api_key", ConstantValue.ApiKey));
            //baseParameters.Add(new RequestParameterEntity("v", "1.0"));

            IDeviceInfoHelper deviceHelper = new DeviceInfoAdaptor(DeviceInfoAdaptor.ImplType.NETWORKADAPTOR);
            var clientInfo = await deviceHelper.GetClientInfo();
            baseParameters.Add(new RequestParameterEntity("client_info", clientInfo));
            baseParameters.Add(new RequestParameterEntity("gz", "compression"));
            //if (!String.IsNullOrEmpty(sessionKey))
            //{
            //    baseParameters.Add(new RequestParameterEntity("session_key", sessionKey));
            //}
            return baseParameters;
        }

        public static string GenerateSig(List<RequestParameterEntity> Parameters, string key)
        {
            StringBuilder sb = new StringBuilder();

            Parameters.Sort(new ParameterComparer());
            foreach (var requestParameter in Parameters)
            {
                sb.Append(string.Format("{0}={1}", requestParameter.Name, requestParameter.Values.Length < 50 ? requestParameter.Values : requestParameter.Values.Substring(0, 50)));
            }
            sb.Append(key);

            return ComputeMD5(sb.ToString());
        }

        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public static string GenerateTime()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        //取大头像
        public static string GetLargeHeaderUrl(int uid, int width)
        {
            Random r = new Random();
            int index = r.Next(5000);
            return "http://ic.m.renren.com/gn?op=resize&w=" + width.ToString() + "&p=" + uid.ToString() + "-L&a=" + index.ToString();
        }

        //取大头像
        public static string GetLargeHeaderUrl(int uid, int width, int height)
        {
            Random r = new Random();
            int index = r.Next(5000);
            return "http://ic.m.renren.com/gn?op=resize&w=" + width.ToString() + "&h=" + height.ToString() + "&p=" + uid.ToString() + "-L&a=" + index.ToString();
        }

        public async static Task<IRandomAccessStream> ScaleImage2Fit(IRandomAccessStream source, int maxWidth)
        {
            InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream();

            // Decode the image
            BitmapDecoder imageDecoder = await BitmapDecoder.CreateAsync(source);

            // Re-encode the image at maxWidth
            if (imageDecoder.OrientedPixelWidth > maxWidth)
            {
                double rat = maxWidth / imageDecoder.OrientedPixelWidth;
                BitmapEncoder imageEncoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, imageDecoder);
                imageEncoder.BitmapTransform.ScaledWidth = (uint)(imageDecoder.OrientedPixelWidth * rat);
                imageEncoder.BitmapTransform.ScaledHeight = (uint)(imageDecoder.OrientedPixelHeight * rat);
                await imageEncoder.FlushAsync();

                return inMemoryStream;
            }
            else
            {
                return source;
            }
        }

        public async static Task<bool> IsUploadQualifiedImage(StorageFile image)
        {
            bool result = false;
            try
            {
                var basicProperties = await image.GetBasicPropertiesAsync();
                var size = basicProperties.Size;
                if (!(size > 0 && size < 3 * 1024 * 1024))
                {
                    return false; // none need the follow step, return right now.
                }

                result = await IsWellFormatImage(image);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public async static Task<bool> IsWellFormatImage(StorageFile image)
        {
            bool result = false;
            try
            {
                var imgProperties = await image.Properties.GetImagePropertiesAsync();
                if (imgProperties.Width > 0 && imgProperties.Height > 0)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public async static Task<ArgType> CheckUploadImageFile<ArgType>(StorageFile file)
        {
            ArgType result = default(ArgType);
            bool qualified = await ApiHelper.IsUploadQualifiedImage(file);
            if (false == qualified)
            {
                // try to use the localized error string
                string error = string.Empty;
                try
                {
                    error = new ResourceLoader().GetString("UploadPhotoFileCheckErrorMessage");
                }
                catch (Exception)
                {
                    error = "图片参数错误，支持JPG、JPEG、GIF和PNG文件，图片最大支持8MB";
                }

                var errEntity = new ErrorEntity() { Error_msg = error, Error_code = 0, Error_Type = RemoteErrorMsgTranslator.ErrorType.AlbumRelevantError };
                result = (ArgType)Activator.CreateInstance(typeof(ArgType), errEntity);
            }

            return result;
        }
    }
}
