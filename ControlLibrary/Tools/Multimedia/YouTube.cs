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
        public static async Task<YouTubeUri> GetVideoUriAsync(string youTubeId, YouTubeQuality maxQuality = YouTubeQuality.Quality360P_MP4)
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
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");               
                var response = await client.GetAsync("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");

                var task = new TaskCompletionSource<YouTubeUri>();
                OnHtmlDownloaded(await response.Content.ReadAsStringAsync(), YouTubeQuality.QualityMP3_FLV_22KHZ, maxQuality, (u, e) =>
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

        
        public static async Task<List<YouTubeUri>> GetVideoAllUrisAsync(string youTubeId, YouTubeFormat youTubeFormat = YouTubeFormat.All)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)");
                var response = await client.GetAsync("https://www.youtube.com/watch?v=" + youTubeId + "&nomobile=1");

                var task = new TaskCompletionSource<List<YouTubeUri>>();
                OnHtmlParse(await response.Content.ReadAsStringAsync(), (u, e) =>
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

        private static void OnHtmlParse(string response, Action<List<YouTubeUri>, Exception> completed)
        {
            var urls = new List<YouTubeUri>();
            try
            {
                var match = Regex.Match(response, "url_encoded_fmt_stream_map\": \"(.*?)\"");
                var data = Uri.UnescapeDataString(match.Groups[1].Value);

                var arr = Regex.Split(data, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
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
                                //else if (key == "type" && value.Contains("video/mp4")) //只获取Mp4
                                else if (key == "type") //获取全部
                                    tuple.Type = value;
                                else if (key == "s")
                                {
                                    //signature = await DecryptWebSignature(value);
                                    signature = DecryptLocalSignature(value);
                                }
                                else if (key == "sig")
                                    signature = value;
                            }
                            catch { }
                        }
                    }

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
        private static async void OnHtmlDownloaded(string response, YouTubeQuality minQuality, YouTubeQuality maxQuality, Action<YouTubeUri, Exception> completed)
        {
            var urls = new List<YouTubeUri>();
            try
            {
                var match = Regex.Match(response, "url_encoded_fmt_stream_map\": \"(.*?)\"");
                var data = Uri.UnescapeDataString(match.Groups[1].Value);

                var arr = Regex.Split(data, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)"); // split by comma but outside quotes
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
                                //else if (key == "type" && value.Contains("video/mp4")) //只获取Mp4
                                else if (key == "type") //获取全部
                                    tuple.Type = value;
                                else if (key == "s")
                                {
                                    //signature = await DecryptWebSignature(value);
                                    signature = DecryptLocalSignature(value);
                                }
                                else if (key == "sig")
                                    signature = value;
                            }
                            catch { }
                        }
                    }

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

        //
        //签名验证算法(外网)
        public async static Task<string> DecryptWebSignature(string sig)
        {
            using (var getHc = new HttpClient())
            {
                sig = await getHc.GetStringAsync("http://vevo.ytdplus.com/index.php?sign=" + sig);
                return sig;
            }
        }

        //签名验证算法(本地)
        public static string DecryptLocalSignature(string sig)
        {
            switch (sig.Length)
            {
                case 79:
                    {
                        sig = Sub(54, sig) + SubR(77, 54, sig) + Sub(39, sig) + SubR(53, 39, sig) + Sub(78, sig) + SubR(38, 34, sig) + Sub(0, sig) + SubR(33, 29, sig) + Sub(34, sig) + SubR(28, 9, sig) + Sub(29, sig) + SubR(8, 0, sig) + Sub(9, sig);
                    }
                    break;
                case 80:
                    {
                        sig = Sub(1, 19, sig) + Sub(0, sig) + Sub(20, 68, sig) + Sub(19, sig) + Sub(69, 80, sig);
                    }
                    break;
                case 81:
                    {
                        sig = Sub(56, sig) + SubR(79, 56, sig) + Sub(41, sig) + SubR(55, 41, sig) + Sub(80, sig) + SubR(40, 34, sig) + Sub(0, sig) + SubR(33, 29, sig) + Sub(34, sig) + SubR(28, 9, sig) + Sub(29, sig) + SubR(8, 0, sig) + Sub(9, sig);
                    }
                    break;
                case 82:
                    {
                        //sig = Sub(12, sig) + SubR(79, 12, sig) + Sub(80, sig) + sig.Substring(0, 12).Reverse();
                        sig = SubR(80, 37, sig) + Sub(7, sig) + SubR(36, 7, sig) + Sub(0, sig) + SubR(6, 0, sig) + Sub(37, sig);
                    }
                    break;
                case 83:
                    {
                        //sig = sig.Substring(64, 17).Reverse() + sig.Substring(0, 1) + sig.Substring(1, 62).Reverse() + sig.Substring(63, 1);
                        sig = SubR(80, 63, sig) + Sub(0, sig) + SubR(62, 0, sig) + Sub(63, sig);
                    }
                    break;
                case 84:
                    {
                        sig = SubR(78, 70, sig) + Sub(14, sig) + SubR(69, 37, sig) + Sub(70, sig) + SubR(36, 14, sig) + Sub(80, sig) + sig.Substring(0, 14).Reverse();
                    }
                    break;
                case 85:
                    {
                        sig = Sub(3, 11, sig) + Sub(0, sig) + Sub(12, 55, sig) + Sub(84, sig) + Sub(56, 84, sig);
                    }
                    break;
                case 86:
                    {
                        sig = SubR(80, 72, sig) + Sub(16, sig) + SubR(71, 39, sig) + Sub(72, sig) + SubR(38, 16, sig) + Sub(82, sig) + sig.Substring(0, 16).Reverse();
                    }
                    break;
                case 87:
                    {
                        sig = Sub(6, 27, sig) + Sub(4, sig) + Sub(28, 39, sig) + Sub(27, sig) + Sub(40, 59, sig) + Sub(2, sig) + sig.Substring(60);
                    }
                    break;
                case 88:
                    {
                        sig = Sub(7, 28, sig) + Sub(87, sig) + Sub(29, 45, sig) + Sub(55, sig) + Sub(46, 55, sig) + Sub(2, sig) + Sub(56, 87, sig) + Sub(28, sig);
                    }
                    break;
                case 89:
                    {
                        sig = SubR(84, 78, sig) + Sub(87, sig) + SubR(77, 60, sig) + Sub(0, sig) + SubR(59, 3, sig);
                    }
                    break;
                case 90:
                    {
                        sig = Sub(25, sig) + Sub(3, 25, sig) + Sub(2, sig) + Sub(26, 40, sig) + Sub(77, sig) + Sub(41, 77, sig) + Sub(89, sig) + Sub(78, 81, sig);
                    }
                    break;
                case 91:
                    {
                        sig = SubR(84, 27, sig) + Sub(86, sig) + SubR(26, 5, sig);
                    }
                    break;
                case 92:
                    {
                        sig = Sub(25, sig) + Sub(3, 25, sig) + Sub(0, sig) + Sub(26, 42, sig) + Sub(79, sig) + Sub(43, 79, sig) + Sub(91, sig) + Sub(80, 83, sig);
                    }
                    break;
                case 93:
                    {
                        sig = SubR(86, 29, sig) + Sub(88, sig) + SubR(28, 5, sig);
                    }
                    break;
            }
            return sig;
        }

        private static string SubR(int a, int b, string c)
        {
            return c.Substring(b + 1, a - b).Reverse();
        }

        private static string Sub(int a, int b, string c)
        {
            return c.Substring(a, Math.Abs(b - a));
        }

        private static string Sub(int a, string c)
        {
            return c.Substring(a, 1);
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
