using System;
using System.Configuration;
using System.Linq;

namespace JerryPlat.Utils.Helpers
{
    public static class ConfigHelper
    {
        public static bool IsExistConfig(string strKey)
        {
            return ConfigurationManager.AppSettings.AllKeys.Contains(strKey, StringComparer.OrdinalIgnoreCase);
        }

        public static string GetConfig(string strKey)
        {
            return GetConfig(strKey, false);
        }

        public static string GetConfig(string strKey, bool bIsRequired)
        {
            if (!IsExistConfig(strKey))
            {
                if (bIsRequired)
                {
                    throw new Exception($"Please confirm the key[{strKey}] must exist at the AppSetting note in the Web.config file.");
                }

                return string.Empty;
            }

            return ConfigurationManager.AppSettings[strKey];
        }
    }
}