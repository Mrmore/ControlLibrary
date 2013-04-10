using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace RenrenCoreWrapper.Helper
{
    /// <summary>
    /// The remote error message translate class
    /// </summary>
    public class RemoteErrorMsgTranslator
    {
        /// <summary>
        /// The singleton property
        /// </summary>
        private static RemoteErrorMsgTranslator _instance = null;
        public static RemoteErrorMsgTranslator Instance
        {
            get
            {
                lock (typeof(RemoteErrorMsgTranslator))
                {
                    if (null == _instance)
                    {
                        _instance = new RemoteErrorMsgTranslator();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Is this component ready to use, since it's initialized in background thread
        /// </summary>
        public bool Ready { get; private set; }

        /// <summary>
        /// The error translator table used to translate the error message
        /// </summary>
        public IDictionary<int, ErrorMsgEntity> ErrorTable { get { return _mesgTranslatorTable; } }

        private IDictionary<int, ErrorMsgEntity> _mesgTranslatorTable = new Dictionary<int, ErrorMsgEntity>();
        private IDictionary<string, ErrorType> _typeTranslatorTable = new Dictionary<string, ErrorType>();
        private Regex _typeRegex = new Regex(@"^##(.*?)##$", RegexOptions.IgnoreCase);
        private Regex _mesgRegex = new Regex(@"^(\d+?)=(.+?)$", RegexOptions.IgnoreCase);

        private RemoteErrorMsgTranslator()
        {
            Ready = false;
        }

        public async Task InitData()
        {
            //await Windows.System.Threading.ThreadPool.RunAsync((wiSender) =>
            //{
            //    init().Wait();
            //}, Windows.System.Threading.WorkItemPriority.Normal);
            await init();
        }

        private async Task init()
        {
            // TODO: Init the _typeTranslatorTable
            _typeTranslatorTable.Add("系统错误", ErrorType.SystemError);
            _typeTranslatorTable.Add("基础业务错误", ErrorType.BasicBussinessError);
            _typeTranslatorTable.Add("相册错误", ErrorType.AlbumRelevantError);
            _typeTranslatorTable.Add("视频错误", ErrorType.VedioRelevantError);
            _typeTranslatorTable.Add("分享错误", ErrorType.ShareRelevantError);
            _typeTranslatorTable.Add("位置业务错误", ErrorType.LocationRelevantError);
            _typeTranslatorTable.Add("好友错误", ErrorType.FriendsRelevantError);
            _typeTranslatorTable.Add("公共主页错误", ErrorType.PageRelevantError);
            _typeTranslatorTable.Add("日志错误", ErrorType.BlogRelevantError);

            // Init the _mesgTranslatorTable
            StorageFile file = null;
            try
            {
                StorageFolder storageFolder = Package.Current.InstalledLocation;
                file = await storageFolder.GetFileAsync(@"RenrenEntites\Resources\Texts\RemoteErrorTranslator.txt");
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp.Message);
                Debug.WriteLine(exp.StackTrace);
                return;
            }


            try
            {
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        ErrorType curType = ErrorType.Unkown;
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            line = line.TrimEnd().TrimStart();
                            if (string.IsNullOrEmpty(line)) { line = reader.ReadLine(); continue; } // It's a blank line

                            Debug.WriteLine(line);
                            // handle mesg section
                            var mesgMatch = _mesgRegex.Match(line);
                            if (mesgMatch.Success)
                            {
                                int value = Convert.ToInt32(mesgMatch.Groups[1].Value);
                                string mesg = mesgMatch.Groups[2].Value;
                                if (!_mesgTranslatorTable.ContainsKey(value))
                                {
                                    _mesgTranslatorTable.Add(value, new ErrorMsgEntity() { ErrorNo = value, Mesg = mesg, Type = curType });
                                }
                            }
                            else
                            {
                                // handle type section
                                var typeMatch = _typeRegex.Match(line);
                                if (typeMatch.Success)
                                {
                                    string type = typeMatch.Groups[1].Value;
                                    if (_typeTranslatorTable.ContainsKey(type))
                                    {
                                        curType = this._typeTranslatorTable[type];
                                    }
                                    else
                                    {
                                        curType = ErrorType.Unkown;
                                    }
                                }
                            }

                            line = reader.ReadLine();
                        }
                    }
                }
                // If you debug here, congrats!
                Ready = true;
            }
            catch (Exception ex)
            {
                Ready = false;
                Debug.WriteLine("###Parse error mesg error!");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }


        public enum ErrorType
        {
            SystemError, // 系统错误
            BasicBussinessError, // 基础业务错误
            AlbumRelevantError, // 相册错误
            VedioRelevantError, // 视频错误
            ShareRelevantError, // 分享错误
            LocationRelevantError, // 位置业务错误
            FriendsRelevantError, // 好友错误
            PageRelevantError, // 公共主页错误
            BlogRelevantError, // 日志错误
            Unkown
        }
        public class ErrorMsgEntity
        {
            public int ErrorNo { get; set; }
            public ErrorType Type { get; set; }
            public string Mesg { get; set; }
        }
    }
}
