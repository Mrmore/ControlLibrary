using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerWrapper.Helper
{
    internal static class SerializationHelper
    {
        public static object DeserializeFromArray(byte[] data, Type type)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                DataContractSerializer serializer = new DataContractSerializer(type);
                return serializer.ReadObject(stream);
            }
        }

        public static T DeserializeFromArray<T>(byte[] data)
        {
            return (T)DeserializeFromArray(data, typeof(T));
        }

        public static byte[] SerializeToArray(object value, Type type)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(type);
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }

        public static byte[] SerializeToArray<T>(T value)
        {
            return SerializeToArray(value, typeof(T));
        }
    }
}
