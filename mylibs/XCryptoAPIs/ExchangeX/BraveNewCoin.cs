using System;
using System.Collections.Generic;
using Quandl.NET;
using Tools;
using static Tools.G;

//using JSON;

namespace CryptoAPIs.ExchangeX
{

    public static class BraveNewCoin
    {
        public static string BaseUrl = null;

        public static void Test()
        {
            cout("BraveNewCoin");

            // BraveNewCoin Daily Global Price Index for Bitcoin
            // https://www.quandl.com/data/BNC3/GWA_BTC-BraveNewCoin-Daily-Global-Price-Index-for-Bitcoin 
            // code: "BNC3/GWA_BTC"

            // BraveNewCoin Daily Global Price Index for Litecoin
            // https://www.quandl.com/data/BNC3/GWA_LTC-BraveNewCoin-Daily-Global-Price-Index-for-Litecoin 
            // code: "BNC3/GWA_LTC"

            /*var gpiBTC = MyQuandl.Q.GetHistorical("BNC3/GWA_BTC");
            var gpiLTC = MyQuandl.Q.GetHistorical("BNC3/GWA_LTC");
            gpiBTC.Print();
            gpiLTC.Print();*/

            GetDailyGlobalPriceIndexBTC();      // BraveNewCoin Daily Global Price Index for Bitcoin
            GetDailyGlobalPriceIndexLTC();      // BraveNewCoin Daily Global Price Index for Litecoin
        }

        /*public static BraveNewCoinTicker GetBraveNewCoinTicker(string symbol)
        {
            return GET<BraveNewCoinTicker>(string.Format(@"https://api.itbit.com/v1/markets/{0}/ticker", symbol));
        }*/

        public static Quandl.NET.Model.Response.TimeseriesDataResponse GetDailyGlobalPriceIndexBTC()
        {
            var gpiBTC = ZQuandl.Q.GetHistorical("BNC3/GWA_BTC");
            ZQuandl.Print(gpiBTC);
            return gpiBTC;
        }

        public static Quandl.NET.Model.Response.TimeseriesDataResponse GetDailyGlobalPriceIndexLTC()
        {
            var gpiLTC = ZQuandl.Q.GetHistorical("BNC3/GWA_LTC");
            ZQuandl.Print(gpiLTC);
            return gpiLTC;
        }

    } // end of class

    //------------------------------------------------------------------------------------------------------------------------

    /*public class BraveNewCoinTicker
    {
        public string pair { get; set; }
        public string bid { get; set; }
        public string bidAmt { get; set; }
        public string ask { get; set; }
        public string askAmt { get; set; }
        public string lastPrice { get; set; }
        public string lastAmt { get; set; }
        public string volume24h { get; set; }
        public string volumeToday { get; set; }
        public string high24h { get; set; }
        public string low24h { get; set; }
        public string highToday { get; set; }
        public string lowToday { get; set; }
        public string openToday { get; set; }
        public string vwapToday { get; set; }
        public string vwap24h { get; set; }
        public string serverTimeUTC { get; set; }
    } // end of class ItBitTicker

    public class BraveNewCoinOrderBook
    {
        // List entries are [price, amount]
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
    }*/

} // end of namespace 
