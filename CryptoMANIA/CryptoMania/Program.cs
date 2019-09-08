using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using Tools;

namespace CryptoMania
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> settings;

            if (Debugger.IsAttached)
            {
                //args = new string[] { "symbols", "neo" };
            }

            // Look for some specific tools that can be run from the command line
            if (args.Length > 0 && args[0].ToLower() == "symbols")
            {
                if (args.Length == 1)
                {
                    CryptoAPIs.Crypto.DisplaySymbols();
                }
                else
                {
                    CryptoAPIs.Crypto.DisplaySymbols(args[1]);
                }
            }
            else    // OTHERWISE, we are attempting to run a "typical" CryptoMania session, which requires a SETTINGS file to operate
            {
                /*string settings_filename = "CryptoMania.settings.txt";
                Console.WriteLine("\nLooking for '{0}' in this directory...", settings_filename);
                string dir = AppDomain.CurrentDomain.BaseDirectory;
                settings = ReadSettingsFile(Path.Combine(dir, settings_filename));
                if (settings.Count == 0)
                {
                    Console.WriteLine("\nEmpty settings file or settings file not found.");
                }
                else
                {
                    CryptoAPIs.Crypto.Test(settings);
                }*/
                if (Settings.Instance.Count > 0)
                {
                    CryptoAPIs.Crypto.Test();
                }
            }

            if (Debugger.IsAttached)
            {
                Console.Write("Press any key to exit... ");
                Console.ReadKey();
            }
        }

        static Dictionary<string, string> ReadSettingsFile(string filename)
        {
            var result = new Dictionary<string, string>();
            try
            {
                using (var reader = new StreamReader(filename))
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
                        string name = line.Substring(0, idx);
                        string value = line.Substring(idx + 1);
                        result[name] = value;
                        //Console.WriteLine("{0} {1}", name, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nERROR: Could not read from the settings file.");
                Console.WriteLine("Try again and specify the correct settings filename on the command line.");
            }
            return result;
        }


    } // end of class Program
} // end of namespace
