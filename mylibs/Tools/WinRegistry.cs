using System;
namespace ToolsCore
{
    public class WinRegistry
    {
        #region Registry Settings --------------------------------------------------------------------------------------
        public static string m_appSubKey = null;
        public static string AppSubKey
        {
            get
            {
                if (m_appSubKey == null)
                {
                    //m_appSubKey = "Software\\" + GetAssemblyName();
                    m_appSubKey = GetAssemblyName();
                    CreateAppRegistryKey();
                }
                return m_appSubKey;
            }
            set
            {
                m_appSubKey = value;
                CreateAppRegistryKey();
            }
        }

        // Get the registry key name for this app that INCLUDES the "Software\\" subkey (use AppSubKey if you do NOT want to include "Software\\")
        public static string AppKey { get { return "Software\\" + AppSubKey; } }


        // Return tru if the specified registry key exists, otherwise return false
        public static bool AppRegistryKeyExists()
        {
            return (Registry.CurrentUser.OpenSubKey(AppKey) != null);
        }

        // Create a registry key as a subkey to "Software"
        public static void CreateAppRegistryKey()
        {
            if (AppRegistryKeyExists()) return;
            RegistryKey progSettings = Registry.CurrentUser.OpenSubKey("Software", true);
            progSettings.CreateSubKey(AppSubKey);
            progSettings.Close();
        }

        // Delete registry key that is a subkey of "Software"
        public static void DeleteAppRegistryKey()
        {
            if (!AppRegistryKeyExists())
            {
                dout("There are no registry settings to delete (key_name='{0}')", AppKey);
            }
            else
            {
                Registry.CurrentUser.DeleteSubKey(AppKey);
                dout("Registry settings cleared successfully (key_name='{0}')", AppKey);
            }
        }

        // Return true if the specified registry value exists, otherwise return false
        public static bool AppRegistryValueExists(string settingName)
        {
            //return (Registry.CurrentUser.GetValue(getKeyName(settingName), null) != null);
            //return GetAppRegistryValue(settingName) != null;
            //return (Registry.CurrentUser.OpenSubKey(getKeyName(settingName)) != null);
            RegistryKey root = Registry.CurrentUser.OpenSubKey(AppKey, false);
            return root.GetValue(settingName) != null;
        }

        // Set a registry value with the specified key that is a subkey to "Software"
        public static void SetAppRegistryValue(string settingName, object settingValue)
        {
            RegistryKey progSettings = Registry.CurrentUser.OpenSubKey(AppKey, true);
            progSettings.SetValue(settingName, settingValue); // store settings
            progSettings.Close();
        }

        // Retrieve a registry value from the specified key that is a subkey to "Software"
        public static T GetAppRegistryValue<T>(string settingName, object defaultValue = null) where T : class
        {
            RegistryKey progSettings = Registry.CurrentUser.OpenSubKey(AppKey, false);
            object settings = progSettings.GetValue(settingName, defaultValue); // retrieve settings
            progSettings.Close();
            return settings as T;
        }

        // Delete a registry value with the specified key that is a subkey to "Software"
        public static void DeleteAppRegistryValue(string settingName)
        {
            if (!AppRegistryValueExists(settingName))
            {
                dout("There are no registry settings to delete (key_name='{0}', setting_name='{1}')", AppKey, settingName);
            }
            else
            {
                RegistryKey progSettings = Registry.CurrentUser.OpenSubKey(AppKey, true);
                progSettings.DeleteValue(settingName, false);
                progSettings.Close();
                dout("Registry value deleted successfully (key_name='{0}', setting_name='{1}')", AppKey, settingName);
            }
        }
        #endregion -----------------------------------------------------------------------------------------------------
    }
}
