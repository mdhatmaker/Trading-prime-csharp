using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Websockets.Models
{
    internal class MarketInfo
    {
        internal string Exchange { get; set; }
        internal string PrimaryCurrency { get; set; }
        internal string SecondaryCurrency { get; set; }

        // A function to parse a string and return our MarketInfo
        internal static MarketInfo ParseMarketInfo(string data)
        {
            var str = data.Replace("--", "-");
            var strArr = str.Split('-');
            return new MarketInfo()
            {
                Exchange = strArr[1],
                PrimaryCurrency = strArr[2],
                SecondaryCurrency = strArr[3]
            };
        }
    } // end of class MarketInfo
} // end of namespace
