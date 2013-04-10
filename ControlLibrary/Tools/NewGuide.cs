using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ControlLibrary.Tools
{
    public class NewGuide
    {
        private static bool isNewGuide = false;
        public static bool IsNewGuide
        {
            get
            {
                return isNewGuide;
            }
            set
            {
                isNewGuide = value;
            }
        }

        private const string FileKey = "FileKey";
        //private static StorageFile storageFile = null;
        //private static StorageFolder storageFolder = null;
        private static string folderName = @"Xml\xmlNewGuide";
        private static string fileName = "NewGuide.xml";
        private static string xmlDocumentRoot = "NewGuide";
        private static string filePath = @"Xml\xmlNewGuide\NewGuide.xml";//System.IO.Path.Combine(folderName, fileName);

        /// <summary>
        /// 系统级沙盒初始化
        /// </summary>
        public static void ApplicationDataInit()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            try
            {
                if (localSettings.Values.ContainsKey(FileKey))
                {

                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 系统级沙盒重置
        /// </summary>
        /// <returns></returns>
        public static void ApplicationDataReset()
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey(FileKey))
                {
                    localSettings.Values.Remove(FileKey);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 选取Xml文件
        /// </summary>
        public static async Task<XmlDocument> LoadXmlFile(string folder, string file)
        {
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folder);
            StorageFile storageFile = await storageFolder.GetFileAsync(file);
            XmlLoadSettings loadSettings = new XmlLoadSettings();
            loadSettings.ProhibitDtd = false;
            loadSettings.ResolveExternals = false;
            return await XmlDocument.LoadFromFileAsync(storageFile, loadSettings);
        }

        /// <summary>
        /// 创建Xml文件
        /// </summary>
        public static async void CreateXmlFile()
        {
            try
            {
                //验证是不是有NewGuideXml这个文件
                //StorageFolder storageFolderHave = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.FailIfExists);
                StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.FailIfExists);
                StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.FailIfExists);

                XmlDocument doc = new XmlDocument();
                XmlElement xdRoot = doc.CreateElement(xmlDocumentRoot);
                XmlElement xmlElement = doc.CreateElement("IsNewGuide");
                xmlElement.InnerText = "false";
                xdRoot.AppendChild(xmlElement);
                doc.AppendChild(xdRoot);
                await doc.SaveToFileAsync(storageFile);

                /*var xpath = "/NewGuide";
                //选择节点的值
                var aAttributes = doc.SelectNodes(xpath);
                for (int i = 0; i < 10; i++)
                {
                    //修改节点的属性
                    XmlElement e11 = doc.CreateElement("IsNewGuide");
                    e11.InnerText = i.ToString();
                    aAttributes[0].AppendChild(e11);
                }
                //保存文件，此时修改和保存文件的路径是系统的安装路径。
                doc.SaveToFileAsync(file);*/
                String xmlStr = doc.GetXml();
                Debug.WriteLine(xmlStr);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 查找是不是看过
        /// </summary>
        /// <returns>默认为False没看过，True为看过，False为米有看过</returns>
        public static async Task<bool> SelectXmlFile()
        {
            var xpath = "/NewGuide";
            XmlDocument doc = await LoadXmlFile(folderName, fileName);
            var nodeList = doc.SelectNodes(xpath);
            return System.Convert.ToBoolean(nodeList[0].SelectSingleNode("IsNewGuide").InnerText);
        }

        public static void EditXmlFile()
        {
            EditXml(true);
        }

        private static async void EditXml(bool isNewGuide = false)
        {
            var xpath = "/NewGuide";
            XmlDocument doc = await LoadXmlFile(folderName, fileName);
            var nodeList = doc.SelectNodes(xpath);
            nodeList[0].SelectSingleNode("IsNewGuide").InnerText = System.Convert.ToString(isNewGuide);
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            StorageFile storageFile = await storageFolder.GetFileAsync(fileName);
            await doc.SaveToFileAsync(storageFile);
        }

        public static async void Reset()
        {
            StorageFolder storageFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            StorageFile storageFile = await storageFolder.GetFileAsync(fileName);
            await storageFile.DeleteAsync();
            await storageFolder.DeleteAsync();
        }

        public static void ResetEditXmlFile()
        {
            EditXml();
        }
    }
}
