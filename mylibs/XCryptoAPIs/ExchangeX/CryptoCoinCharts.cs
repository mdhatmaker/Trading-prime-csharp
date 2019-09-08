using System;
using System.Collections.Generic;
using static Tools.G;

namespace CryptoAPIs.ExchangeX
{
    public static class CryptoCoinCharts
    {
        // https://cryptocoincharts.info/tools/api

        public static string BaseUrl = "http://api.cryptocoincharts.info";

        public static void Test()
        {
            var symbols = GetCoinList();
            cout(symbols);

        }

        public static List<CryptoCoinChartsCoin> GetCoinList()
        {
            return GET<List<CryptoCoinChartsCoin>>(@"http://api.cryptocoincharts.info/listCoins");
        }

       

    } // end of class


    public class CryptoCoinChartsCoin
    {
        public string id { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public string price_btc { get; set; }
        public string volume_btc { get; set; }
    }



} // end of namespace
