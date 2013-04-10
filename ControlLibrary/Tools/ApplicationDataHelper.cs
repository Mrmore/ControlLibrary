using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ControlLibrary.Tools
{
    public class ApplicationDataHelper
    {
        public static object ReadSetting(string key)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(key))
                return localSettings.Values[key];
            return null;
        }

        public static void WriteSetting(string key, object value)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[key] = value;
        }

        public static void RemoveSetting(string key)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(key))
                localSettings.Values.Remove(key);
        }

        public static ApplicationDataCompositeValue ReadCompositeSetting(string key)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (!roamingSettings.Values.ContainsKey(key)) return null;
            var composite = (ApplicationDataCompositeValue)roamingSettings.Values[key];
            return composite;
        }

        public static void WriteCompositeSetting(string key, IDictionary<string, string> dictionary)
        {
            var composite = new ApplicationDataCompositeValue();
            foreach (var kv in dictionary)
            {
                composite[kv.Key] = kv.Value;
            }
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[key] = composite;
        }

        public static void RemoveCompositeSetting(string key)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(key))
                roamingSettings.Values.Remove(key);
        }
    }
}
