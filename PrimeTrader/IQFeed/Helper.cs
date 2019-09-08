using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;                  // added for access to RegistryKey
using Tools;

namespace IQFeed
{
    //public enum IntervalType { Second = 0, Tick, Volume }

    public static class Helper
    {
        public static string ToIntervalTypeString(this HistoryIntervalType itype) { return GetIntervalType(itype); }

        public static string GetIntervalType(HistoryIntervalType intervalType)
        {
            string result = "s";
            if (intervalType == HistoryIntervalType.Tick)
                result = "t";
            else if (intervalType == HistoryIntervalType.Volume)
                result = "v";
            return result;
        }

        // Download the full IQFeed symbols file (tab-delimited)
        public static void DownloadSymbolsFile()
        {
            string url = "http://www.dtniq.com/product/mktsymbols_v2.zip";
            GFile.DownloadFile(url, Folders.raw_path("mktsymbols_v2.zip"));
        }

        // Read the full IQFeed symbols list into a DataFrame
        public static DataFrame dfReadMarketSymbolsFile(string filename = "mktsymbols_v2.txt")
        {
            string pathname = Folders.system_path(filename);
            var df = DataFrame.ReadDataFrame(pathname, createIndex: false, separatorChar: '\t');
            return df;
        }

        /// <summary>
        /// Gets local IQFeed socket ports from the registry
        /// </summary>
        /// <param name="sPort"></param>
        /// <returns></returns>
        public static int GetIQFeedPort(string sPort)
        {
            int iReturn = 0;
            //RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            MyRegistryKey key = MyRegistry.CurrentUser.OpenSubKey("Software\\DTN\\IQFeed\\Startup");
            if (key != null)
            {
                string sData = "";
                switch (sPort)
                {
                    case "Level1":
                        // the default port for Level 1 data is 5009.
                        sData = key.GetValue("Level1Port", "5009").ToString();
                        break;
                    case "Lookup":
                        // the default port for Lookup data is 9100.
                        sData = key.GetValue("LookupPort", "9100").ToString();
                        break;
                    case "Level2":
                        // the default port for Level 2 data is 9200.
                        sData = key.GetValue("Level2Port", "9200").ToString();
                        break;
                    case "Admin":
                        // the default port for Admin data is 9300.
                        sData = key.GetValue("AdminPort", "9300").ToString();
                        break;
                    case "Derivative":
                        // the default port for derivative data is 9400
                        sData = key.GetValue("DerivativePort", "9400").ToString();
                        break;
                }
                iReturn = Convert.ToInt32(sData);
            }
            return iReturn;
        }

    } // end of class Helper
} // end of namespace
