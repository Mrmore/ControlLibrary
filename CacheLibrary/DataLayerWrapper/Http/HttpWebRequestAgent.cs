using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Windows.Storage;
using System.IO.Compression;
using DataLayerWrapper;
using System.Diagnostics;

namespace DataLayerWrapper.Http
{
    public class HttpWebRequestAgent : IRenRenHttpAgent
    {
        private static HttpClient getHc = null;
        private static HttpClient postHc = null;

        #region Members

        /// <summary>
        /// Post data query string.
        /// </summary>
        Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public string Method;
        #endregion

        #region PublicMethod
        public void AddParameters(string Name, string Value)
        {
            _parameters.Add(Name, Value);
        }

        public void RemoveParameters(string Name)
        {
            if (_parameters.ContainsKey(Name))
            {
                _parameters.Remove(Name);
            }
        }

        public void ClearParameters()
        {
            _parameters.Clear();
        }

        string result = string.Empty;
        public async Task<string> DownloadString(Uri uri, StorageFile file = null)
        {
            if (Method == null)
                throw new ArgumentNullException("Method NULL");
            else if (Method.ToUpper().Trim() == "GET")
            {
                result = await Get(uri);
            }
            else if (Method.ToUpper().Trim() == "POST")
            {
                await Post(uri);
            }

            return result;
        }
        #endregion

        #region WebRequest
        private async Task<string> Get(Uri uri, StorageFile file = null)
        {
            string requestUri = uri + "?" + GenerateParameterString();

            if (getHc == null)
            {
                getHc = new HttpClient();
            }

            HttpResponseMessage result = await getHc.GetAsync(new Uri(requestUri));

            return result.Content.ReadAsStringAsync().Result;
        }

        private async Task Post(Uri uri)
        {
            if (postHc == null)
            {
                postHc = new HttpClient(new HttpClientHandler() { MaxRequestContentBufferSize = 5184000 + 5184000});
                postHc.MaxResponseContentBufferSize = 5184000 + 5184000;
            }

            // You should not try anything here, since the outside arg service will handle it
            {
                string parameters = GenerateParameterString();
                var content = new StringContent(parameters);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                HttpResponseMessage response = await postHc.PostAsync(uri, content);

                //using (Stream responstStream = response.Content.ReadAsStreamAsync().Result)
                //{
                //    using (StreamReader reader = new StreamReader(new GZipStream(responstStream, CompressionMode.Decompress)))
                //    {
                //        result = reader.ReadToEnd();
                //    }
                //}
                try
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //using (Stream responstStream = response.Content.ReadAsStreamAsync().Result)
                        //{
                        //    using (StreamReader reader = new StreamReader(new GZipStream(responstStream, CompressionMode.Decompress)))
                        //    {
                        //        result = reader.ReadToEnd();
                        //    }
                        //}
                        using (Stream stream = IsCompressed(response)
                                  ? new GZipStream(response.Content.ReadAsStreamAsync().Result, CompressionMode.Decompress)
                               : response.Content.ReadAsStreamAsync().Result
                               )
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("HTTP Error & status code: " + response.StatusCode);
                        result = "{\"error_code\":105, \"error_msg\":\"系统服务繁忙，请稍后再试\"}";
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Decompress Error: " + ex.Message);
                    result = "{\"error_code\":105, \"error_msg\":\"系统服务繁忙，请稍后再试\"}";
                }
                //result = response.Content.ReadAsStringAsync().Result;
            }
        }
        #endregion

        #region PrivateMethod

        private bool IsCompressed(HttpResponseMessage response)
        {
            var mediaType = response.Content.Headers.ContentType.MediaType;
            return System.Text.RegularExpressions.Regex.IsMatch((mediaType ?? "")
                                     .ToLower(), "(application/json-gz)");
        }

        private string GenerateParameterString()
        {
            StringBuilder parameters = new StringBuilder();
            foreach (var parameter in _parameters)
            {
                parameters.Append(String.Format("{0}={1}&", parameter.Key, parameter.Value));
            }
            string resutl = parameters.ToString();
            return resutl.Substring(0, resutl.Length - 1);
        }

        #endregion

        public void Dispose()
        {
            if (getHc != null)
            {
                getHc.Dispose();
            }
            if (postHc != null)
            {
                postHc.Dispose();
            }
        }
    }
}
