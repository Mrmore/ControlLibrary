using ControlLibrary.Tools.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ControlLibrary.Extensions;

#if WP7 || WP8
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using MyToolkit.Paging;
using MyToolkit.UI;
#endif
#if WINRT
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Diagnostics;
#endif

namespace ControlLibrary.Tools.Multimedia
{
    public static class YouTube
    {

#if WPF || WP8 || WP7 || SL5
		public static Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality360P_MP4)
		{
			var task = new TaskCompletionSource<YouTubeUri>();
			GetVideoUri(youTubeId, maxQuality, (u, e) =>
			{
				if (u != null)
					task.SetResult(u);
				else if (e == null)
                    task.SetResult(u);
					//task.SetCanceled();
				else
					task.SetException(e);
			});
			return task.Task;
		}
#endif

#if WINRT
        public static async Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality360P_MP4, DecryptSignatureMode decryptSignatureMode = DecryptSignatureMode.DecryptLocalSignature)
        {
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.UseDefaultCredentials = true;
            //handler.AllowAutoRedirect = true;
            //handler.CookieContainer = new CookieContainer();
            //using (var client = new HttpClient(handler))
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                //client.DefaultRequestHeaders.Add("Accept", "text/html");
                client.DefaultRequestHeaders.Add("User-Agent", BotUserAgent);
                var response = await client.GetAsync("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");

                var task = new TaskCompletionSource<YouTubeUri>();
                OnHtmlDownloaded(await response.Content.ReadAsStringAsync(), YouTubeQuality.QualityMP3_FLV_22KHZ, maxQuality, decryptSignatureMode, (u, e) =>
                {
                    if (u != null)
                        task.SetResult(u);
                    else if (e == null)
                        task.SetResult(u);
                    //task.SetCanceled();
                    else
                        task.SetException(e);
                });
                return await task.Task;
            }
        }

        public static string GetYouTubeUrl(string txtUrl)
        {     
            string url = txtUrl.Trim();

            if (url.StartsWith("https://")) url = "http://" + url.Substring(8);
            else if (!url.StartsWith("http://")) url = "http://" + url;

            url = url.Replace("youtu.be/", "youtube.com/watch?v=");
            url = url.Replace("www.youtube.com", "youtube.com");

            if (url.StartsWith("http://youtube.com/v/"))
                url = url.Replace("youtube.com/v/", "youtube.com/watch?v=");
            else if (url.StartsWith("http://youtube.com/watch#"))
                url = url.Replace("youtube.com/watch#", "youtube.com/watch?");

            if (!url.StartsWith("http://youtube.com/watch"))
            {
                return string.Empty;
            }
            return url;
        }

        public static string GetYouTubeId(string txtUrl)
        {
            string url = txtUrl.Trim();

            if (url.StartsWith("https://")) url = "http://" + url.Substring(8);
            else if (!url.StartsWith("http://")) url = "http://" + url;

            url = url.Replace("youtu.be/", "youtube.com/watch?v=");
            url = url.Replace("www.youtube.com", "youtube.com");

            if (url.StartsWith("http://youtube.com/watch#"))
            {
                url = url.Replace("youtube.com/watch#", "youtube.com/watch?");
            }

            if (url.StartsWith("http://youtube.com/watch?"))
            {
                url = url.Replace("http://youtube.com/watch?v=", "").TrimStart();
            }
            return url;
        }

        public static string FormatCodeToQuality(int formatCode, bool audio = false)
        {
            string formatDescription = "";

            switch (formatCode)
            {
                #region Webm
                case 43:
                    formatDescription = (audio) ? "" : "WebM Video 360p (*.webm)|*.webm|";
                    break;

                case 44:
                    formatDescription = (audio) ? "" : "WebM Video 480p (*.webm)|*.webm|";
                    break;

                case 45:
                    formatDescription = (audio) ? "" : "WebM HD Video 720p (*.webm)|*.webm|";
                    break;

                case 46:
                    formatDescription = (audio) ? "" : "WebM Full HD Video 1080p (*.webm)|*.webm|";
                    break;
                #endregion

                #region Mp4
                case 37:
                    formatDescription = (audio) ? "" : "Full HD 1080p (*.mp4)|*.mp4|";
                    break;

                case 22:
                    formatDescription = (audio) ? "" : "HD 720p (*.mp4)|*.mp4|";
                    break;

                case 18:
                    formatDescription = (audio) ? "" : "Standard Youtube Qualty 360p (*.mp4)|*.mp4|";
                    break;

                case 38:
                    formatDescription = (audio) ? "" : "4K Resolution (*.mp4)|*.mp4|";
                    break;

                case 82:
                    formatDescription = (audio) ? "" : "3D Standard Youtube Qualty 360p (*.mp4)|*.mp4|";
                    break;

                case 84:
                    formatDescription = (audio) ? "" : "3D HD 720p (*.mp4)|*.mp4|";
                    break;
                #endregion

                #region Flv
                case 35:
                    formatDescription = (audio) ? "[HQ] Advanced Audio Coding [44KHz] (*.aac)|*.aac|" : "HQ Flash Video 480p (*.flv)|*.flv|";
                    break;

                case 34:
                    formatDescription = (audio) ? "[HQ] Advanced Audio Coding [22KHz] (*.aac)|*.aac|" : "LQ Flash Video 360p [AAC] (*.flv)|*.flv|";
                    break;

                case 6:
                    formatDescription = (audio) ? "MP3 Audio [44KHz] (*.mp3)|*.mp3|" : "LQ Flash Video [MP3.44KHz] (*.flv)|*.flv|";
                    break;

                case 5:
                    formatDescription = (audio) ? "MP3 Audio [22KHz] (*.mp3)|*.mp3|" : "LQ Flash Video [MP3.22KHz] (*.flv)|*.flv|";
                    break;
                #endregion

                #region 3gp
                case 13:
                    formatDescription = (audio) ? "" : "Mobile Video XX-Small (*.3gp)|*.3gp|";
                    break;

                case 17:
                    formatDescription = (audio) ? "" : "Mobile Video X-Small (*.3gp)|*.3gp|";
                    break;

                case 36:
                    formatDescription = (audio) ? "" : "Mobile Video Small (*.3gp)|*.3gp|";
                    break;
                #endregion

                default:
                    // New Format?
                    formatDescription = "";
                    break;
            }

            return formatDescription;
        }

        private static List<int> FormatCodeFlvOrMp3 = new List<int>() { 35, 34, 5, 6 };
        private static List<int> FormatCodeMp4 = new List<int>() { 18, 22, 37 };
        private static List<int> FormatCodeQuality = new List<int>() { 36, 35, 34, 5, 6, 18, 22, 37 };
        private static List<int> FormatCodeMP4OrFlv_mp3 = new List<int>() { 18, 22, 37, 34, 5 };


        public static async Task<List<YouTubeUri>> GetVideoAllUrisAsync(string youTubeId, YouTubeFormat youTubeFormat = YouTubeFormat.All, DecryptSignatureMode decryptSignatureMode = DecryptSignatureMode.DecryptLocalSignature)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", BotUserAgent);
                var response = await client.GetAsync("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");

                var task = new TaskCompletionSource<List<YouTubeUri>>();
                OnHtmlParse(await response.Content.ReadAsStringAsync(), decryptSignatureMode, (u, e) =>
                {
                    if (u != null)
                    {
                        List<YouTubeUri> result = null;
                        if (youTubeFormat == YouTubeFormat.All)
                        {
                            result = u;
                        }
                        else if (youTubeFormat == YouTubeFormat.Flv || youTubeFormat == YouTubeFormat.Mp3)
                        {
                            //List<int> result = u.Select(uAll => uAll.Itag).Concat(FormatCodeFlvOrMp3).ToList();

                            result = (from uAll in u
                                      from flvOrMp3 in FormatCodeFlvOrMp3
                                      where uAll.Itag == flvOrMp3
                                      select uAll).ToList();
                        }
                        else if (youTubeFormat == YouTubeFormat.Mp4)
                        {
                            result = (from uAll in u
                                      from mp4 in FormatCodeMp4
                                      where uAll.Itag == mp4
                                      select uAll).ToList();
                        }
                        else if (youTubeFormat == YouTubeFormat.MP4OrFlv_mp3)
                        {
                            result = (from uAll in u
                                      from mP4OrFlv_mp3 in FormatCodeMP4OrFlv_mp3
                                      where uAll.Itag == mP4OrFlv_mp3
                                      select uAll).ToList();
                        }
                        else
                        {
                            result = (from uAll in u
                                      from quality in FormatCodeQuality
                                      where uAll.Itag == quality
                                      select uAll).ToList();
                        }
                        task.SetResult(result);
                    }
                    else if (e == null)
                    {
                        //task.SetCanceled();

                        task.SetResult(u);
                    }
                    else
                        task.SetException(e);
                });
                return await task.Task;
            }
        }

        private static async void OnHtmlParse(string response, DecryptSignatureMode decryptSignatureMode, Action<List<YouTubeUri>, Exception> completed)
        {
            var urls = new List<YouTubeUri>();
            if (sigCodes != null && sigCodes.Count > 0)
                sigCodes.Clear();
            javaScriptCode = string.Empty;
            try
            {
                var match = Regex.Match(response, "url_encoded_fmt_stream_map\": \"(.*?)\"");
                var data = Uri.UnescapeDataString(match.Groups[1].Value);
                match = Regex.Match(response, "adaptive_fmts\": \"(.*?)\"");
                var data2 = Uri.UnescapeDataString(match.Groups[1].Value);

                var arr = Regex.Split(data + "," + data2, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
                foreach (var d in arr)
                {
                    var url = "";
                    var signature = "";
                    var tuple = new YouTubeUri();
                    foreach (var p in d.Replace("\\u0026", "\t").Split('\t'))
                    {
                        var index = p.IndexOf('=');
                        if (index != -1 && index < p.Length)
                        {
                            try
                            {
                                var key = p.Substring(0, index);
                                var value = Uri.UnescapeDataString(p.Substring(index + 1));
                                if (key == "url")
                                    url = value;
                                else if (key == "itag")
                                    tuple.Itag = int.Parse(value);
                                //else if (key == "type" && (value.Contains("video/mp4") || value.Contains("audio/mp4"))) //只获取Mp4
                                else if (key == "type") //获取全部
                                    tuple.Type = value;
                                else if (key == "s")
                                {
                                    if (decryptSignatureMode == DecryptSignatureMode.DecryptLocalSignature)
                                        signature = await DecryptLocalSignature(value, response);
                                    else
                                        signature = await DecryptWebSignature(value);
                                }
                                else if (key == "sig")
                                    signature = value;
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine("YouTube parse exception: " + exception.Message);
                            }
                        }
                    }

                    if (url.Contains("&signature=") || url.Contains("?signature="))
                        tuple.url = url;
                    else
                        tuple.url = url + "&signature=" + signature;

                    if (tuple.IsValid)
                        urls.Add(tuple);
                }
            }
            catch (Exception ex)
            {
                if (completed != null)
                    completed(null, ex);
                return;
            }

            //降序排列
            var entry = urls.OrderByDescending(u => u.Itag).ToList();
            if (entry != null)
            {
                if (completed != null)
                    completed(entry, null);
            }
            else if (completed != null)
                completed(null, new Exception("no_video_urls_found"));
        }

#else
        public static HttpResponse GetVideoUri(string youTubeId, YouTubeQuality maxQuality, Action<YouTubeUri, Exception> completed)
        {
            return GetVideoUri(youTubeId, YouTubeQuality.QualityMP3_FLV_22KHZ, maxQuality, completed);
        }

        public static HttpResponse GetVideoUri(string youTubeId, YouTubeQuality minQuality, YouTubeQuality maxQuality,
            Action<YouTubeUri, Exception> completed)
        {
            var request = new HttpGetRequest("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");
			request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
            return Http.Get(request,r => 
            {
                if (r.Successful)
                    OnHtmlDownloaded(r.Response, minQuality, maxQuality, completed));
                else if (completed != null)
                    completed(null, r.Exception);
            }
        }
#endif
        private static async void OnHtmlDownloaded(string response, YouTubeQuality minQuality, YouTubeQuality maxQuality, DecryptSignatureMode decryptSignatureMode, Action<YouTubeUri, Exception> completed)
        {
            var urls = new List<YouTubeUri>();
            if (sigCodes != null && sigCodes.Count > 0)
                sigCodes.Clear();
            javaScriptCode = string.Empty;
            try
            {             
                var match = Regex.Match(response, "url_encoded_fmt_stream_map\": \"(.*?)\"");
                var data = Uri.UnescapeDataString(match.Groups[1].Value);
                match = Regex.Match(response, "adaptive_fmts\": \"(.*?)\"");
                var data2 = Uri.UnescapeDataString(match.Groups[1].Value);

                var arr = Regex.Split(data + "," + data2, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
                foreach (var d in arr)
                {
                    var url = "";
                    var signature = "";
                    var tuple = new YouTubeUri();
                    foreach (var p in d.Replace("\\u0026", "\t").Split('\t'))
                    {
                        var index = p.IndexOf('=');
                        if (index != -1 && index < p.Length)
                        {
                            try
                            {
                                var key = p.Substring(0, index);
                                var value = Uri.UnescapeDataString(p.Substring(index + 1));
                                if (key == "url")
                                    url = value;
                                else if (key == "itag")
                                    tuple.Itag = int.Parse(value);
                                //else if (key == "type" && (value.Contains("video/mp4") || value.Contains("audio/mp4"))) //只获取Mp4
                                else if (key == "type") //获取全部
                                    tuple.Type = value;
                                else if (key == "s")
                                {
                                    if (decryptSignatureMode == DecryptSignatureMode.DecryptLocalSignature)
                                        signature = await DecryptLocalSignature(value, response);
                                    else
                                        signature = await DecryptWebSignature(value);
                                }
                                else if (key == "sig")
                                    signature = value;
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine("YouTube parse exception: " + exception.Message);
                            }
                        }
                    }
                    
                    if (url.Contains("&signature=") || url.Contains("?signature="))
                        tuple.url = url;
                    else
                        tuple.url = url + "&signature=" + signature;

                    if (tuple.IsValid)
                        urls.Add(tuple);
                }

                var minTag = GetQualityIdentifier(minQuality);
                var maxTag = GetQualityIdentifier(maxQuality);
                //foreach (var u in urls.Where(u => u.Itag < minTag || u.Itag > maxTag).ToArray()) //得到相对命中的,如果没用命中绝对项,会找到目标项[上一级或下一级(相邻)]的项
                foreach (var u in urls.Where(u => u.Itag <= minTag || u.Itag >= maxTag).ToArray()) //得到绝对命中的
                    urls.Remove(u);
            }
            catch (Exception ex)
            {
                if (completed != null)
                    completed(null, ex);
                return;
            }

            var entry = urls.OrderByDescending(u => u.Itag).FirstOrDefault();
            if (entry != null)
            {
                if (completed != null)
                    completed(entry, null);
            }
            else if (completed != null)
                //completed(null, new Exception("no_video_urls_found"));
                completed(entry, null);
        }
        
        //签名验证算法(外网)
        private static List<Tuple<string, int>> sigCodes = null;
        public async static Task<string> DecryptWebSignature(string sig)
        {
            if (sigCodes == null || sigCodes.Count <= 0)
            {
                using (var getHc = new HttpClient())
                {
                    //"http://vevo.ytdplus.com/index.php?sign=" + sig
                    //http://add0n.com/signature.php?request=alg
                    //http://add0n.com/signature2.php
                    //http://inbasic.net/signature2.php
                    getHc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:22.0) Gecko/20100101 Firefox/22.0");
                    var codes = await getHc.GetStringAsync("http://inbasic.net/signature2.php");
                    if (!string.IsNullOrEmpty(codes) && !string.IsNullOrWhiteSpace(codes))
                    {
                        //codes = codes.Trim().Replace("[", "").Replace("]", "").Replace("\"", "");
                        codes = codes.Substring(codes.Trim().IndexOf('['));
                        codes = codes.Substring(0, codes.IndexOf(']'));
                        codes = codes.Replace("[", "").Replace("]", "").Replace("\"", "");
                        var codeArray = codes.Split(',');
                        sigCodes = new List<Tuple<string, int>>();
                        for (int i = 0; i < codeArray.Length; i++)
                        {
                            if (codeArray[i] == "w" || codeArray[i] == "s")
                            {
                                var key = codeArray[i].Trim();
                                var value = System.Convert.ToInt32(codeArray[i + 1].Trim());
                                sigCodes.Add(new Tuple<string, int>(key, value));
                            }
                            else if (codeArray[i] == "r")
                            {
                                sigCodes.Add(new Tuple<string, int>(codeArray[i], 0));
                            }
                        }
                    }
                }
            }
            if (sigCodes != null && sigCodes.Count > 0)
            {
                foreach (var item in sigCodes)
                {
                    if (item.Item1 == "r")
                    {
                        sig = sig.Reverse();
                    }
                    else if (item.Item1 == "s")
                    {
                        sig = sig.Slice(item.Item2);
                    }
                    else if (item.Item1 == "w")
                    {
                        sig = new string(Swap(sig.ToCharArray(), item.Item2));
                    }
                }
            }
            return sig;
        }

        //签名验证算法(本地)
        private static string javaScriptCode = string.Empty;
        private async static Task<string> DecryptLocalSignature(string signature, string response)
        {
            if (string.IsNullOrEmpty(javaScriptCode) || string.IsNullOrWhiteSpace(javaScriptCode))
            {
                var javaScriptUri = "http://s.ytimg.com/yts/jsbin/html5player-" +
                                    Regex.Match(response,
                                        "\"\\\\/\\\\/s.ytimg.com\\\\/yts\\\\/jsbin\\\\/html5player-(.+?)\\.js\"")
                                        .Groups[1] + ".js";
                javaScriptCode = await HttpGet(javaScriptUri);
            }
            //var functionName = Regex.Match(javaScriptCode, "signature=(.*?)\\(").Groups[1].ToString();
            //var functionName = Regex.Match(javaScriptCode, "=[a-zA-Z0-9]+\\.sig\\|\\|(.*?)\\(").Groups[1].ToString();
            var functionName = Regex.Match(javaScriptCode, "=*\\.sig\\|\\|([a-zA-Z0-9$]+?)\\(").Groups[1].ToString();
            var functionMath = Regex.Match(javaScriptCode, "function " + Regex.Escape(functionName) + "\\((\\w+)\\)\\{(.+?)\\}", RegexOptions.Singleline);

            var parameterName = Regex.Escape(functionMath.Groups[1].ToString());
            //var functionBody = @"a=a.split("");$er.U8(a,45);$er.Q6(a,7);$er.Q6(a,19);$er.Q6(a,38);$er.Lr(a,3);$er.Q6(a,41);$er.Lr(a,1);$er.U8(a,13);$er.Q6(a,1);return a.join("")";
            var functionBody = functionMath.Groups[2].ToString();

            Dictionary<string, Func<string, int, string>> methods = null;

            //var lo={wS:function(a){return a.reverse()},IC:function(a,b){return a.slice(b)},rw:function(a,b){var c=a[0];a[0]=a[b%a.length];a[b]=c;return a}};
            //function mo(a){a=a.split("");a=lo.rw(a,1);a=lo.rw(a,32);a=lo.IC(a,1);a=lo.wS(a,77);a=lo.IC(a,3);a=lo.wS(a,77);a=lo.IC(a,3);a=lo.wS(a,44);return a.join("")};

            foreach (var line in functionBody.Split(';').Select(l => l.Trim()))
            {
                if (Regex.IsMatch(line, parameterName + "=" + parameterName + "\\.reverse\\(\\)")) // OLD
                    signature = Reverse(signature);
                else if (Regex.IsMatch(line, parameterName + "=" + parameterName + "\\.slice\\(\\d+\\)"))
                    signature = Slice(signature, Convert.ToInt32(Regex.Match(line, parameterName + "=" + parameterName + "\\.slice\\((\\d+)\\)").Groups[1].ToString()));
                else if (Regex.IsMatch(line, parameterName + "=\\w+\\(" + parameterName + ",\\d+\\)"))
                    signature = Swap(signature, Convert.ToInt32(Regex.Match(line, parameterName + "=\\w+\\(" + parameterName + ",(\\d+)\\)").Groups[1].ToString()));
                else if (Regex.IsMatch(line, parameterName + "\\[0\\]=" + parameterName + "\\[\\d+%" + parameterName + "\\.length\\]"))
                    signature = Swap(signature, Convert.ToInt32(Regex.Match(line, parameterName + "\\[0\\]=" + parameterName + "\\[(\\d+)%" + parameterName + "\\.length\\]").Groups[1].ToString()));
                else
                {
                    var match = Regex.Match(line, "(" + parameterName + "=)?(.*?)\\.(.*?)\\(" + parameterName + ",(.*?)\\)");
                    if (match.Success)
                    {
                        var root = match.Groups[2].ToString();
                        var method = match.Groups[3].ToString();
                        var parameter = int.Parse(match.Groups[4].ToString());

                        if (methods == null)
                        {
                            // Parse methods
                            methods = new Dictionary<string, Func<string, int, string>>();

                            if (root.Contains("$"))
                                root = root.Replace("$", "\\$");
                            //javaScriptCode = @"var $er={Lr:function(a,b){a.splice(0,b)},U8:function(a){a.reverse()},Q6:function(a,b){var c=a[0];a[0]=a[b%a.length];a[b]=c}};function fr(a){a=a.split("");$er.U8(a,45);$er.Q6(a,7);$er.Q6(a,1);";
                            var code = Regex.Match(javaScriptCode, "var " + root + "=\\{(.*?)\\};function").Groups[1].ToString();
                            var methodsArray = code.Split(new[] { "}," }, StringSplitOptions.None);
                            foreach (var m in methodsArray)
                            {
                                var arr = m.Split(':');
                                var methodName = arr[0];
                                var methodBody = arr[1];

                                if (methodBody.Contains("reverse()"))
                                    methods[methodName] = (s, i) => Reverse(s);
                                else if (methodBody.Contains(".splice(") || methodBody.Contains(".slice("))
                                    methods[methodName] = Slice;
                                else
                                    methods[methodName] = Swap;
                            }
                        }

                        signature = methods[method](signature, parameter);
                    }
                    else
                    { 
                        
                    }
                }
            }
            return signature;
        }

        private static string Reverse(string signature)
        {
            var charArray = signature.ToCharArray();
            Array.Reverse(charArray);
            signature = new string(charArray);
            return signature;
        }

        private static string Slice(string input, int length)
        {
            return input.Substring(length);
        }

        private static string Swap(string input, int position)
        {
            var str = new StringBuilder(input);
            var swapChar = str[position];
            str[position] = str[0];
            str[0] = swapChar;
            return str.ToString();
        }

        private static char[] Swap(char[] a, int b)
        {
            var c = a[0];
            a[0] = a[b % a.Length];
            a[b] = c;
            return a;
        }

        private const string BotUserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";
        private static async Task<string> HttpGet(string uri)
        {
            using (var getHc = new HttpClient())
            {
                getHc.DefaultRequestHeaders.Add("User-Agent", BotUserAgent);
                var bytes = await getHc.GetByteArrayAsync(uri);
                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
        }
        //

        public class YouTubeUri
        {
            internal string url;

            public Uri Uri { get { return new Uri(url, UriKind.Absolute); } }
            public int Itag { get; set; }
            public string Type { get; set; }

            public bool IsValid
            {
                get { return url != null && Itag > 0 && Type != null; }
            }
        }

#if WINRT || WP8 || WP7 || SL5
        public static async Task<string> GetVideoTitleAsync(string youTubeId) // should be improved
		{
			var response = await Http.GetAsync("http://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");
			if (response != null)
			{
				var html = response.Response;
				var startIndex = html.IndexOf(" title=\"");
				if (startIndex != -1)
				{
					startIndex = html.IndexOf(" title=\"", startIndex + 1);
					if (startIndex != -1)
					{
						startIndex += 8;
						var endIndex = html.IndexOf("\">", startIndex);
						if (endIndex != -1)
							return html.Substring(startIndex, endIndex - startIndex);
					}
				}
			}
			return null;
		}

#endif

        public static Uri GetThumbnailUri(string youTubeId, YouTubeThumbnailSize size = YouTubeThumbnailSize.Medium)
        {
            switch (size)
            {
                case YouTubeThumbnailSize.Small:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/default.jpg", UriKind.Absolute);
                case YouTubeThumbnailSize.Medium:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/hqdefault.jpg", UriKind.Absolute);
                case YouTubeThumbnailSize.Large:
                    return new Uri("http://img.youtube.com/vi/" + youTubeId + "/maxresdefault.jpg", UriKind.Absolute);
            }
            throw new Exception();
        }

        public static int GetQualityIdentifier(YouTubeQuality quality)
        {
            switch (quality)
            {
                //mp4
                case YouTubeQuality.Quality240P_3GP: return 36;
                case YouTubeQuality.Quality360P_MP4: return 18;
                case YouTubeQuality.Quality720P_MP4: return 22;
                case YouTubeQuality.Quality1080P_MP4: return 37;
                
                //flv
                case YouTubeQuality.Quality480P_FLV_44KHZ: return 35;
                case YouTubeQuality.Quality360P_FLV_22KHZ: return 34;

                //flv 格式为mp3
                case YouTubeQuality.QualityMP3_FLV_44KHZ: return 6;
                case YouTubeQuality.QualityMP3_FLV_22KHZ: return 5;
            }
            throw new ArgumentException("maxQuality");
        }

        #region Phone

#if WP7 || WP8

		public static HttpResponse Play(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality480P, Action<Exception> completed = null)
		{
			return GetVideoUri(youTubeId, maxQuality, (entry, e) =>
			{
				if (e != null)
			    {
					if (completed != null)
						completed(e);
			    }
				else
			    {
					if (completed != null)
						completed(null);

					if (entry != null)
					{
						 var launcher = new MediaPlayerLauncher
						 {
							 Controls = MediaPlaybackControls.All,
							 Media = entry.Uri
						 };
						 launcher.Show();
					}
			    }
			});
		}


		private static HttpResponse httpResponse;
		private static PageDeactivator oldState;

		/// <summary>
		/// This method disables the current page and shows a progress indicator until the youtube movie url has been loaded and starts
		/// </summary>
		/// <param name="youTubeId"></param>
		/// <param name="manualActivatePage">if true add YouTube.CancelPlay() in OnNavigatedTo() of the page (better)</param>
		/// <param name="maxQuality"></param>
		/// <param name="completed"></param>
		public static void Play(string youTubeId, bool manualActivatePage, YouTubeQuality maxQuality = YouTubeQuality.Quality480P, Action<Exception> completed = null)
		{
			lock (typeof(YouTube))
			{
				if (oldState != null)
					return;

				if (SystemTray.ProgressIndicator == null)
					SystemTray.ProgressIndicator = new ProgressIndicator();

				SystemTray.ProgressIndicator.IsVisible = true;
				SystemTray.ProgressIndicator.IsIndeterminate = true; 

				var page = PhonePage.CurrentPage;
				oldState = PageDeactivator.Inactivate();
				httpResponse = Play(youTubeId, YouTubeQuality.Quality480P, ex => Deployment.Current.Dispatcher.BeginInvoke(
					delegate
					{
						if (page == PhonePage.CurrentPage) // !user navigated away
						{
							if (ex == null)
								CancelPlay(manualActivatePage);
							else
								CancelPlay(false);
						}

						if (completed != null)
							completed(ex);
					}));
			}
		}

		/// <summary>
		/// call this in OnBackKeyPress() of the page: 
		/// e.Cancel = YouTube.CancelPlay();
		/// or in OnNavigatedTo() and use manualActivatePage = true
		/// </summary>
		/// <returns></returns>
		public static bool CancelPlay()
		{
			return CancelPlay(false);
		}

		private static bool CancelPlay(bool manualActivate)
		{
			lock (typeof(YouTube))
			{
				if (oldState == null && httpResponse == null)
					return false;

				if (httpResponse != null)
				{
					httpResponse.Abort();
					httpResponse = null;
				}

				if (!manualActivate && oldState != null)
				{
					oldState.Revert();
					SystemTray.ProgressIndicator.IsVisible = false;
					oldState = null;
				}

				return true;
			}
		}

#endif
        #endregion
    }
}
