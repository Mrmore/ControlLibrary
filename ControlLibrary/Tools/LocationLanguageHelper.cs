using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

namespace ControlLibrary.Tools
{
    public class LocationLanguageHelper
    {
        private const string LANG_SETTING = "language";
        public const string LANG_CN = "zh-Hans";
        public const string LANG_EN = "en-US";

        public static void DetermineLanguage()
        {
            //dp6
            //Windows.Globalization.ApplicationPreferences.PreferredLanguage = LANG_CN;
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = LANG_CN;
            return;

            string langSetting = (string)ApplicationDataHelper.ReadSetting(LANG_SETTING);
            if (langSetting == LANG_CN || langSetting == LANG_EN)
            {
                //dp6
                //Windows.Globalization.ApplicationPreferences.PreferredLanguage = LANG_CN;
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = langSetting;
            }
            else
            {
                bool isFindEn = false;
                foreach (var defaultLang in ResourceManager.Current.DefaultContext.Languages)
                {
                    if (defaultLang.Contains(LANG_CN))
                    {
                        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = LANG_CN;
                        return;
                    }
                    else if (defaultLang.Equals(LANG_EN, StringComparison.Ordinal))
                    {
                        isFindEn = true;
                    }
                }

                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = isFindEn ? LANG_EN : LANG_CN;
            }
        }

        public static string PreferredLanguage
        {
            get
            {
                return Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride;
            }
            set
            {
                if (value == LANG_CN || value == LANG_EN)
                {
                    ApplicationDataHelper.WriteSetting(LANG_SETTING, value);
                    Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = value;
                }
            }
        }

        public static string GetString(string key)
        {
            var loader = new ResourceLoader();
            return loader.GetString(key);
        }
    }
}
