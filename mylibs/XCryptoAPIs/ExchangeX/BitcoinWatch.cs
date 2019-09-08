using System;
using System.Collections.Generic;
using Quandl.NET;
using Tools;
using static Tools.G;

//using JSON;

namespace CryptoAPIs.ExchangeX
{

    public static class BitcoinWatch
    {
        public static string BaseUrl = null;

        public static void Test()
        {
            cout("BitcoinWatch");

            // BitcoinWatch Bitcoin Mining Statistics
            // https://www.quandl.com/data/BITCOINWATCH/MINING-Bitcoin-Mining-Statistics 
            // code: "BITCOINWATCH/MINING"

            GetMiningStatistics();              // BitcoinWatch Bitcoin Mining Statistics
        }

        public static Quandl.NET.Model.Response.TimeseriesDataResponse GetMiningStatistics()
        {            
            var res = ZQuandl.Q.GetHistorical("BITCOINWATCH/MINING");
            ZQuandl.Print(res);
            return res;
        }


    } // end of class

    //------------------------------------------------------------------------------------------------------------------------


} // end of namespace 


