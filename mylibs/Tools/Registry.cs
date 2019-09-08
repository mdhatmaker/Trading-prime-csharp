using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tools
{
    public static class MyRegistry
    {
        public static class CurrentUser
        {
            // Where registryPath like "Software\\DTN\\IQFeed\\Startup"
            public static MyRegistryKey OpenSubKey(string registryPath, bool createIfNotExist = false)
            {
                string filename = string.Format("REGISTRY.CurrentUser.{0}.json", registryPath.Replace('\\', '.'));
                string json = GFile.ReadTextFile(Folders.misc_path(filename), createIfNotExist);
                if (json == null) return null;
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return new MyRegistryKey(dict);
            }
        } // end of class CurrentUser
        public static class LocalMachine
        {
            // Where registryPath like "Software\\DTN\\IQFeed\\Startup"
            public static MyRegistryKey OpenSubKey(string registryPath, bool createIfNotExist = false)
            {
                string filename = string.Format("REGISTRY.LocalMachine.{0}.json", registryPath.Replace('\\', '.'));
                string json = GFile.ReadTextFile(Folders.misc_path(filename), createIfNotExist);
                if (json == null) return null;
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return new MyRegistryKey(dict);
            }
        } // end of class LocalMachine
    } // end of class MyRegistry


    public class MyRegistryKey
    {
        private Dictionary<string, string> m_values;

        public MyRegistryKey(Dictionary<string, string> values)
        {
            if (values == null)
                m_values = new Dictionary<string, string>();
            else
                m_values = values;
        }

        public string GetValue(string key, string defaultValue)
        {
            string value;
            if (!m_values.TryGetValue(key, out value))
            {
                value = defaultValue;
            }
            return value;
        }

        public void Close()
        {
            // Close method currently does nothing, but it exists to mirror functionality of typical RegistryKey object
        }


    } // end of class MyRegistryKey

} // end of namespace
