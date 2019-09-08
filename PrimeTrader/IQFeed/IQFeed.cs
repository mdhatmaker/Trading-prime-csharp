using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Tools;

namespace IQFeed
{
    public enum IntervalType { Second = 0, Tick, Volume }

    public static class IQFeed
    {
        public static string GetIntervalType(IntervalType intervalType)
        {
            string result = "s";
            if (intervalType == IntervalType.Tick)
                result = "t";
            else if (intervalType == IntervalType.Volume)
                result = "v";
            return result;
        }

        // Download the full IQFeed symbols file (tab-delimited)
        public static void DownloadSymbolsFile()
        {
            string url = "http://www.dtniq.com/product/mktsymbols_v2.zip";
            GFile.DownloadFile(url, Folders.raw_path("mktsymbols_v2.zip"));
        }

        public static DataFrame dfReadMarketSymbolsFile(string filename = "mktsymbols_v2.txt")
        {
            string pathname = Folders.system_path(filename);
            var df = DataFrame.ReadDataFrame(pathname, createIndex: false, separatorChar: '\t');
            return df;
        }

    } // end of class
} // end of namespace
