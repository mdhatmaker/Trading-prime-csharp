using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tools
{
    // This file also contains an AppConfig class that replaces the missing
    // System.Configuration.ConfigurationManager class in .NET core

    // For an executable "CryptoMania.exe" or "CryptoMania.dll", for example, default settings filename is "CryptoMania.settings.txt"
    // There should only be one "settings" file for an app
    public class Settings
    {
        public Dictionary<string, string> SettingsMap { get { return m_settings; } }
        public string FilePathname { get { return m_settingsPathname; } }

        private static readonly string settings_filename_template = "{0}.settings.txt";     // format string for "settings" file
        private static string m_settingsPathname;                                           // if a settings file is loaded successfully, update this value

        private Dictionary<string, string> m_settings;
        private ApiSecurity m_security;

        public static Settings Instance { get { return m_instance; }}
        private static Settings m_instance = new Settings();
        private Settings()
        {
            //var friendlyName = System.AppDomain.CurrentDomain.FriendlyName;
            string exeFilename = GFile.GetExecutableFilename ();
            string settingsFilename = string.Format(settings_filename_template, exeFilename);
            //string file = object_of_type_in_application_assembly.GetType().Assembly.Location;
            //string app = System.IO.Path.GetFileNameWithoutExtension(file);

            //var processname = System.Diagnostics.Process.GetCurrentProcess().ProcessName;       // filename without extension ("dotnet")
            //var filename = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;  // full path and filename ("/usr/local/share/dotnet/dotnet")
            //System.IO.Path.GetFileName();
            //System.IO.Path.GetFileNameWithoutExtension();
            //System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            //System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;


            //string dir = AppDomain.CurrentDomain.BaseDirectory;
            string dir = GFile.GetExecutablePath();
            try
            {
                Console.WriteLine("\nSETTINGS: Looking for '{0}' in directory: {1}", settingsFilename, dir);
                m_settingsPathname = Path.Combine(dir, settingsFilename);
                m_settings = ReadKeyValueFile(m_settingsPathname);
                if (m_settings.Count == 0)
                {
                    Console.WriteLine("\nSETTINGS: Empty settings file or settings file not found.");
                }
                else
                {
                    if (this["SECURITY_FILENAME"] != null)
                    {
                        m_security = new ApiSecurity(this["SECURITY_FILENAME"]);
                    }

                    /*foreach (var exch in m_security.ApiKeys.Keys)
                    {
                        var c = m_security.ApiKeys[exch];
                        //Console.WriteLine("{0} '{1}' '{2}'", exch, c.ApiKey, c.ApiSecret);
                    }

                    ApiCredentials creds;
                    creds = m_security.ApiKeys["BINANCE"]*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nSETTINGS: Error occurred attempting to load settings file: {0}", ex.Message);
            }
        }

        //public int Count { get { return m_settings.Count(); }}
        public int Count => m_settings.Count();
        public ApiKeyMap ApiKeys => m_security.ApiKeys;

        // Return a (sorted) list of the settings names
        public List<string> Names
        {
            get
            {
                var names = m_settings.Keys.ToList();
                names.Sort();
                return names;
            }
        }

        // Retrieve a specific setting value (given a setting name)
        // Return null if the setting name does not exist
        public string this[string name]
        {
            get
            {
                if (name == null) return null;

                string value;
                if (m_settings.TryGetValue(name.Trim().ToUpper(), out value))
                {
                    return value.Trim();
                }
                else
                    return null;
            }
            set
            {
                if (name == null) return;

                m_settings[name.Trim().ToUpper()] = value;
            }
        }

        // Delete the settings entry with the given name
        public void Delete(string name)
        {
            string value;
            if (m_settings.TryGetValue(name, out value)) m_settings.Remove(name);
            WriteSettingsFile();
        }

        // Writes (to the settings file) the settings in their current state, including any updates or additions
        public void WriteSettingsFile()
        {
            if (m_settingsPathname != null)
            {
                using (var writer = new StreamWriter(m_settingsPathname))
                {
                    var sorted = m_settings.Keys.OrderBy(s => s).ToList();
                    foreach (var k in sorted)
                    {
                        writer.WriteLine("{0}={1}", k, m_settings[k]);
                    }
                }
            }
        }

        // Read a NAME=VALUE text file into a Dictionary<string,string>
        public static Dictionary<string, string> ReadKeyValueFile(string pathname)
        {
            var result = new Dictionary<string, string>();
            try
            {
                using (var reader = new StreamReader(pathname))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;      // skip blank lines
                        int idx = line.IndexOf('=');
                        if (idx < 0)
                        {
                            Console.WriteLine("All lines in settings file should have format NAME=VALUE. Skipping the following line:\n{0}\n", line);
                            continue;
                        }
                        Console.WriteLine(line);
                        string name = line.Substring(0, idx).Trim().ToUpper();
                        string value = line.Substring(idx + 1);
                        result[name] = value;
                        //Console.WriteLine("{0} {1}", name, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR: Could not read from the Key=Value text file: {0}", ex.Message);
                //Console.WriteLine("Try again and specify the correct settings filename on the command line.");
            }
            return result;
        }

    } // end of class Settings



    // Use this class to replace the missing System.Configuration.ConfigurationManager class in .NET core
    public sealed class AppConfig : System.Collections.Generic.Dictionary<string, string>
    {
        private static AppConfig instance = null;
        private static readonly object padlock = new object();

        AppConfig()
        {
            //internalDict = new System.Collections.Generic.Dictionary<string, string>();
            string[] OldAppConfig = System.IO.File.ReadAllLines(@"app.config");
            string keyPattern = "key=(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            string valuePattern = "value=(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            Match m;
            for (int i = 0; i < OldAppConfig.Length; i++)
            {
                string thisString = OldAppConfig[i];
                if (thisString.Contains("add") && thisString.Contains("key") && thisString.Contains("value"))
                {
                    // We have a valid config item. Time to extract.
                    m = Regex.Match(thisString, keyPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    string keyName = m.Groups[1].Value;

                    m = Regex.Match(thisString, valuePattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    string keyValue = m.Groups[1].Value;

                    if (this.ContainsKey(keyName))
                    {
                        this[keyName] = keyValue;
                    }
                    else
                    {
                        this.Add(keyName, keyValue);
                    }
                }
            }
        }

        public static AppConfig Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AppConfig();
                    }
                    return instance;
                }
            }
        }
    } // end of class AppConfig


} // end of namespace
