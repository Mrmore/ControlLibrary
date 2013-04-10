using System;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Threading;
using Windows.Foundation.Collections;
using RenrenCoreWrapper.Entities;

namespace RenrenCore.RRServices.Login
{
    public class UserInfoManager
    {
        private ObservableCollection<LoginUserInfo> _userInfoList = new ObservableCollection<LoginUserInfo>();
        private string _USER_INFO_LIST_KEY = "USER_INFO_LIST_KEY";
        IPropertySet _DataSet = ApplicationData.Current.LocalSettings.Values;

        private static UserInfoManager _instance = new UserInfoManager();
        public static UserInfoManager Instance { get { return _instance; } }

        /// <summary>
        /// Notice!!!!!
        /// Hi man, you should always call this method before use this class
        /// </summary>
        /// <returns></returns>
        public async Task Init()
        {
            await Restore();
        }

        public void Reset()
        {
            this._userInfoList.Clear();
            _DataSet.Remove(_USER_INFO_LIST_KEY);
        }

        public async Task<ObservableCollection<LoginUserInfo>> UserList()
        {
            await Restore();
            return this._userInfoList;
        }

        private async Task Restore()
        {
            _userInfoList.Clear();
            try
            {
                if (_DataSet.ContainsKey(_USER_INFO_LIST_KEY))
                {
                    string infoList = (string)_DataSet[_USER_INFO_LIST_KEY];
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(infoList)))
                    {
                        DataContractSerializer deserializer = new DataContractSerializer(typeof(ObservableCollection<LoginUserInfo>));
                        _userInfoList = (ObservableCollection<LoginUserInfo>)deserializer.ReadObject(stream);
                    }
                }
            }
            catch (Exception)
            { }
        }

        public async void AddOneUser(LoginUserInfo userInfo)
        {
            _userInfoList.Add(userInfo);
            await SaveData();
        }

        public async Task<bool> AddUsers(ObservableCollection<LoginUserInfo> users)
        {
            try
            {
                Reset();
                foreach (var item in users)
                {
                    _userInfoList.Add(item);
                }

                await SaveData();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private async Task SaveData()
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<LoginUserInfo>));
                    serializer.WriteObject(stream, this._userInfoList);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        _DataSet[_USER_INFO_LIST_KEY] = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
