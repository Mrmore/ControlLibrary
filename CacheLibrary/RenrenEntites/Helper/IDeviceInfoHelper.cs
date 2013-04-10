using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RenrenCoreWrapper.Constants;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace RenrenCoreWrapper.Helper
{
    public interface IDeviceInfoHelper
    {
        string GetDeviceID();
        Task<string> GetDeviceName();
        string GetHostName();
        Task<string> GetClientInfo();
    }

    /// <summary>
    /// The Fetch device info Adaptor
    /// </summary>
    public class DeviceInfoAdaptor : IDeviceInfoHelper
    {
        /// <summary>
        /// Define the overall implementation type
        /// </summary>
        public enum ImplType { GUID, NETWORKADAPTOR, KERNELIOCONTROL }
        /// <summary>
        /// The target implementation
        /// </summary>
        private IDeviceInfoHelper _target = null;
        /// <summary>
        /// Construct: using the implementtation type
        /// </summary>
        /// <param name="type"></param>
        public DeviceInfoAdaptor(ImplType type)
        {
            switch (type)
            {
                case ImplType.GUID:
                    _target = new DeviceInfoGuidHelperImpl();
                    break;
                case ImplType.NETWORKADAPTOR:
                    _target = new DeviceInfoＮetworkHelperImpl();
                    break;
                case ImplType.KERNELIOCONTROL:
                    _target = new DeviceInfoKernalIoControlHelperImpl();
                    break;
                default:
                    throw new ArgumentException("Implementation type error!");
            }
        }

        public string GetDeviceID()
        {
            return _target.GetDeviceID();
        }

        public async Task<string> GetDeviceName()
        {
            return await _target.GetDeviceName();
        }

        public string GetHostName()
        {
            return _target.GetHostName();
        }


        public async Task<string> GetClientInfo()
        {
            return await _target.GetClientInfo();
        }
    }

    public class DeviceInfoGuidHelperImpl : IDeviceInfoHelper
    {
        private const string _UDID = "UDID";
        public string GetDeviceID()
        {
            string udid = string.Empty;
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(_UDID))
            {
                udid = (string)localSettings.Values[_UDID];
            }
            else
            {
                udid = Guid.NewGuid().ToString();
                localSettings.Values[_UDID] = udid;
            }

            return udid;
        }

        private static string _DEVICE_NAME = "_DEVICE_NAME";
        public async Task<string> GetDeviceName()
        {
            string deviceName = string.Empty;
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(_DEVICE_NAME))
            {
                deviceName = (string)localSettings.Values[_DEVICE_NAME];
            }
            else
            {
                var selector = "System.Devices.InterfaceClassGuid:=\"" + "{53172480-4791-11D0-A5D6-28DB04C10000}" + "\"";
                //+ " AND System.Devices.InterfaceEnabled:=System.StructuredQueryType.Boolean#True";
                string[] properties = { "System.Devices.Manufacturer", "System.Devices.HardwareIds", "System.Devices.DeviceCharacteristics" };
                var interfaces = await DeviceInformation.FindAllAsync(selector);

                if (interfaces.Count > 0)
                {
                    deviceName = interfaces[0].Name;
                }

                localSettings.Values[_DEVICE_NAME] = deviceName;
            }

            return deviceName;
        }

        public string GetHostName()
        {
            int foundIdx;
            string localName = string.Empty;
            try
            {
                var names = NetworkInformation.GetHostNames();
                for (int i = 0; i < names.Count; i++)
                {
                    foundIdx = names[i].DisplayName.IndexOf(".local");
                    if (foundIdx > 0)
                    {
                        localName = names[i].DisplayName.Substring(0, foundIdx);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // If code reach here, the network would be disconnected.
                localName = string.Empty;
            }
            return localName;
        }

        private static string _CLIENT_INFO = "_CLIENT_INFO";
        public async Task<string> GetClientInfo()
        {
            string info_ = string.Empty;
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(_CLIENT_INFO))
            {
                info_ = (string)localSettings.Values[_CLIENT_INFO];
            }
            else
            {
                var deviceName = await GetDeviceName();

                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;

                string version_ = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

                StringBuilder clientInfo = new StringBuilder("");
                clientInfo.Append("{");
                clientInfo.Append("\"model\"" + ":" + "\"" + deviceName + "\",");
                clientInfo.Append("\"os\"" + ":" + "\"" + ConstantValue.OS + "\",");
                clientInfo.Append("\"screen\"" + ":" + "\"1366*768\",");
                clientInfo.Append("\"font\"" + ":" + "\"Microsoft YaHei\",");
                clientInfo.Append("\"ua\"" + ":" + "\"" + deviceName + "\"" + ",");
                clientInfo.Append("\"from\"" + ":" + "\"" + ConstantValue.ChannelId + "\"" + ",");
                clientInfo.Append("\"cellId\"" + ":" + "\"" + ApiHelper.GenerateTime() + "\"" + ",");
                clientInfo.Append("\"version\"" + ":" + "\"" + version_ + "\"");
                clientInfo.Append("}");

                info_ = clientInfo.ToString();
                localSettings.Values[_CLIENT_INFO] = info_;
            }

            return info_;
        }
    }

    public class DeviceInfoＮetworkHelperImpl : IDeviceInfoHelper
    {
        private const string _UDID = "UDID";
        public string GetDeviceID()
        {
            string udid = string.Empty;
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (localSettings.Values.ContainsKey(_UDID))
                {
                    udid = (string)localSettings.Values[_UDID];
                }
                else
                {
                    var netId = NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.NetworkAdapterId.ToString();
                    var hostName = GetHostName();
                    udid = hostName + netId;
                    localSettings.Values[_UDID] = udid;
                }
            }
            catch (Exception)  {  
                // If code reach here, the network would be disconnected.
                udid = string.Empty;
            }

            return udid;
        }

        private static string _DEVICE_NAME = "_DEVICE_NAME";
        public async Task<string> GetDeviceName()
        {
            //string deviceName = string.Empty;
            //var localSettings = ApplicationData.Current.LocalSettings;
            //if (localSettings.Values.ContainsKey(_DEVICE_NAME))
            //{
            //    deviceName = (string)localSettings.Values[_DEVICE_NAME];
            //}
            //else
            //{
            //    var selector = "System.Devices.InterfaceClassGuid:=\"" + "{53172480-4791-11D0-A5D6-28DB04C10000}" + "\"";
            //    //+ " AND System.Devices.InterfaceEnabled:=System.StructuredQueryType.Boolean#True";
            //    string[] properties = { "System.Devices.Manufacturer", "System.Devices.HardwareIds", "System.Devices.DeviceCharacteristics" };
            //    var interfaces = await DeviceInformation.FindAllAsync(selector);

            //    if (interfaces.Count > 0)
            //    {
            //        deviceName = interfaces[0].Name;
            //    }

            //    localSettings.Values[_DEVICE_NAME] = deviceName;
            //}

            return "Windows 8 tablet";
        }

        public string GetHostName()
        {
            int foundIdx;
            string localName = string.Empty;
            try
            {
                var names = NetworkInformation.GetHostNames();
                for (int i = 0; i < names.Count; i++)
                {
                    foundIdx = names[i].DisplayName.IndexOf(".local");
                    if (foundIdx > 0)
                    {
                        localName = names[i].DisplayName.Substring(0, foundIdx);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // If code reach here, the network would be disconnected.
                localName = string.Empty;
            }
            return localName;
        }

        private static string _CLIENT_INFO = "_CLIENT_INFO";
        public async Task<string> GetClientInfo()
        {
            string info_ = string.Empty;
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(_CLIENT_INFO))
            {
                info_ = (string)localSettings.Values[_CLIENT_INFO];
            }
            else
            {
                var deviceName = await GetDeviceName();
                var deviceId = GetDeviceID();

                Package package = Package.Current;
                PackageId packageId = package.Id;
                PackageVersion version = packageId.Version;
                string version_ = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

                //version_ = "0.0.1";

                StringBuilder clientInfo = new StringBuilder("");
                clientInfo.Append("{");
                clientInfo.Append("\"model\"" + ":" + "\"" + deviceName + "\",");
                clientInfo.Append("\"os\"" + ":" + "\"" + ConstantValue.OS + "\",");
                clientInfo.Append("\"screen\"" + ":" + "\"1366*768\",");
                clientInfo.Append("\"uniqid\"" + ":" + "\"" + deviceId +  "\",");
                //clientInfo.Append("\"ua\"" + ":" + "\"" + deviceName + "\"" + ",");
                clientInfo.Append("\"from\"" + ":" + "\"" + ConstantValue.ChannelId + "\"" + ",");
                //clientInfo.Append("\"cellId\"" + ":" + "\"" + ApiHelper.GenerateTime() + "\"" + ",");
                clientInfo.Append("\"version\"" + ":" + "\"" + version_ + "\"");
                clientInfo.Append("}");

                info_ = clientInfo.ToString();
                localSettings.Values[_CLIENT_INFO] = info_;
            }

            Debug.WriteLine("Client Info: " + info_);
            return info_;
        }
    }

    public class DeviceInfoKernalIoControlHelperImpl : IDeviceInfoHelper
    {
        private static Int32 METHOD_BUFFERED = 0;
        private static Int32 FILE_ANY_ACCESS = 0;
        private static Int32 FILE_DEVICE_HAL = 0x00000101;

        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;

        private static Int32 IOCTL_HAL_GET_DEVICEID =
            ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14)
            | ((21) << 2) | (METHOD_BUFFERED);

        //[DllImport("coredll.dll", SetLastError = true)]
        //private static extern bool KernelIoControl(Int32 dwIoControlCode,
        //    IntPtr lpInBuf, Int32 nInBufSize, byte[] lpOutBuf,
        //    Int32 nOutBufSize, ref Int32 lpBytesReturned);

        public string GetDeviceID()
        {
            return string.Empty;
            //// Initialize the output buffer to the size of a 
            //// Win32 DEVICE_ID structure.
            //byte[] outbuff = new byte[20];
            //Int32 dwOutBytes;
            //bool done = false;

            //Int32 nBuffSize = outbuff.Length;

            //// Set DEVICEID.dwSize to size of buffer.  Some platforms look at
            //// this field rather than the nOutBufSize param of KernelIoControl
            //// when determining if the buffer is large enough.
            //BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
            //dwOutBytes = 0;

            //// Loop until the device ID is retrieved or an error occurs.
            //while (!done)
            //{
            //    if (KernelIoControl(IOCTL_HAL_GET_DEVICEID, IntPtr.Zero,
            //        0, outbuff, nBuffSize, ref dwOutBytes))
            //    {
            //        done = true;
            //    }
            //    else
            //    {
            //        int error = Marshal.GetLastWin32Error();
            //        switch (error)
            //        {
            //            case ERROR_NOT_SUPPORTED:
            //                throw new NotSupportedException(
            //                    "IOCTL_HAL_GET_DEVICEID is not supported on this device");

            //            case ERROR_INSUFFICIENT_BUFFER:

            //                // The buffer is not big enough for the data.  The
            //                // required size is in the first 4 bytes of the output
            //                // buffer (DEVICE_ID.dwSize).
            //                nBuffSize = BitConverter.ToInt32(outbuff, 0);
            //                outbuff = new byte[nBuffSize];

            //                // Set DEVICEID.dwSize to size of buffer.  Some
            //                // platforms look at this field rather than the
            //                // nOutBufSize param of KernelIoControl when
            //                // determining if the buffer is large enough.
            //                BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
            //                break;

            //            default:
            //                throw new Exception("Unexpected error");
            //        }
            //    }
            //}

            //// Copy the elements of the DEVICE_ID structure.
            //Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4);
            //Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8);
            //Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc);
            //Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10);
            //StringBuilder sb = new StringBuilder();

            //for (int i = dwPresetIDOffset;
            //    i < dwPresetIDOffset + dwPresetIDSize; i++)
            //{
            //    sb.Append(String.Format("{0:X2}", outbuff[i]));
            //}

            //sb.Append("-");

            //for (int i = dwPlatformIDOffset;
            //    i < dwPlatformIDOffset + dwPlatformIDSize; i++)
            //{
            //    sb.Append(String.Format("{0:X2}", outbuff[i]));
            //}
            //return sb.ToString();
        }



        public string GetDeviceName()
        {
            throw new NotImplementedException();
        }


        Task<string> IDeviceInfoHelper.GetDeviceName()
        {
            throw new NotImplementedException();
        }

        public string GetHostName()
        {
            throw new NotImplementedException();
        }


        public Task<string> GetClientInfo()
        {
            throw new NotImplementedException();
        }
    }
}
