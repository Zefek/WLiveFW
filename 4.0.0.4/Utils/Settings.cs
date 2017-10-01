using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace WLive.Utils
{
    public static class Settings
    {
        public static int GetSettingsInt(string configName, int value)
        {
            int result = 0;
            if (int.TryParse(GetSettings(configName, value.ToString(CultureInfo.InvariantCulture)), out result))
                return result;
            return value;
        }

        public static string GetSettings(string configName, string defaultValue)
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(configName))
                return ConfigurationManager.AppSettings.Get(configName);
            return defaultValue;
        }
    }
}
