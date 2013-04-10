using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RenrenCoreWrapper.Helper
{
    public class JsonUtility
    {
        public static object DeserializeObj(Stream inputStream, Type objType)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objType);
            object result = serializer.ReadObject(inputStream);
            return result;
        }

        public static string SerializeObj(Type objType, object writeableObj)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(objType);

            string result = string.Empty;

            if (ser != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ser.WriteObject(ms, writeableObj);
                    result = Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length);
                }

                return result;
            }

            return string.Empty;
        }
    }
}
